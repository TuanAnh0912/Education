using Education.Core.Database;
using Education.Core.Interface;
using Education.Core.Model;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;
using Education.Core;
using Education.Core.Model.Core;
using Education.Core.Repositories;
using Education.Application.Service;
using Education.Application.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors();
// Add services to the container.
services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
services.AddSingleton(typeof(IDbContext<>), typeof(MySQLContext<>));
services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
