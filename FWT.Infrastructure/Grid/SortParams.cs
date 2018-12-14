using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Infrastructure.Grid
{
    [ModelBinder(typeof(SortParamsBinder))]
    public class SortParams
    {
        public int ColumnNo { get; set; }

        public SortDirection Direction { get; set; }
    }

    public class SortParamsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            StringValues sorting;
            bindingContext.HttpContext.Request.Query.TryGetValue("sort", out sorting);

            int? l_columnNo = null;
            SortDirection? e_direction = null;

            if (sorting.Count > 0)
            {
                var split = sorting[0].Split('|');
                l_columnNo = split.ElementAtOrDefault(1) != null ? split[0].ToN<int>() : null;
                e_direction = split.ElementAtOrDefault(1) != null ? split[1].ToEnum<SortDirection>() : null;
            }

            var result = new SortParams()
            {
                ColumnNo = l_columnNo ?? 0,
                Direction = e_direction ?? SortDirection.DESC
            };

            var validationResult = new SortParamsValidator().Validate(result);

            if (validationResult.IsValid)
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
                throw new ValidationException(validationResult.Errors);
            }

            return Task.CompletedTask;
        }
    }

    public class SortParamsValidator : AbstractValidator<SortParams>
    {
        public SortParamsValidator()
        {
            RuleFor(x => x.Direction).IsInEnum();
            RuleFor(x => x.ColumnNo).GreaterThanOrEqualTo(0);
        }
    }
}
