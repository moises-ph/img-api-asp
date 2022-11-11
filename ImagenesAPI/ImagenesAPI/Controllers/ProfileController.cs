using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing;
using ImagenesAPI.Models;
using ImagenesAPI.Data;
using Microsoft.AspNetCore.Http;

namespace ImagenesAPI.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        public Image Get(string id)
        {
            return ImgData.GetImage(id);
        }

        [HttpPost]
        public List<OutputMessage> Post(string id, [FromBody] List<IFormFile> img)
        {
            return ImgData.crearPerfil(id, img);
        }
        [HttpDelete]
        public List<OutputMessage> Delete(string id)
        {
            return ImgData.DropProfile(id);
        }

    }
}
