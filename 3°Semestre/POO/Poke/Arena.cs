class Arena
{
    public void Enfrentar(Pokemon Atacante, Pokemon Defensor)
    {
        Console.WriteLine("====================================");
        Console.WriteLine($"{Atacante.nombre} vida = {Atacante.vida}");
        Console.WriteLine($"{Defensor.nombre} vida = {Defensor.vida}");
        Console.WriteLine($"{Atacante.nombre} ({Atacante.ataque}) ataco a {Defensor.nombre}");
        Defensor.vida = Defensor.vida - Atacante.ataque;
        Console.WriteLine($"{Defensor.nombre} vida = {Defensor.vida}");
        Console.WriteLine("====================================");
    }
}