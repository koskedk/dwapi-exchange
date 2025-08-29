namespace Dwapi.Exchange.Core.Domain.Definitions.Dtos
{
    public class RequestFilterDto
    {
        public virtual string code { get; set; }
        public virtual string name { get; set; }
        public virtual int pageNumber { get; set; }
        public virtual int pageSize { get; set; }
        public virtual int[] siteCodes { get; set; }
        public virtual string[] indicators { get; set; }
    }
}