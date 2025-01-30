using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));
builder.Services.AddScoped<IQuestionServices, QuestionServices>();
builder.Services.AddScoped<IQuizServices, QuizServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

await CreateRolesAsync(app);
await CreateAdminUserAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<AppUser>();
app.MapControllers();

app.MapPost("/registerme", async (UserManager<AppUser> userManager, [FromBody] RegisterDto model) =>
{
    var user = new AppUser
    {
        UserName = model.Email,
        Email = model.Email,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(user, model.Password);

    if (!result.Succeeded)
    {
        return Results.BadRequest(result.Errors);
    }

    await userManager.AddToRoleAsync(user, "User");
    await userManager.AddToRoleAsync(user, "Member");

    return Results.Ok();
});

app.Run();


async Task CreateRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
            {
                Console.WriteLine($"Error creating role {role}: {string.Join(", ", result.Errors)}");
            }
        }
    }
}

async Task CreateAdminUserAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    const string adminEmail = "admin@gmail.com";
    const string adminPassword = "P@ssword1234";

    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
    if (existingAdmin != null) return;

    var newAdmin = new AppUser
    {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true
    };

    var createResult = await userManager.CreateAsync(newAdmin, adminPassword);
    if (!createResult.Succeeded)
    {
        Console.WriteLine($"Admin creation failed: {string.Join(", ", createResult.Errors)}");
        return;
    }

    var roleResult = await userManager.AddToRoleAsync(newAdmin, "Admin");
    if (!roleResult.Succeeded)
    {
        Console.WriteLine($"Adding Admin role failed: {string.Join(", ", roleResult.Errors)}");
    }
}