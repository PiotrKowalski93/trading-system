using Microsoft.AspNetCore.Mvc;
using OrderGateway.ApiRest.Redis;
using OrderGateway.Core.Broker;

namespace OrderGateway.ApiRest.Brokers
{
    [ApiController]
    [Route("api/brokerRules")]
    public class BrokerRulesController : Controller
    {
        private IBrokerRulesRedisWriter _rulesRedisWriter;

        public BrokerRulesController(IBrokerRulesRedisWriter rulesRedisWriter)
        {
            _rulesRedisWriter = rulesRedisWriter;
        }

        [HttpPost("{brokerId}")]
        public async Task<IActionResult> SetRules(string brokerId, [FromBody] BrokerRulesRequest request)
        {
            var rules = new BrokerRules(
                brokerId,
                request.AllowedInstruments,
                request.AllowMarketOrders,
                request.MaxQuantity,
                request.TradingStart,
                request.TradingEnd
            );

            await _rulesRedisWriter.SaveAsync(rules);
            return Ok();
        }
    }
}
