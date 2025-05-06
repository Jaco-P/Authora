using Authora.Components;
using Authora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Authora.Application.Interfaces;
using Authora.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Register SQLite Service
builder.Services.AddDbContext<AuthoraDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register User Service
builder.Services.AddScoped<IUserService, UserService>();

//Register Group Service
builder.Services.AddScoped<IGroupService, GroupService>();

//Register Permission Service
builder.Services.AddScoped<IPermissionService, PermissionService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthoraDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
