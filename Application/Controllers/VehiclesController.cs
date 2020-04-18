using System;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Controllers
{
    public class VehiclesController : BaseController
    {
        private readonly DataContext _context;

        public VehiclesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create(VehicleDto vehicleDto)
        {
            var model = await _context.Models
                .FindAsync(vehicleDto.ModelId);
            if (model == null)
            {
                ModelState.AddModelError("Model", "Invalid Id");

                return BadRequest(ModelState);
            }

            var vehicle = Mapper.Map<VehicleDto, Vehicle>(vehicleDto);
            vehicle.LastUpdate = DateTime.Now;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return Mapper.Map<Vehicle, VehicleDto>(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> Edit(int id, VehicleDto vehicleDto)
        {
            var vehicleInDb = await _context.Vehicles
                .Include(v => v.Features) //We need Features in mapping.
                .SingleOrDefaultAsync(v => v.Id == id);

            if (vehicleInDb == null)
            {
                return NotFound();
            }

            Mapper.Map<VehicleDto, Vehicle>(vehicleDto, vehicleInDb);
            vehicleInDb.LastUpdate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Mapper.Map<Vehicle, VehicleDto>(vehicleInDb);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var vehicle = await _context.Vehicles
                .FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return id;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Features)
                .SingleOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Mapper.Map<Vehicle, VehicleDto>(vehicle);
        }
    }
}