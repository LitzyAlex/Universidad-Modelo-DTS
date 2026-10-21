public class Arena
{
    public void Enfrentar(Pokemon Atacante, Pokemon Defensor)
    {
        Console.WriteLine("====================================");
        Console.WriteLine($"{Atacante.nombre} vida = {Atacante.Stats.VidaActual}");
        Console.WriteLine($"{Defensor.nombre} vida = {Defensor.Stats.VidaActual}");
        Console.WriteLine($"{Atacante.nombre} ({Atacante.Stats.Ataque}) ataco a {Defensor.nombre}");
        Atacante.Atacar(Defensor);
        Console.WriteLine($"{Defensor.nombre} vida = {Defensor.Stats.VidaActual} / {Defensor.Stats.VidaMax}");
        Console.WriteLine($"{Atacante.nombre} vida = {Atacante.Stats.VidaActual} / {Atacante.Stats.VidaMax}");
        Console.WriteLine("====================================");
    }
}

