using DCM_Client_Demo.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IHttpClientDemo, HttpClientDemo>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
}) .AddCookie("Cookies", (opt) => 
    {
        opt.AccessDeniedPath = "/Home/AccessDenied";
    })
    .AddOpenIdConnect("oidc", opt =>
    {
        opt.SignInScheme = "Cookies";
        opt.Authority = "http://localhost:5240";
        opt.ClientId = "dcm-mvc-client";
        opt.ResponseType = "code id_token";
        opt.SaveTokens = true;
        opt.ClientSecret = "MVCSecret";
        opt.RequireHttpsMetadata = false;
        opt.GetClaimsFromUserInfoEndpoint = true;
        opt.Scope.Add("full_name");
        opt.Scope.Add("state");
        opt.Scope.Add("position");
        opt.Scope.Add("role");
        opt.Scope.Add("dcmapi");
        opt.ClaimActions.MapUniqueJsonKey("role", "role");
        opt.ClaimActions.MapUniqueJsonKey("state", "state");
        opt.ClaimActions.MapUniqueJsonKey("position", "position");
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
