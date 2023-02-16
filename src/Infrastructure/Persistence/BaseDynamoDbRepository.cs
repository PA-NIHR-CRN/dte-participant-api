using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Application.Extensions;
using Dte.Common.Helpers;

namespace Infrastructure.Persistence
{
    public class BaseDynamoDbRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly IDynamoDBContext _context;
        
        private const int MaxTransactionGetItems = 25;

        protected BaseDynamoDbRepository(IAmazonDynamoDB client, IDynamoDBContext context)
        {
            _client = client;
            _context = context;
        }

        protected async Task<QueryResults<T>> ScanTableAsync<T>(string sk, int limit, string paginationToken, RangeKeyScanOperator rangeKeyScanOperator, DynamoDBOperationConfig config)
        {
            var table = _context.GetTargetTable<T>(config);

            var scanOperationConfig = new ScanOperationConfig
            {
                Limit = limit,
                PaginationToken = EncodingHelper.DecodeBase64(paginationToken),
                Filter = new ScanFilter()
            };
            scanOperationConfig.Filter.AddCondition("SK", rangeKeyScanOperator == RangeKeyScanOperator.Equal ? ScanOperator.Equal : ScanOperator.BeginsWith, sk);
            
            var search = table.Scan(scanOperationConfig);

            var items = await search.GetNextSetAsync();

            return new QueryResults<T>
            {
                PaginationToken = string.IsNullOrWhiteSpace(search.PaginationToken) || search.PaginationToken == "{}" || !items.Any() ? null : EncodingHelper.EncodeBase64(search.PaginationToken),
                Items = _context.FromDocuments<T>(items)
            };
        }
        
        protected async Task<QueryResults<T>> QueryTableAsync<T>(string pk, string sk, int limit, string paginationToken, RangeKeyScanOperator rangeKeyScanOperator, DynamoDBOperationConfig config)
        {
            var table = _context.GetTargetTable<T>(config);
            
            var skExpression = rangeKeyScanOperator switch
            {
                RangeKeyScanOperator.Equal => "SK = :sk",
                RangeKeyScanOperator.BeginsWith => "begins_with(SK, :sk)",
                _ => throw new Exception($"Unknown RangeKeyScanOperator: {rangeKeyScanOperator}")
            };
   
            var search = table.Query(new QueryOperationConfig
            {
                Limit = limit,
                BackwardSearch = false,
                PaginationToken = EncodingHelper.DecodeBase64(paginationToken),
                KeyExpression = new Expression
                {
                    ExpressionStatement = $"PK = :pk AND {skExpression}",
                    ExpressionAttributeValues = { { ":pk", pk }, { ":sk", sk } }
                }
            });

            var items = await search.GetNextSetAsync();

            return new QueryResults<T>
            {
                PaginationToken = string.IsNullOrWhiteSpace(search.PaginationToken) || search.PaginationToken == "{}" || !items.Any() ? null : EncodingHelper.EncodeBase64(search.PaginationToken),
                Items = _context.FromDocuments<T>(items)
            };
        }

        protected async Task<IEnumerable<T>> GetMultipleItems<T>(IEnumerable<string> pks, string rangeKey, DynamoDBOperationConfig config)
        {
            var items = new List<Dictionary<string, AttributeValue>>();
            foreach (var batch in pks.GetByBatch(MaxTransactionGetItems))
            {
                var transactionItems = new List<TransactGetItem>();
                batch.ToList().ForEach(x =>
                {
                    transactionItems.Add(new TransactGetItem
                    {
                        Get = new Get
                        {
                            TableName = config.OverrideTableName,
                            Key = new Dictionary<string, AttributeValue>
                            {
                                {"PK", new AttributeValue(x)},
                                {"SK", new AttributeValue(rangeKey)}
                            }
                        }
                    });
                });
                var getItems = new TransactGetItemsRequest { TransactItems = transactionItems };
                var transGetItems = await _client.TransactGetItemsAsync(getItems);

                items.AddRange(transGetItems.Responses.Select(r => r.Item));
            }

            var results = items
                .Where(x => x.Count > 0)
                .Select(x => _context.FromDocument<T>(Document.FromAttributeMap(x)));

            return results;
        }
    }
}