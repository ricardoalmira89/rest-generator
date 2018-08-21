using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;


namespace restgen.Generator
{

    public enum RestField
    {
        String,
        Int,
        DateTime,
        Boolean,
        Object
    };

    public class EntityGenerator
    {
        private string entityName { get; set; }
        private List<Field> fields = new List<Field>();

        public void start() {
            this.AskName();

            Console.WriteLine();
            Console.WriteLine("Available types: String, Int, Datetime, Boolean, Object");
            Console.WriteLine();

            while (this.AskField()) {}

            this.generate();
            Console.ReadLine();

        }

        private void AskName()
        {
            Console.Write("Entity Name: ");
            entityName = Console.ReadLine();
        }

        private bool AskField() {

            Field f = new Field();

            
            Console.Write("New field name (press <return> to stop adding fields): ");

            string name = Console.ReadLine();
            f.Name = name;

            if (name == "")
                return false;

            Console.Write("Field type [String]: ");
            string tipo = Console.ReadLine();

            try
            {
                RestField kind = (tipo == null) ? RestField.String : (RestField)Enum.Parse(typeof(RestField), (tipo != "") ? tipo : "String");
                f.Kind = kind;
                fields.Add(f);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Type!");
            }

            this.AskRequired(f);

            return true;
        }

        private void AskRequired(Field f)
        {
            Console.Write("is required? [true] : ");
            string required = Console.ReadLine();

            f.Required = (required == "false" || required == "False") ? false : true;
        }

        public void generate() {

            this.entityName = "Instalacion";

            string template = File.ReadAllText(@"Template\entity.t");

            Regex rgx = new Regex("{{ lowerclass }}");
            template = rgx.Replace(template, this.entityName.ToLower());

            rgx = new Regex("{{ classname }}");
            template = rgx.Replace(template, this.entityName);

            string props = this.getGeneratedProperties();

            rgx = new Regex("{{ properties }}");
            template = rgx.Replace(template, props);


            Console.WriteLine(template);

            Console.ReadKey();

        }

        /// <summary>
        /// Obtiene las propiedades convertidas a texto, listas para meter en el fichero
        /// </summary>
        /// <returns></returns>
        private string getGeneratedProperties() {

            string result = "";
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            foreach (Field field in this.fields)
            {
                string fieldTemplate = File.ReadAllText(@"Template\field.t");

                Regex rgx = new Regex("{{ column }}");
                fieldTemplate = rgx.Replace(fieldTemplate, field.Name.ToLower());

                rgx = new Regex("{{ prop }}");
                fieldTemplate = rgx.Replace(fieldTemplate, textInfo.ToTitleCase(field.Name));

                string req = (field.Required) ? "Required, " : "";
                rgx = new Regex("{{ required }}");
                fieldTemplate = rgx.Replace(fieldTemplate, req);

                result = result + fieldTemplate + "\n\n";
            }

            return result;
        }
       
    }

    public class Field {

        private RestField kind;
        private string name;
        private bool required;

        public Field() { }

        public Field(RestField kind, string name, bool required)
        {
            this.kind = kind;
            this.name = name;
            this.required = required;
        }

        public RestField Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

    }

    
}
