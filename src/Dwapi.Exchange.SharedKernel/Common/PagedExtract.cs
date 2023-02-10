using System.Collections.Generic;
using Dwapi.Exchange.Contracts;

namespace Dwapi.Exchange.SharedKernel.Common
{
	public abstract class Paged<T>
	{
		public int PageNumber { get; }
		public int PageSize { get; }
		public int PageCount { get; }
		public int TotalItemCount { get; }
		public IEnumerable<T> Extract { get; }

		protected Paged(int pageNumber, int pageSize, int pageCount,List<T> extract)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			PageCount = pageCount;
			TotalItemCount = extract.Count;
			Extract = extract;
		}
		public override string ToString()
		{
			return $"Page {PageNumber} of {PageCount} | [{TotalItemCount}] Rows";
		}
	}

	public class PagedExtract:Paged<dynamic>
	{
		public PagedExtract(int pageNumber, int pageSize, int pageCount, List<dynamic> extract) : base(pageNumber, pageSize, pageCount, extract)
		{
		}
	}

	public class PagedProfileExtract:Paged<Patients>
	{
		public PagedProfileExtract(int pageNumber, int pageSize, int pageCount, List<Patients> extract) : base(pageNumber, pageSize, pageCount, extract)
		{
		}
	}
}
