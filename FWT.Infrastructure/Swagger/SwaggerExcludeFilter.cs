using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FWTL.Infrastructure.Swagger
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Schema schema, SchemaFilterContext context)
        {
            var excludedProperties = context.SystemType.GetProperties().Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name))
                {
                    schema.Properties.Remove(excludedProperty.Name);
                }
            }
        }
    }
}
