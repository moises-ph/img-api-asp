using ImagenesAPI.Data;
using ImagenesAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImagenesAPI.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        public Image Consultar(string id)
        {
            return ImgData.GetImage(id);
        }

        [HttpPost]
        public List<OutputMessage> Subir(string id, [Microsoft.AspNetCore.Mvc.FromForm] ProfileModel img)
        {
            return ImgData.CrearPerfil(id, img);
        }

        [HttpDelete]
        public List<OutputMessage> Eliminar(string id)
        {
            return ImgData.DropProfile(id);
        }
    }
}
