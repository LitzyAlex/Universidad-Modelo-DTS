using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Leer el json
        string json = File.ReadAllText("pokemon.json");

        List<PokemonJson> datos =JsonSerializer.Deserialize<List<PokemonJson>>(json) ?? new List<PokemonJson>();

        // Crear Pokemon normales 
        Pokemon Bulbasaur = new Pokemon(datos[0]);
        Pokemon Ivysaur = new Pokemon(datos[1]);

        // Mostrar informacion
        Bulbasaur.MostrarInformacion();
        Ivysaur.MostrarInformacion();

        // Crear Pokemon especiales
        Pokemon Charmander = new PokemonFuego("Charmander", 100, 20);
        Pokemon Squirtle = new PokemonAgua("Squirtle", 100, 15);

        Console.WriteLine("\n=== FUEGO ATACA A AGUA ===");

        Arena arena = new Arena();
        arena.Enfrentar(Charmander, Squirtle);

        Console.WriteLine("\n=== AGUA ATACA A FUEGO ===");

        arena.Enfrentar(Squirtle, Charmander);

        Console.WriteLine("\n=== POKEMON NORMAL ===");

        arena.Enfrentar(Bulbasaur, Charmander);

        // Probar captura
        Console.WriteLine("\n=== CAPTURAR ===");

        Entrenador Papu = new Entrenador("Papu");
        Papu.Capturar(Ivysaur);
        Papu.Capturar(Bulbasaur);
    }
}