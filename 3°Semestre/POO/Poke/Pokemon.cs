public class Pokemon
{
    //Atributos
    public string nombre ="";
    public List<string> tipos;
    public int nivel {
        get{return _nivel; }
        set
        {
            if(value < 0 || value > 100){throw new Exception("El nivel debe ser mayor a 0 menor a 100");}
            else{_nivel = value;}
        }
        }
    private int _nivel {get; set;}

    public Estadisticas Stats;
    public EstadoAlterado EstadoActual;

    //Métodos
    public void MostrarInformacion()
    {
        Console.WriteLine($"=== Pokemon: {nombre}, Nivel {nivel} ===");
        Console.WriteLine($"Tipos: {string.Join(", ", tipos)}");
        Console.WriteLine($"Vida: {Stats.VidaActual}/{Stats.VidaMax}");
        Console.WriteLine($"Ataque: {Stats.Ataque}");
        Console.WriteLine($"Defensa: {Stats.Defensa}");
        Console.WriteLine($"Estado: {EstadoActual}");
        Console.WriteLine("====================================");
    }

    public virtual void Atacar(Pokemon defensor)
    {
        int daño = Stats.Ataque - defensor.Stats.Defensa; 
        if (daño < 0)
        {
            daño = 0;
        }

        defensor.Stats.VidaActual -= daño;
        if (defensor.Stats.VidaActual < 0)
        {
            defensor.Stats.VidaActual = 0;
        }
    }

    public enum EstadoAlterado
    {
        Normal, 
        Quemado, 
        Paralizado, 
        Dormido
    }

    //Constructores
    public Pokemon()
    {
        nombre = "Default";
        tipos = new List<string> { "Normal" };
        nivel = 1;
        Stats = new Estadisticas(1, 1, 1, 0);
        EstadoActual = EstadoAlterado.Normal;
    }

    public Pokemon(string nombre, int vida, int ataque)
    {
        this.nombre = nombre;
        tipos = new List<string> { "Normal" };
        nivel = 1;

        Stats = new Estadisticas(vida, vida, ataque, 0);
        EstadoActual = EstadoAlterado.Normal;
    }

    public Pokemon(PokemonJson data)
    {
        nombre = data.Name;
        tipos = data.Type;
        nivel = 1;

        Stats = new Estadisticas(data.Hp, data.Hp, data.Attack, 0);
        EstadoActual = EstadoAlterado.Normal;
    }
 

}