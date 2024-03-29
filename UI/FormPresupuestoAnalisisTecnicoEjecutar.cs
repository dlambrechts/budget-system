using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using Servicios;
using BLL;
using BE.PresupuestoEstado;

namespace UI
{
    public partial class FormPresupuestoAnalisisTecnicoEjecutar : Form
    {
        public FormPresupuestoAnalisisTecnicoEjecutar()
        {
            InitializeComponent();
            comboBoxAccion.DataSource = Enum.GetValues(typeof(Accion));
            
        }
        public enum Accion : int { Aprobar=1,Rechazar=2}
        public BE.PresupuestoBE oPresup;
        private BLL.PresupuestoBLL bllP = new PresupuestoBLL();


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void buttonConfirmar_Click(object sender, EventArgs e)
        {
            oPresup = bllP.SeleccionarPresupuestoPorId(oPresup.Id); // Actualizo datos del presupuesto por si fue aprobado por otro usuario

            DialogResult Respuesta = MessageBox.Show("Confirma "+ comboBoxAccion.Text + "Presupuesto?", comboBoxAccion.Text, MessageBoxButtons.YesNo);

              if (Respuesta == DialogResult.Yes)

             {
                PresupuestoTipoAprobacionBE Tipo = new PresupuestoTipoAprobacionBE();
                Tipo = bllP.SeleccionarAprobacionTipo("Tecnica");
                PresupuestoAprobacionBE nAprob = new PresupuestoAprobacionBE(oPresup, Tipo, SesionSingleton.Instancia.Usuario);

                if (((comboBoxAccion.Text=="Aprobar" && oPresup.Estado.AprobacionTecnica()==true))|| ((comboBoxAccion.Text == "Rechazar" && oPresup.Estado.RechazoTecnico() == true)))
                          { 

                            nAprob.Fecha = DateTime.Now;
                            nAprob.Accion = comboBoxAccion.Text;
                            nAprob.Observaciones = textBoxObs.Text;
                            PresupuestoBLL bllAp = new PresupuestoBLL();
                            
                                    if (nAprob.Accion == "Aprobar") 
                    
                                     {
                                         PresupuestoEstadoBE nEstado = new ApComPend();
                                         bllAp.ActualizarEstado(oPresup, nEstado);            
                                      }
                                     else 
                                     {
                                        PresupuestoEstadoBE nEstado = new ApTecRech();
                                        bllAp.ActualizarEstado(oPresup, nEstado);
                                      }

                                bllAp.AnalisisTecnico(nAprob);
                                MessageBox.Show("Operaci�n realizada correctamente");
                                this.Close();
                        }
                            else 
                
                         { 
                    
                            MessageBox.Show("No es posible realizar la acci�n en el Estado actual");


                           }
               }            
        }
        private void FormPresupuestoAnalisisTecnicoEjecutar_Load(object sender, EventArgs e)
        {
            textBoxPresup.Text = Convert.ToString(oPresup.Id);
        }
    }
}
