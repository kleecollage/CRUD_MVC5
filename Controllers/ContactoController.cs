using CRUD_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace CRUD_MVC5.Controllers
{
    public class ContactoController : Controller
    {

        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Contacto> oLista = new List<Contacto>();


        // GET: Contacto
        public ActionResult Index()
        {

            oLista = new List<Contacto>();

            using (SqlConnection oConexion = new SqlConnection(conexion)) {

                SqlCommand cmd = new SqlCommand("SELECT * FROM ContactoDB", oConexion);
                cmd.CommandType = CommandType.Text;
                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read() ){
                        Contacto nuevoContacto = new Contacto();

                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombre = dr["Nombre"].ToString();
                        nuevoContacto.Apellido = dr["Apellido"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        oLista.Add(nuevoContacto);

                    }
                }

            }

            return View(oLista);
        }



        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Registrar(Contacto oContacto)
        {
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_Registrar", oConexion);

                cmd.Parameters.AddWithValue("Nombre", oContacto.Nombre);
                cmd.Parameters.AddWithValue("Apellido", oContacto.Apellido);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);

                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Contacto");
        }


        [HttpGet]
        public ActionResult Editar(int? idContacto)
        {
            if(idContacto == null)
                return RedirectToAction("Index", "Contacto");

            Contacto oContacto = oLista.Where(c => c.IdContacto ==idContacto).FirstOrDefault();
            return View(oContacto);
        }



        [HttpPost]
        public ActionResult Editar(Contacto oContacto)
        {
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oConexion);
                cmd.Parameters.AddWithValue("IdContacto", oContacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombre", oContacto.Nombre);
                cmd.Parameters.AddWithValue("Apellido", oContacto.Apellido);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Contacto");
        }

        [HttpGet]
        public ActionResult Eliminar(int? idcontacto)
        {
            if (idcontacto == null)
                return RedirectToAction("Index", "Contacto");


            Contacto ocontacto = oLista.Where(c => c.IdContacto == idcontacto).FirstOrDefault();
            return View(ocontacto);
        }


        [HttpPost]
        public ActionResult Eliminar(string IdContacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", IdContacto);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Contacto");
        }
    }
}