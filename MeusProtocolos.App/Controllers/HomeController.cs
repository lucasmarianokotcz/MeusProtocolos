using MeusProtocolos.App.Context;
using MeusProtocolos.App.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MeusProtocolos.App.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly Contexto db = new Contexto();

        // Método que autentica o usuário logado.
        private void AutenticarUsuario(Usuario user)
        {
            // Seleciona o único usuário com o e-mail informado para em seguida usar o seu nome para dar boas-vindas.
            var usuario = db.Usuario.Single(u => u.Login == user.Login);

            // Identidade do usuário
            var identity = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Name, usuario.Login)
                },
            "ApplicationCookie");

            // Contexto da autenticação
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            // SignIn (autenticação de login)
            authManager.SignIn(identity);
        }

        // Método que pega o RedirectUrl de acordo com sucesso ou falha do login do usuário.
        private string GetRedirectUrl(string returnUrl)
        {
            // Caso o usuário tenha feito login
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            // Caso o usuário tenha um login inválido (volta para a mesma URL atual (action Login))
            return returnUrl;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            // Verifica se o usuário está autenticado. Se estiver, desloga.
            if (User.Identity.IsAuthenticated)
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index");
            }

            // Usuário tem a ReturnUrl de acordo com sua tentativa de login (se logou ou não logou)
            var usuario = new Usuario
            {
                ReturnUrl = returnUrl
            };

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario usuario)
        {
            // Valida o form.
            if (ModelState.IsValid)
            {
                // Limpa o form em caso de usuário já existir.
                ModelState.Clear();

                // Busca pelo usuário digitado.
                var user = db.Usuario.SingleOrDefault(u => u.Login == usuario.Login);

                // Se o usuário não existe.
                if (user == null)
                {
                    ModelState.AddModelError("", "Login ou senha inválidos");
                    return View();
                }

                // Verifica se o login e senha digitados conferem com o usuário encontrado no banco.
                if (usuario.Login == user.Login && usuario.Senha == user.Senha)
                {
                    // Autentica o usuário.
                    AutenticarUsuario(usuario);

                    // Redireciona o usuário para a action Index
                    return Redirect(GetRedirectUrl(usuario.ReturnUrl));
                }

                // Usuário e senha não conferem.
                ModelState.AddModelError("", "Login ou senha inválidos");
                return View();
            }

            // Form inválido.
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastro(Usuario usuario)
        {
            // Valida o form.
            if (ModelState.IsValid)
            {
                // Limpa o form em caso de usuário já existir.
                ModelState.Clear();

                // Busca pelo usuário digitado.
                var user = db.Usuario.SingleOrDefault(u => u.Login == usuario.Login);

                // Se o usuário já existir.
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário já cadastrado.");
                    return View();
                }

                // Insere o usuário no banco.
                db.Usuario.Add(usuario);
                await db.SaveChangesAsync();

                // Autentica o usuário recém-cadastrado.
                AutenticarUsuario(usuario);

                return RedirectToAction("Index");
            }

            // Form inválido.
            ModelState.AddModelError(string.Empty, "Preencha os campos corretamente.");
            return View(usuario);
        }
    }
}