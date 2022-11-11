using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImagenesAPI.Models;
using Microsoft.AspNetCore.Http;
using stream = System.IO.File;
using System.Data.SqlClient;
using System.Drawing;

namespace ImagenesAPI.Data
{
    public class ImgData
    {
        public static List<OutputMessage> CrearPerfil(string id, ProfileModel imgs)
        {
            try
            {
                List<OutputMessage> output = new List<OutputMessage>();
                SqlConnection sqlConnection = new SqlConnection("Data Source=MOISESPH;Initial Catalog = LoginImg; Integrated Security = True");
                string filePath = $"C:\\Users\\moise\\OneDrive\\Documentos\\SENA\\CSharp 3\\Imagenes\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\{imgs.Image.FileName}";
                var file = stream.Create(filePath);
                imgs.Image.CopyToAsync(file);
                string sentencia = $"execute actualizar_perfil '{id}', '{filePath}'";
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(sentencia, sqlConnection);
                SqlDataReader reader = query.ExecuteReader();
                reader.Read();
                int err = Convert.ToInt32(reader["Error"].ToString());
                if (err == 1)
                {
                    output.Add(new OutputMessage()
                    {
                        Error = true,
                        Respuesta = reader["Respuesta"].ToString()
                    });
                }
                else
                {
                    output.Add(new OutputMessage()
                    {
                        Error = false,
                        Respuesta = reader["Respuesta"].ToString()
                    });
                }
                reader.Close();
                sqlConnection.Close();
                return output;
            }
            catch (Exception err)
            {
                List<OutputMessage> output = new List<OutputMessage>();
                output.Add(new OutputMessage
                {
                    Error = true,
                    Respuesta = err.Message
                });
                return output;
            }
        }

        public static Image GetImage(string id)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection("Data Source=MOISESPH;Initial Catalog = LoginImg; Integrated Security = True");
                string sentencia = $"execute select_perfil '{id}'";
                SqlCommand query = new SqlCommand(sentencia, sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
                    string image = reader["Img_perfil"].ToString();
                    reader.Close();
                    if(image != "")
                    {
                        var file = stream.OpenRead(image);
                        Image perfil = Image.FromStream(file);
                        return perfil;
                    }
                    else
                    {
                        string defaultImg = "C:\\Users\\moise\\OneDrive\\Documentos\\SENA\\CSharp 3\\Imagenes\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\defualt.png";
                        var file = stream.OpenRead(defaultImg);
                        Image perfil = Image.FromStream(file);
                        return perfil;
                    }
                }
                else
                {
                    string defaultImg = "C:\\Users\\moise\\OneDrive\\Documentos\\SENA\\CSharp 3\\Imagenes\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\defualt.png";
                    var file = stream.OpenRead(defaultImg);
                    Image perfil = Image.FromStream(file);
                    return perfil;
                }
            }catch(Exception err)
            {
                Console.WriteLine(err.Message);
                string defaultImg = "C:\\Users\\moise\\OneDrive\\Documentos\\SENA\\CSharp 3\\Imagenes\\ImagenesAPI\\ImagenesAPI\\IMAGENES\\defualt.png";
                var file = stream.OpenRead(defaultImg);
                Image perfil = Image.FromStream(file);
                return perfil;
            }
        }

        public static List<OutputMessage> DropProfile(string id)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            try
            {
                SqlConnection sqlConnection = new SqlConnection("Data Source=MOISESPH;Initial Catalog = LoginImg; Integrated Security = True");
                string sentencia = $"execute eliminar_perfil '{id}'";
                SqlCommand query = new SqlCommand(sentencia, sqlConnection);
                SqlDataReader reader = query.ExecuteReader();
                reader.Read();
                int err = Convert.ToInt32(reader["Error"].ToString());
                if (err == 1)
                {
                    output.Add(new OutputMessage()
                    {
                        Error = true,
                        Respuesta = reader["Respuesta"].ToString()
                    });
                }
                else
                {
                    output.Add(new OutputMessage()
                    {
                        Error = false,
                        Respuesta = reader["Respuesta"].ToString()
                    });
                }
                reader.Close();
                sqlConnection.Close();
                return output;
            }
            catch (Exception err)
            {
                output.Add(new OutputMessage
                {
                    Error = true,
                    Respuesta = err.Message
                });
                return output;
            }
        }
    }   
}