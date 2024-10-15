using System.Text.Json;
using DiscordBot.Worker.Interfaces.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DiscordBot.Worker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AppController : ControllerBase, IAppController
{
    [HttpGet("hello")]
    public IActionResult Hello()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(new { message = "Hello World!" }));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}