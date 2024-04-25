using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.Repositories;
using cia_aerea_api.ViewModels.Generics;
using Microsoft.AspNetCore.Mvc;

namespace cia_aerea_api.Controllers;

[Route("api/airplanes")]
[ApiController]
public class AirplaneController : ControllerBase
{
    private readonly AirplaneRepository _airplaneRepository;

    public AirplaneController(AirplaneRepository airplaneRepository)
    {
        _airplaneRepository = airplaneRepository;
    }
    
    [HttpGet]
    public  async Task<ActionResult<ResponseViewModel<List<ListAirplaneViewModel>>>> GetAllAirplanesAsync()
    {
        var airplanes =  await _airplaneRepository.GetAllAsync();
        var listAirplaneViewModel = airplanes.Select(
            a => new ListAirplaneViewModel(
                a.Id,
                a.Manufacturer,
                a.Model,
                a.Prefix
            )).ToList();
        var response = new ResponseViewModel<List<ListAirplaneViewModel>>(
            200,
            "OK",
            listAirplaneViewModel
        );
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public  async Task<ActionResult<ResponseViewModel<DetailAirplaneViewModel>>> GetAirplaneByIdAsync(int id)
    {
        var airplane =  await _airplaneRepository.GetByIdAsync(id);
        if (airplane is null)
        {
            var notFound =   new ResponseViewModel<string>(
                404, 
                "Airplane is not registered.",
                null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailAirplaneViewModel>(
            200,
            "OK",
            airplane
        );
        return Ok(response);
    }

    [HttpPost]
    public  async Task<ActionResult<ResponseViewModel<DetailAirplaneViewModel>>> AddAirplaneAsync( AddAirplaneViewModel airplaneData)
    {
        var airplane =  await _airplaneRepository.AddAsync(airplaneData);
        var response = new ResponseViewModel<DetailAirplaneViewModel>(
            201, "CREATED", airplane
        );
        return Ok(response);
    }
    
    [HttpPut]
    public  async Task<ActionResult<ResponseViewModel<DetailAirplaneViewModel>>> UpdateAirplaneAsync( UpdateAirplaneViwModel airplaneData)
    {
        var airplane =  await _airplaneRepository.UpdateAsync(airplaneData);
        if (airplane is null)
        {
            var notFound = new ResponseViewModel<string>(
                404, "Airplane is not registered", null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailAirplaneViewModel>(
            200, "OK", airplane
        );
        return Ok(response);
    }
    
    [HttpDelete]
    public  async Task<ActionResult<ResponseViewModel<string>>> DeleteirplaneAsync( int id)
    {
        if (await _airplaneRepository.DeleteAsync(id))
        {
            var response = new ResponseViewModel<string>(
                200, "DELETED", null
            );
            return Ok(response);
        }

        var notFound = new ResponseViewModel<string>(
            404, "Airplane is not registered", null
        );

        return NotFound(notFound);
    }
}