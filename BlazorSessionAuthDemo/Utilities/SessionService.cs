namespace BlazorSessionAuthDemo.Utilities;

public class SessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetString(string key, string value)
    {
        _httpContextAccessor.HttpContext.Session.SetString(key, value);
    }

    public string GetString(string key)
    {
        return _httpContextAccessor.HttpContext.Session.GetString(key);
    }

}
