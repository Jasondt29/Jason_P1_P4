using Microsoft.AspNetCore.Mvc;
using Jason_P1_P4.Models;
using Jason_P1_P4.Services;

namespace Jason_P1_P4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController(AutoresService autoresService) : ControllerBase
{
    // GET: api/Autores
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await autoresService.GetListAsync());
    }

    // GET: api/Autores/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var autor = await autoresService.GetByIdAsync(id);
        if (autor == null) return NotFound();
        return Ok(autor);
    }

    // POST: api/Autores
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AutorRecord autor)
    {
        var id = await autoresService.SaveAsync(autor);
        var creado = autor with { IdAutor = id };
        return CreatedAtAction(nameof(GetById), new { id }, creado);
    }

    // PUT: api/Autores/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AutorRecord autor)
    {
        var existente = await autoresService.GetByIdAsync(id);
        if (existente == null) return NotFound();

        var actualizado = autor with { IdAutor = id };
        var guardado = await autoresService.UpdateAsync(actualizado);
        if (!guardado) return StatusCode(500, "No se pudo guardar el cambio.");

        return Ok(actualizado);
    }

    // DELETE: api/Autores/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await autoresService.DeleteAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}