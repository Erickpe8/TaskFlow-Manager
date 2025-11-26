using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExportController : ControllerBase
{
    private readonly IExportService _exportService;

    public ExportController(IExportService exportService)
    {
        _exportService = exportService;
    }

    /// <summary>
    /// Exporta todas las tareas del usuario a Excel
    /// </summary>
    /// <returns>Archivo Excel con las tareas</returns>
    [HttpGet("excel")]
    public async Task<IActionResult> ExportToExcel()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            var fileBytes = await _exportService.ExportTasksToExcelAsync(userId.Value);
            var fileName = $"TaskFlow_Tareas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al generar el archivo Excel", error = ex.Message });
        }
    }

    /// <summary>
    /// Exporta todas las tareas del usuario a PDF
    /// </summary>
    /// <returns>Archivo PDF con las tareas</returns>
    [HttpGet("pdf")]
    public async Task<IActionResult> ExportToPdf()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            var fileBytes = await _exportService.ExportTasksToPdfAsync(userId.Value);
            var fileName = $"TaskFlow_Tareas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al generar el archivo PDF", error = ex.Message });
        }
    }

    private int? GetUserId()
    {
        var subjectId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(subjectId) || !int.TryParse(subjectId, out var userId))
        {
            return null;
        }

        return userId;
    }
}