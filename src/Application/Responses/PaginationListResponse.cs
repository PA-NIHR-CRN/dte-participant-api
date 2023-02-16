using System.Collections.Generic;

namespace Application.Responses
{
    public class PaginationListResponse<T> where T : class
    {
        public string PaginationToken { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}