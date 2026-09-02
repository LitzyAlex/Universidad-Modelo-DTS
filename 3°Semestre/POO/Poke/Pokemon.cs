class Pokemon
{
    //Atributos
    public string nombre ="";
    public List<string> tipos;
    public int nivel;
    public int vida;
    public int  ataque;

    //Métodos
    public void MostrarInformacion()
    {
        Console.WriteLine($"=== Pokemon: {nombre}, Nivel {nivel} ===");
        Console.WriteLine($"Tipos: {string.Join(", ", tipos)}");
        Console.WriteLine($"Vida (HP): {vida}");
        Console.WriteLine($"Ataque: {ataque}");
        Console.WriteLine("====================================");
    }

    //Constructores
    public Pokemon()
    {
        nombre = "Default";
        vida = 1;
        tipos= new List<string>();
        nivel = 1;
        ataque = 1;
    }

    public Pokemon(string nombre,  int vida, int ataque )
    {
        this.nombre = nombre;
        this.tipos = new List<string>{"Normal"};
        this.vida = vida;
        this.nivel = 1;
        this.ataque = ataque;
    }

    public Pokemon(PokemonJson data)
    {
        this.nombre = data.Name;
        this.vida = data.Hp;
        this.tipos= data.Type;
        this.ataque = data.Attack;
        this.nivel = 1;
    }
 

}