using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroSolidario.Data;
using AgroSolidario.Models;

namespace AgroSolidario.Controllers
{
    public class AlimentosController : Controller
    {
        private readonly AppDbContext _context;

        public AlimentosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string busca)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var alimentos = _context.Alimentos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                alimentos = alimentos.Where(a => a.Nome.Contains(busca));
            }

            ViewBag.Busca = busca;
            return View(alimentos.ToList());
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Alimento alimento, IFormFile? foto)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            if (foto != null && foto.Length > 0)
            {
                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                var caminho = Path.Combine(pasta, nomeArquivo);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    foto.CopyTo(stream);
                }

                alimento.Foto = "/uploads/" + nomeArquivo;
            }

            if (ModelState.IsValid)
            {
                _context.Alimentos.Add(alimento);

                // Registra no histórico
                _context.Historicos.Add(new Historico
                {
                    NomeAlimento = alimento.Nome,
                    Quantidade = alimento.Quantidade,
                    Doador = alimento.Doador,
                    Destino = alimento.Destino,
                    DataDoacao = DateTime.Now
                });

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(alimento);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var alimento = _context.Alimentos.Find(id);
            if (alimento == null) return NotFound();
            return View(alimento);
        }

        [HttpPost]
        public IActionResult Edit(Alimento alimento, IFormFile? foto)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            if (foto != null && foto.Length > 0)
            {
                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                var caminho = Path.Combine(pasta, nomeArquivo);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    foto.CopyTo(stream);
                }

                alimento.Foto = "/uploads/" + nomeArquivo;
            }
            else
            {
                var alimentoAtual = _context.Alimentos.AsNoTracking().FirstOrDefault(a => a.Id == alimento.Id);
                alimento.Foto = alimentoAtual?.Foto;
            }

            if (ModelState.IsValid)
            {
                _context.Alimentos.Update(alimento);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(alimento);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var alimento = _context.Alimentos.Find(id);
            if (alimento == null) return NotFound();
            return View(alimento);
        }

        public IActionResult AlterarStatus(int id)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var alimento = _context.Alimentos.Find(id);
            if (alimento != null)
            {
                alimento.Status = alimento.Status == "Pendente" ? "Entregue" : "Pendente";
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("UsuarioNome") == null)
                return RedirectToAction("Index", "Login");

            var alimento = _context.Alimentos.Find(id);
            if (alimento != null)
            {
                _context.Alimentos.Remove(alimento);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}