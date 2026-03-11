using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MeseroController : ControllerBase
{
    private readonly IMeseroService _meseroService;

    public MeseroController(IMeseroService meseroService)
    {
        _meseroService = meseroService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _meseroService.GetAllAsync());
}