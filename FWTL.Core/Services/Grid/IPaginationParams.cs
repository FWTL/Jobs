using static FWTL.Core.Helpers.Enum;

namespace FWTL.Core.Services.Grid
{
    public interface IPaginationParams
    {
        long Offset { get; }

        PerPage PerPage { get; }
    }
}