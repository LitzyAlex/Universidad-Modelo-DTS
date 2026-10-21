class PokemonAgua : Pokemon
{
    public PokemonAgua(string nombre, int vida, int ataque) : base(nombre, vida, ataque)
    {
        this.tipos = new List<string> { "Agua" };
    }

    public override void Atacar(Pokemon Defensor)
    {
        base.Atacar(Defensor);
        Stats.VidaActual += 5;
        if (Stats.VidaActual > Stats.VidaMax)
        {
            Stats.VidaActual = Stats.VidaMax;
        }
        Console.WriteLine($"Curacion de agua, {nombre} recupera 5 HP");
     
    }
}