using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPerfilUsuario : Form
    {
        public BLL.UsuarioBLL bllUsuario;
        public BE.UsuarioBE beUsuario;
        public BLL.PerfilComponenteBLL bllComp;
        public BLL.PerfilFamilaBLL bllFam;
        public BLL.PerfilPatenteBLL bllPat;
        private BE.UsuarioBE tmpUs;

        public FormPerfilUsuario()
        {
            InitializeComponent();
            bllComp = new PerfilComponenteBLL();
            bllUsuario = new UsuarioBLL();
            bllFam = new PerfilFamilaBLL();
            bllPat = new PerfilPatenteBLL();
            comboUsuario.DataSource = bllUsuario.ListarUsuarios();
            comboGrupos.DataSource = bllFam.ObtenerFamilias();
            comboPermisos.DataSource = bllPat.ObtenerPatentes();
        }


        private void FormPerfilUsuario_Load(object sender, EventArgs e)
        {

        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            beUsuario = (UsuarioBE)this.comboUsuario.SelectedItem;

            tmpUs = new UsuarioBE();
            tmpUs.Id = beUsuario.Id;
            tmpUs.Nombre = beUsuario.Nombre;
            tmpUs.Apellido = beUsuario.Apellido;
            bllComp.CargarPerfilUsuario(tmpUs);
            MostrarPerfil(tmpUs);
        }
        public void MostrarPerfil(UsuarioBE Us)         
        {
            this.treeArbolPermisos.Nodes.Clear();
            TreeNode raiz = new TreeNode(beUsuario.ToString());
            
            foreach(var item in Us.Permisos)

            {
                LlenarTree(raiz, item);
            }

            this.treeArbolPermisos.Nodes.Add(raiz);
            this.treeArbolPermisos.ExpandAll();
        }

        public void LlenarTree (TreeNode Padre, PerfilComponenteBE Comp)
        {
            TreeNode Hijo = new TreeNode(Comp.Descripcion);
            Hijo.Tag = Comp;
            Padre.Nodes.Add(Hijo);

            foreach (var item in Comp.Hijos)
            {
                LlenarTree(Hijo, item);
            }
        }

        private void buttonAddGrupo_Click(object sender, EventArgs e)
        {
            if (tmpUs != null)
            {
                var Grupo = (PerfilFamiliaBE)comboGrupos.SelectedItem;
                if (Grupo != null)
                {
                    var existe = false;
                   
                    foreach (var item in tmpUs.Permisos)
                    {
                        if (bllComp.Existe(item, Grupo.Id))
                        {
                            existe = true;
                        }
                    }

                    if (existe)
                        MessageBox.Show("El usuario ya pertenece al Grupo");
                    else
                    {
                        
                            bllComp.CompletarComponentesFamilia(Grupo);
                            tmpUs.AgregarPermiso(Grupo);
                            MostrarPerfil(tmpUs);
                        
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un usuario");
        }

        private void buttonQuitarGrupo_Click(object sender, EventArgs e)
        {
            if (tmpUs != null)
            {
                var Grupo = (PerfilFamiliaBE)comboGrupos.SelectedItem;
                if (Grupo != null)
                {
                    var existe = false;

                    foreach (var item in tmpUs.Permisos)
                    {
                        if (bllComp.Existe(item, Grupo.Id))
                        {
                            existe = true;
                        }
                    }

                    if (!existe)
                        MessageBox.Show("El Usuario no pertenece al Grupo");
                    else
                    {
                        if (tmpUs.ExistePermisoExplisito(Grupo) == true)
                        {                            
                            tmpUs.QuitarPermiso(Grupo);
                            MostrarPerfil(tmpUs);
                        }

                        else MessageBox.Show("No se puede Quitar el Grupo. El Usuario no pertenece al Grupo seleccionado de forma directa.");
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un usuario");
        }
        private void buttonAddPerm_Click(object sender, EventArgs e)
        {
            if (tmpUs != null)
            {
                var Permiso = (PerfilPatenteBE)comboPermisos.SelectedItem;
                if (Permiso != null)
                {
                    var existe = false;

                    foreach (var item in tmpUs.Permisos)
                    {
                        if (bllComp.Existe(item, Permiso.Id))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (existe)
                        MessageBox.Show("El usuario ya posee el Permiso");
                    else
                    {
                        {
                            tmpUs.AgregarPermiso(Permiso);
                            MostrarPerfil(tmpUs);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un Usuario");
        }

        private void buttonQuitarPermiso_Click(object sender, EventArgs e)
        {
            if (tmpUs != null)
            {
                var Permiso = (PerfilPatenteBE)comboPermisos.SelectedItem;
                if (Permiso != null)
                {
                    var existe = false;

                    foreach (var item in tmpUs.Permisos)
                    {
                        if (bllComp.Existe(item, Permiso.Id))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe)
                        MessageBox.Show("El usuario no posee el Permiso");
                    else
                    {
                        {
                            if (tmpUs.ExistePermisoExplisito(Permiso) == true)
                            {
                                tmpUs.QuitarPermiso(Permiso);
                                MostrarPerfil(tmpUs);
                            }

                            else MessageBox.Show("No se puede Quitar el Permiso. El Usuario no tiene asignado el permiso de forma directa.");
                        }
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un Usuario");
        }
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bllUsuario.GuardarPermisos(tmpUs);
                MessageBox.Show("Perfil de Usuario Guardado Correctamente");
            }
            catch (Exception)
            {

                MessageBox.Show("Error al Guarda Perfil de Usuario");
            }
        }


    }
}
