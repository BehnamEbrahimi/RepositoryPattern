using System;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class VehiclesController : BaseController
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VehiclesController(IVehicleRepository vehicleRepository, IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create(SaveVehicleDto saveVehicleDto)
        {
            var model = await _modelRepository.Details(saveVehicleDto.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("Model", "Invalid Id");

                return BadRequest(ModelState);
            }

            var vehicle = Mapper.Map<SaveVehicleDto, Vehicle>(saveVehicleDto);
            vehicle.LastUpdate = DateTime.Now;

            _vehicleRepository.Create(vehicle);
            await _unitOfWork.CompleteAsync();

            vehicle = await _vehicleRepository.Details(vehicle.Id);

            return Mapper.Map<Vehicle, VehicleDto>(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> Edit(int id, SaveVehicleDto saveVehicleDto)
        {
            var vehicleInDb = await _vehicleRepository.Details(id);

            if (vehicleInDb == null)
            {
                return NotFound();
            }

            Mapper.Map<SaveVehicleDto, Vehicle>(saveVehicleDto, vehicleInDb);
            vehicleInDb.LastUpdate = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            vehicleInDb = await _vehicleRepository.Details(id);

            return Mapper.Map<Vehicle, VehicleDto>(vehicleInDb);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var vehicle = await _vehicleRepository.Details(id, false);

            if (vehicle == null)
            {
                return NotFound();
            }

            _vehicleRepository.Delete(vehicle);
            await _unitOfWork.CompleteAsync();

            return id;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            var vehicle = await _vehicleRepository.Details(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Mapper.Map<Vehicle, VehicleDto>(vehicle);
        }
    }
}