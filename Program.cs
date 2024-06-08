using System.Text;
using english_learning_server.Data;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Grwm.Module;
using LangChain.Providers.Google;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("MySqlConn");

builder.Services.AddDbContext<EnglishLearningDbContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0)),
    mySqlOptions => 
    {
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<EnglishLearningDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"])
                )
        };
    });

builder.Services.AddSingleton<GoogleProvider>();

builder.Services.AddControllers();

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((container) =>
    {
        container.RegisterModule<RepositoryModule>();
        container.RegisterModule<ServiceModule>();
    });

var googleCloudCredentialsPath = builder.Configuration["GoogleCloud:Credentials"];

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(Directory.GetCurrentDirectory(), googleCloudCredentialsPath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
