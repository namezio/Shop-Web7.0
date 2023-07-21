using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Common.Cache;
using WebApp.Database;
using WebApp.Library.Extension;

namespace WebApp.Common.Repositories;

public class UserTypeRepository : BaseRepository
{
    private readonly ILogger<UserRepository> _logger;

    public UserTypeRepository(WebAppEntities dbContext, ILogger<UserRepository> logger, IDistributedCache cache = null)
        : base(dbContext, cache)
    {
        _logger = logger;
    }

    public override void InitData()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Database", "UserTypes.json");
        if (!File.Exists(path))
        {
            Console.WriteLine($"File {path} is not exist.");
            return;
        }

        try
        {
            var content = File.ReadAllText(path, Encoding.UTF8);
            var types = JsonConvert.DeserializeObject<List<UserType>>(content);
            if (types?.Any() == true)
            {
                foreach (var type in types)
                {
                    if (!DbContext.UserTypes.Any(x => x.Id == type.Id))
                    {
                        DbContext.UserTypes.Add(type);
                        DbContext.SaveChanges();
                        Console.WriteLine($"Inserted user type {type.Name} to database.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}