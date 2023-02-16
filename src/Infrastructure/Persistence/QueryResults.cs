using System.Collections.Generic;

namespace Infrastructure.Persistence
{
    public class QueryResults<T>
    {
        public string PaginationToken { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}