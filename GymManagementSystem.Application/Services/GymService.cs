using GymManagementSystem.Application.DTOs.Gym;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Services;

public class GymService : IGymService
{
    private readonly IUnitOfWork _unitOfWork;

    public GymService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GymResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var gyms = await _unitOfWork.Gyms.GetAllAsync(cancellationToken);
        return gyms.Select(g => new GymResponse
        {
            Id = g.Id,
            Name = g.Name,
            Address = g.Address,
            Phone = g.Phone,
            Email = g.Email,
            Website = g.Website
        });
    }

    public async Task<GymResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var g = await _unitOfWork.Gyms.GetByIdAsync(id, cancellationToken);
        if (g == null) return null;
        
        return new GymResponse
        {
            Id = g.Id,
            Name = g.Name,
            Address = g.Address,
            Phone = g.Phone,
            Email = g.Email,
            Website = g.Website
        };
    }

    public async Task<Guid> CreateAsync(CreateGymDto dto, CancellationToken cancellationToken = default)
    {
        var gym = new Gym
        {
            Name = dto.Name,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            Website = dto.Website
        };
        
        await _unitOfWork.Gyms.AddAsync(gym);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return gym.Id;
    }

    public async Task UpdateAsync(UpdateGymDto dto, CancellationToken cancellationToken = default)
    {
        var gym = await _unitOfWork.Gyms.GetByIdAsync(dto.Id, cancellationToken);
        if (gym == null) return;
        
        gym.Name = dto.Name;
        gym.Address = dto.Address;
        gym.Phone = dto.Phone;
        gym.Email = dto.Email;
        gym.Website = dto.Website;
        
        _unitOfWork.Gyms.Edit(gym);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var gym = await _unitOfWork.Gyms.GetByIdAsync(id, cancellationToken);
        if (gym != null)
        {
            _unitOfWork.Gyms.Remove(gym);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
