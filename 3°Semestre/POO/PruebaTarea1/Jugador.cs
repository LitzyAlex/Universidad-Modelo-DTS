class Jugador
{
    public string nombre="";
    public int vida;
    public int ataque;
    public int nivel;

    public Jugador(string nombre, int vida, int ataque, int nivel)
    {
    this.nombre = nombre;
    this.vida = vida;
    this.ataque = ataque;
    this.nivel = nivel;
    }


    public void Datos()
    {
        Console.WriteLine(nombre);
        Console.WriteLine("Vida: " + vida);
        Console.WriteLine("Ataque: " + ataque);
        Console.WriteLine("Nivel: " + nivel);
        Console.WriteLine();
    }

    public void Atacar(Jugador victima)
    {
        Console.WriteLine();
        Console.WriteLine(nombre + " atacó a " + victima.nombre);
        victima.RecibirDaño(ataque);
        Console.WriteLine(victima.nombre + " tiene " + victima.vida + " de vida");
    }

    public void RecibirDaño(int daño)
    {
        vida -= daño;
        if (vida < 0)
        {
            vida = 0;
        }
    }
}