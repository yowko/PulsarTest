using DotPulsar;
using DotPulsar.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var pulsarClient = PulsarClient
    .Builder()
    .ServiceUrl(new Uri("pulsar://localhost:6650"))
    .RetryInterval(TimeSpan.FromSeconds(3))
    .Build();

builder.Services.AddSingleton(_ =>pulsarClient.NewProducer(Schema.String)
    .Topic("persistent://public/default/yowkotest")
    .Create());

// builder.Services.AddSingleton(_ =>
// {
//     var options = new ProducerOptions<string>("persistent://public/default/yowkotest",Schema.String);
//     return pulsarClient.CreateProducer(options);
// });

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