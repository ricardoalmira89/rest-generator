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

    public class EntityGenerator : BaseGenerator
    {
        public string entityName { get; set; }
        private List<Field> fields = new List<Field>();
        private string template;
        private TextInfo textInfo;

        public void start() {

            this.textInfo = new CultureInfo("en-US", false).TextInfo;

            this.AskName();

            Console.WriteLine();
            Console.WriteLine("Available types: String, Int, Datetime, Boolean, Object");
            Console.WriteLine();

            while (this.AskField()) {}

            this
                .generate()
                .save();
        }

        /// <summary>
        /// Pregunta por el nombre de la entidad
        /// </summary>
        private void AskName()
        {
            Console.Write("Entity Name: ");
            entityName = Console.ReadLine();
        }

        /// <summary>
        /// Pide campo a capo
        /// </summary>
        /// <returns></returns>
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

                if (kind == RestField.String)
                    this.AskLenght(f);

                fields.Add(f);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Type!");
            }

            this.AskRequired(f);

            if (f.Kind == RestField.Object)
                this.AskForeign(f);

            Console.WriteLine();

            return true;
        }

        /// <summary>
        /// Pregunta si el campo es requerido
        /// </summary>
        /// <param name="f"></param>
        private void AskRequired(Field f)
        {
            Console.Write("is required? [true] : ");
            string required = Console.ReadLine();

            f.Required = (required == "false" || required == "False") ? false : true;
        }

        /// <summary>
        /// Pregunta por el tamanno del campo si es String
        /// </summary>
        /// <param name="f"></param>
        private void AskLenght(Field f) {
            Console.Write("field lenght? [100] : ");
            string lenght = Console.ReadLine();
            f.Lenght = (lenght == "") ? "100" : lenght;
        }

        /// <summary>
        /// Pregunta por la entidad foranea, en caso de existir relacion
        /// </summary>
        /// <param name="f"></param>
        private void AskForeign(Field f) {

            Console.Write("Foreign Entity Name: [{0}] ", textInfo.ToTitleCase(f.Name));
            string foreignEntity = Console.ReadLine();

            if (foreignEntity == "")
                foreignEntity = textInfo.ToTitleCase(f.Name);
            
            f.ForeignEntity = foreignEntity;

        }

        /// <summary>
        /// Genera el texto completo de una entidad
        /// </summary>
        /// <returns></returns>
        public EntityGenerator generate() {

            //template = File.ReadAllText(@"Template\entity.t");
            template = this.loadTemplate("entity.t");

            Regex rgx = new Regex("{{ lowerclass }}");
            template = rgx.Replace(template, this.entityName.ToLower());

            rgx = new Regex("{{ classname }}");
            template = rgx.Replace(template, this.entityName);

            string props = this.getGeneratedProperties();

            rgx = new Regex("{{ properties }}");
            template = rgx.Replace(template, props);

            return this;
        }

        /// <summary>
        /// Genera el fichero de la Entidad
        /// </summary>
        public void save() {

            string path = Directory.GetCurrentDirectory() + @"\Models\";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory("Models");
            }

            File.WriteAllText(path + this.entityName + ".cs", this.template);
        }

        /// <summary>
        /// Obtiene las propiedades convertidas a texto, listas para meter en el fichero
        /// </summary>
        /// <returns></returns>
        private string getGeneratedProperties() {

            string result = "";
           

            foreach (Field field in this.fields)
            {
                string fieldTemplate = (field.ForeignEntity != null) 
                    ? this.generateForeignProperties(field) 
                    : this.generateStandardProperties(field);

                result = result + fieldTemplate + "\n\n";
            }

            return result;
        }

        /// <summary>
        /// Genera el texto de las propiedades de relacion M:1
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string generateForeignProperties(Field field) {
            //string fieldTemplate = File.ReadAllText(@"Template\relation-m1.t");
            string fieldTemplate = this.loadTemplate("relation-m1.t");

            string req = (field.Required) ? "[Required]" : "";
            Regex rgx = new Regex("{{ required }}");
            fieldTemplate = rgx.Replace(fieldTemplate, req);

            rgx = new Regex("{{ foreignEntity }}");
            fieldTemplate = rgx.Replace(fieldTemplate, field.ForeignEntity);

            rgx = new Regex("{{ foreignLower }}");
            fieldTemplate = rgx.Replace(fieldTemplate, field.ForeignEntity.ToLower());

            return fieldTemplate;
        }

        /// <summary>
        /// Genera el texto de las propiedades normales (string, int, etc)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string generateStandardProperties(Field field) {

            // string fieldTemplate = File.ReadAllText(@"Template\field.t");
            string fieldTemplate = this.loadTemplate("field.t");

            Regex rgx = new Regex("{{ column }}");
            fieldTemplate = rgx.Replace(fieldTemplate, field.Name.ToLower());

            rgx = new Regex("{{ prop }}");
            fieldTemplate = rgx.Replace(fieldTemplate, textInfo.ToTitleCase(field.Name));

            string req = (field.Required) ? "Required, " : "";
            rgx = new Regex("{{ required }}");
            fieldTemplate = rgx.Replace(fieldTemplate, req);


            string lenght = (field.Lenght != null)
                ? String.Format("[MaxLength({0}, ErrorMessage = \"{1} no debe exederse de {0} caracteres.\")]", field.Lenght, textInfo.ToTitleCase(field.Name))
                : "";

            rgx = new Regex("{{ lenght }}");
            fieldTemplate = rgx.Replace(fieldTemplate, lenght);

            return fieldTemplate;
        }
       
    }

    public class Field {

        private RestField kind;
        private string name;
        private bool required;
        private string foreignEntity;
        private string lenght;

        public Field() { }

        public Field(RestField kind, string name, bool required, string foreignEntity = null, string lenght = "100")
        {
            this.kind = kind;
            this.name = name;
            this.required = required;
            this.foreignEntity = foreignEntity;
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

        public string ForeignEntity
        {
            get { return foreignEntity; }
            set { foreignEntity = value; }
        }

        public string Lenght
        {
            get { return lenght; }
            set { lenght = value; }
        }

    }

    
}
