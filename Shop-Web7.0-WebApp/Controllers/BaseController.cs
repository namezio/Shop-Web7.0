using System.Reflection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using WebApp.Database;

namespace Shop_Web7._0_WebApp.Controllers;

public class BaseController :Controller
{
    public WebAppEntities DbContext;
    public IDistributedCache Cache;
    public readonly IStringLocalizer Languages;
    public string LanguageCode;
    
    public BaseController(IStringLocalizerFactory factory)
    {
        // var type = typeof(Resources.Languages);
        // var assemblyFullname = type.GetTypeInfo().Assembly.FullName;
        // if (string.IsNullOrEmpty(assemblyFullname))
        //     return;
        //     
        // var assemblyName = new AssemblyName(assemblyFullname).Name;
        // if (string.IsNullOrEmpty(assemblyName))
        //     return;
        //     
        // Languages = factory.Create(type.Name, assemblyName);
    }

    // public override void OnActionExecuting(ActionExecutingContext context)
    // {
    //     base.OnActionExecuting(context);
    //     LanguageCode = HttpContext.Items.ContainsKey(CustomRequestCultureProvider.ContextName) ? HttpContext.Items[CustomRequestCultureProvider.ContextName]?.ToString() : null;
    //     ViewBag.LanguageCode = LanguageCode;
    // }
}