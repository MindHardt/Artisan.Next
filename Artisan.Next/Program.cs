using System.Net;
using Artisan.Next.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Artisan.Next.Client.Pages;
using Artisan.Next.Components;
using Artisan.Next.Components.Account;
using Artisan.Next.Data;
using Artisan.Next.Data.Entities;
using Artisan.Next.EmailSender;
using Artisan.Next.Handlers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    ;

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password = new PasswordOptions
        {
            RequiredLength = 8,
            RequireNonAlphanumeric = false,
            RequireLowercase = false,
            RequireUppercase = false,
            RequireDigit = false
        };
        options.User.AllowedUserNameCharacters =
            "АаБбВвГгДдЕеËëЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя" +
            "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" +
            "0123456789+-._@!?*#()[]{}";
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddDataProtection().PersistKeysToDbContext<DataContext>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.SetDefaults());

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["OAuth:Google:ClientId"]!;
        googleOptions.ClientSecret = builder.Configuration["OAuth:Google:ClientSecret"]!;
        googleOptions.CallbackPath = "/signin-google";
    });

builder.Services.AddScoped<IEmailSender<ApplicationUser>, MailKitEmailSender>();
builder.Services.AddHandlers();
builder.Services.ConfigureJsonOptions();

builder.Services.AddOptions<SmtpOptions>().BindConfiguration("Smtp");

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;

    var knownProxies = builder.Configuration.GetSection("KnownProxies").Get<string[]>() ?? [];
    foreach (var proxy in knownProxies.Select(IPAddress.Parse))
    {
        options.KnownProxies.Add(proxy);
    }
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Artisan.Next API",
        Version = "v1"
    });
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();
}

app.Use((ctx, next) =>
{
    ctx.Request.Scheme = "https";
    return next();
});

app.UseForwardedHeaders();

app.Use((ctx, next) =>
{
    ctx.RequestServices
        .GetRequiredService<ILogger<Program>>()
        .LogInformation("Headers: [{Headers}]", ctx.Request.Headers);
    return next();
});

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artisan.Next Api");
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Home).Assembly);

app.MapAdditionalIdentityEndpoints();

await app.RunAsync();