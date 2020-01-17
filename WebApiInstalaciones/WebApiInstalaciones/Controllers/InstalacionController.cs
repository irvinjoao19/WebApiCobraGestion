using Entidades;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace WebApiInstalaciones.Controllers
{
    public class InstalacionController : ApiController
    {
        [HttpPost]
        [Route("api/Instalacion/Login")]
        public IHttpActionResult GetLogin(Query q)
        {
            Usuario u = NegocioDao.GetOne(q);

            if (u != null)
            {
                if (u.mensaje == "Pass")
                {
                    return BadRequest("Contraseña Incorrecta");
                }
                else
                {
                    return Ok(u);
                }

            }
            else return BadRequest("Usuario no existe");

        }

        [HttpGet]
        [Route("api/Instalacion/Encriptar")]
        public IHttpActionResult GetEncriptar(string nombre, bool activo)
        {
            string login = NegocioDao.EncriptarClave(nombre, activo);
            return Ok(login);
        }

        [HttpPost]
        [Route("api/Instalacion/GetSincronizar")]
        public IHttpActionResult GetSincronizar(Query q)
        {
            try
            {
                return Ok(NegocioDao.GetSincronizar(q));
            }
            catch (Exception)
            {
                return BadRequest("No puedes Sincronizar");
            }
        }

        // Save Registro Parte Diario
        [HttpPost]
        [Route("api/Instalacion/SaveParteDiario")]
        public IHttpActionResult SaveParteDiario()
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                var files = HttpContext.Current.Request.Files;
                var testValue = HttpContext.Current.Request.Form["data"];
                ParteDiario p = JsonConvert.DeserializeObject<ParteDiario>(testValue);

                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);
                    files[i].SaveAs(path + fileName);
                }

                Mensaje mensaje = NegocioDao.SaveRegistro(p);
                return Ok(mensaje);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        // SOLICITUD

        [HttpPost]
        [Route("api/Instalacion/SaveGeneralSolicitud")]
        public IHttpActionResult SaveGeneralSolicitud(Solicitud s)
        {
            Mensaje m = NegocioDao.SaveRegistroGeneral(s);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error");
        }

        [HttpPost]
        [Route("api/Instalacion/SaveRegistroSolicitudMaterial")]
        public IHttpActionResult SaveRegistroDetalle(RegistroMaterialSolicitud r)
        {
            Mensaje m = NegocioDao.SaveRegistroDetalle(r);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error Verificar");

        }

        [HttpPost]
        [Route("api/Instalacion/SaveRegistroPhoto")]
        public IHttpActionResult SaveRegistroPhoto()
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                var files = HttpContext.Current.Request.Files;
                var testValue = HttpContext.Current.Request.Form["data"];
                RegistroPhoto r = JsonConvert.DeserializeObject<RegistroPhoto>(testValue);

                if (r.tipo == 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Path.GetFileName(files[i].FileName);
                        files[i].SaveAs(path + fileName);
                    }
                }
                else
                {
                    string fileName = Path.GetFileName(files[0].FileName);
                    string foto = path + fileName;
                    if (File.Exists(foto))
                    {
                        try
                        {
                            File.Delete(foto);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }

                Mensaje mensaje = NegocioDao.SaveRegistroPhoto(r);
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // STOCK SOLICITUD

        [HttpPost]
        [Route("api/Instalacion/GetPaginationSolicitud")]
        public IHttpActionResult GetPaginationSolicitud(Query q)
        {
            List<Solicitud> m = NegocioDao.GetSolicitudes(q);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("No hay Solicitudes");
        }

        // STOCK DE MATERIALES
        [HttpPost]
        [Route("api/Instalacion/GetStockMaterial")]
        public IHttpActionResult GetStockMaterial(Query q)
        {
            List<Materiales> m = NegocioDao.GetStockMaterial(q);
            if (m != null)
            {
                return Ok(m);
            }
            else
            {
                return BadRequest("No hay Datos");
            }
        }

        [HttpPost]
        [Route("api/Instalacion/Aprobation")]
        public IHttpActionResult Aprobation(Query q)
        {
            Mensaje m = NegocioDao.GetAprobation(q);
            if (m != null)
            {
                return Ok(m);
            }
            else
            {
                return BadRequest("Error verificar");
            }
        }

        // SERVICIOS

        [HttpPost]
        [Route("api/Servicio/SaveOperarioGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario o)
        {
            Mensaje m = NegocioDao.SaveOperarioGps(o);
            if (m != null)
                return Ok(m);
            else
                return BadRequest("Error");
        }

        [HttpPost]
        [Route("api/Servicio/SaveEstadoMovil")]
        public IHttpActionResult SaveEstadoMovil(EstadoMovil e)
        {
            Mensaje m = NegocioDao.SaveEstadoMovil(e);
            if (m != null)
                return Ok(m);
            else
                return BadRequest("Error");
        }
    }
}
