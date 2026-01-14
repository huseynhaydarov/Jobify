using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Jobify.WebAPI.Filters;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        IEnumerable<ApiParameterDescription> formFileParameters = context.ApiDescription.ParameterDescriptions
            .Where(x => x.Type == typeof(IFormFile));

        foreach (ApiParameterDescription parameter in formFileParameters)
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
