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
    public  async Task<ActionResult<IEnumerable<ResponseViewModel<ListPilotsViewModel>>>> GetAllPilotsAsync()
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
        var pilot =  await _pilotRepository.GetByIdAsync(id);
        if (pilot is null)
        {
            var notFound =   new ResponseViewModel<string>(
                404, 
                "Piloto não registrado.",
                null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailPilotViewModel>(
            200,
            "OK",
            pilot
        );
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  async Task<ActionResult<ResponseViewModel<object>>> AddPilotAsync( AddPilotViewModel pilotData)
    {
        if (pilotData is null)
        {
            var nullModelResponse = new ResponseViewModel<object>(400, "Todos os dados do piloto devem ser informados", null);
            return BadRequest(nullModelResponse);
        }
        var validationResult = await _validationService.ValidateModel(pilotData, _addPilotValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }

        var pilot =  await _pilotRepository.AddAsync(pilotData);
        var response = new ResponseViewModel<DetailPilotViewModel>(
            201, "CREATED", pilot
        );
        return CreatedAtAction("AddPilot", response);
    }
    
    [HttpPut]
    public  async Task<ActionResult<ResponseViewModel<DetailPilotViewModel>>> UpdatePilotAsync( UpdatePilotViewModel pilotData)
    {
        if (pilotData is null)
        {
            var nullResponse = new ResponseViewModel<string>(
                404, "Os dados do piloto devem ser informados.", null
            );
            return NotFound(nullResponse);
        }

        var existsPilot = await _pilotRepository.GetByIdAsync(pilotData.Id);
        var validationResult = await _validationService.ValidateModel(pilotData, _updatePilotValidator); 
        if (!validationResult.IsValid && existsPilot != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        
        var pilot =  await _pilotRepository.UpdateAsync(pilotData);
        
        if (pilot is null)
        {
            var notFound = new ResponseViewModel<string>(
                404, "Piloto não registrado.", null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailPilotViewModel>(
            200, "OK", pilot
        );
        return Ok(response);
    }
    
    [HttpDelete]
    public  async Task<ActionResult<ResponseViewModel<string>>> DeletePilotAsync( int id)
    {
        var existsPilot = await _pilotRepository.GetByIdAsync(id);
        var validationResult = await _validationService.ValidateModel(id, _deletePilotValidator);
        if (!validationResult.IsValid && existsPilot != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Falha de validação", null, validationResult.Errors
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
            404, "Piloto não registrado.", null
        );

        return NotFound(notFound);
    }
}