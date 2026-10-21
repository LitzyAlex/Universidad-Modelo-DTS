class PokemonFuego : Pokemon
{
    public PokemonFuego(string nombre, int vida, int ataque) : base(nombre, vida, ataque)
    {
        this.tipos = new List<string> { "Fuego" };
    }

    public override void Atacar(Pokemon Defensor)
    {
        base.Atacar(Defensor);
        Console.WriteLine($"Quemadura, {Defensor.nombre} recibe 5 de daño extra.");
    }
}