using MeusProtocolos.App.Context;
using MeusProtocolos.App.Models;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MeusProtocolos.App.Controllers
{
    public class ProtocolosController : Controller
    {
        private readonly Contexto db = new Contexto();

        // Método que pega o login do usuário logado.
        private string PegarLoginUsuarioLogado()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return identity.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        }

        // Faz a verficação se login existe e se é válido.
        private bool VerifId(int? id)
        {
            if (id == null)
                return false;

            Protocolo protocolo = db.Protocolo.Find(id);
            if (protocolo == null)
                return false;

            if (protocolo.Login != PegarLoginUsuarioLogado())
                return false;

            return true;
        }

        // GET: Protocolo
        public async Task<ViewResult> Index(string ordem, string textoPesquisa, string filtroAtual, int? pagina)
        {
            // Armazena a ordem escolhida pelo usuário como OrdemAtual.
            ViewBag.OrdemAtual = ordem;

            // Define se a ordem é crescente ou decrescente.
            ViewBag.OrdemTitulo = string.IsNullOrEmpty(ordem) ? "titulo_" : "";
            ViewBag.OrdemNumero = ordem == "numero" ? "numero_" : "numero";
            ViewBag.OrdemMotivo = ordem == "motivo" ? "motivo_" : "motivo";
            ViewBag.OrdemData = ordem == "data" ? "data_" : "data";
            ViewBag.OrdemResolvido = ordem == "resolvido" ? "resolvido_" : "resolvido";

            // Pega o usuário logado.
            string usuarioLogado = PegarLoginUsuarioLogado();
            
            List<Protocolo> protocolos = new List<Protocolo>();

            // Paginação.
            if (textoPesquisa != null)
                pagina = 1;
            else
                textoPesquisa = filtroAtual;

            // Define o filtro atual com a pesquisa do usuário.
            ViewBag.FiltroAtual = textoPesquisa;

            // Ordenar.
            switch (ordem)
            {
                case "titulo_":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderByDescending(p => p.Titulo).ToListAsync();
                    break;
                case "numero":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderBy(p => p.Numero).ToListAsync();
                    break;
                case "numero_":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderByDescending(p => p.Numero).ToListAsync();
                    break;
                case "motivo":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderBy(p => p.Motivo).ToListAsync();
                    break;
                case "motivo_":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderByDescending(p => p.Motivo).ToListAsync();
                    break;
                case "data":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderBy(p => p.Dia).ToListAsync();
                    break;
                case "data_":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderByDescending(p => p.Dia).ToListAsync();
                    break;
                case "resolvido":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderBy(p => p.Resolvido).ToListAsync();
                    break;
                case "resolvido_":
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderByDescending(p => p.Resolvido).ToListAsync();
                    break;
                default:
                    protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado).OrderBy(p => p.Titulo).ToListAsync();
                    break;
            }

            // Pesquisar.
            if (!string.IsNullOrEmpty(textoPesquisa))
            {
                protocolos = await db.Protocolo.Where(u => u.Usuario.Login == usuarioLogado)
                    .Where(p => p.Titulo.Contains(textoPesquisa) || p.Numero.Contains(textoPesquisa)).ToListAsync();
            }

            // Define o tamanho da página e qual a página atual.
            int tamanhoPagina = 5;
            int paginaAtual = pagina ?? 1;

            ViewBag.Mensagem = TempData["mensagem"];

            return View(protocolos.ToPagedList(paginaAtual, tamanhoPagina));
        }

        // GET: Protocolo/Details/5
        public async Task<ActionResult> Detalhes(int? id)
        {
            if (!VerifId(id))
            {
                return RedirectToAction("Index");
            }

            Protocolo protocolo = await db.Protocolo.FindAsync(id);
            return View(protocolo);
        }

        // GET: Protocolo/Create
        public ActionResult Novo()
        {
            return View();
        }

        // POST: Protocolo/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Novo([Bind(Include = "CodProtocolo,Titulo,Numero,Motivo,Descricao,Dia,Hora,Atendente,OutrasInfo,Resolvido")] Protocolo protocolo)
        {
            if (ModelState.IsValid)
            {
                string userLogado = PegarLoginUsuarioLogado();
                protocolo.Usuario = db.Usuario.SingleOrDefault(u => u.Login == userLogado);
                db.Protocolo.Add(protocolo);
                await db.SaveChangesAsync();
                
                TempData["mensagem"] = "Protocolo cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(protocolo);
        }

        // GET: Protocolo/Edit/5
        public async Task<ActionResult> Editar(int? id)
        {
            if (!VerifId(id))
            {
                return RedirectToAction("Index");
            }

            Protocolo protocolo = await db.Protocolo.FindAsync(id);
            return View(protocolo);
        }

        // POST: Protocolo/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar([Bind(Include = "CodProtocolo,Titulo,Numero,Motivo,Descricao,Dia,Hora,Atendente,OutrasInfo,Resolvido")] Protocolo protocolo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(protocolo).State = EntityState.Modified;
                string userLogado = PegarLoginUsuarioLogado();
                protocolo.Usuario = db.Usuario.SingleOrDefault(u => u.Login == userLogado);
                await db.SaveChangesAsync();

                TempData["mensagem"] = "Protocolo alterado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(protocolo);
        }

        // GET: Protocolo/Delete/5
        public async Task<ActionResult> Excluir(int? id)
        {
            if (!VerifId(id))
            {
                return RedirectToAction("Index");
            }

            Protocolo protocolo = await db.Protocolo.FindAsync(id);
            return View(protocolo);
        }

        // POST: Protocolo/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmaExclusao(int id)
        {
            Protocolo protocolo = await db.Protocolo.FindAsync(id);
            db.Protocolo.Remove(protocolo);
            await db.SaveChangesAsync();

            TempData["mensagem"] = "Protocolo excluído com sucesso!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
