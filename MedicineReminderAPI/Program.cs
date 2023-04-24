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


//�������������� ��������� WebApplicationBuilder � �������������� ������������ ���������� �� ���������.
var builder = WebApplication.CreateBuilder(args);

// ������������� ���� ��� ������������ �� �������� ������� WebApplication
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

//����������� � ���� ������  
string connection = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<AppApiContext>(options => options.UseMySql(connection,
     new MySqlServerVersion(new Version(10,4,27))));

// ���������� ����� ����������
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//��������� Swagger, ������� ������� SwaggerDocument������� ��������������� �� ������������ � �������.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"������� JWT ����� �����������.",
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

// ���������� �������� ��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
    // ���������, ����� �� �������������� �������� ��� ��������� ������
    ValidateIssuer = true,
    // ������, �������������� ��������
    ValidIssuer = AuthOptions.ISSUER,
    // ����� �� �������������� ����������� ������
    ValidateAudience = true,
    // ��������� ����������� ������
    ValidAudience = AuthOptions.AUDIENCE,
    // ����� �� �������������� ����� �������������
    ValidateLifetime = true,
    // ��������� ����� ������������
    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
    // ��������� ����� ������������
    ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IFindAuthorizedUser, FindAuthorizedUser>();

/*
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    options.HttpsPort = 5001;
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 44344;
});*/

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
}

// ���-����������, ������������ ��� ��������� ��������� HTTP � ���������.
var app = builder.Build();

app.Logger.LogInformation($"Time:{DateTime.Now.ToString()} Hello APP");

//��������� ������������� �� Swagger UI ��� �������� ���-API
app.UseSwagger();
app.UseSwaggerUI();
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
/*
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
*/
//�� �������������� ���� ��������������� HTTPS ��� ��������������� HTTP-�������� �� HTTPS
//app.UseHttpsRedirection();

//��������� ������������� �� ������������� �������� �����
app.UseRouting();

// ���������� middleware ��������������
app.UseAuthentication();
//���������� ����� ������ UseAuthentication, ����� �� ��� �������� ������ � ����������.
app.UseAuthorization();

//����� MapControllers ����������� �������� ����������� API � ���������� ��� �������� �����
// ������ ���� ���������, ����� ��������� ���������� � ���������� �����������
app.MapControllers();

app.Run();



public class AuthOptions
{
    public const string ISSUER = "UMRServer"; // �������� ������
    public const string AUDIENCE = "UMRClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
