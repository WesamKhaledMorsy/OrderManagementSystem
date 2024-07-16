using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderManagementSystem.BL.EntityService.CustomerService;
using OrderManagementSystem.BL.EntityService.EmailService;
using OrderManagementSystem.BL.EntityService.InvoiceService;
using OrderManagementSystem.BL.EntityService.OrderItemService;
using OrderManagementSystem.BL.EntityService.OrderService;
using OrderManagementSystem.BL.EntityService.ProductService;
using OrderManagementSystem.BL.EntityService.UserService;
using OrderManagementSystem.BL.Repository;
using OrderManagementSystem.BL.UnitOfWork;
using OrderManagementSystem.Constants;
using OrderManagementSystem.DL;
using OrderManagementSystem.DL.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.BL.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<Constants>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped< InvoiceService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<Constants,Constants>();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

// Add authentication and JWT configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddSingleton(new MailHelper(
    builder.Configuration["SmtpSettings:Server"],
    int.Parse(builder.Configuration["SmtpSettings:Port"]),
    builder.Configuration["SmtpSettings:Username"],
    builder.Configuration["SmtpSettings:Password"]
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management API v1");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Seed data method
async Task SeedData(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    // Ensure roles exist
    var roles = new[] { "Admin", "Customer" };
    //foreach (var role in roles)
    //{
    //    if (!await roleManager.RoleExistsAsync(role))
    //    {
    //        await roleManager.CreateAsync(new IdentityRole<int>(role));
    //    }
    //}

    // Ensure admin user exists
    var adminUser = await userManager.FindByNameAsync("wesammorsy");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "wesammorsy",
            Email = "admin@gmail.com"
            //Role = "admin"
        };

        var result = await userManager.CreateAsync(adminUser, "Wesam@123");

        //if (result.Succeeded)
        //{
        //    await userManager.AddToRoleAsync(adminUser, "Admin");
        //}
        //else
        //{
            // Handle errors if user creation fails
            //foreach (var error in result.Errors)
            //{
            //    // Log or display the error message
            //    Console.WriteLine(error.Description);
            //}
        //}
    }
}

// Ensure seeding happens
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData(services);
}

app.Run();
