using Microsoft.AspNetCore.Mvc;
using AgroSolidario.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AgroSolidario.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult GerarPDF()
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            QuestPDF.Settings.License = LicenseType.Community;

            var alimentos = _context.Alimentos.ToList();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header().Text("Relatório de Alimentos — AgroSolidario")
                        .FontSize(18).Bold().AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Nome").Bold();
                            header.Cell().Text("Quantidade").Bold();
                            header.Cell().Text("Doador").Bold();
                            header.Cell().Text("Destino").Bold();
                            header.Cell().Text("Validade").Bold();
                        });

                        foreach (var item in alimentos)
                        {
                            table.Cell().Text(item.Nome);
                            table.Cell().Text(item.Quantidade.ToString());
                            table.Cell().Text(item.Doador);
                            table.Cell().Text(item.Destino);
                            table.Cell().Text(item.Validade.ToShortDateString());
                        }
                    });

                    page.Footer().Text($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .AlignCenter();
                });
            });

            var bytes = pdf.GeneratePdf();

            return File(bytes, "application/pdf", "relatorio-alimentos.pdf");
        }
    }
}