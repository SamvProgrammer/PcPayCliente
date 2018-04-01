using PcPay.Code.Usuario;
using PcPay.Code.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PcPay.Forms.Pages
{
    /// <summary>
    /// Interaction logic for frmVtaSrvSeleccionar.xaml
    /// </summary>
    public partial class frmVtaSrvSeleccionar : Page
    {
        private string AuxXML { get; set; }
        private string AuxCat { get; set; }
        private string IdCategoria { get; set; }
        private string IdProducto { get; set; }
        private string IdProveedor { get; set; }
        private string st_capt_tel_imp { get; set; }
        public abrirFrm abrirFrmNuevo;
        private Dictionary<string, Categoria> aux { get; set; }
        public frmVtaSrvSeleccionar()
        {
            InitializeComponent();
        }

        private void cargandoVentana(object sender, RoutedEventArgs e)
        {
            Globales.VServicios.dbgSetVsUrl(Globales.URL_VTASERV);
            AuxXML = TypeUsuario.ProdsVtaServ;
            ObtenerCategorias();
            
        }

        private void ObtenerCategorias()
        {
            aux = Globales.getProductos();
            CboCategoria.ItemsSource = aux;
            CboCategoria.DisplayMemberPath = "Value.descripcion";
            CboCategoria.SelectedValuePath = "Value";
        }

        private void cambiaCategoria(object sender, SelectionChangedEventArgs e)
        {
            fraProducto.Visibility = Visibility.Visible;
            ComboBox combo = (ComboBox)sender;
            Categoria categoria = combo.SelectedValue as Categoria;
            IdCategoria = categoria.id_categoria;
            CboProductos.ItemsSource = null;
            CboProductos.Items.Clear();
            CboProductos.ItemsSource = categoria.listaProductos;
            st_capt_tel_imp = categoria.st_capt_tel_imp;
            CboProductos.DisplayMemberPath = "descripcion";
        }

        private void aceptarProducto(object sender, RoutedEventArgs e)
        {
           if(CboProductos.SelectedIndex == -1){
               Globales.MessageBoxMit("Debe seleccionar un producto");
               return;
           }
           productoCategoria aux = CboProductos.SelectedItem as productoCategoria;
           IdProducto = aux.id;
           IdProveedor = aux.id_proveedor;
           Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
           Globales.VServicios.dbgSetVsUrl(Globales.URL_VTASERV);
           Globales.VServicios.sndVsRequestTx(TypeUsuario.usu, TypeUsuario.Pass, TypeUsuario.Id_Company, TypeUsuario.Id_Branch, IdProveedor, IdCategoria, IdProducto);
           Mouse.OverrideCursor = null;
           if (!string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsIdProducto()))
           {
               if (st_capt_tel_imp != "1" && st_capt_tel_imp != "2")
               {
                   if (Globales.VServicios.getRspVsExistencia() == "0")
                   {
                       Globales.MessageBoxMit("El producto no se encuentra en existencia");
                       return;
                   }
                   else
                   {
                       if (Globales.VServicios.getRspVsEfectivo() == "1" && Globales.VServicios.getRspVsTarjeta() == "1")
                       {
                           //Se le debe preguntar al usuario la forma de pago.
                           this.PicSeleccionar.Visibility = Visibility.Visible;
                           this.fraCategoria.Visibility = Visibility.Hidden;
                           this.fraProducto.Visibility = Visibility.Hidden;
                       }
                       else
                       {
                           if (!string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsEfectivo()) && Globales.VServicios.getRspVsEfectivo() != "0")
                           {
                               //Se manda el flujo a la pantalla de cobro con efectivo
                               frmVtaSvrEfectivo efectivo = new frmVtaSvrEfectivo();
                               efectivo.abriFrmNuevo = abrirFrmNuevo;
                               efectivo.idCategoria = IdCategoria;
                               efectivo.idproducto = IdProducto;
                               efectivo.idProveedor = IdProveedor;
                               efectivo.st_capt_tel_imp = st_capt_tel_imp;
                               abrirFrmNuevo(efectivo);
                               return;
                           }
                           else if (!string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsTarjeta()) && Globales.VServicios.getRspVsTarjeta() != "0")
                           {
                               //Se manda la pantalla de cobro con tarjeta..
                               frmVtaSrvBanda tarjeta = new frmVtaSrvBanda();
                               tarjeta.abrirFrmNuevo = abrirFrmNuevo;
                               tarjeta.idCategoria = IdCategoria;
                               tarjeta.idProducto = IdProducto;
                               tarjeta.idProveedor = IdProveedor;
                               tarjeta.st_capt_tel_imp = st_capt_tel_imp;
                               abrirFrmNuevo(tarjeta);
                               return;
                           }
                           else
                           {

                               Globales.MessageBoxMit("No hay opciones para forma de pago");
                               return;
                           }
                       }
                   }
               }
               else {
                   if (Globales.VServicios.getRspVsEfectivo() == "1" && Globales.VServicios.getRspVsTarjeta() == "1")
                   {
                       fraCategoria.Visibility = Visibility.Hidden;
                       PicSeleccionar.Visibility = Visibility.Visible;
                   }
                   else {
                       if (!string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsEfectivo()) && Globales.VServicios.getRspVsEfectivo() != "0")
                       {
                           //Se manda el flujo a la pantalla de cobro con efectivo
                           //Código faltante
                           frmVtaSvrEfectivo efectivo = new frmVtaSvrEfectivo();
                           efectivo.abriFrmNuevo = abrirFrmNuevo;
                           efectivo.idCategoria = IdCategoria;
                           efectivo.idproducto = IdProducto;
                           efectivo.idProveedor = IdProveedor;
                           efectivo.st_capt_tel_imp = st_capt_tel_imp;
                           abrirFrmNuevo(efectivo);
                           return;
                       }
                       else if (Globales.VServicios.getRspVsTarjeta() != "0" && !string.IsNullOrWhiteSpace(Globales.VServicios.getRspVsTarjeta()))
                       {
                           frmVtaSrvBanda tarjeta = new frmVtaSrvBanda();
                           tarjeta.abrirFrmNuevo = abrirFrmNuevo;
                           tarjeta.idCategoria = IdCategoria;
                           tarjeta.idProducto = IdProducto;
                           tarjeta.idProveedor = IdProveedor;
                           tarjeta.st_capt_tel_imp = st_capt_tel_imp;
                           //if (!(Globales.TiempoAire.dbgSetReader()))
                           //{
                           //    Globales.MessageBoxMit("Imposible establecer comunicacion con el lector");
                           //    return;
                           //}
                           abrirFrmNuevo(tarjeta);
                       }
                       else {
                           Globales.MessageBoxMit("No hay opciones par forma de pago.");
                           return;
                       }
                       
                   }
               }
           }
           else
           {

               Globales.MessageBoxMit("No hay informacion en la consulta, intente nuevamente");

           }
        }

        private void BSALIR_Click(object sender, RoutedEventArgs e)
        {
            Globales.botonSalir();
        }
    }
}
