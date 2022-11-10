using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImagenesAPI.Models;
using System.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Http;
using stream = System.IO.File;

namespace ImagenesAPI.Data
{
    public class UsuarioData
    {
        public static List<OutputMessage> crearUsuario(User user)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            try
            {
                string pass = BC.HashPassword(user.contraseña);
                string insertQuery = $"execute create '{user.id_usr}', '{pass}', '{user.Nombre}', '{user.Apellido}'";
                SqlConnection sqlconnection = new SqlConnection("Data Source=MOISESPH;Initial Catalog = LoginImg; Integrated Security = True");

                sqlconnection.Open();

                SqlCommand query = new SqlCommand(insertQuery, sqlconnection);
                SqlDataReader reader = query.ExecuteReader();
                reader.Read();
                output.Add(new OutputMessage() { 
                    Error = Convert.ToBoolean(reader["Error"].ToString()),
                    Respuesta = reader["Respuesta"].ToString()
                });;
                reader.Close();
                sqlconnection.Close();
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

        public static List<OutputMessage> imgPerfil(List<IFormFile> images, string id)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            SqlConnection sqlconnection = new SqlConnection("Data Source=MOISESPH;Initial Catalog = LoginImg; Integrated Security = True");
            try
            {
                if(images.Count != 1)
                {
                    new Exception("Solo se puede una imagen");
                }
                else
                {
                    foreach(var image in images)
                    {
                        string filePath = "D:\\System32\\CSharp\\img-api-asp\\ImagenesAPI\\ImagenesAPI\\Profiles" + image.FileName;
                        var created = stream.Create(filePath);
                        image.CopyToAsync(created);
                        string sentencia = $"execute actualizar_perfil '{id}', '{filePath}'";
                        sqlconnection.Open();
                        SqlCommand query = new SqlCommand(sentencia, sqlconnection);
                        SqlDataReader reader = query.ExecuteReader();
                        reader.Read();
                        output.Add(new OutputMessage()
                        {
                            Error = Convert.ToBoolean(reader["Error"].ToString()),
                            Respuesta = reader["Respuesta"].ToString()
                        }); ;
                        reader.Close();
                        sqlconnection.Close();
                        return output;
                    }

                }
            }
            catch(Exception err)
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