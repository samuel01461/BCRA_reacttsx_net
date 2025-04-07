using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using api_bcra.Context;
using api_bcra.Repositories;
using api_bcra.Repositories.interfaces;
using api_bcra.Request;
using api_bcra.Request.interfaces;
using api_bcra.Services;
using api_bcra.Services.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BCRADbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Production"));
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var jwt_config = builder.Configuration.GetSection("JWT_Config");
var jwt_issuer = jwt_config.GetValue<string>("Issuer");
var jwt_audience = jwt_config.GetValue<string>("Audience");
var jwt_signingkey = jwt_config.GetValue<string>("SigningKey");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateActor = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt_issuer,
        ValidAudience = jwt_audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_signingkey))
    };
});

builder.Services.AddCors(cors =>
{
    cors.AddPolicy("CorsApi", policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddScoped<IScoringRepository, ScoringRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepositry>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, GenerateTokenService>();
builder.Services.AddScoped<IMainService, MainService>();
//builder.Services.AddScoped<IBCRAClient, BCRAClient>();

var bcra_cfg = builder.Configuration.GetSection("API_Config");
var bcra_url = bcra_cfg.GetValue<string>("URL");

builder.Services.AddHttpClient<IBCRAClient, BCRAClient>(client =>
{
    client.BaseAddress = new Uri(bcra_url);
    client.DefaultRequestHeaders.Add("accept", "application/json");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsApi");

app.MapControllers();

app.Run();
