using ImagenesAPI.Data;
using ImagenesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ImagenesAPI.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public HttpResponseMessage Consultar(string id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ImgData.GetImage(id));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public List<OutputMessage> Post(string id, [FromForm] Microsoft.AspNetCore.Http.IFormFile img)
        {
            return ImgData.CrearPerfil(id, img);
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public List<OutputMessage> Eliminar(string id)
        {
            return ImgData.DropProfile(id);
        }
    }
}