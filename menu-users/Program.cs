using System.Text;
using menu_users.Application.Features.Auth.LoginUserUseCase;
using menu_users.Application.Features.Auth.RegisterUser;
using menu_users.Application.Features.Users.CreateUserUseCase;
using menu_users.Application.Features.Users.GeneratePasswordUseCase;
using menu_users.Application.Features.Users.GetAllUsersUseCase;
using menu_users.Application.Features.Users.GetUserByIdUseCase;
using menu_users.Application.Features.Users.RemoveUserUseCase;
using menu_users.Application.Features.Users.UpdatePasswordUseCase;
using menu_users.Application.Features.Users.UpdateUserUseCase;
using menu_users.Application.Services;
using menu_users.Domain.Interfaces.Features.Auth;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;
using menu_users.Infrastructure.Data;
using menu_users.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var db = builder.Configuration.GetSection("db");
//Servicios
builder.Services.AddSingleton<IPasswordHasher, PasswordHasherService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMenuToUserService, MenuToUserService>();

//Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuToUserRepository, MenuToUserRepository>();

//UseCases
builder.Services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
builder.Services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
builder.Services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
builder.Services.AddScoped<IRemoveUserUseCase, RemoveUserUseCase>();
builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
builder.Services.AddScoped<IGeneratePasswordUseCase, GeneratePasswordUseCase>();
builder.Services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(db["connectionString"]));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var jwt = builder.Configuration.GetSection("jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Secret"]!)
            )
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

