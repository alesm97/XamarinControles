using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinControles
{
    public class Contacto
    {
        /// <summary>
        /// Campos pertenecientes a Contacto.
        /// </summary>
        public String nombre;
        public int edad;
        public String NIF;

        /// <summary>
        /// Constructor del Contacto, el cual recibe nombre, edad y NIF.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="edad"></param>
        /// <param name="NIF"></param>
        public Contacto(String nombre, int edad, String NIF)
        {
            this.nombre = nombre;
            this.edad = edad;
            this.NIF = NIF;
        }

        /// <summary>
        /// Tostring en el cual mostramos los datos del contacto.
        /// </summary>
        /// <returns>Descripción del Contacto</returns>
        override
        public string ToString()
        {
            return string.Format("Nombre: {0} | Edad: {1} | NIF: {2}", nombre, edad, NIF);
        }
    }
}
