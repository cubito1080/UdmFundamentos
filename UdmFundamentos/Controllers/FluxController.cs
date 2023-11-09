using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UdmFundamentos.Models;

namespace UdmFundamentos.Controllers
{
    public class FluxController : Controller
    {
        private UDMdbEntities db = new UDMdbEntities();
        // GET: Flux
        public ActionResult Index()
        {
            return View("~/Views/Flux/Index.cshtml");
        }



        public ActionResult MostrarLoginEstudiante()
        {
            return View("~/Views/Flux/LoginEstudiante.cshtml");
        }


        public ActionResult MostrarLoginAdministrador()
        {
            return View("~/Views/Flux/LoginAdministrador.cshtml");
        }




        [HttpGet]
        public ActionResult LogearAdministrador()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogearAdministrador(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verifica que el correo y la contraseña no sean nulos
                    if (string.IsNullOrEmpty(usuario.correo) || string.IsNullOrEmpty(usuario.contraseña))
                    {
                        throw new ArgumentException("El correo y la contraseña son requeridos.");
                    }

                    // Obtén el correo y la contraseña del usuario
                    string correo = usuario.correo;
                    string contraseña = usuario.contraseña;

                    // Verifica si el usuario existe en la base de datos y es de tipo 'Administrador'
                    var usuarioEncontrado = db.Usuario.FirstOrDefault(u => u.correo == correo && u.contraseña == contraseña && u.tipo == "Administrador");

                    if (usuarioEncontrado != null)
                    {
                        // Si el usuario existe, las credenciales son correctas y es de tipo 'Administrador', redirige al usuario a la página principal
                        return RedirectToAction("Index", "Administrador");
                    }
                    else
                    {
                        // Si el usuario no existe, las credenciales no son correctas o no es de tipo 'Administrador', muestra un mensaje de error
                        ViewBag.Error = "El correo o la contraseña son incorrectos, o el usuario no es un administrador.";
                    }
                }
            }
            catch (Exception)
            {
                // Aquí puedes manejar el error, por ejemplo, puedes registrar el error en un archivo de log
                // y mostrar un mensaje de error al usuario
                ViewBag.Error = "Ocurrió un error al intentar iniciar sesión. Por favor, inténtalo de nuevo más tarde.";
            }

            // Si las credenciales no son correctas, el usuario no es de tipo 'Administrador' o si ocurre un error, vuelve a mostrar la página de inicio de sesión
            return View("~/Views/Flux/LoginAdministrador.cshtml", usuario);
        }

        [HttpGet]
        public ActionResult LogearEstudiante()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogearEstudiante(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verifica que el correo y la contraseña no sean nulos
                    if (string.IsNullOrEmpty(usuario.correo) || string.IsNullOrEmpty(usuario.contraseña))
                    {
                        throw new ArgumentException("El correo y la contraseña son requeridos.");
                    }

                    // Obtén el correo y la contraseña del usuario
                    string correo = usuario.correo;
                    string contraseña = usuario.contraseña;

                    // Verifica si el usuario existe en la base de datos y es de tipo 'Estudiante'
                    var usuarioEncontrado = db.Usuario.FirstOrDefault(u => u.correo == correo && u.contraseña == contraseña && u.tipo == "Estudiante");

                    if (usuarioEncontrado != null)
                    {
                        // Si el usuario existe, las credenciales son correctas y es de tipo 'Estudiante', redirige al usuario a la vista de postulación
                        Session["EstudianteId"] = usuarioEncontrado.id;
                        return RedirectToAction("HacerPostulacion", "Estudiantes", new { id = usuarioEncontrado.id });
                    }
                    else
                    {
                        // Si el usuario no existe, las credenciales no son correctas o no es de tipo 'Estudiante', muestra un mensaje de error
                        ViewBag.Error = "El correo o la contraseña son incorrectos, o el usuario no es un estudiante.";
                    }
                }
            }
            catch (Exception)
            {
                // Aquí puedes manejar el error, por ejemplo, puedes registrar el error en un archivo de log
                // y mostrar un mensaje de error al usuario
                ViewBag.Error = "Ocurrió un error al intentar iniciar sesión. Por favor, inténtalo de nuevo más tarde.";
            }

            // Si las credenciales no son correctas, el usuario no es de tipo 'Estudiante' o si ocurre un error, vuelve a mostrar la página de inicio de sesión
            return View("~/Views/Flux/LoginEstudiante.cshtml", usuario);
        }






    }




}