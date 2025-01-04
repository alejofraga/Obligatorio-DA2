using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;
using SmartHome.DataLayer;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<CustomExceptionAttribute>();
        });

        builder.Services.AddDbContext<SmartHomeDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SmartHomeDb"),
                b => b.MigrationsAssembly("SmartHome.WebApi")));

        builder.Services.AddScoped<DbContext>(provider => provider.GetService<SmartHomeDbContext>()!);
        builder.Services.AddScoped<IDeviceImporter, DeviceImporter>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IModelValidator, ModelValidator>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<IRepository<DeviceType>, Repository<DeviceType>>();
        builder.Services.AddScoped<IDeviceTypesService, DeviceTypesService>();
        builder.Services.AddScoped<IRepository<Camera>, Repository<Camera>>();
        builder.Services.AddScoped<IRepository<LampHardware>, Repository<LampHardware>>();
        builder.Services.AddScoped<IRepository<SensorHardware>, Repository<SensorHardware>>();
        builder.Services.AddScoped<IHomeRepository, HomeRepository>();
        builder.Services.AddScoped<IRepository<Coordinates>, Repository<Coordinates>>();
        builder.Services.AddScoped<IRepository<Location>, Repository<Location>>();
        builder.Services.AddScoped<IHardwareRepository, HardwareRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IRepository<Device>, Repository<Device>>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        builder.Services.AddScoped<IHomeService, HomeService>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<ISessionService, SessionService>();

        var app = builder.Build();

        app.UseCors(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyHeader().AllowAnyOrigin();
            options.AllowAnyMethod();
        });

        // app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
