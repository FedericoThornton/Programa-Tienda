using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Ropa
    {
        public int Id { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public string ImagenUrl { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public Categoria Tipo { get; set; }

        public Marca Marca { get; set; }


        public decimal Precio { get; set; }

  
    }
}
