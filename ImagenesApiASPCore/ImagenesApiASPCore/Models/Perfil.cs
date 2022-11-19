using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagenesApiASPCore.Models
{
    public class Perfil
    {
        public IFormFile Image { get; set; }
    }
}
