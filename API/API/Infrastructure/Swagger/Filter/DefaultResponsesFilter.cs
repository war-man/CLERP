﻿using CLERP.API.Features.v1;
using CLERP.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLERP.API.Infrastructure.Swagger.Filter
{
    /// <summary>
    /// Registers all default responses and their appearance and adds them globally for every endpoint in the swagger doc
    /// </summary>
    public class DefaultResponsesFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // register validation failed response only if the endpoint has atleast one parameter
            if (context.MethodInfo.GetParameters().Any())
            {
                // Unprocessable Entity (422) - Failed Validation
                var validationFailedResponseSchema = context.SchemaRegistry.GetOrRegister(typeof(ValidationFailedResponse));
                operation.Responses.Add("422", new Response() { Description = "Validation failed", Schema = validationFailedResponseSchema });
            }

            // register Unauthorized and Forbiden responses if there isn't an AllowAnonymous attribute or a specific Authorize attribute
            if (!context.MethodInfo.GetCustomAttributes(true).Any(attr => attr is AuthorizeAttribute) 
                && !context.MethodInfo.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType.Name == nameof(AuthorizeAttribute)) // check controller
                && !context.MethodInfo.GetCustomAttributes(true).Any(attr => attr is AllowAnonymousAttribute)
                && !context.MethodInfo.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType.Name == nameof(AllowAnonymousAttribute))) // check controller
            {
                operation.Responses.Add("401", new Response() { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response() { Description = "Forbidden" });

                // register required auth type for operation
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", Array.Empty<string>()}
                    }
                };
            }           

            // Internal Server Error (500)
            var internalSeverErrorResponseSchema = context.SchemaRegistry.GetOrRegister(typeof(MessageResponse));
            operation.Responses.Add("500", new Response() { Description = "An unknown error occured", Schema = internalSeverErrorResponseSchema });
        }
    }
}
