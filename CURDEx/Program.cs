using CURDEx.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<CompanyContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("scon")));

builder.Services.AddEndpointsApiExplorer();

var xmlFile =$"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath =
Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "CURD API used to demo!!!",
        Description = "implemented curd operations using asp.net core api ef core and linq",
        Contact = new OpenApiContact()
        {
            Name = "Sunil",
            Email = "sunil@hotmail.com",
            Url = new Uri("https://www.ritechpune.com")
        }
    });
    opt.IncludeXmlComments(xmlPath);
}).AddSwaggerGenNewtonsoftSupport();  //activate

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
