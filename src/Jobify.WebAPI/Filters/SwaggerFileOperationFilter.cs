using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Jobify.WebAPI.Filters;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formFileParameters = context.ApiDescription.ParameterDescriptions
            .Where(x => x.Type == typeof(IFormFile));

        foreach (var parameter in formFileParameters)
        {
            Dictionary<string, OpenApiMediaType> content = new()
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["file"] = new() { Type = "string", Format = "binary" }
                        }
                    }
                }
            };

            operation.RequestBody = new OpenApiRequestBody { Content = content };
        }
    }
}
