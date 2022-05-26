namespace API.Helpers
{
    public class VisitsParams : PaginationParams
    {
        public int UserID { get; set; }
        public string Predicate { get; set; }
        public bool Filter { get; set; } = false;
    }
}