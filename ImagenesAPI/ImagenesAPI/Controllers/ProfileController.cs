using ImagenesAPI.Data;
using ImagenesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImagenesAPI.Controllers
{
    public class ProfileController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Consultar(string id)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ImgData.GetImage(id));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }

        [System.Web.Http.HttpPost]
        public List<OutputMessage> Subir(string id, [FromForm] Microsoft.AspNetCore.Http.IFormFile img)
        {
            return ImgData.CrearPerfil(id, img);
        }

        [System.Web.Http.HttpDelete]
        public List<OutputMessage> Eliminar(string id)
        {
            return ImgData.DropProfile(id);
        }
    }
}
