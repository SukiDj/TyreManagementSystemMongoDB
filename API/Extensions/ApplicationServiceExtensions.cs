using Application.Actions;
using Application.Core;
using Application.Interfaces;
using Application.Productions;
using Application.Sales;
using FluentValidation;
using FluentValidation.AspNetCore;
using Persistence;
using Infrastructure.Security;

// NEW: Mongo
using MongoDB.Driver;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                });
            });

            services.Configure<MongoOptions>(config.GetSection("Mongo"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoOptions>>().Value;
                var settings = MongoClientSettings.FromConnectionString(opts.ConnectionString);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                return new MongoClient(settings);
            });

            services.AddSingleton<MongoDbContext>();

            // MediatR / AutoMapper / Validation
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Tyres.List.Handler).Assembly));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterProduction>();
            services.AddValidatorsFromAssemblyContaining<RegisterTyreSale>();

            // Helpers
            services.AddHttpContextAccessor();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<ActionLogger>();

            return services;
        }
    }
}
