using System.Collections.Generic;

namespace Dwapi.Exchange.SharedKernel
{
	public class PagedExtract
	{
		public int PageNumber { get; }
		public int PageSize { get; }
		public int PageCount { get; }
		public int TotalItemCount { get; }
		public IEnumerable<dynamic> Extract { get; }

		public PagedExtract(int pageNumber, int pageSize, int pageCount, List<dynamic> extract)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			PageCount = pageCount;
			TotalItemCount = extract.Count;
			Extract = extract;
		}
	}
}
