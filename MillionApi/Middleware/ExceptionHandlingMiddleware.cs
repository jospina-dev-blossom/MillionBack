using Microsoft.AspNetCore.Mvc;
using MillionApi.Domain.Exceptions;
using FluentValidation;

namespace MillionApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Manejar 404 cuando la respuesta no ha comenzado
                if (context.Response.StatusCode == StatusCodes.Status404NotFound
                  && !context.Response.HasStarted)
                {
                    await HandleNotFoundAsync(context);
                }
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, problemDetails) = exception switch
            {
                FluentValidation.ValidationException fluentValidationException => HandleFluentValidationException(fluentValidationException),
                MillionApi.Domain.Exceptions.ValidationException domainValidationException => HandleDomainValidationException(domainValidationException),
                NotFoundException notFoundException => HandleNotFoundException(notFoundException),
                BadRequestException badRequestException => HandleBadRequestException(badRequestException),
                _ => HandleServerError(exception)
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static (int statusCode, ProblemDetails problemDetails) HandleFluentValidationException(FluentValidation.ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred."
            };

            problemDetails.Extensions["errors"] = exception.Errors
                 .GroupBy(e => e.PropertyName)
          .ToDictionary(
                 g => g.Key,
          g => g.Select(e => e.ErrorMessage).ToArray()
             );

            return (StatusCodes.Status400BadRequest, problemDetails);
        }

        private static (int statusCode, ProblemDetails problemDetails) HandleDomainValidationException(MillionApi.Domain.Exceptions.ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = exception.Message
            };

            problemDetails.Extensions["errors"] = exception.Errors;

            return (StatusCodes.Status400BadRequest, problemDetails);
        }

        private static (int statusCode, ProblemDetails problemDetails) HandleNotFoundException(NotFoundException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = exception.Message
            };

            return (StatusCodes.Status404NotFound, problemDetails);
        }

        private static (int statusCode, ProblemDetails problemDetails) HandleBadRequestException(BadRequestException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = exception.Message
            };

            return (StatusCodes.Status400BadRequest, problemDetails);
        }

        private static (int statusCode, ProblemDetails problemDetails) HandleServerError(Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = "An unexpected error occurred. Please try again later."
            };

            return (StatusCodes.Status500InternalServerError, problemDetails);
        }

        private static async Task HandleNotFoundAsync(HttpContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"The requested resource '{context.Request.Path}' was not found."
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
