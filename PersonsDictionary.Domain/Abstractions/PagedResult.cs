namespace Domain.Abstractions
{
    public class PagedResult<Person>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<Person> Results { get; set; }
    }
}
