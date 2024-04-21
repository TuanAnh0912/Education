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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using Education.API.Middleware.TokenHandle;
using Amazon.S3;
using Education.Core.Model.AWS_S3;
using Education.Application.Service.Base;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
// Add services to the container.
var services = builder.Services;

services.AddControllers();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration.GetSection("JwtIssuerOptions:Issuer").Value,
               ValidAudience = builder.Configuration.GetSection("JwtIssuerOptions:Audience").Value,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtIssuerOptions:SecretKey").Value))
           };
       });
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
services.Configure<JwtIssuerOptions>(builder.Configuration.GetSection("JwtIssuerOptions"));
services.Configure<AwsCredentials>(builder.Configuration.GetSection("AwsCredentials"));

services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
services.AddSingleton(typeof(IDbContext<>), typeof(MySQLContext<>));
services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
services.AddTransient<IAuthenService, AuthenService>();
services.AddTransient<IJwtUtils, JwtUtils>();
services.AddTransient<IUserRepository, UserRepository>();
services.AddTransient<IUserService, UsersService>();
services.AddTransient<IClaimProvider, ClaimProvider>();
services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
services.AddTransient<IAnalysisContentService, AnalysisContentService>();
services.AddTransient<IAnalysisHeaderService, AnalysisHeaderService>();
services.AddTransient<IAnalysisContentRepository, AnalysisContentRepository>();
services.AddTransient<IAnalysisHeaderRepository, AnalysisHeaderRepository>();
services.AddTransient<IUserPermissionRepository, UserPermissionRepository>();
services.AddTransient<IUserPermissionService, UserPermissionService>();
services.AddTransient<ICheckExamsService, CheckExamsService>();
services.AddTransient<IExamTestRepository, ExamTestRepository>();
services.AddTransient<IExamTestService, ExamTestService>();
services.AddTransient<IQuestionRepository, QuestionRepository>();
services.AddTransient<IQuestionService, QuestionService>();
services.AddTransient<IStudentExamsService, StudentExamsService>();
services.AddTransient<IStudentExamsRepository, StudentExammsRepository>();
services.AddTransient<IRoleUserRepository, RoleUserRepository>();
services.AddTransient<IExamGeneralRepository, ExamGeneralRepository>();
services.AddTransient<IBlockRepository, BlockRepository>();
services.AddTransient<IExamGeneralService, ExamGeneralService>();
services.AddTransient<IBlockService, BlockService>();
builder.Services.AddAWSService<IAmazonS3>();


services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

    // global error handler
    app.UseMiddleware<AuthHandleMiddleware>();

    app.MapControllers();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
