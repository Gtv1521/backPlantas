using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlantasBackend.Collections;
using PlantasBackend.Interfaces;
using PlantasBackend.Models;
using PlantasBackend.Models.Responses;
using PlantasBackend.Models.settings;
using PlantasBackend.Repositories;
using PlantasBackend.Services;
using PlantasBackend.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add settings of database to application
builder.Services.Configure<ContextModel>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<CloudinaryModel>(builder.Configuration.GetSection("Cloudinary"));

// Add services to the container.
builder.Services.AddControllers();

// declared of Services and Collections
builder.Services.AddScoped<Context>();

builder.Services.AddScoped<IDataCrud<PlantsModel>, PlantsCollection>();
builder.Services.AddScoped<IOneData<PlantsModel>, PlantsCollection>();
builder.Services.AddScoped<PlantsService>();

builder.Services.AddScoped<IDataCrud<DiseasesModel>, DiseasesCollection>();
builder.Services.AddScoped<IOneData<DiseasesModel>, DiseasesCollection>();
builder.Services.AddScoped<DiseasesService>();

builder.Services.AddScoped<upImage>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Plantas API",
        Version = "v1",
        Description = "Esta API REST permite la creación, lectura, actualización y eliminación de notas para una aplicación de gestión de notas. Está diseñada para proporcionar una manera sencilla y eficiente de que los usuarios gestionen sus notas personales, con soporte para la organización de las mismas por categorías o etiquetas, y para la sincronización entre diferentes dispositivos.",
        // TermsOfService = new Uri(""),
        Contact = new OpenApiContact
        {
            Name = "Gustavo Bernal",
            Url = new Uri("https://folio-three-inky.vercel.app/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en el formato: Bearer {token} para poder acceder a las rutas que requerien autenticacion ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Se agrega configuracion del acess token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    // varify if token valid
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Evitar el manejo predeterminado
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new ResultData { StatusCode = 401, Message = "No autorizado. Token invalido o expirado." };
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    };
});

builder.Services.AddAuthorization();
// Agrega policía cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo",
    builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTodo");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
