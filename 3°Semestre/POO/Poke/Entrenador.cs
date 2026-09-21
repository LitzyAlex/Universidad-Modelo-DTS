class Entrenador
{
    public string nombre= "";
    public List<Pokemon> Equipo;
    public Mochila miInventario;
    
    public Entrenador(string nombre)
    {
        this.nombre=nombre;
        Equipo= new List<Pokemon>();
        miInventario = new Mochila();
    }
    public void Capturar(Pokemon nuevo)
    {
        Console.WriteLine("====================================");
        Equipo.Add(nuevo);
        Console.WriteLine($"El entrenador {nombre} capturo a {nuevo.nombre}, se agrego al equipo");
        Console.WriteLine("Equipo:");
        foreach(var elemento in Equipo)
        {
            Console.WriteLine(elemento.nombre);
        }
        Console.WriteLine("====================================");
    }
}