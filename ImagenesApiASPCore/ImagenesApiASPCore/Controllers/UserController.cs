using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ImagenesApiASPCore.Models;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace ImagenesApiASPCore.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly string cadenaSQL;
        public UserController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }
        [HttpGet]
        [Route("usuario/{id}")]
        public IActionResult Get(string id)
        {
            List<User> user = new List<User>();
            try
            {
                using( var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("select_usr", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        user.Add(new User()
                        {
                            Nombre = rd["Nombre"].ToString(),
                            Apellido = rd["Apellido"].ToString()
                        });
                    }
                    conexion.Close();
                }
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = user });
            }
            catch(Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message, response = user });
            }
        }

        [HttpPost]
        [Route("usuario/")]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                string contraseña = BC.HashPassword(user.contraseña, 10);
                using(var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("create_usr", conexion);
                    cmd.Parameters.AddWithValue("id_usr", user.id_usr);
                    cmd.Parameters.AddWithValue("contraseña", contraseña);
                    cmd.Parameters.AddWithValue("Nombre", user.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", user.Apellido);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        int err = Convert.ToInt32(rd["Error"].ToString());
                        if (err == 1)   
                        {
                            throw new Exception(rd["Respuesta"].ToString());
                        }
                    }
                    conexion.Close();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario creado correctamente" });
            }
            catch(Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message });
            }
        }

        [HttpPut]
        [Route("usuario/{id}")]
        public IActionResult Pull(string id, [FromBody] User user)
        {
            try
            {
                string contraseña = BC.HashPassword(user.contraseña, 10);
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("actualizar_usr", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.Parameters.AddWithValue("contraseña", contraseña);
                    cmd.Parameters.AddWithValue("Nombre", user.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", user.Apellido);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        if (Convert.ToInt32(rd["Error"].ToString()) == 1)
                        {
                            throw new Exception(rd["Respuesta"].ToString());
                        }
                    }
                    conexion.Close();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario actualizado correctamente" });
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message });
            }
        }

        [HttpDelete]
        [Route("usuario/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                using(var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("eliminar_usr", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using(var rd = cmd.ExecuteReader()) 
                    {
                        rd.Read();
                        int err = Convert.ToInt32(rd["Error"].ToString());
                        if (err == 1)
                        {
                            throw new Exception(rd["Respuesta"].ToString());
                        }
                    }
                    conexion.Close();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario eliminado correctamente" });
            }
            catch(Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = err.Message });
            }
        }
    }
}
