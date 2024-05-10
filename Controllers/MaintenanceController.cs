using cia_aerea_api.Models.Enums;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Airplanes;
using cia_aerea_api.Validators.Maintenances;
using cia_aerea_api.Validators.Services;
using cia_aerea_api.ViewModels.Flight;
using cia_aerea_api.ViewModels.Generics;
using cia_aerea_api.ViewModels.Maintenance;
using Microsoft.AspNetCore.Mvc;

namespace cia_aerea_api.Controllers;

[Route("/api/maintenances")]
[ApiController]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceRepository _maintenanceRepository;
    private readonly ValidationService? _validationService;
    private readonly AddMaintenanceValidator _addMaintenanceValidator;
    private readonly UpdateMaintenanceValidator _updateMaintenanceValidator;
    private readonly DeleteMaintenanceValidator _deleteMaintenanceValidator;

    public MaintenanceController(MaintenanceRepository maintenanceRepository, ValidationService? validationService, AddMaintenanceValidator addMaintenanceValidator, UpdateMaintenanceValidator updateMaintenanceValidator, DeleteMaintenanceValidator deleteMaintenanceValidator)
    {
        _maintenanceRepository = maintenanceRepository;
        _validationService = validationService;
        _addMaintenanceValidator = addMaintenanceValidator;
        _updateMaintenanceValidator = updateMaintenanceValidator;
        _deleteMaintenanceValidator = deleteMaintenanceValidator;
    }

    [HttpGet("airplane/{airplaneId}")]
    public async Task<ActionResult<IEnumerable<ResponseViewModel<ListMaintenanceViewModel>>>>
        GetAllMaintenanceByAirplaneId(int airplaneId,
            DateTime? startMaintenanceDate, DateTime? endMaintenanceDate, MaintenanceType? type)
    {
        var maintenances =
            await _maintenanceRepository.GetAllByAirplaneIdAsync(airplaneId, startMaintenanceDate, endMaintenanceDate,
                type);
        if (maintenances == null)
        {
            var notFoundResponse = new ResponseViewModel<List<ListMaintenanceViewModel>>(
                404,
                $"Não há manutenções registradas para a aeronave de Id {airplaneId} ",
                null
            );
            NotFound(notFoundResponse);
        }

        var ListMaintenances = maintenances.Select(m =>
        {
            return new ListMaintenanceViewModel(
                m.Id,
                m.MaintenanceDateTime,
                m.Comments,
                m.TypeOfMaintenance,
                m.AirplaneId
            );
        }).ToList();

        var response = new ResponseViewModel<List<ListMaintenanceViewModel>>(
            200,
            "OK",
            ListMaintenances
        );

        return Ok(response);
    }

    [HttpGet("{id}")]
        public async Task<ActionResult<ResponseViewModel<ListMaintenanceViewModel>>> GetMaintenanceById(int id)
        {
            var maintenance = await _maintenanceRepository.GetMaintenanceByIdAsync(id);

            if (maintenance == null)
            {
                var notFoundResponse = new ResponseViewModel<List<ListMaintenanceViewModel>>(
                    404,
                    "Não há manutenção registrada com o Id {id}.",
                    null
                );

                return NotFound(notFoundResponse);
            }
            else
            {
                var maintenanceViewModel = new ListMaintenanceViewModel(
                    maintenance.Id,
                    maintenance.MaintenanceDateTime,
                    maintenance.Comments,
                    maintenance.TypeOfMaintenance,
                    maintenance.AirplaneId
                );
            
                var response = new ResponseViewModel<ListMaintenanceViewModel>(
                    200,
                    "OK",
                    maintenanceViewModel
                );
            
                return Ok(response);
            
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ResponseViewModel<ListMaintenanceViewModel>>> AddMaintenance(AddManintenanceViewModel maintenanceData)
        {
            if (maintenanceData is null)
            {
                var nullModelResponse = new ResponseViewModel<object>(400, "Os dados da manutenção devem ser inormados.", null);
                return BadRequest(nullModelResponse);
            }

            var validationResult = await _validationService.ValidateModel(maintenanceData, _addMaintenanceValidator);
            if (!validationResult.IsValid)
            {
                var validationFailedResponse = new ResponseViewModel<object>(
                    400, "Validation failed", null, validationResult.Errors
                );
        
                return BadRequest(validationFailedResponse);
            }
        
            var maintenance =  await _maintenanceRepository.AddMaintenanceAsync(maintenanceData);
            var response = new ResponseViewModel<ListMaintenanceViewModel>(
                201, "CREATED", maintenance
            );
            return CreatedAtAction("AddMaintenance", response);
        }
        
        [HttpPut]
        public  async Task<ActionResult<ResponseViewModel<ListMaintenanceViewModel>>> UpdateMaintenance( UpdateMaintenanceViewModel maintenanceData)
        {
            if (maintenanceData is null)
            {
                var nullResponse = new ResponseViewModel<string>(
                    404, "Os dados da manutenção devem ser informados.", null
                );
                return NotFound(nullResponse);
            }

            var existsMaintenance = await _maintenanceRepository.GetMaintenanceByIdAsync(maintenanceData.Id);
            var validationResult = await _validationService.ValidateModel(maintenanceData, _updateMaintenanceValidator); 
            if (!validationResult.IsValid && existsMaintenance != null)
            {
                var validationFailedResponse = new ResponseViewModel<object>(
                    400, "Validation failed", null, validationResult.Errors
                );
        
                return BadRequest(validationFailedResponse);
            }
        
            var maintenance =  await _maintenanceRepository.UpdateMaintenanceAsync(maintenanceData);
        
            if (maintenance is null)
            {
                var notFound = new ResponseViewModel<string>(
                    404, "Manutenção não registrada.", null
                );
                return NotFound(notFound);
            }
            var response = new ResponseViewModel<ListMaintenanceViewModel>(
                200, "OK", maintenance
            );
            return Ok(response);
        }
        
        [HttpDelete]
        public  async Task<ActionResult<ResponseViewModel<string>>> DeleteMaintenance( int id)
        {
            var existsMaintenance = await _maintenanceRepository.GetMaintenanceByIdAsync(id);
            var validationResult = await _validationService.ValidateModel(id, _deleteMaintenanceValidator);
            if (!validationResult.IsValid && existsMaintenance != null)
            {
                var validationFailedResponse = new ResponseViewModel<object>(
                    400, "Validation failed", null, validationResult.Errors
                );
        
                return BadRequest(validationFailedResponse);
            }
            if (await _maintenanceRepository.DeleteMaintenanceAsync(id))
            {
                var response = new ResponseViewModel<string>(
                    200, "DELETED", null
                );
                return Ok(response);
            }

            var notFound = new ResponseViewModel<string>(
                404, "MAnutenção não registrada.", null
            );

            return NotFound(notFound);
        }
}