using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VigCovidApp.Models
{
    public class Enums
    {
      public enum ModoIngreso
        {
            Sintomatico =1,
            Sospechoso = 2,
            ContactoDirecto = 3,
            AsintomaticoPositivo = 4,
            Reingreso = 5,
            CovidConfirmado = 6
        }

        public enum ViaIngreso
        {
            Tamizaje = 1,
            Garita = 2,
            AreaTrabajo = 3,
            Domicilio = 4            
        }

        public enum CodigoEmpresa
        {
            SalusLaboris = 1,
            Backus = 2,
            Ambev = 3
        }

        public enum ResultadoCovid19
        {
            Negativo  = 0,
            Novalido = 1,
            IgMPositivo =2,
            IgGPositivo =3,
            IgMeIgGpositivo = 4,
            Noserealizo = 5            
        }

        public enum EstadoClinico
        {
            alta =1
        }

        public enum TipoEstado
        {
            cuarentena = 1,
            hospitalizado = 2,
            Fallecido = 3,
            Aislamiento = 4,
            AltaEpidemiologica = 5,
        }
    }
}