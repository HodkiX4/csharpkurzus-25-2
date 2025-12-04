using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MissingPetFinder.DTOs;
using MissingPetFinder.Services.Interfaces;

namespace MissingPetFinder.Controllers;

[Authorize]
public class PetsController(IPetsService petsService) : Controller
{
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var pets = await petsService.GetAllMissingPetsAsync();
        return View(pets);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(PetDTO petCreateDto)
    {
        if (!ModelState.IsValid)
            return View(petCreateDto);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await petsService.AddMissingPetAsync(petCreateDto, userId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Error creating pet: " + ex.Message);
            return View(petCreateDto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var pet = await petsService.EditMissingPetAsync(id, new UpdatePetDTO(), userId);
            return View(pet);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdatePetDTO petDto)
    {
        if (!ModelState.IsValid)
            return View(petDto);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await petsService.EditMissingPetAsync(id, petDto, userId);
            return RedirectToAction("Index");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error: " + ex.Message);
            return View(petDto);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await petsService.ToggleStatusAsync(id, userId);
            return RedirectToAction("Index");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
