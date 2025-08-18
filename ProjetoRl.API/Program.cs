using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using NLog.Web;
using FluentValidation.AspNetCore;
using ProjetoRl.ProjetoRl.Domain.Rentals;
using ProjetoRl.ProjetoRl.Domain.Motorcycles;
using ProjetoRl.ProjetoRl.Domain.Users;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Rentals;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Users;
using MongoDB.Driver;
using ProjetoRl.Data.Repositories.MongoDB.Implementation.Users;
using ProjetoRl.Data.Repositories.MongoDB.Implementation.Rentals;
using ProjetoRl.Data.Repositories.MongoDB.Implementation.Couriers;
using ProjetoRl.Data.Repositories.MongoDB.Implementation.Bikes;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.Motorcycles;
using ProjetoRl.ProjetoRl.API.Config;
using ProjetoRl.ProjetoRl.Domain.AccessTokens;
using ProjetoRl.Infra.Data.Repositories.MongoDB.Implementation.AccessTokens;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// ========================= CORS =========================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

// ========================= Controllers + FluentValidation =========================

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<IRentalRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<IBikeRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<ICourierRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<IUserRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<IAccessTokenRepository>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// ========================= Logging =========================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseNLog();

// ========================= Swagger =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ProjetoRl.Security API",
        Description = "Portal ProjetoRl security microservice API."
    });

    c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
    c.TagActionsBy(api => new[] { api.GroupName });
    c.CustomSchemaIds(x => x.FullName);

    // JWT no Swagger
    var bearer = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header using Bearer. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };
    c.AddSecurityDefinition("Bearer", bearer);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // XML docs (precisa estar habilitado no .csproj)
    var docPath = Path.Combine(AppContext.BaseDirectory, "ProjetoRl.API.xml");
    c.IncludeXmlComments(docPath, true);
});

// ========================= JWT Authentication =========================
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false; // alterar para true em produção
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthConfig.JWTKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ========================= MongoDB Config =========================
var conString = builder.Configuration.GetValue<string>("MongoDBConnectionString:ConnectionString")
    ?? throw new KeyNotFoundException("The connection string parameter was not found.");

var database = builder.Configuration.GetValue<string>("MongoDBConnectionString:Database")
    ?? throw new KeyNotFoundException("The database parameter was not found.");

BsonClassMap.RegisterClassMap<BikeScheme>(map =>
{
    map.AutoMap();
    map.MapIdField(u => u.ID)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
    map.SetIgnoreExtraElements(true);
});

BsonClassMap.RegisterClassMap<RentalScheme>(map =>
{
    map.AutoMap();
    map.MapIdField(a => a.ID)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
    map.SetIgnoreExtraElements(true);
});

BsonClassMap.RegisterClassMap<CourierScheme>(map =>
{
    map.AutoMap();
    map.MapIdField(v => v.ID)
        .SetIdGenerator(StringObjectIdGenerator.Instance);
    map.SetIgnoreExtraElements(true);
});

BsonClassMap.RegisterClassMap<UserSchema>(map =>
{
    map.AutoMap();
    map.MapIdMember(c => c.ID)
        .SetIdGenerator(StringObjectIdGenerator.Instance)
        .SetSerializer(new StringSerializer(BsonType.ObjectId));
    map.MapMember(u => u.Email)
        .SetIgnoreIfNull(true)
        .SetElementName("email")
        .SetIsRequired(false);
    map.SetIgnoreExtraElements(true);
});

// ========================= DI =========================
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(conString));

builder.Services.AddScoped<BikeContext>();
builder.Services.AddScoped<RentalContext>();
builder.Services.AddScoped<CourierContext>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<AccessTokenContext>();

builder.Services.AddScoped<IUserRepository, UserRepositoryMongoDB>();
builder.Services.AddScoped<IRentalRepository, RentalRepositoryMongoDB>();
builder.Services.AddScoped<ICourierRepository, CourierRepositoryMongoDB>();
builder.Services.AddScoped<IBikeRepository, BikeRepositoryMongoDB>();
builder.Services.AddScoped<IAccessTokenRepository, AccessTokenRepositoryMongoDB>();

builder.Services.AddHttpClient();

builder.Services.AddFluentValidationRulesToSwagger();

// ========================= App =========================
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Environment: {env}", app.Environment.EnvironmentName);


app.Run();
