using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace restgen.Sync
{
    public class SyncManager
    {
        private List<string> references;

        public SyncManager() {
            references =  new List<string>() {
                "<Compile Include = \"Controllers\\{0}Controller.cs\" />",
                "<Compile Include = \"Models\\{0}.cs\" />",
                "<Compile Include = \"Repository\\{0}Repository.cs\" />"
            };
        }

        /// <summary>
        /// Obtiene el filename del fichero .csproj del proyecto externo
        /// </summary>
        /// <returns></returns>
        private string getCsprojFile() {
            string[] files =  Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.TopDirectoryOnly);

            if (files.Length > 0)
                return files[0];
            return null;
        }

        /// <summary>
        /// Almacena las referencias de los ficheros recien generados
        /// en el fichero csproj del proyecto externo
        /// </summary>
        public void writeReferencesCsproj(string entityName) {
            
            references = references.ConvertAll(element => String.Format(element, entityName));
            string fileText = File.ReadAllText(this.getCsprojFile());
            string pattern = "<Compile Include=\".+";

            // Salva el fichero por si se jode con extension .csproj~
            File.WriteAllText(this.getCsprojFile()+"~", fileText);

            Regex rgx = new Regex(pattern);
            Match m = rgx.Match(fileText);

            references.Add(m.Value);

            string textReferences = String.Join("\n", references);
            string firstMatch = m.Value.Replace(@"\", @"\\");
            rgx = new Regex(firstMatch);
            fileText = rgx.Replace(fileText, textReferences);

            File.WriteAllText(this.getCsprojFile(), fileText);
        }

        public string encode(string text)
        {
           return Regex.Replace(text, Regex.Escape(@"\"), Regex.Escape(@"\\"), RegexOptions.ExplicitCapture); 
        }
    }
}
