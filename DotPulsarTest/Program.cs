using DotPulsar;
using DotPulsar.Extensions;
using PulsarTest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddHostedService<ConsumerBuilderService>();
//builder.Services.AddHostedService<ConsumerWithoutBuilderService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var pulsarClient = PulsarClient
    .Builder()
    .ServiceUrl(new Uri("pulsar://localhost:6650"))
    .RetryInterval(TimeSpan.FromSeconds(3))
    .Build();

builder.Services.AddSingleton(pulsarClient);

builder.Services.AddSingleton(_ =>pulsarClient.NewProducer(Schema.String)
    .Topic("persistent://public/default/yowkotest")
    .Create());

//builder.Services.AddHostedService<ReaderBuilderService>();
builder.Services.AddHostedService<ReaderWithoutBuilderService>();
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