using System.Text.Json;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Cancellation;
using cia_aerea_api.Validators.Flights;
using cia_aerea_api.Validators.Services;
using cia_aerea_api.ViewModels.Cancellation;
using cia_aerea_api.ViewModels.Flight;
using cia_aerea_api.ViewModels.Generics;
using Microsoft.AspNetCore.Mvc;

namespace cia_aerea_api.Controllers;

[Route("/api/flights")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly FlightRepository _flightRepository;
    private readonly ValidationService _validationService;
    private readonly AddFlightValidator _addFlightValidator;
    private readonly UpdateFlightValidator _updateFlightValidator;
    private readonly DeleteFlightValidator _deleteFlightValidator;
    private readonly FlightCancellationValidator _cancellationValidator;
    
    public FlightController(FlightRepository flightRepository, ValidationService validationService, AddFlightValidator addFlightValidator, UpdateFlightValidator updateFlightValidator, DeleteFlightValidator deleteFlightValidator, FlightCancellationValidator cancellationValidator)
    {
        _flightRepository = flightRepository;
        _validationService = validationService;
        _addFlightValidator = addFlightValidator;
        _updateFlightValidator = updateFlightValidator;
        _deleteFlightValidator = deleteFlightValidator;
        _cancellationValidator = cancellationValidator;
    }
    
    [HttpGet]
    public  async Task<ActionResult<ResponseViewModel<List<ListFlightViewModel>>>> GetAllFlightsAsync(string? origin, string? destiny, DateTime? departure, DateTime? arrival)
    {
        var flights =  await _flightRepository.GetAllAsync( origin,  destiny,  departure,  arrival);
        var listFlightViewModel = flights
            .Select(f =>
            {
                return new ListFlightViewModel(
                    f.Id,
                    f.Origin,
                    f.Destiny,
                    f.DepartureDateTime,
                    f.ArrivalDateTime,
                    f.AirplaneId,
                    f.PilotId
                );
            }).ToList();


        var response = new ResponseViewModel<List<ListFlightViewModel>>(
            200,
            "OK",
            listFlightViewModel
        );
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public  async Task<ActionResult<ResponseViewModel<DetailFlightViewModel>>> GetFlightByIdAsync(int id)
    {
        var flight =  await _flightRepository.GetByIdAsync(id);
        if (flight is null)
        {
            var notFound =   new ResponseViewModel<string>(
                404, 
                "Flight is not registered.",
                null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailFlightViewModel>(
            200,
            "OK",
            flight
        );
        return Ok(response);
    }
    
    [HttpPost]
    public  async Task<ActionResult<ResponseViewModel<object>>> AddFlightAsync( AddFlightViewModel flightData)
    {
        if (flightData is null)
        {
            var nullModelResponse = new ResponseViewModel<object>(400, "All flight details must be filled in", null);
            return BadRequest(nullModelResponse);
        }
        var validationResult = await _validationService.ValidateModel(flightData, _addFlightValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }

        var flight =  await _flightRepository.AddAsync(flightData);
        var response = new ResponseViewModel<DetailFlightViewModel>(
            201, "CREATED", flight
        );
        return Ok(response);
    }
    
    [HttpPut]
    public  async Task<ActionResult<ResponseViewModel<DetailFlightViewModel>>> UpdateFlightAsync( UpdateFlightViewModel flightData)
    {
        if (flightData is null)
        {
            var nullResponse = new ResponseViewModel<string>(
                404, "The flight data can not be null.", null
            );
            return NotFound(nullResponse);
        }

        var existsFlight = await _flightRepository.GetByIdAsync(flightData.Id);
        var validationResult = await _validationService.ValidateModel(flightData, _updateFlightValidator); 
        if (!validationResult.IsValid && existsFlight != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        
        var flight =  await _flightRepository.UpdateAsync(flightData);
        
        if (flight is null)
        {
            var notFound = new ResponseViewModel<string>(
                404, "flight is not registered", null
            );
            return NotFound(notFound);
        }
        var response = new ResponseViewModel<DetailFlightViewModel>(
            200, "OK", flight
        );
        return Ok(response);
    }
    
    [HttpDelete]
    public  async Task<ActionResult<ResponseViewModel<string>>> DeleteFlightAsync( int id)
    {
        var existsFlight = await _flightRepository.GetByIdAsync(id);
        var validationResult = await _validationService.ValidateModel(id, _deleteFlightValidator);
        if (!validationResult.IsValid && existsFlight != null)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        if (await _flightRepository.DeleteAsync(id))
        {
            var response = new ResponseViewModel<string>(
                200, "DELETED", null
            );
            return Ok(response);
        }

        var notFound = new ResponseViewModel<string>(
            404, "Flight is not registered", null
        );

        return NotFound(notFound);
    }

    [HttpPost("cancellation")]
    public async Task<ActionResult<ResponseViewModel<object>>> CancelFlightAsync(CancellationViewModel cancellationData)
    {
        var validationResult = await _validationService.ValidateModel(cancellationData, _cancellationValidator);
        if (!validationResult.IsValid)
        {
            var validationFailedResponse = new ResponseViewModel<object>(
                400, "Validation failed", null, validationResult.Errors
            );
        
            return BadRequest(validationFailedResponse);
        }
        
        var flyCancelled = await _flightRepository.FlightCancellation(cancellationData);
        var response =  new ResponseViewModel<DetailFlightViewModel>(200, "OK", flyCancelled);
        return Ok(response);
    
    }
}