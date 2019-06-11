using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.WebApi.Filters {

    public class ProtectFilter : Attribute, IActionFilter
    {
        private readonly Role _role;

        public ProtectFilter(Role role) 
        {
            _role = role;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Se requiere un Token",
                };
                return;
            }
            Guid result;
            bool isValidGuid = Guid.TryParse(token, out result);
            if (!isValidGuid)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "El Token es inválido",
                };
                return;
            }

            var sessions = (ISessionLogic)context.HttpContext.RequestServices
                .GetService(typeof(ISessionLogic));

            try
            {
                if (!sessions.IsValidToken(result))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 400,
                        Content = "El Token es inválido",
                    };
                    return;
                }
                if (!sessions.HasLevel(result, _role))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 400,
                        Content = "El usuario no es " + _role,
                    };
                    return;
                }
            }
            catch(DataAccessException de)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 503,
                        Content = de.Message
                    };
                    return;
                }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // after the action executes
        }
    }
}