namespace WebApp.Common.Cache;

public class UserCache
{
    public int Id { get; set; }

    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsBlocked { get; set; }
    public int IdUserType { get; set; }
}