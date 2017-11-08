using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace XamarinControles.resources
{
    class Leer
    {

        public static List<Contacto> LeerArchivo(String ruta)
        {

            List<Contacto> contactos = new List<Contacto>();
            String edad;
            String nombre;
            String dni;

            var assembly = typeof(Leer).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(ruta);

            StreamReader objReader = new StreamReader(stream);

            do
            {

                nombre = objReader.ReadLine();
                edad = objReader.ReadLine();
                dni = objReader.ReadLine();
                if (nombre != null && edad != null && dni != null)
                {
                    contactos.Add(new Contacto(nombre, Int32.Parse(edad), dni));
                }

            } while (nombre != null && edad != null && dni != null);

            return contactos;
        }

        public static List<Contacto> LeerArchivoXML(String ruta)
        {
            var assembly = typeof(Leer).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(ruta);
            StreamReader objReader = new StreamReader(stream);
            var doc = XDocument.Load(stream);
            List<Contacto> contactos = new List<Contacto>();

            foreach (XElement element in doc.Root.Elements())
            {
                contactos.Add(new Contacto(element.Element("NOMBRE").Value, Int32.Parse(element.Element("EDAD").Value), element.Element("DNI").Value));
            }

            return contactos;

        }


    }
}
