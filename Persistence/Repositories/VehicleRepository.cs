using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context)
        {
            _context = context;
        }

        public void Create(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public async Task<Vehicle> Details(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _context.Vehicles.FindAsync(id);
            }

            return await _context.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feautre)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Envelope<Vehicle>> List(VehicleFilter filter)
        {
            var envelope = new Envelope<Vehicle>();

            var query = _context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feautre)
                .AsQueryable(); //After Include it returns IIncludableQueryable so we have to change it.

            query = query.ApplyVehicleFiltering(filter);

            var path = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName
            };
            query = query.ApplyOrdering(filter, path);
            envelope.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(filter);
            envelope.Items = await query.ToListAsync();

            return envelope;
        }
    }
}