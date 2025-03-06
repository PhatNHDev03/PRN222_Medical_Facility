using MedicaiFacility.DataAccess;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service
{
    public static class DenpendenceInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IHealthRecordRepository,HealthRecordRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IMedicalFacilityRepository, MedicalFacilityRepository>();
            services.AddScoped<IFacilityDepartmentRepository, FacilityDepartmentRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHealthRecordService, HealhRecordService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDiseaseService, DiseaseService>();
            services.AddScoped<IMedicalFacilityService, MedicalFacilityService>();
            services.AddScoped<IFacilityDepartmentService, FacilityDepartmentService>();
            return services;
        }

        public static IServiceCollection AddDatabaseAndConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }
    }
}
