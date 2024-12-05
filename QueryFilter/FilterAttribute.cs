namespace ScaffoldedApi.QueryFilter
{
    public class FilterAttribute<T> : FilterAttribute where T : IQueryFilter, new()
    {
        public FilterAttribute(string name = "", string description = "") 
            : base(FilterType.Custom)
        {
            Name = name;
            Description = description;
            Filter = new T();
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IQueryFilter Filter { get; set; } = null!;

        public FilterAttribute(FilterType type)
        {
            switch (type)
            {
                case FilterType.String:
                    Name = "string";
                    Description = "Case insensitive string search";
                    Filter = new StringQueryFilter();
                    break;
                case FilterType.DateRange:
                    Name = "date-range";
                    Description = "Date range search (startDate-endDate)";
                    Filter = new DateRangeQueryFilter();
                    break;
            }
        }
    }
}
