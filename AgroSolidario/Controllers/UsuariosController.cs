using Microsoft.AspNetCore.Mvc;
using AgroSolidario.Data;
using AgroSolidario.Models;

namespace AgroSolidario.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UsuarioTipo") == "ADMIN";
        }

        private bool IsLogado()
        {
            return HttpContext.Session.GetString("UsuarioNome") != null;
        }

        private int GetUsuarioLogadoId()
        {
            var email = HttpContext.Session.GetString("UsuarioEmail");
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            return usuario?.Id ?? 0;
        }

        public IActionResult Index()
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        public IActionResult Edit(int id)
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");

            // ADMIN pode editar qualquer um
            // Doador/Beneficiário só pode editar o próprio perfil
            if (!IsAdmin() && GetUsuarioLogadoId() != id)
                return RedirectToAction("Index", "Home");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");

            if (!IsAdmin() && GetUsuarioLogadoId() != usuario.Id)
                return RedirectToAction("Index", "Home");

            // Se não for ADMIN, não pode mudar para ADMIN
            if (!IsAdmin() && usuario.Tipo == "ADMIN")
                usuario.Tipo = _context.Usuarios.Find(usuario.Id)?.Tipo ?? "Doador";

            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();

                // Se editou o próprio perfil, atualiza a sessão
                if (GetUsuarioLogadoId() == usuario.Id)
                {
                    HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                    HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                    HttpContext.Session.SetString("UsuarioTipo", usuario.Tipo);
                }

                if (IsAdmin())
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction("Perfil", "Login");
            }

            return View(usuario);
        }

        public IActionResult Delete(int id)
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsLogado()) return RedirectToAction("Index", "Login");
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}