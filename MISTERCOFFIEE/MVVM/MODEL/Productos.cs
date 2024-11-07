using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODEL
{
    public class Productos
    {
        public string? Id { get; set; }
        public string Producto { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int Precio_venta{ get; set; }
        public string Foto { get; set; }
        public string Categoria { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
