using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Support.Helpers
{
    public static class PageHelper<T> where T : class
    {
        public static PagedResult<T> Paging(List<T> list, int? page, int? pageSize)
        {
            try
            {
                if (page == null && pageSize == null)
                {
                    pageSize = list.Count;
                    page = 1;
                }
                else
                if (page < 1 || pageSize < 1)
                {
                    return null;
                }
                var skipAmount = pageSize * (page - 1);
                var totalNumberOfRecords = list.Count;
                var results = list.Skip((int)skipAmount).Take((int)pageSize).ToList();
                var mod = totalNumberOfRecords % pageSize;
                var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);
                return new PagedResult<T>
                {
                    Results = results,
                    PageNumber = (int)page,
                    PageSize = (int)pageSize,
                    TotalNumberOfPages = (int)totalPageCount,
                    TotalNumberOfRecords = totalNumberOfRecords,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<T> Sorting(SortOrder sortType, IEnumerable<T> searchResult, string colName)
        {
            if (sortType == SortOrder.Ascending)
            {
                return searchResult.OrderBy(item => typeof(T).GetProperties().First(x => x.Name.Contains(colName, StringComparison.CurrentCultureIgnoreCase)).GetValue(item)).ToList();
            }
            else if (sortType == SortOrder.Descending)
            {
                return searchResult.OrderByDescending(item => typeof(T).GetProperties().First(x => x.Name.Contains(colName, StringComparison.CurrentCultureIgnoreCase)).GetValue(item)).ToList();
            }
            else
            {
                return searchResult.ToList();
            }
        }
}
}
