using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Files = System.IO.File;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using ImagenesApiASPCore.Models;

namespace ImagenesApiASPCore.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly string cadenaSQL;
        public PerfilController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("image/{id}")]
        public FileStream GetImage(string id)
        {
            try
            {
                string path = "D:\\System32\\CSharp\\img-api-asp\\ImagenesApiASPCore\\ImagenesApiASPCore\\IMAGENES\\";
                string image_name;
                using(var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("select_perfil", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        image_name = rd["Img_perfil"].ToString();
                        rd.Close();
                    }
                    conexion.Close();
                }
                path += image_name;
                FileStream perfil = System.IO.File.OpenRead(path);
                return perfil;

            }
            catch(Exception err)
            {
                string defaultImg = "D:\\System32\\CSharp\\img-api-asp\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\defualt.png";
                FileStream perfil = System.IO.File.OpenRead(defaultImg);
                return perfil;
            }
        }


        [HttpPost]
        [Route("upimage/{id}")]
        public async Task<IActionResult> PostImage(string id, [FromForm] Perfil perfil)
        {
            try
            {
                string path = "D:\\System32\\CSharp\\img-api-asp\\ImagenesApiASPCore\\ImagenesApiASPCore\\IMAGENES\\";
                int error;
                string Respuesta;
                using (FileStream stream = new FileStream(path + perfil.Image.FileName, FileMode.Create))
                {
                    await perfil.Image.CopyToAsync(stream);
                }
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("actualizar_perfil", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.Parameters.AddWithValue("Img_perfil", perfil.Image.FileName);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        error = Convert.ToInt32(rd["Error"].ToString());
                        Respuesta = rd["Respuesta"].ToString();
                        rd.Close();
                    }
                    conexion.Close();
                }
                if (error == 1)
                {
                    throw new Exception(Respuesta);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { Mensaje = Respuesta });
                }
            }
            catch(Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message, Error = err });
            }
        }

        [HttpDelete]
        [Route("upimage/{id}")]
        public IActionResult DeleteImage(string id)
        {
            try
            {
                string path = "D:\\System32\\CSharp\\img-api-asp\\ImagenesApiASPCore\\ImagenesApiASPCore\\IMAGENES\\";
                int error;
                string Image_name;
                string Respuesta;
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd2 = new SqlCommand("select_perfil", conexion);
                    cmd2.Parameters.AddWithValue("id_usr", id);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd2 = cmd2.ExecuteReader())
                    {
                        rd2.Read();
                        Image_name = rd2["Img_perfil"].ToString();
                        rd2.Close();
                    }

                    var cmd = new SqlCommand("eliminar_perfil", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        error = Convert.ToInt32(rd["Error"].ToString());
                        Respuesta = rd["Respuesta"].ToString();
                        rd.Close();
                    }
                    conexion.Close();
                }

                Files.Delete(path + Image_name);

                if (error == 1)
                {
                    throw new Exception(Respuesta);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { Mensaje = Respuesta });
                }
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message, Error = err });
            }
        }
    }
}
