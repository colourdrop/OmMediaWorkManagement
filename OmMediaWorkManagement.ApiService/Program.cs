using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Middleware;
using OmMediaWorkManagement.ApiService.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<OmContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    options.UseNpgsql(connectionString,
        npgsqlOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
            // Adjust maxRetryCount and maxRetryDelay as needed.
        });
});

builder.Services.AddIdentity<UserRegistration, IdentityRole>()
   .AddEntityFrameworkStores<OmContext>()
   .AddDefaultTokenProviders();

// Other service registrations



// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;
	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	
});
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidAudience = configuration["JWT:ValidAudience"],
		ValidIssuer = configuration["JWT:ValidIssuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
	};
});
// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "OmMediaWorkManagement", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


builder.Logging.AddSerilog(logger);
var app = builder.Build();

// Use Swagger and SwaggerUI
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<TokenExpirationMiddleware>();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");
app.UseHsts();

// Map endpoints
app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
