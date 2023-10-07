using BlazorSessionAuthDemo.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<SessionService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    // Set the timeout for the session to 1 minute after which the session expires if there's no activity
    options.IdleTimeout = TimeSpan.FromMinutes(1);

    // Ensure the cookie is accessible only by the server (prevents client-side scripts from accessing the cookie)
    options.Cookie.HttpOnly = true;

    // Mark the cookie as essential, so it won't be blocked by cookie consent functionality
    options.Cookie.IsEssential = true;

    //Make sure the session cookie is sent over HTTPS only.
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    //Use the SameSite property to control when cookies are sent to the server. This can prevent cross-site request forgery attacks.
    //options.Cookie.SameSite = SameSiteMode.Strict;


    //Change the default session cookie name to something less obvious.
    options.Cookie.Name = "BlazorSessionAuthDemoSessionKey";

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // This should come after UseRouting


// Workaround: The session cannot be established after the response has started
// https://stackoverflow.com/questions/60660923/session-setstring-in-server-side-net-core-produces-error-the-session-cannot
//begin SetString() hack
app.Use(async delegate (HttpContext Context, Func<Task> Next)
{
    //this throwaway session variable will "prime" the SetString() method
    //to allow it to be called after the response has started
    var TempKey = Guid.NewGuid().ToString(); //create a random key
    Context.Session.Set(TempKey, Array.Empty<byte>()); //set the throwaway session variable
    Context.Session.Remove(TempKey); //remove the throwaway session variable
    await Next(); //continue on with the request
});

//end SetString() hack


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
