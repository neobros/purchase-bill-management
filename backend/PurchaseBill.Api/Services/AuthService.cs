using AutoMapper;
using PurchaseBill.Api.DTOs;
using PurchaseBill.Api.Entities;
using PurchaseBill.Api.Interfaces;

namespace PurchaseBill.Api.Services;

public class AuthService : IAuthService
{
    private readonly IExternalPosApiClient _posApiClient;
    private readonly ILocationRepository _locationRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public AuthService(
        IExternalPosApiClient posApiClient,
        ILocationRepository locationRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _posApiClient = posApiClient;
        _locationRepository = locationRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var posResponse = await _posApiClient.GetLoginDataAsync(request.Email, request.Password);
        var loginData = posResponse.ResponseBody?.FirstOrDefault();

        if (loginData is null || loginData.UserLocations is null || loginData.UserLocations.Count == 0)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var locations = loginData.UserLocations
            .Select(l => new LocationDetail
            {
                LocationCode = l.LocationCode,
                LocationName = l.LocationName
            })
            .ToList();

        await _locationRepository.UpsertRangeAsync(locations);

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(request.Email);
        var savedLocations = await _locationRepository.GetAllAsync();

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Email = request.Email,
            Locations = _mapper.Map<List<LocationDto>>(savedLocations)
        };
    }
}
