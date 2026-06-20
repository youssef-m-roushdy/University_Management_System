// Infrastructure/Presentation/Filters/ValidationFilter.cs
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Infrastructure.Presentation.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = context.HttpContext.RequestServices
                    .GetService(validatorType) as IValidator;

                if (validator == null) continue; // no validator registered for this DTO, skip

                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(e => new ApiError
                    {
                        Field = e.PropertyName,
                        Message = e.ErrorMessage,
                        Code = e.ErrorCode
                    }).ToList();

                    context.Result = new BadRequestObjectResult(
                        ApiResponse<object>.ErrorResponse("Validation failed", 400, errors));
                    return; // short-circuit — never reaches the controller action
                }
            }

            await next();
        }
    }
}