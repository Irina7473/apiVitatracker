using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MedicineReminderAPI.Models;
using MedicineReminderAPI.Service;
using System.Net;


//�������������� ��������� WebApplicationBuilder � �������������� ������������ ���������� �� ���������.
var builder = WebApplication.CreateBuilder(args);

// ������������� ���� ��� ������������ �� �������� ������� WebApplication
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

//����������� � ���� ������  
string connection = builder.Configuration.GetConnectionString("RemoteConnection");
builder.Services.AddDbContext<AppApiContext>(options => options.UseMySql(connection,
     new MySqlServerVersion(new Version(8,0,31))));

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

builder.Services.AddCors();

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

// ��� �������� �� web � ������� ������
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

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

