using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static VigCovid.Common.Resource.Enums;

namespace VigCovidApp.ViewModels
{
    public class ListaTrabajadoresViewModel
    {
        public int RegistroTrabajadorId { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Edad { get; set; }
        public string FechaIngreso { get; set; }
        public ModoIngreso ModoIngreso { get; set; }
        public string HistorialEnfermedad { get; set; }
        public string EstadoDiario { get; set; }
        public string Clasificacion { get; set; }
        public string UltimoComentario { get; set; }
        public string UltimoSeguimiento { get; set; }
        public bool Alerta { get; set; }

        public string ContadorSeguimiento { get; set; }
        public string DiaSinSintomas { get; set; }

        public int? EstadoClinicoId { get; set; }

    }
}