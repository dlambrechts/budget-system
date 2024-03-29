﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;


namespace Servicios
{
    public class Sesion
    {
        private UsuarioBE _usuario { get; set; }
        public UsuarioBE Usuario { get { return _usuario;} }
        public void Login (UsuarioBE Usuario) { _usuario = Usuario;  }
        public void Logout () {_usuario = null; }
        public bool IsLogged() { return _usuario != null; }

        bool isInRole(PerfilComponenteBE Comp, PerfilTipoPermisoBE Permiso, bool existe)
        {
            if (Comp.Permiso.Equals(Permiso))
                existe = true;
            else
            {
                foreach (var item in Comp.Hijos)
                {
                    existe = isInRole(item, Permiso, existe);
                    if (existe) return true;
                }
            }
            return existe;
        }

        public bool IsInRole(PerfilTipoPermisoBE Permiso)
        {
            bool existe = false;
            foreach (var item in _usuario.Permisos)
            {
                if (item.Permiso.Equals(Permiso))
                    return true;
                else
                {
                    existe = isInRole(item, Permiso, existe);
                    if (existe) return true;
                }
            }
            return existe;
        }

        static IList<IIdiomaObserver> _observers = new List<IIdiomaObserver>();
        public void SuscribirObs(IIdiomaObserver Obs) { _observers.Add(Obs);}

        public void DesuscribirObs(IIdiomaObserver Obs) { _observers.Remove(Obs); }

        public void CambiarIdioma (IdiomaBE Idioma) 
        
        { 
            if(_usuario!=null) { _usuario.Idioma = Idioma; Notificar(Idioma); }
        }
        private static void Notificar (IdiomaBE Idioma) 
        
        { 
            foreach (var Obs in _observers) { Obs.UpdateLanguage(Idioma); }
        }
    }
}
