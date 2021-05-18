using System;
using System.Collections.Generic;
using System.Text;

namespace DATN.Infrastructure.Responses
{
    public class ListResponse<T> where T: class
    {
        public List<T> Data { get; set; }
        public int TotalRecord { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public ActionResponse SetResponse(ActionResponse actionResponse)
        {
            if (PageIndex.HasValue && PageSize.HasValue)
            {
                actionResponse.TotalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRecord) / Convert.ToDouble(PageSize.Value)));
                actionResponse.PageIndex = PageIndex.Value;
            }
            actionResponse.Data = Data;
            return actionResponse;
        }
    }
}
