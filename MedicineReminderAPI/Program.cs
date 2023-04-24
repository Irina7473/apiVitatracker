using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MedicineReminderAPI.Models;
using MedicineReminderAPI.Service;
using MedicineReminderAPI;
using System.Net;
using Microsoft.AspNetCore.Rewrite;


//Инициализирует экземпляр WebApplicationBuilder с предварительно настроенными значениями по умолчанию.
var builder = WebApplication.CreateBuilder(args);

// устанавливаем файл для логгирования до создания объекта WebApplication
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

//Подключение к базе данных  
string connection = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<AppApiContext>(options => options.UseMySql(connection,
     new MySqlServerVersion(new Version(10,4,27))));


builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppApiContext>();

// Добавление служб приложения
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//генератор Swagger, который создает SwaggerDocumentобъекты непосредственно из контроллеров и моделей.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Введите JWT токен авторизации.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
             {
                Name = "Bearer",
                In = ParameterLocation.Header,
                 Reference = new OpenApiReference
                 {
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer"
                 },
             },
            new List<string>()
         }
    });
});

// добавление сервисов аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
    // указывает, будет ли валидироваться издатель при валидации токена
    ValidateIssuer = true,
    // строка, представляющая издателя
    ValidIssuer = AuthOptions.ISSUER,
    // будет ли валидироваться потребитель токена
    ValidateAudience = true,
    // установка потребителя токена
    ValidAudience = AuthOptions.AUDIENCE,
    // будет ли валидироваться время существования
    ValidateLifetime = true,
    // установка ключа безопасности
    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
    // валидация ключа безопасности
    ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IFindAuthorizedUser, FindAuthorizedUser>();


// Веб-приложение, используемое для настройки конвейера HTTP и маршрутов.
var app = builder.Build();

app.Logger.LogInformation($"Time:{DateTime.Now.ToString()} Hello APP");

//Добавляет промежуточное ПО Swagger UI для изучения веб-API
app.UseSwagger();
app.UseSwaggerUI();
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

//ПО промежуточного слоя перенаправления HTTPS для перенаправления HTTP-запросов на HTTPS
//app.UseHttpsRedirection();

app.UseRewriter(new RewriteOptions().AddRedirectToHttps());

//Добавляет промежуточное ПО маршрутизации конечных точек
app.UseRouting();

// добавление middleware аутентификации
app.UseAuthentication();
//разместить после метода UseAuthentication, чтобы он мог получить доступ к принципалу.
app.UseAuthorization();

//Метод MapControllers настраивает действия контроллера API в приложении как конечные точки
// должен быть последним, после установки принципала и применения авторизации
app.MapControllers();

app.Run();



public class AuthOptions
{
    public const string ISSUER = "UMRServer"; // издатель токена
    public const string AUDIENCE = "UMRClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
