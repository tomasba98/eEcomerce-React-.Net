using eEcomerce.BackEnd.Data;
using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Services.Authentication.IAuthenticationService;
using eEcomerce.BackEnd.Services.Authentication.Implementation.AuthenticationService;
using eEcomerce.BackEnd.Services.Category;
using eEcomerce.BackEnd.Services.Category.Implementation;
using eEcomerce.BackEnd.Services.DataAccesLayer.Implementation.GenericService;
using eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;
using eEcomerce.BackEnd.Services.Order;
using eEcomerce.BackEnd.Services.Order.Implementation;
using eEcomerce.BackEnd.Services.OrderProduct;
using eEcomerce.BackEnd.Services.OrderProduct.Implementation;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Product.Implementation;
using eEcomerce.BackEnd.Services.Users;
using eEcomerce.BackEnd.Services.Users.Implementation;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add DB connection string
        string? connectionString = builder.Configuration.GetConnectionString("DbConnection");

        //// Register services for product-related operations.        
        builder.Services.AddScoped<IGenericService<Product>, GenericService<Product>>();
        builder.Services.AddScoped<IProductService, ProductService>();

        //// Register services for order-related operations.        
        builder.Services.AddScoped<IGenericService<Order>, GenericService<Order>>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        //// Register services for OrderProduct-related operations.        
        builder.Services.AddScoped<IGenericService<OrderProduct>, GenericService<OrderProduct>>();
        builder.Services.AddScoped<IOrderProductService, OrderProductService>();

        //// Register services for category-related operations.        
        builder.Services.AddScoped<IGenericService<Category>, GenericService<Category>>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();

        //// Register services for user-related operations.
        builder.Services.AddScoped<IGenericService<User>, GenericService<User>>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserService, UserService>();

        // Configure the database context with PostgreSQL.
        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // Add controllers, API explorer, Swagger, and HttpContextAccessor.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        // Add CORS for cross-origin resource sharing.
        builder.Services.AddCors();

        // Configure JWT-based authentication.
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Bearer";
        }).AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "ToDoList",
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!£@0#y~9I1.p0goq1£1+12345678901234567890123456789012"))
            };
        });

        // Configure Swagger documentation.
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = builder.Configuration["Swagger:Title"],
                Version = builder.Configuration["Swagger:Version"],
            });

            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

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
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
        });

        WebApplication app = builder.Build();

        // Enable CORS for specified origins.
        app.UseCors(policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });

        // Configure middleware components.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}