using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", false, false);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddDbContext<SimpleForumContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database") ??
                      throw new InvalidOperationException("Database connection string must be set"));
});

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddTransient<ICurrentUserAccessor, CurrentUserAccessor>();

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/site.css", "css/**/*.css");
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();