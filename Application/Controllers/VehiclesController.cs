using System;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Domain;
using Core.Interfaces;
using Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class VehiclesController : BaseController
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;

        public VehiclesController(IVehicleRepository vehicleRepository, IUserRepository userRepository, IModelRepository modelRepository, IUnitOfWork unitOfWork, IUserAccessor userAccessor)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create(SaveVehicleDto saveVehicleDto)
        {
            var model = await _modelRepository.Details(saveVehicleDto.ModelId);
            if (model == null)
                return BadRequest("Invalid ModelId");

            var user = await _userRepository.Details(_userAccessor.GetCurrentUsername());

            var vehicle = Mapper.Map<SaveVehicleDto, Vehicle>(saveVehicleDto);
            vehicle.User = user;
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
                return NotFound();

            var user = await _userRepository.Details(_userAccessor.GetCurrentUsername());
            if (user.Id != vehicleInDb.UserId)
                return Forbid();

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
                return NotFound();

            var user = await _userRepository.Details(_userAccessor.GetCurrentUsername());
            if (user.Id != vehicle.UserId)
                return Forbid();

            _vehicleRepository.Delete(vehicle);
            await _unitOfWork.CompleteAsync();

            return id;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> Details(int id)
        {
            var vehicle = await _vehicleRepository.Details(id);

            if (vehicle == null)
                return NotFound();

            return Mapper.Map<Vehicle, VehicleDto>(vehicle);
        }

        [HttpGet]
        public async Task<EnvelopeDto<VehicleDto>> List([FromQuery] VehicleFilterDto filterDto)
        {
            var filter = Mapper.Map<VehicleFilterDto, VehicleFilter>(filterDto);
            var envelope = await _vehicleRepository.List(filter);

            return Mapper.Map<Envelope<Vehicle>, EnvelopeDto<VehicleDto>>(envelope);
        }
    }
}