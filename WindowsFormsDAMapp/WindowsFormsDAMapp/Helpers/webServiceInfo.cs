using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDAMapp.Helpers
{
    public class webServiceInfo
    {
        const string cadenaConexion = @"https://webserviceproyectodam20190522124503.azurewebsites.net";

        public string CadenaConexion
        {
            get { return cadenaConexion; }
        }
    }
}
