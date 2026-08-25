namespace DocumentManagement.Data.Resources
{
    /// <summary>
    /// Base paging and sorting options shared by all list endpoints.
    /// </summary>
    public abstract class ResourceParameter
    {
        public ResourceParameter(string orderBy)
        {
            this.OrderBy = orderBy;
        }
        const int maxPageSize = 100;
        /// <summary>Number of records to skip (0-based). Default: 0.</summary>
        public int Skip { get; set; } = 0;

        private int _pageSize = 10;
        /// <summary>Page size. Default: 10. Maximum: 100.</summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {

                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        /// <summary>Free-text search across supported fields.</summary>
        public string SearchQuery { get; set; }

        /// <summary>Property to order by, e.g. "Name", "CreatedDate". Default: Name.</summary>
        public string OrderBy { get; set; }

        /// <summary>Comma-separated list of properties to return (sparse fields).</summary>
        public string Fields { get; set; }
    }
}
