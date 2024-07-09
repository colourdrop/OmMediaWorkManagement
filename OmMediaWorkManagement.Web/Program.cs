using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OmMediaWorkManagement.Web;
using OmMediaWorkManagement.Web.AuthInterface;
using OmMediaWorkManagement.Web.AuthService;
using OmMediaWorkManagement.Web.Components;
using OmMediaWorkManagement.Web.Components.Services;
using OmMediaWorkManagement.Web.Helper;
using Radzen;
using Radzen.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddOutputCache();
builder.Services.AddScoped<RadzenDialog>();
 
builder.Services.AddScoped<IOmService, OmServices>();
 
builder.Services.AddHttpClient("ServerAPI",
      client => client.BaseAddress = new Uri("https+http://localhost:7439"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("ServerAPI"));
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
 
app.UseOutputCache();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
     ;

app.MapDefaultEndpoints();

app.Run();
