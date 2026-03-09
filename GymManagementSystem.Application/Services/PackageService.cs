using GymManagementSystem.Application.DTOs.Package;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;

    public PackageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PackageResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _unitOfWork.Packages.GetAllAsync(cancellationToken);
        return packages.Select(p => new PackageResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            DurationInDays = p.DurationInDays,
            IsActive = p.IsActive,
            MaxMembers = p.MaxMembers
        });
    }

    public async Task<PackageResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var p = await _unitOfWork.Packages.GetByIdAsync(id, cancellationToken);
        if (p == null) return null;
        
        return new PackageResponse
        {
             Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            DurationInDays = p.DurationInDays,
            IsActive = p.IsActive,
            MaxMembers = p.MaxMembers
        };
    }

    public async Task<Guid> CreateAsync(CreatePackageDto dto, CancellationToken cancellationToken = default)
    {
        var package = new Package
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            DurationInDays = dto.DurationInDays,
            IsActive = dto.IsActive,
            MaxMembers = dto.MaxMembers
        };
        
        await _unitOfWork.Packages.AddAsync(package);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return package.Id;
    }

    public async Task UpdateAsync(UpdatePackageDto dto, CancellationToken cancellationToken = default)
    {
        var p = await _unitOfWork.Packages.GetByIdAsync(dto.Id, cancellationToken);
        if (p == null) return;
        
        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.DurationInDays = dto.DurationInDays;
        p.IsActive = dto.IsActive;
        p.MaxMembers = dto.MaxMembers;
        
        _unitOfWork.Packages.Edit(p);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
         var p = await _unitOfWork.Packages.GetByIdAsync(id, cancellationToken);
         if (p != null)
         {
             _unitOfWork.Packages.Remove(p);
             await _unitOfWork.SaveChangesAsync(cancellationToken);
         }
    }
}
