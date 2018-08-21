using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace restgen.Generator
{
    class ControllerGenerator : BaseGenerator
    {
        private string entityName;
        private string template;

        public ControllerGenerator(string entityName) {
            this.entityName = entityName;      
        }

        public ControllerGenerator generate() {

            Console.WriteLine("Generating controller");
            //template = File.ReadAllText(@"Template\controller.t");
            template = this.loadTemplate("controller.t");

            Regex rgx = new Regex("{{ classname }}");
            template = rgx.Replace(template, this.entityName);

            return this;
        }

        /// <summary>
        /// Genera el fichero del controlador
        /// </summary>
        public void save()
        {
            Console.WriteLine("Writing controller to disk");

            string path = Directory.GetCurrentDirectory() + @"\Controllers\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory("Controllers");

            File.WriteAllText(path + this.entityName + "Controller.cs", this.template);
        }
    }
}
