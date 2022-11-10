using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagenesAPI.Models
{
    public class User
    {
        public string id_usr { get; set; }
        public string contraseña { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Img_perfil { get; set; }
    }
}