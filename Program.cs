
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MmuAPI;
using Serilog;


var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
if (MyConfig != null)
{
    cConfig.sUrlForApi_MailAuth = MyConfig.GetValue<string>("Config:UrlForApi_MailAuth");
    cConfig.sConnectionString = MyConfig.GetValue<string>("Config:ConnectionString");
    cConfig.sConnectionStringForLoggerX = MyConfig.GetValue<string>("Config:ConnectionStringForLoggerX");
    cConfig.sConnectionStringForKurumsal = MyConfig.GetValue<string>("Config:ConnectionStringForKurumsal");
    cConfig.sConnectionStringForIdari = MyConfig.GetValue<string>("Config:ConnectionStringForIdari");
    cConfig.sDosyaYoluDilekce = MyConfig.GetValue<string>("Config:DosyaYoluDilekce");
    cConfig.sDosyaYoluTarim = MyConfig.GetValue<string>("Config:DosyaYoluTarim");
    cConfig.sShared_Dir = MyConfig.GetValue<string>("Config:Shared_Dir");
    cConfig.sMobilPath = MyConfig.GetValue<string>("Config:MobilPath");
    cConfig.sArsivPath = MyConfig.GetValue<string>("Config:ArsivPath");
    
    cConfig.sWinLoginUser = MyConfig.GetValue<string>("Config:WinLoginUser");
    cConfig.sWinLoginPass = MyConfig.GetValue<string>("Config:WinLoginPass");
    cConfig.sEnvProdOrTest = MyConfig.GetValue<string>("Config:EnvProdOrTest");
}
    
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 

builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(opts =>
//{
//    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
//    // opts.JsonSerializerOptions.Converters.Add(new NullToEmptyStringConverter());
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "JWTToken_Auth_API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

Action<cConfig> oConfig = (opt =>
{
    opt.sGrant_type = MyConfig.GetValue<string>("Config:sGrant_type");
    opt.sClient_id = MyConfig.GetValue<string>("Config:sClient_id");
    opt.sClient_secret = MyConfig.GetValue<string>("Config:sClient_secret");
    opt.sScope = MyConfig.GetValue<string>("Config:sScope");

    opt.sToken = MyConfig.GetValue<string>("Config:sToken");
    opt.sUrlGetUsers = MyConfig.GetValue<string>("Config:sUrlGetUsers");
    opt.sUrlGetUser = MyConfig.GetValue<string>("Config:sUrlGetUser");        
});

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
object value = builder.Logging.AddSerilog(logger);

builder.Services.Configure(oConfig);
// Add services to the container.
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<cConfig>>().Value);


builder.Services.AddOptions();
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
    {
        var key = MyConfig.GetValue<string>("JwtConfig:Key");
        byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        jwtOptions.SaveToken = true;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),            
            ValidateLifetime = true,            
            ValidateAudience = false,            
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero,
        };
    });
builder.Services.AddSingleton(typeof(IJwtTokenManager), typeof(JwtTokenManager));

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<CreateSession>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "media")),
//    RequestPath = "/media"
//});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
