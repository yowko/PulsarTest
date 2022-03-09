using Microsoft.AspNetCore.Mvc;
using Pulsar.Client.Api;
using Pulsar.Client.Common;

namespace Pulsar.ClientTest.Controllers;

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
    public async Task<long> Publish()
    {
        var result=await _pulsarProducer.SendAsync("Hello World");

        return result.LedgerId;
    }
}