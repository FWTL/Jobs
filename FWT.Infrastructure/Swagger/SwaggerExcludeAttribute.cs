using System;

namespace FWTL.Infrastructure.Swagger
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}
