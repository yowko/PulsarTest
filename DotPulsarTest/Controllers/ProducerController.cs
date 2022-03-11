using DotPulsar;
using DotPulsar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace PulsarTest.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;
    private readonly IProducer<string> _pulsarProducer;

    public ProducerController(ILogger<ProducerController> logger,IProducer<string> pulsarProducer)
    {
        _logger = logger;
        _pulsarProducer = pulsarProducer;
    }

    [HttpGet(Name = "Publish")]
    public async Task<MessageId> Publish()
    {
        var data = "Hello World from DotPulsar";
        return await _pulsarProducer.Send(new MessageMetadata(),data);
    }
}