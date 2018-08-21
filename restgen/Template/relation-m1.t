	{{ required }}
        [ForeignKey("{{ foreignEntity }}")]
        public int {{ foreignLower }}_id { get; set; }

        public virtual {{ foreignEntity }} {{ foreignEntity }} { get; set; } }