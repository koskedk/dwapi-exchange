using System;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.SharedKernel.Custom
{
    public class Utils
    {
        public static int PageCount(int batchSize, long totalRecords)
        {
            if (totalRecords > 0)
            {
                if (totalRecords < batchSize)
                {
                    return 1;
                }

                return (int) Math.Ceiling(totalRecords / (double) batchSize);
            }

            return 0;
        }

        public static ExtractBlock CreateBlock(long pageNumber, long pageSize)
        {
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            /*
              Offset = (pageNumber - 1) * pageSize,
                        PageSize = pageSize
             */

            var first = pageNumber == 1 ? pageNumber : (pageNumber - 1) * pageSize + 1;
            var last = (first + pageSize) - 1;

            return new ExtractBlock(first, last);
        }
    }
}
