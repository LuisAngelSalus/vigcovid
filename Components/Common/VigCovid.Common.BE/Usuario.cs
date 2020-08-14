using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigCovid.Common.BE
{
   public class Usuario
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordUsuario { get; set; }
        public int TipoUsuarioId { get; set; }

    }
}
