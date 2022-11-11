using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImagenesAPI.Models;
using ImagenesAPI.Data;


namespace ImagenesAPI.Controllers
{
    public class UserController : ApiController
    {
        public List<User> Get(string id)
        {
            return UsuarioData.consultarUsuario(id);
        }

        public List<OutputMessage> Post([FromBody] User user)
        {
            return UsuarioData.crearUsuario(user);
        }

        public List<OutputMessage> Put(string id, [FromBody] User user)
        {
            return UsuarioData.actualizarUsuario(id, user);
        }

        public List<OutputMessage> Delete(string id)
        {
            return UsuarioData.eliminarUsuario(id);
        }
    }
}
