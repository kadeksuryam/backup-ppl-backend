using App.Data;
using App.Helpers;
using App.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
{
    var services = builder.Services;
    // setup prod/dev DB

    services.AddCors();
    services.AddControllers();

    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
    services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
    services.AddScoped<UserRepository>();

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cakrawala Id API", Version = "v1" });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}


{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

    app.MapControllers();

}

app.Run();
