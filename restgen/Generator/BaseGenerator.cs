using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace restgen.Generator
{
    public class BaseGenerator
    {
        private string templatesPath = ConfigurationManager.AppSettings["templates-path"];

        /// <summary>
        /// Carga un template
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string loadTemplate(string name) {
            return File.ReadAllText(this.templatesPath + name);
        }

    }
}
