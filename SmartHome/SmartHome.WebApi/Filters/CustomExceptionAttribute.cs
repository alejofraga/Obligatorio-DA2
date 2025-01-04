using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Exceptions;

namespace SmartHome.WebApi.Filters;

public sealed class CustomExceptionAttribute : IExceptionFilter
{
    private readonly Dictionary<Type, Func<Exception, ObjectResult>> _errors = new()
    {
        {
            typeof(NullReferenceException),
            (Exception exception) =>
            {
                var concreteException = (NullReferenceException)exception;
                return new ObjectResult(new
                {
                    Message = "Missing references",
                    Details = "Request is missing"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(ArgumentOutOfRangeException),
            (Exception exception) =>
            {
                var concreteException = (ArgumentOutOfRangeException)exception;
                return new ObjectResult(new
                {
                    Message = "Argument is not in a valid range",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(ArgumentNullException),
            (Exception exception) =>
            {
                var concreteException = (ArgumentNullException)exception;
                return new ObjectResult(new
                {
                    Message = "Argument cannot be null or empty",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(ArgumentException),
            (Exception exception) =>
            {
                var concreteException = (ArgumentException)exception;
                return new ObjectResult(new
                {
                    Message = "Argument is invalid",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(FormatException),
            (Exception exception) =>
            {
                var concreteException = (FormatException)exception;
                return new ObjectResult(new
                {
                    Message = "Argument format is invalid",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        },
        {
            typeof(NotFoundException),
            (Exception exception) =>
            {
                var concreteException = (NotFoundException)exception;
                return new ObjectResult(new
                {
                    Message = "Element not found",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
        },
        {
            typeof(InvalidOperationException),
            (Exception exception) =>
            {
                var concreteException = (InvalidOperationException)exception;
                return new ObjectResult(new
                {
                    Message = "A conflict occurred with existing resources",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                };
            }
        },
        {
            typeof(UnauthorizedAccessException),
            (Exception exception) =>
            {
                var concreteException = (UnauthorizedAccessException)exception;
                return new ObjectResult(new
                {
                    Message = "You are not authenticated",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
        },
        {
            typeof(ForbiddenAccessException),
            (Exception exception) =>
            {
                var concreteException = (ForbiddenAccessException)exception;
                return new ObjectResult(new
                {
                    Message = "Access is forbidden",
                    Details = concreteException.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
        },
    };

    public void OnException(ExceptionContext context)
    {
        var response = _errors.GetValueOrDefault(context.Exception.GetType());

        if (response == null)
        {
            context.Result = new ObjectResult(new
            {
                Message = "There was an error when processing the request",
                Details = "There was an error when processing the request"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            return;
        }

        context.Result = response(context.Exception);
    }
}
