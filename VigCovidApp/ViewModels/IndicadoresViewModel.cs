﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static VigCovid.Common.Resource.Enums;

namespace VigCovidApp.ViewModels
{
    public class IndicadoresViewModel
    {
        public string Trabajador { get; set; }
        public string Empresa { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string FechaRegistro { get; set; }
        public string DiasEnSeguimiento { get; set; }
        public string FechaInicioSintomas { get; set; }
        public string NroDiasSinSintomas { get; set; }
        public string NroDiasPrueba { get; set; }
        public string ResultadoPrueba { get; set; }
        public int? Edad { get; set; }
        public string Dni { get; set; }
        public string PuestoTrabajo { get; set; }
        public ModoIngreso ModoIngreso { get; set; }
        public ViaIngreso ViaIngreso { get; set; }
        public string FechaIngreso { get; set; }
    }
}