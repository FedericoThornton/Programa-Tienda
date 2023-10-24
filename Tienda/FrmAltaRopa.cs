using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace Tienda
{
    public partial class FrmAltaRopa : Form
    {
        private Ropa Ropa = null;
        OpenFileDialog archivo = null;
        public FrmAltaRopa()
        {
            InitializeComponent();
        }

        public FrmAltaRopa(Ropa Ropa)
        {
            InitializeComponent();
            this.Ropa = Ropa;
            Text = "Modificar prenda";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            RopaNegocio negocio = new RopaNegocio();
            try
            {
                if (Ropa == null)
                {
                    Ropa = new Ropa();
                }
                Ropa.Codigo = txtCodigo.Text;
                Ropa.Nombre = txtNombre.Text;
                Ropa.Descripcion = txtDescripcion.Text;
                Ropa.Precio = decimal.Parse(txtPrecio.Text);
                Ropa.ImagenUrl = txtUrlImagen.Text;
                Ropa.Tipo = (Categoria)cboTipo.SelectedItem;
                Ropa.Marca =(Marca)cboMarca.SelectedItem;

                if(Ropa.Id != 0)
                {
                    negocio.modificar(Ropa);
                    MessageBox.Show("Modificado exitosamente");
                } else
                {
                    negocio.agregar(Ropa);
                    MessageBox.Show("Agregado exitosamente");
                }
                // guarda imagen  si es local
                if ((archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP"))))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                }

                Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmAltaRopa_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoria = new CategoriaNegocio();
            MarcaNegocio marca = new MarcaNegocio();

            try
            {
                

                cboTipo.DataSource = categoria.listar();
                cboTipo.ValueMember = "Id";
                cboTipo.DisplayMember = "Descripcion";
                cboMarca.DataSource = marca.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if (Ropa!= null)
                {
                    txtCodigo.Text = Ropa.Codigo;
                    txtNombre.Text = Ropa.Nombre;
                    txtDescripcion.Text = Ropa.Descripcion;
                    txtPrecio.Text = Ropa.Precio.ToString();
                    txtUrlImagen.Text = Ropa.ImagenUrl;
                    cargarImagen(Ropa.ImagenUrl);
                    cboTipo.SelectedValue = Ropa.Tipo.Id;
                    cboMarca.SelectedValue = Ropa.Marca.Id;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxUrlImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxUrlImagen.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png");

            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog archivo = new OpenFileDialog();
                archivo.Filter = "jpg |*.jpg;|png|*.png|webp|*.webp";
                if (archivo.ShowDialog() == DialogResult.OK)

                    txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           

                // guardar imagen   

                // File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }
        }
    }

