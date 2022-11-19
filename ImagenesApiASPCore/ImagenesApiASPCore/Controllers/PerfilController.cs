using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ImagenesApiASPCore.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly string cadenaSQL;
        public PerfilController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("image/{id}")]
        public FileStream GetImage(string id)
        {
            try
            {
                string path = "D:\\System32\\CSharp\\img-api-asp\\ImagenesApiASPCore\\ImagenesApiASPCore\\IMAGENES\\";
                string image_name;
                using(var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("select_perfil", conexion);
                    cmd.Parameters.AddWithValue("id_usr", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        rd.Read();
                        image_name = rd["Img_perfil"].ToString();
                        rd.Close();
                    }
                    conexion.Close();
                }
                if(image_name != "")
                {
                    path += image_name;
                    FileStream perfil = System.IO.File.OpenRead(path);
                    return perfil;
                }
                else
                {
                    path += "default.png";
                    FileStream perfil = System.IO.File.OpenRead(path);
                    return perfil;
                }

            }
            catch(Exception err)
            {
                string defaultImg = "D:\\System32\\CSharp\\img-api-asp\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\defualt.png";
                FileStream perfil = System.IO.File.OpenRead(defaultImg);
                return perfil;
            }
        }
    }
}
