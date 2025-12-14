using Microsoft.AspNetCore.Mvc;
using OrderGateway.ApiRest.Redis;
using OrderGateway.Core.Instruments;

namespace OrderGateway.ApiRest.Instruments
{
    [ApiController]
    [Route("api/instruments")]
    public class InstrumentsController : Controller
    {
        private IInstrumentMetadataRedisWriter _instrumentMetadataRedisWriter;

        public InstrumentsController(IInstrumentMetadataRedisWriter instrumentMetadataRedisWriter)
        {
            _instrumentMetadataRedisWriter = instrumentMetadataRedisWriter;
        }

        [HttpPost]
        public async Task<IActionResult> AddInstrment(AddInstrumentRequest request)
        {
            var metadata = new InstrumentMetadata(
                request.Symbol,
                request.TickSize,
                request.MinPrice,
                request.MaxPrice,
                request.MinQuantity,
                request.MaxQuantity,
                request.MaxDeviationPercent);

            await _instrumentMetadataRedisWriter.SaveAsync(metadata);
            return Ok();
        }
    }
}
