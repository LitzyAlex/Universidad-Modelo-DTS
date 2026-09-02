using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


class Program {
    static void Main() {      
      
        string json = File.ReadAllText("pokemon.json");   //Lee un archivo, el json
        List<PokemonJson> datos = JsonSerializer.Deserialize<List<PokemonJson>>(json) ?? new List<PokemonJson>();

        Pokemon Bulbasaur = new Pokemon();
        Bulbasaur.nombre = datos[0].Name;
        Bulbasaur.tipos = datos[0].Type;
        Bulbasaur.vida = datos[0].Hp;
        Bulbasaur.ataque = datos[0].Attack;
        Bulbasaur.nivel = 1;
        Bulbasaur.MostrarInformacion();

        Pokemon Default = new Pokemon();
        Default.MostrarInformacion();

        Pokemon Pokemon2 = new Pokemon("Papu", 100, 5);
        Pokemon2.MostrarInformacion();
        
        Pokemon Pokemon3 = new Pokemon(datos[1]);
        Pokemon3.MostrarInformacion();
    }
}