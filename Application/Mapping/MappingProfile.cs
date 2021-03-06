using System.Collections.Generic;
using System.Linq;
using Application.Dtos;
using AutoMapper;
using Core.Domain;
using Core.Types;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make, MakeDto>();
            CreateMap<Make, IdNameDto>();
            CreateMap<Model, IdNameDto>();
            CreateMap<Feature, IdNameDto>();
            CreateMap<Photo, PhotoDto>();
            CreateMap(typeof(Envelope<>), typeof(EnvelopeDto<>));
            CreateMap<Vehicle, SaveVehicleDto>()
                .ForMember(svd => svd.Contact, opt => opt.MapFrom(v => new ContactDto { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(svd => svd.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(vd => vd.Make, opt => opt.MapFrom(v => v.Model.Make))
                .ForMember(vd => vd.Contact, opt => opt.MapFrom(v => new ContactDto { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vd => vd.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new IdNameDto { Id = vf.FeatureId, Name = vf.Feautre.Name })));
            CreateMap<User, ProfileDto>()
                .ForMember(p => p.VehiclesCount, opt => opt.MapFrom(u => u.Vehicles.Count()));

            CreateMap<VehicleFilterDto, VehicleFilter>();
            CreateMap<SaveVehicleDto, Vehicle>()
                .ForMember(v => v.ContactName, opt => opt.MapFrom(svd => svd.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(svd => svd.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(svd => svd.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((svd, v) =>
                {
                    var removedFeatures = v.Features
                        .Where(vf => !svd.Features.Contains(vf.FeatureId));
                    var cloned = new List<VehicleFeature>(removedFeatures);

                    foreach (var vehicleFeature in cloned)
                    {
                        v.Features.Remove(vehicleFeature);
                    }

                    var addedFeatures = svd.Features
                        .Where(id => !v.Features.Any(vf => vf.FeatureId == id))
                        .Select(id => new VehicleFeature { FeatureId = id, VehicleId = v.Id });

                    foreach (var vehicleFeature in addedFeatures)
                    {
                        v.Features.Add(vehicleFeature);
                    }
                });
        }
    }
}