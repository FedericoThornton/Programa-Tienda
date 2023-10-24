using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Tienda
{
    public partial class frmTienda : Form
    {
        private List<Ropa> listaRopa;
        private Ropa seleccionado;
        public frmTienda()
        {
            InitializeComponent();
        }

        private void frmTienda_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Tipo");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");

        }

        private void cargar()
        {
            RopaNegocio ropaNegocio = new RopaNegocio();
            try
            {

                listaRopa = ropaNegocio.listar();
                DgbRopa.DataSource = listaRopa;
                pbxRopa.Load(listaRopa[0].ImagenUrl);
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void ocultarColumnas()
        {
            DgbRopa.Columns["ImagenUrl"].Visible = false;
            DgbRopa.Columns["Id"].Visible = false;
        
        }
        private void DgbRopa_SelectionChanged(object sender, EventArgs e)
        {
            if(DgbRopa.CurrentRow != null)
            {
                Ropa seleccionado = (Ropa)DgbRopa.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxRopa.Load(imagen);
            }
            catch (Exception)
            {
                pbxRopa.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png");

            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaRopa alta  = new FrmAltaRopa();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Ropa seleccionado;
            seleccionado = (Ropa)DgbRopa.CurrentRow.DataBoundItem;
            FrmAltaRopa modificar = new FrmAltaRopa(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            RopaNegocio negocio = new RopaNegocio();
            Ropa seleccionado; 

            try
            {
                DialogResult respuesta = MessageBox.Show("Esto lo eliminará definitivamente","Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Ropa)DgbRopa.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
            {
                    MessageBox.Show("Por favor, cargue números");
                    return true;
                }
                if ((!(soloNumeros(txtFiltroAvanzado.Text)))) 
                {
                    MessageBox.Show("Por favor, cargue números");
                    return true;
                }
               
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            RopaNegocio negocio = new RopaNegocio();

            
            try
            { 
               if (validarFiltro())
                return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                DgbRopa.DataSource = negocio.Filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Ropa> listafiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
            {
                listafiltrada = listaRopa.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listafiltrada = listaRopa;
            }

            DgbRopa.DataSource = null;
            DgbRopa.DataSource = listafiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
