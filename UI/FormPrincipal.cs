﻿using BLL;
using BE;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPrincipal : Form
    {

        UsuarioBLL bllUsuario;
        public FormPrincipal()
        {
            InitializeComponent();
            ValidarFormulario();
            bllUsuario = new UsuarioBLL();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void iniciarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin frmlogin = new FormLogin();
            frmlogin.MdiParent = this;
            frmlogin.Show();
        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUsuarios frmUsuario = new FormUsuarios();
            frmUsuario.MdiParent = this;
            frmUsuario.Show();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }

        public void ValidarFormulario() 
        
        {
            this.iniciarSesiónToolStripMenuItem.Enabled = !SesionSingleton.Instancia.IsLogged();
            this.cerrarSesiónToolStripMenuItem.Enabled= SesionSingleton.Instancia.IsLogged();
            this.administradorToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsLogged();
            this.presupuestosToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsLogged();
            this.pedidosToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsLogged();
            this.clientesToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsLogged();

            if (SesionSingleton.Instancia.IsLogged())
            {
                this.UsuarioBarraEstado.Text = SesionSingleton.Instancia.Usuario.Nombre + " "+ SesionSingleton.Instancia.Usuario.Apellido;
                ValidarPermisos();
            }
            else { this.UsuarioBarraEstado.Text = "Aún no ha iniciado sesión"; }
         
        }
        public void ValidarPermisos ()
        
        {
            this.usuariosToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoA); // ABM Usuarios
            this.permisosToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoB); // Perfiles de Acceso
            this.permisoPorUsuarioToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoC); // Asignacion de Perfil a Usuarios
            this.emitirPresupuestoToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoD); // Emitir Presupuestos
            this.aprobarPresupuestoToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoE); // Aprobacion Tecnica
            this.aprobaciónComercialToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoF); // Aprobacion Comercial
            this.visualizarPresupuestoToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoG); // Visualizar Presupuesto
            this.anularToolStripMenuItem.Enabled = SesionSingleton.Instancia.IsInRole(PerfilTipoPermisoBE.PermisoH); // Anular Presupuesto
        }
        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Está seguro que desea cerrar la sesión?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bllUsuario.Logut();
                if(ActiveMdiChild!=null)                    
                    ActiveMdiChild.Close();
                ValidarFormulario();
            }
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPerfiles fPerf = new FormPerfiles();
            fPerf.MdiParent = this;
            fPerf.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void permisoPorUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPerfilUsuario frmPerfUs = new FormPerfilUsuario();
            frmPerfUs.MdiParent = this;
            frmPerfUs.Show();
        }

        private void aprobaciónComercialToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aprobarPresupuestoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
