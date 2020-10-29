﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace DAL
{
    public class PedidoDAL
    {

        public string AltaPedido(PedidoBE Pedido)

        {
            Hashtable ParamCabecera = new Hashtable();                        // Cabecera
            ParamCabecera.Add("@IdCliente", Pedido.Cliente.Id);
            ParamCabecera.Add("@IdPresupuesto", Pedido.Presupuesto.Id);
            ParamCabecera.Add("@FechaEmision", Pedido.FechaEmision);
            ParamCabecera.Add("@FechaEntrega", Pedido.FechaEntrega);

            Acceso AccesoDB = new Acceso();
            string Id = AccesoDB.Escribir("sp_InsertarPedidoCabecera", ParamCabecera);

            foreach (PedidoItemBE item in Pedido.Items)               // Items

            {
                Hashtable ParamItems = new Hashtable();
                ParamItems.Add("@IdPedido", Convert.ToInt32(Id));
                ParamItems.Add("@IdProducto", item.Producto.Id);
                ParamItems.Add("@Cantidad", item.Cantidad);

                AccesoDB.Escribir("sp_InsertarPedidoItem", ParamItems);
            }

            return Id;
        }
        public List<PedidoBE> ListarPedidos() // Solo muestra la cabecera para el listado

        {
            List<PedidoBE> ListaPedidos = new List<PedidoBE>();
            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            DS = AccesoDB.LeerDatos("sp_ListarPedidos", null);

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {

                    ClienteBE Cliente = new ClienteBE();
                    Cliente.Id = Convert.ToInt32(Item["IdCliente"]);
                    Cliente.RazonSocial = Convert.ToString(Item["RazonSocial"]).Trim();
                    PresupuestoBE Presupuesto = new PresupuestoBE();
                    Presupuesto.Cliente = Cliente;
                    Presupuesto.Id = Convert.ToInt32(Item["IdPresupuesto"]);
                    PedidoBE oPedido = new PedidoBE(Cliente,Presupuesto);

                    oPedido.Id = Convert.ToInt32(Item[0]);
                    oPedido.FechaEmision = Convert.ToDateTime(Item["FechaEmision"]);
                    oPedido.FechaEntrega = Convert.ToDateTime(Item["FechaEntrega"]);

                    ListaPedidos.Add(oPedido);
                }

            }
            return ListaPedidos;
        }

        public List<PedidoItemBE> ObtenerItems(PedidoBE Pedido) 

        {          
            Acceso AccesoDB = new Acceso();
            DataSet DS = new DataSet();
            Hashtable Param = new Hashtable();         
            Param.Add("@IdPedido", Pedido.Id);
            DS = AccesoDB.LeerDatos("sp_ListarPedidoItems", Param);

            List<PedidoItemBE> Items = new List<PedidoItemBE>();

            if (DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow Item in DS.Tables[0].Rows)
                {
                    ProductoBE Producto= new ProductoBE();
                    Producto.Id = Convert.ToInt32(Item["IdProducto"]);
                    Producto.Descripcion = Convert.ToString(Item["Descripcion"]).Trim();
                    int Cantidad = Convert.ToInt32(Item["Cantidad"]);
                    PedidoItemBE _Item = new PedidoItemBE(Producto,Cantidad);
                    
                    Items.Add(_Item);
                }
            }
            return Items;
        }

        public void CambiarFechaEntrega(PedidoBE Pedido)

        {
            Hashtable Param = new Hashtable();                        
            Param.Add("@IdPedido", Pedido.Id);
            Param.Add("@Fecha", Pedido.FechaEntrega);

            Acceso AccesoDB = new Acceso();
            AccesoDB.Escribir("sp_EditarPedidoEntrega", Param);           
           
        }

    }
}
