using MissingPetFinder.DTOs;
using MissingPetFinder.Models;

namespace MissingPetFinder.Services.Interfaces;

public interface IPetsService
{
    Task<IEnumerable<Pet>> GetAllMissingPetsAsync();
    Task<Pet> AddMissingPetAsync(PetDTO petCreateDto, int userId);
    Task<Pet> EditMissingPetAsync(int petId, UpdatePetDTO petUpdateDto, int userId);
    Task<Pet> ToggleStatusAsync(int petId, int userId);
}