﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Servicios;

namespace BLL
{
    public class UsuarioBLL
    {
        public List<UsuarioBE> ListarUsuarios() // Traer Lista de usuarios para ABM
        {
            List<UsuarioBE> Lista = new List<UsuarioBE>();
            UsuarioDAL dUsuario= new UsuarioDAL();
            return Lista = dUsuario.ListarUsuarios();
        }

        public UsuarioBE GetUsuarioLogin(string Mail) // Traer usuario para luego validar el login
        
        {
            UsuarioBE oUsuario = new UsuarioBE();
            UsuarioDAL dUsuario = new UsuarioDAL();
            oUsuario = dUsuario.LeerUsuario(Mail);
            return oUsuario;
        
        }

        public UsuarioBE SeleccionarUsuarioPorId(int Id) 
        {
            UsuarioDAL dUsuario = new UsuarioDAL();
            return dUsuario.SeleccionarUsuarioPorId(Id);
        }
        public ResultadoLogin Login (string Mail, string Password) 
        
        {
            if (SesionSingleton.Instancia.IsLogged())
                throw new Exception("La sesión ya está iniciada");

            UsuarioBE oUsuario = new UsuarioBE();
            PerfilComponenteBLL bllComp = new PerfilComponenteBLL();
            oUsuario = GetUsuarioLogin(Mail);
            bllComp.CargarPerfilUsuario(oUsuario);

            if (oUsuario.Mail == null) throw new ExceptionLogin(ResultadoLogin.UsuarioInvalido);



            if (!oUsuario.Password.Equals(Encriptador.Hash(Password)))
                throw new ExceptionLogin(ResultadoLogin.PasswordInvalido);
            else

            {
                SesionSingleton.Instancia.Login(oUsuario);
                return ResultadoLogin.UsuarioValido;
            }
 
        }

        public void GuardarPermisos(UsuarioBE Usuario)

        {
            UsuarioDAL uDal = new UsuarioDAL();
            uDal.GuardarPermisos(Usuario);
        }

        public void Alta (UsuarioBE Usuario) 
        
        {
            UsuarioDAL dUsuario = new UsuarioDAL();
            string Id= dUsuario.Alta(Usuario);

            BitacoraActividadBE nActividad = new BitacoraActividadBE();
            BitacoraBLL bllBit = new BitacoraBLL();
            nActividad.Clasificacion = (BitacoraClasifActividad)System.Enum.Parse(typeof(BitacoraClasifActividad), "Mensaje");
            nActividad.Detalle = "Se agregó el Usuario " + Id;
            bllBit.NuevaActividad(nActividad);

        }

        public void Editar(UsuarioBE Usuario)

        {
            UsuarioDAL dUsuario = new UsuarioDAL();
            dUsuario.Editar(Usuario);

            BitacoraActividadBE nActividad = new BitacoraActividadBE();
            BitacoraBLL bllBit = new BitacoraBLL();
            nActividad.Clasificacion = (BitacoraClasifActividad)System.Enum.Parse(typeof(BitacoraClasifActividad), "Mensaje");
            nActividad.Detalle = "Se modificó el Usuario " + Usuario.Id;
            bllBit.NuevaActividad(nActividad);

        }
   
        public void Logut ()
        
        {                 
                BitacoraActividadBE nAct = new BitacoraActividadBE();
                BitacoraBLL bllAct = new BitacoraBLL();
               
                nAct.Detalle = "Sesión cerrada con éxito";
                nAct.Clasificacion = (BitacoraClasifActividad)System.Enum.Parse(typeof(BitacoraClasifActividad), "Mensaje");
                bllAct.NuevaActividad(nAct);
                SesionSingleton.Instancia.Logout();
        }
    }
}
