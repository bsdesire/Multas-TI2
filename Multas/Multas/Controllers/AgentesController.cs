using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        private MultasDB db = new MultasDB();

        // GET: Agentes
        public ActionResult Index()
        {
            return View(db.Agentes.ToList());
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        /// <summary>
        /// Recolhe os dados da view, sobe um novo Agente
        /// </summary>
        /// <returns></returns>
        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Recolhe os dados da view, sobe um novo Agente
        /// </summary>
        /// <param name="agente"></param>
        /// <param name="fotografia"></param>
        /// <returns></returns>
        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase fotografia)
        {
            // Confronta os dados que vêm da view com a forma que os dados devem ter,
            // ie, valida os dados com o Modelo
            if (ModelState.IsValid)
            {
                try
                {
                    db.Agentes.Add(agente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
                
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        /// <summary>
        /// mostra na view os dados de um agente para posterior, eventual, remoção.
        /// </summary>
        /// <param name="id">Identificador do agente a remover</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            // o id do agente não foi fornecido
            // não é possivel procurar o Agente
            // o que devo fazer?
            if (id == null)
            {
                // opção por defeito do template:
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                /// e não há ID de Agente, uma das duas coisas aconteceu:
                ///     - há um erro nos links da aplicação
                ///     - há um 'chico experto' a fazer asneiras no URL
                return RedirectToAction("Index");
            }
            // Procura os dados do agente cujo ID é fornecido.
            Agentes agente = db.Agentes.Find(id);

            // se o agente não for encontrado
            /// ou há um erro,
            /// ou há um 'chico experto' a fazer asneiras no URL
            if (agente == null)
            {
                return RedirectToAction("Index");
            }

            // Para o caso do utilizador alterar o valor da action no post, de forma fraudulenta.
            // Para isso vou guardar o valor do ID do agente.
            // - Guardar o ID do Agente num cookie cifrado.
            // - Guardar o ID numa variavél de sessão. (quem estiver a usar o Asp.Net CORE já não tem esta ferramenta.)
            Session["idAgente"] = agente.ID;
            Session["Metodo"] = "Agentes/Delete";

            // Envia para a view os dados do agente em conta
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                // se entrei aqui, é porque há um erro
                // não se sabe o ID do agente a remover
                return RedirectToAction("Index");
            }
            // Avaliar se o ID do agente que é fornecido é o mesmo ID do agente que foi apresentado no ecrã
            if (id != (int)Session["idAgente"])
            {
                return RedirectToAction("Index");
            }
            // Avaliar se o metodo que é fornecido é o mesmo método que foi apresentado no ecrã
            string operacao = "Agentes/Delete";
            if (operacao != (string)Session["Metodo"])
            {
                return RedirectToAction("Index");
            }

            Agentes agente = db.Agentes.Find(id);



            if(agente == null)
            {
                // não foi possivel encontrar o Agente
                RedirectToAction("Index");
            }
            try
            {
                db.Agentes.Remove(agente);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // Captura a exceção e processa o código para resolver resolver o problema
                // pode haver mais que um 'catch' associado a um 'try'

                // enviar mensagem de erro para o utilizador
                ModelState.AddModelError("","Ocorreu um erro com a eliminação do Agente " +
                    agente.Nome + ". Provavelmente relacionado com o facto do agente" +
                    " ter emitido multas...");

                // devolver os dados do agente à View
                return View(agente);
            }
            
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
