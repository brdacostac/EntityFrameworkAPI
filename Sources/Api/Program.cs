using Model;
using StubLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDataManager, StubData>(); //un seul pour tout le monde //il y a des semaphore et de concurrence d acces
/*builder.Services.AddScoped<IDataManager, StubData>();//un seul pour le cycle de vie de l'environnement //c est le meuilleur si on sait pas
builder.Services.AddTransient<IDataManager, StubData>();//un seul pour chque environnment demainder un nouveua instance*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
