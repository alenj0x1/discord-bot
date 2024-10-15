using Microsoft.AspNetCore.Mvc;

namespace DiscordBot.Worker.Interfaces.Controllers;

public interface IAppController
{
    IActionResult Hello();
}