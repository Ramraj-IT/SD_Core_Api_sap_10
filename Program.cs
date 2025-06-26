using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using SD_Core_Api.Interfaces;
using SD_Core_Api.Jobs;
using SD_Core_Api.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddScoped<ISapPosting, SapPostingRepo>();

var app = builder.Build();
 
app.UseSwagger();
app.UseSwaggerUI();
 app.UseAuthorization();

app.MapControllers();
 
app.Run();
