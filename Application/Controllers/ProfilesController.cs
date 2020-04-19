using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Domain;
using Core.Interfaces;
using Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class ProfilesController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ProfilesController(IUserRepository userRepository, IVehicleRepository vehicleRepository, IUserAccessor userAccessor, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _userAccessor = userAccessor;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileDto>> Details(string username)
        {
            var user = await _userRepository.Details(username);

            if (user == null)
                return NotFound();

            return Mapper.Map<ProfileDto>(user);
        }

        [HttpPut]
        public async Task<ActionResult<ProfileDto>> Edit(SaveProfileDto saveProfileDto)
        {
            var user = await _userRepository.Details(_userAccessor.GetCurrentUsername());

            user.DisplayName = saveProfileDto.DisplayName ?? user.DisplayName;
            await _unitOfWork.CompleteAsync();

            return Mapper.Map<ProfileDto>(user);
        }

        [HttpGet("{username}/vehicles")]
        public async Task<ActionResult<List<VehicleDto>>> GetUserVehicles(string username)
        {
            var user = await _userRepository.Details(username);

            if (user == null)
                return NotFound();

            var envelope = await _vehicleRepository.List(new VehicleFilter { UserId = user.Id });

            return Mapper.Map<List<Vehicle>, List<VehicleDto>>(envelope.Items);
        }
    }
}