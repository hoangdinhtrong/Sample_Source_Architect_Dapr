using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Application.Shared.Features;

namespace SampeDapr.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> Get()
        {
            var response = await _mediator.Send(new GetAllWeatherForecastRequest());
            return Ok(response);
        }
    }
}