using Microsoft.AspNetCore.Mvc;
using AgroSolidario.Data;

namespace AgroSolidario.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalAlimentos = _context.Alimentos.Count();

            ViewBag.ProximosValidade = _context.Alimentos
                .Where(a => a.Validade <= DateTime.Now.AddDays(30))
                .Count();

            ViewBag.UltimosAlimentos = _context.Alimentos
                .OrderByDescending(a => a.Id)
                .Take(5)
                .ToList();

            ViewBag.AlimentosVencendo = _context.Alimentos
                .Where(a => a.Validade <= DateTime.Now.AddDays(30) && a.Validade >= DateTime.Now)
                .OrderBy(a => a.Validade)
                .ToList();

            ViewBag.TotalDoadores = _context.Usuarios
                .Where(u => u.Tipo == "Doador")
                .Count();

            ViewBag.TotalBeneficiarios = _context.Usuarios
                .Where(u => u.Tipo == "Beneficiario")
                .Count();

            ViewBag.TotalAdmins = _context.Usuarios
                .Where(u => u.Tipo == "ADMIN")
                .Count();

            return View();
        }
    }
}