using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODEL
{
    public class User
    {
        public string Id { get; set; }
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
