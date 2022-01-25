namespace APIBusqueda
{
    public class Biblioteca
    {
        public string nombre { get; set; }
		public string tipo { get; set; }
		public string direccion { get; set; }
		public string codigoPostal { get; set; }
		public string longitud { get; set; }
		public string latitud { get; set; }
		public string telefono { get; set; }
		public string email { get; set; }
		public string descripcion { get; set; }

        public Biblioteca(string nombre, string tipo, string direccion, string codigoPostal, string longitud, string latitud, string telefono, string email, string descripcion)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.direccion = direccion;
            this.codigoPostal = codigoPostal;
            this.longitud = longitud;
            this.latitud = latitud;
            this.telefono = telefono;
            this.email = email;
            this.descripcion = descripcion;
        }
    }
}