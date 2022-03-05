namespace AccelerexTask.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
};

