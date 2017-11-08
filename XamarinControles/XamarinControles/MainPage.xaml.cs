using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinControles
{
    public partial class MainPage : ContentPage
    {
        List<Contacto> contactos = new List<Contacto>();
        ObservableCollection<Contacto> contactosMostrar;
        MatchCollection matches;

        public MainPage()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Al hacer click en el botón, se cargarán los contactos en la lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchFile_Clicked(object sender, EventArgs e)
        {
            if (PickerType.SelectedIndex == 0)
            {
                contactos = resources.Leer.LeerArchivo("XamarinControles.data.Info.txt");
                listUsers.ItemsSource = contactos;
            }
            else
            {
                contactos = resources.Leer.LeerArchivoXML("XamarinControles.data.Info.xml");
                listUsers.ItemsSource = contactos;
            }
            
        }

        /// <summary>
        /// Al hacer click en filtrar se mostrarán sólo los contactos que cumplan el filtro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltrar_Clicked(object sender, EventArgs e)
        {
            int edad1;
            if (!txtMaxAge.Text.Equals("") && !txtMinAge.Text.Equals("") && !txtName.Text.Equals(""))
            {
                //Se hace control numerico
                controlNumerico();
            }
            else if (!txtMaxAge.Text.Equals("") && !txtMinAge.Text.Equals(""))
            {
                //Se hace control numerico
                controlNumerico();
            }
            else if (!txtMaxAge.Text.Equals(""))
            {
                
                if (int.TryParse(txtMaxAge.Text.Trim(), out edad1))
                {
                    //Se hace control numerico
                    realizarBusqueda();
                }
                else
                {
                    txtMaxAge.Text = txtMinAge.Text = "";
                    lanzarAdvertencia("Ningun campo edad puede componerse por letras, solo aceptan valores numéricos.");
                }
            }
            else if (!txtMinAge.Text.Equals(""))
            {
                if (int.TryParse(txtMinAge.Text.Trim(), out edad1))
                {
                    //Se hace control numerico
                    realizarBusqueda();
                }
                else
                {
                    txtMaxAge.Text = txtMinAge.Text = "";
                    lanzarAdvertencia("Ningun campo edad puede componerse por letras, solo aceptan valores numéricos.");
                }
            }
            else { realizarBusqueda(); }
        }

        /// <summary>
        /// Se mostrará un mensaje cuando haya un error.
        /// </summary>
        /// <param name="v"></param>
        private void lanzarAdvertencia(string v)
        {
            lblError.Text=v;
        }

        /// <summary>
        /// Se comprobará el valor del filtro de edad.
        /// </summary>
        private void controlNumerico()
        {
            int edad1, edad2;

            if (int.TryParse(txtMaxAge.Text.Trim(), out edad1) && int.TryParse(txtMinAge.Text.Trim(), out edad2))
            {
                if (int.Parse(txtMinAge.Text.ToString()) >= int.Parse(txtMaxAge.Text.ToString()))
                {
                    //Mostrar advertencia
                    lanzarAdvertencia("La edad mínima no puede ser mayor o igual a la máxima.");
                }
                else { realizarBusqueda(); }
            }
            else
            {
                txtMaxAge.Text = txtMinAge.Text = "";
                lanzarAdvertencia("Ningun campo edad puede componerse por letras, solo aceptan valores numéricos.");
            }
        }

        /// <summary>
        /// Se realizará la búsqueda por edad.
        /// </summary>
        /// <param name="edad1">edad mínima</param>
        /// <param name="edad2">edad máxima</param>
        private void BuscarPorEdad(int edad1, int edad2)
        {
            String name = txtName.Text;

            contactos.Clear();

            if (name.Equals(""))
            {
                foreach (Contacto contacto in contactos)
                {
                    if (contacto.edad <= edad2 && contacto.edad >= edad1)
                    {
                        contactos.Add(contacto);
                    }
                }
            }
            else
            {
                foreach (Contacto contacto in contactos)
                {
                    if(contacto.edad <= edad2 && contacto.edad >= edad1 && contacto.nombre.Equals(name))
                    {
                        contactos.Add(contacto);
                    }
                }
                    
            }
            
        }

        /// <summary>
        /// Se realizará la búsqueda simple por nombre.
        /// </summary>
        private void realizarBusqueda()
        {
            //Limpiamos los contactos cargados para volver a cargar los correctos
            contactosMostrar.Clear();
            //Se obtiene resultado de la busqueda con los valores introducidos y se carga en listView
            buscarContacto(txtName.Text);

        }

        /// <summary>
        /// Se comprobarán todos los contactos y se añadirán a la lista.
        /// </summary>
        /// <param name="filtro"></param>
        public void buscarContacto(string filtro)
        {
            /// Recorremos el array contactos y si encontramos una coincidencia la añadimos al arraylist resultado
            for (int i = 0; i < contactos.Count; i++)
            {
                if (comprobarNombre(contactos[i], filtro))
                {
                    contactosMostrar.Add(contactos[i]);
                }
            }

            //Si no se encontro ninguna coincidencia se informa
            if (contactosMostrar.Count == 0) { lanzarAdvertencia("No se encontro ninguna coincidencia, prueba de nuevo."); }
        }

        /// <summary>
        /// Comprobamos que el contacto cumpla con el filtro por nombre.
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="filtro">filtro</param>
        /// <returns></returns>
        public Boolean comprobarNombre(Contacto contacto, string filtro)
        {
            Boolean ok = false;
            Regex rgx = new Regex("%", RegexOptions.IgnoreCase);

            matches = rgx.Matches(filtro);

            /// Primero tenemos que controlar que en el filtro no hemos introducido mas de un %.
            if (matches.Count > 1)
            {
                lanzarAdvertencia("No puede indicar más de un % en una busqueda.");
            }
            else
            {
                /// Si no se ha escrito nada en el patron de busqueda, o solo se ha escrito %...
                if (filtro.Trim().Equals("") || filtro.Trim().Equals("%"))
                {
                    if (txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length == 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), true);
                    }
                    else if (txtMinAge.Text.Trim().Length == 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMaxAge.Text.Trim(), false);
                    }
                    else if (txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), txtMaxAge.Text.Trim());
                    }
                    else
                    {
                        ok = true;
                    }
                }
                /// Si el ultimo caracter es %...
                else if (filtro.Substring(filtro.Length - 1).Equals("%"))
                {
                    /// Quitamos el caracter % para poder usarlo como patron de busqueda.
                    filtro = filtro.Replace("%", "");
                    rgx = new Regex(String.Format("^" + filtro + ".*"), RegexOptions.IgnoreCase);
                    matches = rgx.Matches(contacto.nombre);
                    /// Si encuentra alguna coincidencia, devolvemos true siempre y cuando la edad tambien coincida.
                    if (matches.Count > 0 && txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length == 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), true);
                    }
                    else if (matches.Count > 0 && txtMinAge.Text.Trim().Length == 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMaxAge.Text.Trim(), false);
                    }
                    else if (matches.Count > 0 && txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), txtMaxAge.Text.Trim());
                    }
                    else if (matches.Count > 0)
                    {
                        ok = true;
                    }
                }
                /// Si en el patron de busqueda no hemos puesto como ultimo caracter un %...
                else
                {
                    rgx = new Regex("^" + filtro + "$", RegexOptions.IgnoreCase);
                    matches = rgx.Matches(contacto.nombre);
                    /// Si encuentra alguna coincidencia, devolvemos true siempre y cuando la edad tambien coincida.
                    if (matches.Count > 0 && txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length == 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), true);
                    }
                    else if (matches.Count > 0 && txtMinAge.Text.Trim().Length == 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMaxAge.Text.Trim(), false);
                    }
                    else if (matches.Count > 0 && txtMinAge.Text.Trim().Length > 0 && txtMaxAge.Text.Trim().Length > 0)
                    {
                        ok = comprobarEdad(contacto, txtMinAge.Text.Trim(), txtMaxAge.Text.Trim());
                    }
                    else if (matches.Count > 0)
                    {
                        ok = true;
                    }
                }
            }

            return ok;
        }

        /// <summary>
        /// Se comprueba que el contacto pase el filtro por edad
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="edad">edad</param>
        /// <param name="modoMayor">boolean modo mayor</param>
        /// <returns></returns>
        public Boolean comprobarEdad(Contacto contacto, string edad, Boolean modoMayor)
        {
            Boolean ok = false;

            if ((contacto.edad < Int32.Parse(edad) && !modoMayor) || ((contacto.edad) >= Int32.Parse(edad) && modoMayor))
            {
                ok = true;
            }

            return ok;
        }

        /// <summary>
        /// Se comprueba que el contacto pase el filtro por edad
        /// </summary>
        /// <param name="contacto">contacto</param>
        /// <param name="edadMin">edad minima</param>
        /// <param name="edadMax">edad maxima</param>
        /// <returns></returns>
        public Boolean comprobarEdad(Contacto contacto, string edadMin, string edadMax)
        {
            Boolean ok = false;

            if ((contacto.edad <= Int32.Parse(edadMax)) && (contacto.edad > Int32.Parse(edadMin)))
            {
                ok = true;
            }

            return ok;
        }
    }
}
