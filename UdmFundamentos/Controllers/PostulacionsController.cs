using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UdmFundamentos.Models;

namespace UdmFundamentos.Controllers
{
    public class PostulacionsController : Controller
    {
        private UDMdbEntities db = new UDMdbEntities();

        // GET: Postulacions
        public ActionResult Index()
        {
            var postulacion = db.Postulacion.Include(p => p.Estudiante).Include(p => p.Materia);
            return View(postulacion.ToList());
        }

        // GET: Postulacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postulacion postulacion = db.Postulacion.Find(id);
            if (postulacion == null)
            {
                return HttpNotFound();
            }
            return View(postulacion);
        }

        // GET: Postulacions/Create
        public ActionResult Create()
        {
            ViewBag.estudiante_id = new SelectList(db.Estudiante, "id", "cedula");
            ViewBag.materia_id = new SelectList(db.Materia, "id", "nombre");
            return View();
        }

        // POST: Postulacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,estudiante_id,materia_id,razon,nota_materia")] Postulacion postulacion)
        {
            if (ModelState.IsValid)
            {
                db.Postulacion.Add(postulacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.estudiante_id = new SelectList(db.Estudiante, "id", "cedula", postulacion.estudiante_id);
            ViewBag.materia_id = new SelectList(db.Materia, "id", "nombre", postulacion.materia_id);
            return View(postulacion);
        }

        // GET: Postulacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postulacion postulacion = db.Postulacion.Find(id);
            if (postulacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.estudiante_id = new SelectList(db.Estudiante, "id", "cedula", postulacion.estudiante_id);
            ViewBag.materia_id = new SelectList(db.Materia, "id", "nombre", postulacion.materia_id);
            return View(postulacion);
        }

        // POST: Postulacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,estudiante_id,materia_id,razon,nota_materia")] Postulacion postulacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postulacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.estudiante_id = new SelectList(db.Estudiante, "id", "cedula", postulacion.estudiante_id);
            ViewBag.materia_id = new SelectList(db.Materia, "id", "nombre", postulacion.materia_id);
            return View(postulacion);
        }

        // GET: Postulacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Postulacion postulacion = db.Postulacion.Find(id);
            if (postulacion == null)
            {
                return HttpNotFound();
            }
            return View(postulacion);
        }

        // POST: Postulacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Postulacion postulacion = db.Postulacion.Find(id);
            db.Postulacion.Remove(postulacion);
            db.SaveChanges();
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
