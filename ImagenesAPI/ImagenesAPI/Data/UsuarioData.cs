using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImagenesAPI.Models;
using System.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Http;

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
                string insertQuery = $"execute create_usr    '{user.id_usr}', '{pass}', '{user.Nombre}', '{user.Apellido}'";
                SqlConnection sqlconnection = new SqlConnection("Data Source=ARMDFPCCIFSD036\\SQLEXPRESS;Initial Catalog = LoginImg; Integrated Security = True");

                sqlconnection.Open();

                SqlCommand query = new SqlCommand(insertQuery, sqlconnection);
                SqlDataReader reader = query.ExecuteReader();
                reader.Read();
                int err =Convert.ToInt32( reader["Error"].ToString());
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

        public static List<OutputMessage> actualizarUsuario(string id, User user)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            try
            {
                string pass = BC.HashPassword(user.contraseña);
                string insertQuery = $"execute actualizar_usr '{id}', '{pass}', '{user.Nombre}', '{user.Apellido}'";
                SqlConnection sqlconnection = new SqlConnection("Data Source=ARMDFPCCIFSD036\\SQLEXPRESS;Initial Catalog = LoginImg; Integrated Security = True");

                sqlconnection.Open();

                SqlCommand query = new SqlCommand(insertQuery, sqlconnection);
                SqlDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
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
                }
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

        public static List<OutputMessage> eliminarUsuario(string id)
        {
            List<OutputMessage> output = new List<OutputMessage>();
            try
            {
                string insertQuery = $"execute eliminar_usr '{id}'";
                SqlConnection sqlconnection = new SqlConnection("Data Source=ARMDFPCCIFSD036\\SQLEXPRESS;Initial Catalog = LoginImg; Integrated Security = True");

                sqlconnection.Open();

                SqlCommand query = new SqlCommand(insertQuery, sqlconnection);
                SqlDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
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
                }
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

        public static List<User> consultarUsuario(string id)
        {
            List<User> users = new List<User>();
            try
            {
                string sentencia = $"execute select_usr '{id}'";
                SqlConnection sqlconnection = new SqlConnection("Data Source=ARMDFPCCIFSD036\\SQLEXPRESS;Initial Catalog = LoginImg; Integrated Security = True");

                sqlconnection.Open();

                SqlCommand query = new SqlCommand(sentencia, sqlconnection);;
                SqlDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
                    users.Add(new User()
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        id_usr = id
                    });
                    return users;
                }
                else
                {
                    return users;
                }

            }
            catch(Exception err)
            {
                users = new List<User>();
                users.Add(new User()
                {
                    Nombre = err.Message
                });
                return users;
            }
        }
    }
}