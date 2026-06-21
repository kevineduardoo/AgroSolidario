using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AgroSolidario.Data;
using AgroSolidario.Models;

namespace AgroSolidario.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Email ou senha incorretos.";
                return View();
            }

            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioTipo", usuario.Tipo);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var email = HttpContext.Session.GetString("UsuarioEmail");
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == email);

            return View(usuario);
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario usuario)
        {
            // Verifica se email já existe
            var existe = _context.Usuarios.Any(u => u.Email == usuario.Email);
            if (existe)
            {
                ViewBag.Erro = "Este email já está cadastrado.";
                return View(usuario);
            }

            // Não permite criar ADMIN pelo cadastro público
            if (usuario.Tipo == "ADMIN")
                usuario.Tipo = "Doador";

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Sucesso"] = "Conta criada com sucesso! Faça login.";
            return RedirectToAction("Index");
        }

        public IActionResult CriarAdmin()
        {
            var existe = _context.Usuarios
                .Any(u => u.Email == "admin@agrosolidario.com");

            if (!existe)
            {
                _context.Usuarios.Add(new Usuario
                {
                    Nome = "Kevin",
                    Email = "admin@agrosolidario.com",
                    Senha = "99886655",
                    Tipo = "ADMIN"
                });
                _context.SaveChanges();
            }

            return Content("Admin criado! Agora faça login.");
        }
    }
}