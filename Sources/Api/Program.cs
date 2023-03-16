using Model;
using StubLib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();s
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDataManager, StubData>(); //un seul pour tout le monde //il y a des semaphore et de concurrence d acces

builder.Services.AddControllers().AddControllersAsServices(); // Add controllers as services

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*else
{
    app.UseSwagger();


    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/containers/ramikhedair-entityframework_api_lol/swagger", "Api V1");
        c.RoutePrefix = "/containers/ramikhedair-entityframework_api_lol/swagger";
        c.ConfigObject.AdditionalItems.Add("url", "/containers/ramikhedair-entityframework_api_lol/swagger");
    });
}*/

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
