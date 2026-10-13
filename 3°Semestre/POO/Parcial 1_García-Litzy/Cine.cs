public class Cine
{
    public string NombreCine;
    public List<Sala> SalasHabilitadas; 
    public Cine(string nombre)
    {
        NombreCine = nombre;
        SalasHabilitadas = new List<Sala>();
      
    }

    public void AgregarSala(Sala nueva)
    {
        SalasHabilitadas.Add(nueva);
    }
   
  
    public void VenderBoleto(Espectador cliente, Sala salaCine)
    {
        Console.WriteLine($"\nProcesando venta para {cliente.Nombre} - Película: {salaCine.PeliculaProyectada.Titulo}...");
        if(cliente.Edad < salaCine.PeliculaProyectada.EdadMinima)
        {
            Console.WriteLine("No tiene edad minima para ver la pelicula. Fallo de venta");
            return;
        }
        if (salaCine.AsientosDisponibles == 0)
        {
            Console.WriteLine("La sala ya no tiene asientos disponibles. Fallo de venta");
            return;
        }

        salaCine.AsientosOcupados++;
        Console.WriteLine("La venta se realizo con exito :)");

    }
}