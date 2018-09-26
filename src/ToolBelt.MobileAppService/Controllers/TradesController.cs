using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolBelt.Data;
using ToolBelt.MobileAppService.Services;

namespace ToolBelt.MobileAppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ToolBeltContext _context;

        public TradesController(ToolBeltContext context)
        {
            _context = context;
        }

        // GET: api/Trades
        [HttpGet]
        public IEnumerable<Trade> GetTrades()
        {
            return _context.Trades;
        }

        // GET: api/Trades/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrade([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
            {
                return NotFound();
            }

            return Ok(trade);
        }
    }
}