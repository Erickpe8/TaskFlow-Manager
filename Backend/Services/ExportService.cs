using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Services;

public class ExportService : IExportService
{
    private readonly AppDbContext _context;

    public ExportService(AppDbContext context)
    {
        _context = context;
        // Configuración de licencia gratuita para EPPlus (no comercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        // Configuración de licencia para QuestPDF (Community)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> ExportTasksToExcelAsync(int userId)
    {
        var columns = await _context.Columns
            .Include(c => c.Tasks)
            .OrderBy(c => c.Order)
            .ToListAsync();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Tareas");

        // Encabezados con estilo
        worksheet.Cells["A1"].Value = "ID";
        worksheet.Cells["B1"].Value = "Título";
        worksheet.Cells["C1"].Value = "Descripción";
        worksheet.Cells["D1"].Value = "Columna";
        worksheet.Cells["E1"].Value = "Prioridad";
        worksheet.Cells["F1"].Value = "Fecha Límite";
        worksheet.Cells["G1"].Value = "Orden";

        // Estilo del encabezado
        using (var range = worksheet.Cells["A1:G1"])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));
            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        // Datos
        int row = 2;
        foreach (var column in columns)
        {
            foreach (var task in column.Tasks.OrderBy(t => t.Order))
            {
                worksheet.Cells[row, 1].Value = task.Id;
                worksheet.Cells[row, 2].Value = task.Title;
                worksheet.Cells[row, 3].Value = task.Description ?? "";
                worksheet.Cells[row, 4].Value = column.Name;
                
                // Manejo de Priority nullable
                int priority = task.Priority ?? 0;
                worksheet.Cells[row, 5].Value = GetPriorityText(priority);
                
                worksheet.Cells[row, 6].Value = task.DueDate?.ToString("dd/MM/yyyy") ?? "";
                worksheet.Cells[row, 7].Value = task.Order;

                // Color según prioridad
                if (priority >= 3)
                {
                    worksheet.Cells[row, 5].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    worksheet.Cells[row, 5].Style.Font.Bold = true;
                }

                row++;
            }
        }

        // Ajustar ancho de columnas
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Bordes
        worksheet.Cells[worksheet.Dimension.Address].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        worksheet.Cells[worksheet.Dimension.Address].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        worksheet.Cells[worksheet.Dimension.Address].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        worksheet.Cells[worksheet.Dimension.Address].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return package.GetAsByteArray();
    }

    public async Task<byte[]> ExportTasksToPdfAsync(int userId)
    {
        var columns = await _context.Columns
            .Include(c => c.Tasks)
            .OrderBy(c => c.Order)
            .ToListAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text("📋 TaskFlow Manager - Reporte de Tareas")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Spacing(5);

                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(10).FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        foreach (var column in columns)
                        {
                            // Título de columna
                            col.Item().PaddingTop(15).Text(column.Name)
                                .FontSize(16).SemiBold().FontColor(Colors.Blue.Darken1);

                            if (column.Tasks.Any())
                            {
                                // Tabla de tareas
                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3); // Título
                                        columns.RelativeColumn(4); // Descripción
                                        columns.RelativeColumn(1); // Prioridad
                                        columns.RelativeColumn(2); // Fecha
                                    });

                                    // Encabezado
                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("Título").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Descripción").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Prioridad").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Fecha Límite").SemiBold();

                                        static IContainer CellStyle(IContainer container)
                                        {
                                            return container
                                                .DefaultTextStyle(x => x.SemiBold())
                                                .PaddingVertical(5)
                                                .BorderBottom(1)
                                                .BorderColor(Colors.Grey.Lighten1);
                                        }
                                    });

                                    // Filas
                                    foreach (var task in column.Tasks.OrderBy(t => t.Order))
                                    {
                                        int priority = task.Priority ?? 0;
                                        
                                        table.Cell().Element(CellStyle).Text(task.Title);
                                        table.Cell().Element(CellStyle).Text(task.Description ?? "-");
                                        table.Cell().Element(CellStyle).Text(GetPriorityText(priority));
                                        table.Cell().Element(CellStyle).Text(task.DueDate?.ToString("dd/MM/yyyy") ?? "-");

                                        static IContainer CellStyle(IContainer container)
                                        {
                                            return container
                                                .BorderBottom(1)
                                                .BorderColor(Colors.Grey.Lighten2)
                                                .PaddingVertical(5);
                                        }
                                    }
                                });
                            }
                            else
                            {
                                col.Item().PaddingVertical(5).Text("Sin tareas")
                                    .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    private string GetPriorityText(int priority)
    {
        return priority switch
        {
            1 => "Baja",
            2 => "Media",
            3 => "Alta",
            4 => "Crítica",
            _ => "Sin prioridad"
        };
    }
}