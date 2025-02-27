using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrastructure.Data;
using ECom.Infrastructure.Repositories;
using ECom.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Apply unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            //Apply dbcontext
            services.AddDbContext<AppDbContext>(op =>
                {
                    op.UseSqlServer(configuration.GetConnectionString("EComDatabase"));
                });
            return services;
        }
    }
}
