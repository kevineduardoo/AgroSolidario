using Microsoft.AspNetCore.Mvc;
using AgroSolidario.Data;

namespace AgroSolidario.Controllers
{
    public class HistoricoController : Controller
    {
        private readonly AppDbContext _context;

        public HistoricoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var historicos = _context.Historicos
                .OrderByDescending(h => h.DataDoacao)
                .ToList();

            return View(historicos);
        }
    }
}