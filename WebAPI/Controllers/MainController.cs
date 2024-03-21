using Application.PriceItems.Commands.LoadPriceListFromEmailToDb;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> LoadPriceListFromEmailToDb(ISender sender, LoadPriceListFromEmailToDbCommand command)
        {
            return Ok(await sender.Send(command));
        }
    }
}
