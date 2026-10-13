class Program
{
    static void Main()
    {
        
        Cine miCine = new Cine("Cine POO Plaza");

        Pelicula peliTerror = new Pelicula("Masacre en la Consola", 18);
        Sala sala1 = new Sala(1, 2, peliTerror);
        miCine.AgregarSala(sala1);

        Espectador cliente1 = new Espectador("Timmy", 12);
        Espectador cliente2 = new Espectador("Juan"); 
        Espectador cliente3 = new Espectador("María", 25);

        Console.WriteLine("--- ABRIENDO TAQUILLA ---");
        Console.WriteLine($"Comprobación asientos ocupados: {sala1.AsientosOcupados}");
        
       
        miCine.VenderBoleto(cliente1, sala1); 

        miCine.VenderBoleto(cliente2, sala1);
        miCine.VenderBoleto(cliente3, sala1);

        Espectador hacker = new Espectador("Hacker", 30);
        miCine.VenderBoleto(hacker, sala1);

        Console.WriteLine("\n--- PRUEBA DE SEGURIDAD ---");
        sala1.AsientosOcupados = 5000; 
        Console.WriteLine($"Ocupados reales: {sala1.AsientosOcupados} / {sala1.CapacidadMaxima}");
        Console.WriteLine($"Disponibles: {sala1.AsientosDisponibles}");

        Console.WriteLine("\n-- Puntos de la Tarjeta --");
        Console.WriteLine($"cliente: {cliente1.Nombre}, puntos: { cliente1.MiTarjeta.Puntos}");
        Console.WriteLine($"cliente: {cliente2.Nombre}, puntos: { cliente1.MiTarjeta.Puntos}");
        Console.WriteLine($"cliente: {cliente3.Nombre}, puntos: { cliente1.MiTarjeta.Puntos}");

        Console.WriteLine("\n-- Comprobar otra sala --");
        Pelicula peliComedia = new Pelicula("Que paso ayer", 20);
        Sala sala2 = new Sala(2, 4, peliComedia);
        miCine.AgregarSala(sala2);
        foreach (var elemento in miCine.SalasHabilitadas)
        {
            Console.WriteLine($"Num sala: {elemento.NumeroSala}, Titulo: {elemento.PeliculaProyectada.Titulo} ");
        }
        sala2.AsientosOcupados = 6;
        Console.WriteLine($"Comprobación asientos ocupados: {sala2.AsientosOcupados} / {sala2.CapacidadMaxima}");
        miCine.VenderBoleto(cliente3, sala2);
    }
}