using OmMediaWorkManagement.Web;
using OmMediaWorkManagement.Web.Components;
using OmMediaWorkManagement.Web.Components.Services;
using Radzen;
using Radzen.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddOutputCache();
builder.Services.AddScoped<RadzenDialog>();
builder.Services.AddScoped<IOmService, OmServices>();
builder.Services.AddHttpClient<IOmService ,OmServices>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
       //   client.BaseAddress = new("http://192.168.1.14:81");
         client.BaseAddress = new("https+http://localhost:7439");
        //client.BaseAddress = new("https+http://b359-13-60-77-120.ngrok-free.app");
    });

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
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
