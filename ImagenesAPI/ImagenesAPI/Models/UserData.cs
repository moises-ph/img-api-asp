using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagenesAPI.Models
{
    public class UserData
    {
        public string id_usr { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public byte[] Img_perfil { get; set; }
    }
}