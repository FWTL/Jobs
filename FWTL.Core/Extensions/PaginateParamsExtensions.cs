using System;
using System.Collections.Generic;
using System.Text;
using FWTL.Core.Services.Grid;

namespace FWTL.Core.Extensions
{
    public static class PaginateParamsExtensions
    {
        public static string ToSql(this IPaginationParams @params)
        {
            return $"OFFSET {@params.Offset} ROWS FETCH NEXT {(int)@params.PerPage} ROWS ONLY; ";
        }
    }
}
