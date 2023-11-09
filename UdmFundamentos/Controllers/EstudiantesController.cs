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
    public class EstudiantesController : Controller
    {
        private UDMdbEntities db = new UDMdbEntities();

        // GET: Estudiantes
        public ActionResult Index()
        {
            var estudiante = db.Estudiante.Include(e => e.Usuario);
            return View(estudiante.ToList());
        }

        // GET: Estudiantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        public ActionResult Create()
        {
            // Filtrar solo usuarios de tipo "Estudiante"
            var usuariosEstudiantes = db.Usuario.Where(u => u.tipo == "Estudiante");
            ViewBag.usuario_id = new SelectList(usuariosEstudiantes, "id", "correo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cedula,nombre,apellido,promedio_general,usuario_id")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Estudiante.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Filtrar solo usuarios de tipo "Estudiante"
            var usuariosEstudiantes = db.Usuario.Where(u => u.tipo == "Estudiante");
            ViewBag.usuario_id = new SelectList(usuariosEstudiantes, "id", "correo", estudiante.usuario_id);
            return View(estudiante);
        }



        // GET: Estudiantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_id = new SelectList(db.Usuario, "id", "correo", estudiante.usuario_id);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cedula,nombre,apellido,promedio_general,usuario_id")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usuario_id = new SelectList(db.Usuario, "id", "correo", estudiante.usuario_id);
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiante.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiante estudiante = db.Estudiante.Find(id);

            // Remove all Postulacion that reference the Estudiante
            var postulaciones = db.Postulacion.Where(p => p.estudiante_id == id);
            db.Postulacion.RemoveRange(postulaciones);

            db.Estudiante.Remove(estudiante);
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


        public ActionResult HacerPostulacion()
        {
            // Asegúrate de que el estudiante está logueado
            if (Session["EstudianteId"] == null)
            {
                return View("~/Views/Flux/LoginEstudiante.cshtml");
            }

            // Obtén el ID del estudiante de la sesión
            int estudianteId = (int)Session["EstudianteId"];

            // Obtén las materias que el estudiante ya ha cursado
            var materiasCursadas = db.Estudiante_Materia
                .Where(em => em.estudiante_id == estudianteId)
                .Select(em => em.Materia)
                .ToList();

            // Crea un nuevo objeto Postulacion
            Postulacion postulacion = new Postulacion
            {
                estudiante_id = estudianteId
            };

            // Pasa el objeto Postulacion y las materias cursadas a la vista
            ViewBag.MateriasCursadas = new SelectList(materiasCursadas, "id", "nombre");
            return View("~/Views/Estudiantes/Postulacion.cshtml", postulacion);
        }

        [HttpPost]
        public ActionResult HacerPostulacion(Postulacion postulacion)
        {
            if (ModelState.IsValid)
            {
                // Verifica si ya existe una postulación para el estudiante y la materia específicos
                var postulacionExistente = db.Postulacion
                    .FirstOrDefault(p => p.estudiante_id == postulacion.estudiante_id && p.materia_id == postulacion.materia_id);

                if (postulacionExistente != null)
                {
                    // Si ya existe una postulacion, muestra un mensaje de error
                    ModelState.AddModelError("", "Ya has postulado a esta materia.");
                }
                else
                {
                    // Si no existe una postulación, agrega la nueva postulación al contexto de la base de datos
                    db.Postulacion.Add(postulacion);

                    // Guarda los cambios en la base de datos
                    db.SaveChanges();

                    // Redirige al usuario a una página de confirmación o a donde prefieras
                    return RedirectToAction("HacerPostulacion", "Estudiantes");
                }
            }

            // Si algo salió mal, vuelve a establecer los datos necesarios en ViewBag
            int estudianteId = (int)Session["EstudianteId"];
            var materiasCursadas = db.Estudiante_Materia
                .Where(em => em.estudiante_id == estudianteId)
                .Select(em => em.Materia)
                .ToList();
            ViewBag.MateriasCursadas = new SelectList(materiasCursadas, "id", "nombre");

            // Si algo salió mal, vuelve a mostrar el formulario
            return View("~/Views/Estudiantes/Postulacion.cshtml");
        }





         
    }
}
