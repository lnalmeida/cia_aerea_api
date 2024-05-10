using cia_aerea_api.ViewModels.Airplane;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Airplanes;
using cia_aerea_api.Validators.Services;
using cia_aerea_api.ViewModels.Generics;
using Microsoft.AspNetCore.Mvc;

namespace cia_aerea_api.Controllers;

[Route("api/airplanes")]
[ApiController]
public class AirplaneController : ControllerBase
{
    private readonly AirplaneRepository _airplaneRepository;
    private readonly ValidationService _validationService;
    private readonly AddAirplaneValidator _addAirplaneValidator;
    private readonly UpdateAirplaneValidator _updateAirplaneValidator;
    private readonly DeleteAirplaneValidator _deleteAirplaneValidator;

    public AirplaneController(AirplaneRepository airplaneRepository, ValidationService validationService, AddAirplaneValidator addAirplaneValidator, UpdateAirplaneValidator updateAirplaneValidator, DeleteAirplaneValidator deleteAirplaneValidator)
    {
        _airplaneRepository = airplaneRepository;
        _validationService = validationService;
        _addAirplaneValidator = addAirplaneValidator;
        _updateAirplaneValidator = updateAirplaneValidator;
        _deleteAirplaneValidator = deleteAirplaneValidator;
    }
    
    [HttpGet]
    public  async Task<ActionResult<IEnumerable<ResponseViewModel<ListAirplaneViewModel>>>> GetAllAirplanes()
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
    public  async Task<ActionResult<ResponseViewModel<DetailAirplaneViewModel>>> GetAirplaneById(int id)
    {
        var airplane =  await _airplaneRepository.GetByIdAsync(id);
        if (airplane is null)
        {
            var notFound =   new ResponseViewModel<string>(
                404, 
                "Aeronave não registrada.",
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  async Task<ActionResult<ResponseViewModel<object>>> AddAirplane( AddAirplaneViewModel airplaneData)
    {
        if (airplaneData is null)
        {
            var nullModelResponse = new ResponseViewModel<object>(400, "Todos os dados da aeronavem devem ser informados", null);
            return BadRequest(nullModelResponse);
        }
        var validationResult = await _validationService.ValidateModel(airplaneData, _addAirplaneValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }

        var airplane =  await _airplaneRepository.AddAsync(airplaneData);
        var response = new ResponseViewModel<DetailAirplaneViewModel>(
            201, "CREATED", airplane
        );
        return CreatedAtAction("AddAirplane", response);
    }
    
    [HttpPut]
    public  async Task<ActionResult<ResponseViewModel<DetailAirplaneViewModel>>> UpdateAirplane( UpdateAirplaneViewModel airplaneData)
    {
        if (airplaneData is null)
        {
            var nullResponse = new ResponseViewModel<string>(
                404, "Os dados da aeronave devem ser informados.", null
            );
            return NotFound(nullResponse);
        }

        var existsAirplane = await _airplaneRepository.GetByIdAsync(airplaneData.Id);
        var validationResult = await _validationService.ValidateModel(airplaneData, _updateAirplaneValidator); 
        if (!validationResult.IsValid && existsAirplane != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        
        var airplane =  await _airplaneRepository.UpdateAsync(airplaneData);
        
        if (airplane is null)
        {
            var notFound = new ResponseViewModel<string>(
                404, "Aeronave não registrada", null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailAirplaneViewModel>(
            200, "OK", airplane
        );
        return Ok(response);
    }
    
    [HttpDelete]
    public  async Task<ActionResult<ResponseViewModel<string>>> DeleteAirplane( int id)
    {
        var existsAirplane = await _airplaneRepository.GetByIdAsync(id);
        var validationResult = await _validationService.ValidateModel(id, _deleteAirplaneValidator);
        if (!validationResult.IsValid && existsAirplane != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        if (await _airplaneRepository.DeleteAsync(id))
        {
            var response = new ResponseViewModel<string>(
                200, "DELETED", null
            );
            return Ok(response);
        }

        var notFound = new ResponseViewModel<string>(
            404, "Aeronave não registrada.", null
        );

        return NotFound(notFound);
    }
}