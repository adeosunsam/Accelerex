namespace AccelerexTask.Controllers;

public class OpenHourController : ApiController
{
    [HttpPost]
    public async Task<ActionResult<Dictionary<string, string>>> FormatOpenHour(OpenHourQuery.Command command)
    {
       var a = await Mediator.Send(command);
        return Ok(a);
    }
}
