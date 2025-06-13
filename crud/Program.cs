// For .NET 6+ (Program.cs)
using crud.Domain; // Assuming your Product model is in YourApp.Domain
using crud.Infrastructure; // Assuming your repository is in YourApp.Infrastructure

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure connection string
var connectionString = builder.Configuration.GetConnectionString("RmaDatabase");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'RmaDatabase' not found.");
}

// Register Dapper repository with dependency injection
builder.Services.AddSingleton<IProductRepository>(new ProductRepository(connectionString));

// Add CORS policy to allow your React frontend to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://localhost:5173", "https://localhost:3000", "https://localhost:5173") // Replace with your React app's actual URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();