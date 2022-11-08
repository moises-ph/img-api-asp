using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImagenesAPI.Models;
using System.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;

namespace ImagenesAPI.Data
{
    public class UsuarioData
    {
        public static List<OutputMessage> crearUsuario(UserAuth user_aut, UserData user_data)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            try
            {
                string pass = BC.HashPassword(user_aut.contraseña);
                string insertQuery = $"execute create '{user_aut.id_usr}', '{pass}', '{user_data.Nombre}', '{user_data.Apellido}'";
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
    }
}