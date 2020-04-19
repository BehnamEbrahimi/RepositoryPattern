using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Types;
using Core.Domain;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Application.Controllers
{
    [Route("api/vehicles")]
    public class PhotosController : BaseController
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PhotoSettings _photoSettings;
        private readonly IPhotoStorage _photoStorage;


        public PhotosController(IVehicleRepository vehicleRepository, IPhotoRepository photoRepository, IUnitOfWork unitOfWork, IOptionsSnapshot<PhotoSettings> options, IPhotoStorage photoStorage)
        {
            _vehicleRepository = vehicleRepository;
            _photoRepository = photoRepository;
            _unitOfWork = unitOfWork;
            _photoSettings = options.Value;
            _photoStorage = photoStorage;
        }

        [HttpPost("{vehicleId}/photos")]
        public async Task<ActionResult<PhotoDto>> Add(int vehicleId, [FromForm] IFormFile file)
        {
            var vehicle = await _vehicleRepository.Details(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var photoUploadResult = await _photoStorage.AddPhoto(file);

            var photo = new Photo
            {
                Id = photoUploadResult.PublicId,
                Url = photoUploadResult.Url
            };
            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return Mapper.Map<Photo, PhotoDto>(photo);
        }

        [HttpGet("{vehicleId}/photos")]
        public async Task<ActionResult<List<PhotoDto>>> List(int vehicleId)
        {
            var photos = await _photoRepository.List(vehicleId);

            return Mapper.Map<List<Photo>, List<PhotoDto>>(photos);
        }
    }
}