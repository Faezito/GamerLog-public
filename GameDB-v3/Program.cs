using GameDB_v3.Libraries.Login;
using GameDB_v3.Libraries.Sessao;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using System.Data.Common;
using Z1.Model.Email;
using Z2.Services;
using Z2.Services.Externo;
using Z3.DataAccess;
using Z3.DataAccess.Database;
using Z3.DataAccess.Externo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using Z4.Bibliotecas;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();

// Autenticação única
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
    googleOptions.CallbackPath = "/signin-google";

    // Claims extras
    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
});

// Data Protection persistente
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys")) // LOCAL NO SERVIDOR
    .SetApplicationName("LOGIN");  //MUDAR DE ACORDO COM O NOME DO APP

// Sessão
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IDapper, DapperDatabase>(x =>
{
    string strProv = "Microsoft.Data.SqlClient";
    string strConn = builder.Configuration.GetValue<string>("ConnectionStrings:GAMEDB");
    //var strConn = Environment.GetEnvironmentVariable("MONSTER_DB");

    if (string.IsNullOrWhiteSpace(strConn))
    {
        throw new InvalidOperationException(
            "A variável de ambiente para o banco de dados não está definida."
        );
    }

    DbProviderFactories.RegisterFactory(strProv, Microsoft.Data.SqlClient.SqlClientFactory.Instance);
    return new DapperDatabase(strProv, strConn);
});

builder.Services.AddScoped<ManipularModels>();
builder.Services.AddScoped<RawgService>();
builder.Services.AddScoped<IAPIsDataAccess, APIsDataAccess>();
builder.Services.AddScoped<IAPIsServicos, APIsServicos>();
builder.Services.AddScoped<IGeminiServicos, GeminiServicos>();
builder.Services.AddScoped<IUsuarioServicos, UsuarioServicos>();
builder.Services.AddScoped<IUsuarioDataAccess, UsuarioDataAccess>();
builder.Services.AddScoped<IEmailServicos, EmailServicos>();
builder.Services.AddScoped<IEmailDataAccess, EmailDataAccess>();
builder.Services.AddScoped<IRegistroJogoServicos, RegistroJogoServicos>();
builder.Services.AddScoped<IRegistroJogoDataAccess, RegistroJogoDataAccess>();
builder.Services.AddScoped<IJogoServicos, JogoServicos>();
builder.Services.AddScoped<IJogoDataAccess, JogoDataAccess>();
builder.Services.AddScoped<IPlataformaServicos, PlataformaServicos>();
builder.Services.AddScoped<IPlataformaDataAccess, PlataformaDataAccess>();

//// Cookies

//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.CheckConsentNeeded = context => true;
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//});

builder.Services.AddHttpClient<RawgService>(client =>
{
    client.BaseAddress = new Uri("https://api.rawg.io/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});


//// Configurações de sessão

builder.Services.AddMemoryCache(); // serve para guardar os dados na memória
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromHours(1);
//    options.IOTimeout = TimeSpan.FromHours(1);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.AddAuthentication("CookieAuth")
//    .AddCookie("CookieAuth", options =>
//    {
//        options.LoginPath = "/Login/Index";
//    });


builder.Services.AddScoped<Sessao>();
builder.Services.AddScoped<LoginUsuario>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();