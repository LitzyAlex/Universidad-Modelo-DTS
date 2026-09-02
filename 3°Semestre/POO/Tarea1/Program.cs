using System;
using System.Reflection.Metadata;

class Program {
    static void Main() {
        /*listas
        List<int> numeros = new List<int>();
        List<Jugador> jugadores = new List<Jugador>();
        jugadores.Add(jugador1); //agregar a la lista


        Dictionary<int, string> numeros1 = new Dictionary<int, string>();
        Dictionary<int, Jugador> jugadores1 = new Dictionary<int, Jugador>();

        Tuple<int,string> tuple1 = new Tuple<int, string>(1, "Alex");
        tuple1.Item1;

        (int daño, bool isCritic) CalcularDaño()
        {
            int daño=0;
            bool isCritic = false;
            return (daño, isCritic);
        }

        bool critico = CalcularDaño.Item2();
        */



        Jugador jugador1 = new Jugador();
        jugador1.nombre= "Guerrero";
        jugador1.vida = 100;
        jugador1.ataque = 20;
        jugador1.nivel = 5;

        Jugador jugador2 = new Jugador();
        jugador2.nombre= "Mago";
        jugador2.vida = 80;
        jugador2.ataque = 30;
        jugador2.nivel = 7;

        Jugador jugador3 = new Jugador();
        jugador3.nombre= "Arquero";
        jugador3.vida = 90;
        jugador3.ataque = 25;
        jugador3.nivel = 6;

        jugador1.Datos();
        jugador2.Datos();
        jugador3.Datos();

        jugador1.Atacar(jugador2);
        jugador2.Atacar(jugador1);
        jugador3.Atacar(jugador1);


    }
}



    

