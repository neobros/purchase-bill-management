using AutoMapper;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Interfaces;

namespace PurchaseBill.Api.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public LocationService(ILocationRepository locationRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<List<LocationDto>> GetAllAsync()
    {
        var locations = await _locationRepository.GetAllAsync();
        return _mapper.Map<List<LocationDto>>(locations);
    }
}
