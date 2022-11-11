using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagenesAPI.Models
{
    public class ProfileModel
    {
        public IFormFile Image { get; set; }
    }
}