using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.Domain;

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
                    Content = "Se requiere un Token"
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
                    Content = "El Token es inválido"
                };
                return;
            }

            var sessions = (ISessionLogic)context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));

            User user = sessions.GetUser(tokenId);
            if(user == null || user.IsDeleted)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "El Token es inválido"
                };
                return;
            }

            var indicatorsLogic = (IIndicatorLogic)context.HttpContext.RequestServices.GetService(typeof(IIndicatorLogic));
            
            string[] path = context.HttpContext.Request.Path.Value.Split('/');
            Guid indicatorId;
            bool validIndicatorId = Guid.TryParse(path[path.Length -1], out indicatorId);
            if(validIndicatorId && user.Role == Role.Manager && !indicatorsLogic.GetManagerIndicators(tokenId).Any(i => i.Indicator.Id == indicatorId))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "El gerente no tiene acceso a este Indicador"
                };
                return;
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}