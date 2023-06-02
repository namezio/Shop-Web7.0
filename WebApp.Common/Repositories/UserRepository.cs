using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Common.Cache;
using WebApp.Database;
using WebApp.Library.Extension;

namespace WebApp.Common.Repositories;

public class UserRepository : BaseRepository
{
    private readonly ILogger<UserRepository> _logger;
    public UserRepository(WebAppEntities dbContext, IDistributedCache cache = null) : base(dbContext, cache)
    {
    }

    public void InitData()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Users.json");
        if (!File.Exists(path))
        {
            Console.WriteLine($"File {path} is not exist");
            return;
        }

        try
        {
            var content = File.ReadAllText(path, Encoding.UTF8);
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            if (users?.Any() == true)
            {
                foreach (var user in users)
                {
                    user.UserName = user.UserName?.ToLower().Trim();
                    if (!string.IsNullOrEmpty(user.UserName) &&
                        !DbContext.Users.Any(x => !x.IsDeleted && x.UserName.Equals(user.UserName)) &&
                        !DbContext.Users.Any(x => x.Id == user.Id))
                    {
                        user.CreatedAt = DateTime.Now;
                        user.ModifiedAt = DateTime.Now;
                        user.Password = (string.IsNullOrEmpty(user.Password) ? "123456" : user.Password).Md5();
                        DbContext.Users.Add(user);
                        DbContext.SaveChanges();
                        Console.WriteLine($"Inserted user {user.FullName}-{user.UserName} to database.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void ClearCache(User user)
    {
        Cache.Remove("user_username:" + user.UserName);
        Cache.Remove("user_id:" + user.Id);
    }

    private UserCache SetCache(User user)
    {
        var cached = new UserCache
        {
            Id = user.Id,
            FullName = user.FullName,
            UserName = user.UserName,
            Password = user.Password,
            IsBlocked = user.IsBlocked,
            IdUserType = user.IdUserType
        };

        if (Cache == null)
            return cached;
        
        var json = JsonConvert.SerializeObject(cached);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddHours(8)
        };

        Cache.SetString("user_id:" + cached.Id, json, cacheOptions);
        Cache.SetString("user_username:" + cached.UserName, json, cacheOptions);
        return cached;
    }

    public User FindOneById(int id)
    {
        var user = DbContext.Users.FirstOrDefault(x => x.Id == id);
        return (user?.IsDeleted == false ? user : null)!;
    }

    private UserCache FindOneCache(string cacheKey)
    {
        var json = Cache?.GetString(cacheKey);
        if (string.IsNullOrEmpty(json))
            return null;

        return JsonConvert.DeserializeObject<UserCache>(json);
    }
    
    public UserCache FindOneCacheById(int id)
    {
        var cached = FindOneCache("user_id:" + id);
        if (cached != null)
            return cached;

        var user = DbContext.Users.FirstOrDefault(x => x.Id == id);
        if (user?.IsDeleted != false)
            return null;

        return SetCache(user);
    }
    
    public UserCache FindOneCacheByUsername(string username)
    {
        var cached = FindOneCache("usr_username:" + username);
        if (cached != null)
            return cached;

        var user = DbContext.Users.FirstOrDefault(x => x.UserName.Equals(username) && !x.IsDeleted);
        if (user == null)
            return null;

        return SetCache(user);
    }
}