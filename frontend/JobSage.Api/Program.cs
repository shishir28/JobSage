using JobSage.Application;
using JobSage.Application.Mappings;
using JobSage.Infrastructure;
using JobSage.Infrastructure.Persistence;
using JobSage.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<JobSageDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
        );

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        builder.Services.AddControllers();


        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Job Sage API",
                Description = "An API for Job Sage, a job management system",
            });
        });
        builder.Services.AddApplicationDependencies();
        builder.Services.AddInfrastructureDependencies();
        builder.Services.AddHttpClientServices(builder.Configuration);

        MappingConfig.RegisterMappings();

        var app = builder.Build();
        app.UseCors("AllowAllOrigins");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "JobSage API v1");
            options.RoutePrefix = "";  // This makes Swagger UI available at the root
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobSage API v1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        });

        // SampleDataGenerator.GenerateSampleData();
        // Seed sample data
        // using (var scope = app.Services.CreateScope())
        // {
        //     var dbContext = scope.ServiceProvider.GetRequiredService<JobSageDbContext>();
        //     var seeder = new SampleDataSeeder(dbContext);

        //     // Adjust paths as needed
        //     var contractorsPath = Path.Combine(app.Environment.ContentRootPath, "Seeders", "contractors.json");
        //     var propertiesPath = Path.Combine(app.Environment.ContentRootPath, "Seeders", "properties.json");
        //     var jobsPath = Path.Combine(app.Environment.ContentRootPath, "Seeders", "jobs.json");
        //     var schedulingPath = Path.Combine(app.Environment.ContentRootPath, "Seeders", "schedulinginfo.json");

        //     // Seed data synchronously at startup
        //     seeder.SeedAsync(contractorsPath, propertiesPath, jobsPath, schedulingPath).GetAwaiter().GetResult();
        // }

        app.MapControllers();
        app.Run();
    }
}