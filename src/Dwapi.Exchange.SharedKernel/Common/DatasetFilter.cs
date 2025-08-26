namespace Dwapi.Exchange.SharedKernel.Common
{
    public class DatasetFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int[] SiteCodes { get; set; } = null;
        public string[] Indicators { get; set; } = null;
    }
}