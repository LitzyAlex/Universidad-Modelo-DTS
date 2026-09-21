class Pokemon
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

    public int vida {
        get{return _vida; }
        set
        {
            if(value<0){_vida=0;}
            else
            {
                {_vida = value;}
            }
        }
        }
    private int _vida{get; set;}
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