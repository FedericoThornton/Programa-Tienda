using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data.SqlClient;

namespace Negocio
{
    public class RopaNegocio
    {
        public List<Ropa> listar()
        {
            List<Ropa> lista = new List<Ropa>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {

                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=TIENDA_ROPA; integrated security= true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Codigo, Nombre, A.Descripcion, ImagenUrl, M.Descripcion as Marca, C.Descripcion as Tipo, Precio, A.IdMarca, A.IdCategoria, A.Id from ARTICULOS_ROPA A, CATEGORIAS_ROPA C, MARCAS_ROPA M where C.Id = A.IdCategoria and M.Id = A.IdMarca";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Ropa aux = new Ropa();
                    aux.Id = (int)lector["Id"];
                    if (!(lector["Codigo"] is DBNull))
                        aux.Codigo = (string)lector["Codigo"];
                    if (!(lector["Nombre"] is DBNull))
                         aux.Nombre = (string)lector["Nombre"];
                    if (!(lector["Descripcion"] is DBNull))
                         aux.Descripcion = (string)lector["Descripcion"];
                    if (!(lector["Precio"] is DBNull))
                        aux.Precio = (decimal)lector["Precio"];
                    if (!(lector.IsDBNull(lector.GetOrdinal("ImagenUrl"))));
                        aux.ImagenUrl = (string)lector["ImagenUrl"];
                    aux.Tipo = new Categoria();
                    aux.Tipo.Id = (int)lector["IdCategoria"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Marca.Descripcion = (string)lector["Marca"];

                    lista.Add(aux);
                }



                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            } finally { conexion.Close(); }
         
        }

        public void modificar(Ropa modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS_ROPA set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @precio where Id = @id");
                datos.setearParametro("@codigo", modificar.Codigo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@IdMarca", modificar.Marca.Id);
                datos.setearParametro("@IdCategoria", modificar.Tipo.Id);
                datos.setearParametro("@ImagenUrl", modificar.ImagenUrl);
                datos.setearParametro("@precio", modificar.Precio);
                datos.setearParametro("@id", modificar.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            } finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Ropa ropa)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS_ROPA (Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdCategoria, IdMarca) values (@Codigo,@Nombre, @Descripcion,@ImagenUrl,@Precio,@IdCategoria, @IdMarca)");
                datos.setearParametro("@Codigo", ropa.Codigo);
                datos.setearParametro("@Nombre", ropa.Nombre);
                datos.setearParametro("@Descripcion", ropa.Descripcion);
                datos.setearParametro("@ImagenUrl", ropa.ImagenUrl);
                datos.setearParametro("@Precio", ropa.Precio);
                datos.setearParametro("@IdCategoria", ropa.Tipo.Id);
                datos.setearParametro("@IdMarca", ropa.Marca.Id);
            }
            catch (Exception ex)
            {

                throw ex;
            } finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos ();
                datos.setearConsulta("delete from ARTICULOS_ROPA where id = @id");
                datos.setearParametro("id", @id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            } 
        }

        public List<Ropa> Filtrar(string campo, string criterio, string filtro)
        {

            List<Ropa> lista = new List<Ropa> ();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Codigo, Nombre, A.Descripcion, ImagenUrl, M.Descripcion as Marca, C.Descripcion as Tipo, Precio, A.IdMarca, A.IdCategoria, A.Id from ARTICULOS_ROPA A, CATEGORIAS_ROPA C, MARCAS_ROPA M where C.Id = A.IdCategoria and M.Id = A.IdMarca and ";

                switch (campo)
                {

                   
                     
                    case "Precio":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Precio >" + filtro;
                                break;
                            case "Menor a":
                                consulta += "Precio <" + filtro;
                                break;

                            default:
                                consulta += "Precio =" + filtro;
                                break;
                        }

                        break;
                    case "Tipo":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "C.Descripcion like '" + filtro + "%'";
                                break;
                            case "termina con":
                                consulta += "C.Descripcion like '%" + filtro + "'";
                                break;

                            default:
                                consulta += "C.Descripcion like '%" + filtro + "%'";

                                break;
                        }

                        break;
                    case "Marca":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "M. Descripcion like '" + filtro + "%'";
                                break;
                            case "termina con":
                                consulta += "M. Descripcion like '%" + filtro + "'";
                                break;

                            default:
                                consulta += "M. Descripcion like '%" + filtro + "%'";

                                break;
                        }
                        break;

                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Ropa aux = new Ropa();
                    aux.Id = (int)datos.Lector["Id"];
                    if (!(datos.Lector["Codigo"] is DBNull))
                        aux.Codigo = (string)datos.Lector["Codigo"];
                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];
                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["Precio"] is DBNull))
                        aux.Precio = (decimal)datos.Lector["Precio"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")))) ;
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Tipo = new Categoria();
                    aux.Tipo.Id = (int)datos.Lector["IdCategoria"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(aux);

                }

                    return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
