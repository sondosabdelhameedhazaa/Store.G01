using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServicesAbstractions;

namespace Presentation.Attributes
{
    public class CasheAttribute(int durationInSec)  : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().casheService;

            var casheKey = GenerateCasheKey(context.HttpContext.Request);
            var result = await casheService.GetCasheValueAsynk(casheKey);

            if (!string.IsNullOrEmpty(result)) {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result

                };
                return;
            }
              var contextResult = await next.Invoke();
            if(contextResult.Result is ObjectResult okObject)
            {
                casheService.SetCasheValueAsynk(casheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec)); 
            }

        }

        private string GenerateCasheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);

            foreach (var item in request.Query.OrderBy(q => q.Key)) {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();
        }





    }
}