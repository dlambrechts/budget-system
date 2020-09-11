﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace DAL
{
    public class ClienteDAL
    {
        public ClienteBE SeleccionarPorId(int Id)

        {
            Acceso AccesoDB = new Acceso();
            Hashtable Param = new Hashtable();
            DataSet Ds = new DataSet();
            ClienteBE oCliente = new ClienteBE();
            Param.Add("@Id", Id);

            Ds = AccesoDB.LeerDatos("sp_SeleccionarClientePorId", Param);

            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in Ds.Tables[0].Rows)
                {

                    oCliente.Id = Convert.ToInt32(Item["Id"]);
                    oCliente.RazonSocial = Item["RazonSocial"].ToString().Trim();
                    oCliente.Direccion = Item["Direccion"].ToString().Trim();
                    oCliente.CodigoPostal = Convert.ToInt32(Item["CodigoPostal"]);
                    oCliente.Telefono = Item["Telefono"].ToString().Trim();
                    oCliente.Mail = Item["Mail"].ToString().Trim();
                    oCliente.Tipo.Id = Item["Tipo"].ToString().Trim();
                    oCliente.Tipo.Tipo = Item["Tipo"].ToString().Trim();
                    oCliente.Cuit = Item["Cuit"].ToString().Trim();
                    oCliente.Contacto = Item["Contacto"].ToString().Trim();
                    oCliente.Activo = Convert.ToBoolean(Item["Activo"]);

                }

            }
            return oCliente;

        }
        public List<ClienteBE> ListarClientes()

        {
            List<ClienteBE> ListaClientes = new List<ClienteBE>();

            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            DS = AccesoDB.LeerDatos("sp_ListarClientes", null);

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    ClienteBE oCli = new ClienteBE();
                    oCli.Id = Convert.ToInt32(Item[0]);
                    oCli.RazonSocial = Convert.ToString(Item[1]).Trim();
                    oCli.Direccion = Convert.ToString(Item[2]).Trim();
                    oCli.CodigoPostal = Convert.ToInt32(Item[3]);
                    oCli.Telefono= Convert.ToString(Item[4]).Trim();
                    oCli.Mail = Convert.ToString(Item[5]).Trim();
                    oCli.Tipo.Id = Convert.ToString(Item[6]).Trim();
                    oCli.Tipo.Tipo = Convert.ToString(Item[7]).Trim();
                    oCli.Cuit = Convert.ToString(Item[8]).Trim();
                    oCli.Contacto = Convert.ToString(Item[9]).Trim();                   
                    oCli.Activo = Convert.ToBoolean(Item[10]);
                    oCli.UsuarioCreacion.Id = Convert.ToInt32(Item[11]);
                    oCli.FechaCreacion = Convert.ToDateTime(Item[12]);
                    if (Item[13]!=DBNull.Value) { oCli.UsuarioModificacion.Id = Convert.ToInt32(Item[13]); }
                    if (Item[14]!= DBNull.Value) { oCli.FechaModificacion = Convert.ToDateTime(Item[14]); }

                    ListaClientes.Add(oCli);
                }
               
            }
            return ListaClientes;
        }
        public string AltaCliente(ClienteBE nCli)

        {
            Hashtable Parametros = new Hashtable();

            Parametros.Add("@Razon", nCli.RazonSocial);
            Parametros.Add("@Direccion", nCli.Direccion);
            Parametros.Add("@CodigoPostal", nCli.CodigoPostal);
            Parametros.Add("@Tel", nCli.Telefono);
            Parametros.Add("@Mail", nCli.Mail);
            Parametros.Add("@Tipo", nCli.Tipo.Id);
            Parametros.Add("@Cuit", nCli.Cuit);
            Parametros.Add("@Contacto", nCli.Contacto);
            Parametros.Add("@UsuarioIns", nCli._UsuarioCreacion.Id);
            Parametros.Add("@FechaIns", nCli.FechaCreacion);

            Acceso AccesoDB = new Acceso();
            return AccesoDB.Escribir("sp_InsertarCliente", Parametros);
        }

        public void EditarCliente(ClienteBE nCli)

        {
            Hashtable Parametros = new Hashtable();

            Parametros.Add("@Id", nCli.Id);
            Parametros.Add("@Razon", nCli.RazonSocial);
            Parametros.Add("@Direccion", nCli.Direccion);
            Parametros.Add("@CodigoPostal", nCli.CodigoPostal);
            Parametros.Add("@Tel", nCli.Telefono);
            Parametros.Add("@Mail", nCli.Mail);
            Parametros.Add("@Tipo", nCli.Tipo.Id);
            Parametros.Add("@Cuit", nCli.Cuit);
            Parametros.Add("@Contacto", nCli.Contacto);
            Parametros.Add("@Activo", nCli.Activo);
            Parametros.Add("@UsuarioMod", nCli._UsuarioModificacion.Id);
            Parametros.Add("@FechaMod", nCli.FechaModificacion);


            Acceso AccesoDB = new Acceso();
            AccesoDB.Escribir("sp_EditarCliente", Parametros);
        }

        public void InsertarHistorico (ClienteVersionBE Version)
        
        {

            Acceso AccesoDB = new Acceso();
            Hashtable Parametros = new Hashtable();


            Parametros.Add("@Fecha", Version.Fecha);
            Parametros.Add("@UsVers", Version.UsuarioVersion.Id);
            Parametros.Add("@IdCli", Version.Cliente.Id);
            Parametros.Add("@Razon", Version.Cliente.RazonSocial);
            Parametros.Add("@Direccion", Version.Cliente.Direccion);
            Parametros.Add("@CodigoPostal", Version.Cliente.CodigoPostal);
            Parametros.Add("@Tel", Version.Cliente.Telefono);
            Parametros.Add("@Mail", Version.Cliente.Mail);
            Parametros.Add("@Tipo", Version.Cliente.Tipo.Id);
            Parametros.Add("@Cuit", Version.Cliente.Cuit);
            Parametros.Add("@Contacto", Version.Cliente.Contacto);
            Parametros.Add("@Activo", Version.Cliente.Activo);
            if (Version.Cliente.UsuarioModificacion.Id == 0) 
            { Parametros.Add("@UsuarioMod", DBNull.Value); }
            else { Parametros.Add("@UsuarioMod", Version.Cliente.UsuarioModificacion.Id); }
            
            if (Version.Cliente.FechaModificacion.Year!=1) 
            { Parametros.Add("@FechaMod", Version.Cliente.FechaModificacion); }
            else { Parametros.Add("@FechaMod", DBNull.Value); }

           string Id=AccesoDB.Escribir("sp_InsertarClienteVersion", Parametros);

            Hashtable ParametrosCampos = new Hashtable();

            ParametrosCampos.Add("@IdVers", Convert.ToInt32(Id));
            ParametrosCampos.Add("@FlagRazon", Version.Cambios.RazonSocial);
            ParametrosCampos.Add("@FlagDireccion", Version.Cambios.Direccion);
            ParametrosCampos.Add("@FlagCodigoPostal", Version.Cambios.CodigoPostal);
            ParametrosCampos.Add("@FlagTel", Version.Cambios.Telefono);
            ParametrosCampos.Add("@FlagMail", Version.Cambios.Mail);
            ParametrosCampos.Add("@FlagTipo", Version.Cambios.Tipo);
            ParametrosCampos.Add("@FlagCuit", Version.Cambios.Cuit);
            ParametrosCampos.Add("@FlagContacto", Version.Cambios.Contacto);
            ParametrosCampos.Add("@FlagActivo", Version.Cambios.Activo);

            AccesoDB.Escribir("sp_InsertarClienteVersionCampos", ParametrosCampos);


        }
        public void EliminarCliente(ClienteBE nCli)

        {
            Hashtable Parametros = new Hashtable();
            Parametros.Add("@Id", nCli.Id);
            Acceso AccesoDB = new Acceso();
            AccesoDB.Escribir("sp_EliminarCliente", Parametros);
        }

        public List<ClienteTipoBE> ListarTipoCliente()

        {
            List<ClienteTipoBE> TiposCliente = new List<ClienteTipoBE>();

            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            DS = AccesoDB.LeerDatos("sp_ListaClienteTipo", null);

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    ClienteTipoBE oTipo = new ClienteTipoBE();

                    oTipo.Id = Convert.ToString(Item[0]).Trim();
                    oTipo.Tipo = Convert.ToString(Item[1]).Trim();


                    TiposCliente.Add(oTipo);
                }

            }
            return TiposCliente;
        }

        public List<ClienteVersionBE> ListarVersionesClientePorId (ClienteBE Cli)

        {
            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            Hashtable Parametros = new Hashtable();
            Parametros.Add("@Cliente", Cli.Id);
            DS = AccesoDB.LeerDatos("sp_SeleccionarVersionesPorCliente", Parametros);

            List<ClienteVersionBE> Lista = new List<ClienteVersionBE>();

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    ClienteVersionBE Version = new ClienteVersionBE();

                    Version.IdVersion = Convert.ToInt32(Item[0]);
                    Version.Fecha = Convert.ToDateTime(Item[1]);
                    Version.UsuarioVersion.Nombre = Convert.ToString(Item[2]).Trim();
                    Version.UsuarioVersion.Apellido = Convert.ToString(Item[3]).Trim();

                    Lista.Add(Version);
                }

            }
            return Lista;
        }

        public ClienteVersionBE ObtenerVersionPorIdVersion(ClienteVersionBE Vers)

        {
            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            Hashtable Parametros = new Hashtable();
            Parametros.Add("@IdVers", Vers.IdVersion);
            DS = AccesoDB.LeerDatos("sp_SeleccionarVersionClientePorVersion", Parametros);
            ClienteVersionBE Ver = new ClienteVersionBE();

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    Ver.IdVersion = Convert.ToInt32(Item[0]);
                    Ver.Fecha= Convert.ToDateTime(Item[1]);
                    Ver.Cliente.RazonSocial = Convert.ToString(Item[2]);
                    Ver.Cliente.Direccion = Convert.ToString(Item[3]);
                    Ver.Cliente.CodigoPostal = Convert.ToInt32(Item[4]);
                    Ver.Cliente.Telefono = Convert.ToString(Item[5]);
                    Ver.Cliente.Mail = Convert.ToString(Item[6]);
                    Ver.Cliente.tipo.Id = Convert.ToString(Item[7]);
                    Ver.Cliente.tipo.Tipo = Convert.ToString(Item[8]);
                    Ver.Cliente.Cuit = Convert.ToString(Item[9]);
                    Ver.Cliente.Contacto = Convert.ToString(Item[10]);
                    if (Item[11] != DBNull.Value) { Ver.Cliente.FechaModificacion=Convert.ToDateTime(Item[11]);}
                }
            }
            return Ver;
        }

        public ClienteVersionCambiosBE ObtenerCamposAfectadorEnVersion(ClienteVersionBE Vers)

        {
            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            Hashtable Parametros = new Hashtable();
            Parametros.Add("@IdVers", Vers.IdVersion);
            DS = AccesoDB.LeerDatos("sp_SeleccionarVersionCamposPorId", Parametros);
            ClienteVersionBE Ver = new ClienteVersionBE();

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    Ver.IdVersion = Convert.ToInt32(Item[0]);
                    Ver.Cambios.RazonSocial = Convert.ToBoolean(Item[1]);
                    Ver.Cambios.Direccion = Convert.ToBoolean(Item[2]);
                    Ver.Cambios.CodigoPostal = Convert.ToBoolean(Item[3]);
                    Ver.Cambios.Telefono = Convert.ToBoolean(Item[4]);
                    Ver.Cambios.Mail = Convert.ToBoolean(Item[5]);
                    Ver.Cambios.Tipo = Convert.ToBoolean(Item[6]);
                    Ver.Cambios.Cuit = Convert.ToBoolean(Item[7]);
                    Ver.Cambios.Contacto = Convert.ToBoolean(Item[8]);
                    Ver.Cambios.Activo = Convert.ToBoolean(Item[9]);
                }
            }
            return Ver.Cambios;
        }
    }
}
