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

        public List<OutputMessage> Subir([FromForm] ProfileModel img, string id)
        {
            return ImgData.CrearPerfil(id, img.Image);
        }

        [System.Web.Http.HttpDelete]
        public List<OutputMessage> Eliminar(string id)
        {
            return ImgData.DropProfile(id);
        }
    }
}
