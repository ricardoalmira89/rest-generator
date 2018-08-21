using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace restgen.Generator
{
    class RepositoryGenerator
    {
        private string entityName;
        private string template;

        public RepositoryGenerator(string entityName) {
            this.entityName = entityName;      
        }

        public RepositoryGenerator generate() {

            Console.WriteLine("Generating repository");
            template = File.ReadAllText(@"Template\repository.t");

            Regex rgx = new Regex("{{ classname }}");
            template = rgx.Replace(template, this.entityName);

            return this;
        }

        /// <summary>
        /// Genera el fichero del repositorio
        /// </summary>
        public void save()
        {
            Console.WriteLine("Writing repository to disk");

            string path = Directory.GetCurrentDirectory() + @"\Repository\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory("Repository");

            File.WriteAllText(path + this.entityName + "Repository.cs", this.template);
        }
    }
}
