using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.WebApi.Filters
{
    public class IndicatorFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) 
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if(token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "A token is required."
                };
                return;
            }
            Guid tokenId;
            bool isValidGuid = Guid.TryParse(token, out tokenId);
            if (!isValidGuid)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "The token is invalid."
                };
                return;
            }

            var sessions = (ISessionLogic)context.HttpContext.RequestServices
                .GetService(typeof(ISessionLogic));

            try
            {
                User user = sessions.GetUser(tokenId);
                if(user == null || user.IsDeleted)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 400,
                        Content = "The  token is invalid."
                    };
                    return;
                }

                var indicatorsLogic = (IIndicatorLogic)context.HttpContext.RequestServices
                    .GetService(typeof(IIndicatorLogic));
                
                string[] path = context.HttpContext.Request.Path.Value.Split('/');
                Guid indicatorId;
                bool validIndicatorId = Guid.TryParse(path[path.Length -1], out indicatorId);
                if(validIndicatorId && user.Role == Role.Manager && !indicatorsLogic
                    .GetManagerIndicators(tokenId).Any(i => i.Indicator.Id == indicatorId))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "The  manager cannot see this Indicator."
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
        
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}