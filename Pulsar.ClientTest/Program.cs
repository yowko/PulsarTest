using System.Text;
using Pulsar.Client.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string serviceUrl = "pulsar://localhost:6650";
const string topicName = "yowkotest2";

var client = await new PulsarClientBuilder()
    .ServiceUrl(serviceUrl)
    .BuildAsync();


builder.Services.AddSingleton( _ =>  client
    .NewProducer(Schema.STRING(Encoding.UTF8))
    .Topic(topicName)
    .CreateAsync().Result);

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