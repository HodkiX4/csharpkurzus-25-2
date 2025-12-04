using Microsoft.EntityFrameworkCore;
using MissingPetFinder.Data;
using MissingPetFinder.DTOs;
using MissingPetFinder.Models;
using MissingPetFinder.Services.Interfaces;

namespace MissingPetFinder.Services.Implementations;

public class PetsService(AppDbContext context) : IPetsService
{
    public async Task<IEnumerable<Pet>> GetAllMissingPetsAsync() => await context.Pets.ToListAsync();

    public async Task<Pet> AddMissingPetAsync(PetDTO petCreateDto, int userId)
    {
        var pet = new Pet
        {
            Name = petCreateDto.Name,
            Type = petCreateDto.Type,
            Breed = petCreateDto.Breed,
            Color = petCreateDto.Color,
            Description = petCreateDto.Description,
            LocationLastSeen = petCreateDto.LocationLastSeen,
            DateLastSeen = petCreateDto.DateLastSeen,
            ContactEmail = petCreateDto.ContactEmail,
            ImageUrl = petCreateDto.ImageUrl,
            UserId = userId
        };

        await context.Pets.AddAsync(pet);
        await context.SaveChangesAsync();
        return pet;
    }

    public async Task<Pet> EditMissingPetAsync(int petId, UpdatePetDTO petUpdateDto, int userId)
    {
        var pet = await context.Pets.FindAsync(petId);
        if (pet == null)
            throw new KeyNotFoundException($"Pet with ID {petId} not found.");
        if (pet.UserId != userId)
            throw new UnauthorizedAccessException("You do not have permission to edit this.");

        if (petUpdateDto.Name is not null) pet.Name = petUpdateDto.Name;
        if (petUpdateDto.Type is not null) pet.Type = petUpdateDto.Type;
        if (petUpdateDto.Breed is not null) pet.Breed = petUpdateDto.Breed;
        if (petUpdateDto.Color is not null) pet.Color = petUpdateDto.Color;
        if (petUpdateDto.Description is not null) pet.Description = petUpdateDto.Description;
        if (petUpdateDto.LocationLastSeen is not null) pet.LocationLastSeen = petUpdateDto.LocationLastSeen;
        if (petUpdateDto.DateLastSeen.HasValue) pet.DateLastSeen = petUpdateDto.DateLastSeen.Value;
        if (petUpdateDto.ImageUrl is not null) pet.ImageUrl = petUpdateDto.ImageUrl;
        if (petUpdateDto.ContactEmail is not null) pet.ContactEmail = petUpdateDto.ContactEmail;

        await context.SaveChangesAsync();
        return pet;
    }


    public async Task<Pet> ToggleStatusAsync(int petId, int userId)
    {
        var pet = await context.Pets.FindAsync(petId);
        if (pet == null)
            throw new KeyNotFoundException($"Pet with ID {petId} not found.");
        if (pet.UserId != userId)
            throw new UnauthorizedAccessException("You do not have permission to change the status of this.");

        pet.IsFound = !pet.IsFound;
        await context.SaveChangesAsync();
        return pet;
    }
}