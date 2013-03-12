using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Application.DTO
{
    public class PageableList<T>
    {
        public int TotalElements { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
        {
            get { return TotalElements / PageSize + (TotalElements % PageSize > 0 ? 1 : 0); }
        }

        public IEnumerable<T> CurrentElements { get; set; }

        public static PageableList<T> From(IEnumerable<T> currentElements, int pageSize, int currentPage, int totalElements)
        {
            return new PageableList<T>()
            {
                TotalElements = totalElements,
                CurrentPage = currentPage,
                PageSize = pageSize,
                CurrentElements = currentElements
            };
        }
    }
}
