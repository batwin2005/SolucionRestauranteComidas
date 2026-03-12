using Microsoft.AspNetCore.Mvc;
using ProyectoData;

public class MeseroController : ControllerBase
{
    private readonly MeseroData _meseroData;

    public MeseroController(MeseroData meseroData)
    {
        _meseroData = meseroData;
    }

   
}