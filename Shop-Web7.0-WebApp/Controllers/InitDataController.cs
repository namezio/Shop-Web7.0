using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using WebApp.Common.Repositories;
using WebApp.Database;

namespace Shop_Web7._0_WebApp.Controllers;

public class InitDataController:BaseController
{
    public InitDataController(IStringLocalizerFactory factory, WebAppEntities dbContext, IDistributedCache cache) : base(factory)
    {
        DbContext = dbContext;
        Cache = cache;
    }

    public IActionResult Index()
    {
        new UserRepository(DbContext).InitData();
        
    }
}