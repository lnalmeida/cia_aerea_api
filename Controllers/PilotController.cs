using cia_aerea_api.ViewModels.Pilot;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Pilots;
using cia_aerea_api.Validators.Services;
using cia_aerea_api.ViewModels.Generics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cia_aerea_api.Controllers;

[Route("api/pilots")]
[ApiController]
public class PilotController : ControllerBase
{
    private readonly PilotRepository _pilotRepository;
    private readonly ValidationService _validationService;
    private readonly AddPilotValidator _addPilotValidator;
    private readonly UpdatePilotValidator _updatePilotValidator;
    private readonly DeletePilotValidator _deletePilotValidator;

    public PilotController(ValidationService validationService, PilotRepository pilotRepository, AddPilotValidator addPilotValidator, UpdatePilotValidator updatePilotValidator, DeletePilotValidator deletePilotValidator)
    {
        _validationService = validationService;
        _pilotRepository = pilotRepository;
        _addPilotValidator = addPilotValidator;
        _updatePilotValidator = updatePilotValidator;
        _deletePilotValidator = deletePilotValidator;
    }
    
    [HttpGet]
    public  async Task<ActionResult<ResponseViewModel<List<ListPilotsViewModel>>>> GetAllPilotsAsync()
    {
        var pilots =  await _pilotRepository.GetAllAsync();
        var listPilotViewModel = pilots.Select(
            p => new ListPilotsViewModel(
                p.Id,
                p.Name,
                p.Registration
            )).ToList();
        var response = new ResponseViewModel<List<ListPilotsViewModel>>(
            200,
            "OK",
            listPilotViewModel
        );
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public  async Task<ActionResult<ResponseViewModel<DetailPilotViewModel>>> GetPilotByIdAsync(int id)
    {
        var airplane =  await _pilotRepository.GetByIdAsync(id);
        if (airplane is null)
        {
            var notFound =   new ResponseViewModel<string>(
                404, 
                "Pilot is not registered.",
                null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailPilotViewModel>(
            200,
            "OK",
            airplane
        );
        return Ok(response);
    }

    [HttpPost]
    public  async Task<ActionResult<ResponseViewModel<object>>> AddPilotAsync( AddPilotViewModel pilotData)
    {
        if (pilotData is null)
        {
            var nullModelResponse = new ResponseViewModel<object>(400, "All pilot details must be filled in", null);
            return BadRequest(nullModelResponse);
        }
        var validationResult = await _validationService.ValidateModel(pilotData, _addPilotValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }

        var pilot =  await _pilotRepository.AddAsync(pilotData);
        var response = new ResponseViewModel<DetailPilotViewModel>(
            201, "CREATED", pilot
        );
        return Ok(response);
    }
    
    [HttpPut]
    public  async Task<ActionResult<ResponseViewModel<DetailPilotViewModel>>> UpdatePilotAsync( UpdatePilotViewModel airplaneData)
    {
        if (airplaneData is null)
        {
            var nullResponse = new ResponseViewModel<string>(
                404, "The airplane data can not be null.", null
            );
            return NotFound(nullResponse);
        }
        
        var validationResult = await _validationService.ValidateModel(airplaneData, _updatePilotValidator); 
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        
        var airplane =  await _pilotRepository.UpdateAsync(airplaneData);
        
        if (airplane is null)
        {
            var notFound = new ResponseViewModel<string>(
                404, "Pilot is not registered", null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailPilotViewModel>(
            200, "OK", airplane
        );
        return Ok(response);
    }
    
    [HttpDelete]
    public  async Task<ActionResult<ResponseViewModel<string>>> DeletePilotAsync( int id)
    {
        var validationResult = await _validationService.ValidateModel(id, _deletePilotValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        if (await _pilotRepository.DeleteAsync(id))
        {
            var response = new ResponseViewModel<string>(
                200, "DELETED", null
            );
            return Ok(response);
        }

        var notFound = new ResponseViewModel<string>(
            404, "Pilot is not registered", null
        );

        return NotFound(notFound);
    }
}