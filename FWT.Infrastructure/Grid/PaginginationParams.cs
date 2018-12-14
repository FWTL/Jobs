using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Infrastructure.Grid
{
    [ModelBinder(typeof(PaginationParamsModelBinder))]
    public class PaginationParams
    {
        public PaginationParams()
        {
        }

        public long Offset
        {
            get { return (Page - 1) * (int)PerPage; }
        }

        public int Page { get; set; }

        public PageSize PerPage { get; set; }

        internal string Host { get; set; }

        internal string Path { get; set; }
    }

    public class PaginationParamsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            StringValues page;
            bindingContext.HttpContext.Request.Query.TryGetValue("page", out page);
            int? l_page = page.Count > 0 ? page[0].ToN<int>() : null;

            StringValues perPage;
            bindingContext.HttpContext.Request.Query.TryGetValue("per_page", out perPage);
            PageSize? e_pageSize = perPage.Count > 0 ? perPage[0].ToEnum<PageSize>() : null;

            string host = $"{bindingContext.HttpContext.Request.Scheme}://{bindingContext.HttpContext.Request.Host.Value}";
            string path = bindingContext.HttpContext.Request.Path.Value;

            var result = new PaginationParams()
            {
                Page = l_page ?? 0,
                PerPage = e_pageSize ?? PageSize.p10,
                Host = host,
                Path = path
            };

            var validationResult = new PaginationParamsValidator().Validate(result);

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

    public class PaginationParamsValidator : AbstractValidator<PaginationParams>
    {
        public PaginationParamsValidator()
        {
            RuleFor(x => x.PerPage).IsInEnum();
            RuleFor(x => x.Page).InclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.Host).NotEmpty();
            RuleFor(x => x.Path).NotEmpty();
        }
    }
}
