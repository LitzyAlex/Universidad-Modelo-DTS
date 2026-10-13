public class Sala
{
    public int NumeroSala;
    public int CapacidadMaxima;
    public Pelicula PeliculaProyectada;

  
    public int AsientosOcupados 
    { 
        get { return _asientosOcupados; }
        set
        {
            if(value < 0) { _asientosOcupados = 0; }
            else if(value > CapacidadMaxima) { _asientosOcupados = CapacidadMaxima; }
            else { _asientosOcupados = value; }

        }
        
    }

    private int _asientosOcupados;


    public int AsientosDisponibles 
    {
        get
        {
            int resultado =  CapacidadMaxima - AsientosOcupados;
            return resultado;
        }
    }

    public Sala(int numero, int capacidad, Pelicula pelicula)
    {
        NumeroSala = numero;
        CapacidadMaxima = capacidad;
        PeliculaProyectada = pelicula;
        AsientosOcupados = -1; //Se cambio a la variable publica para que pase por la comprobación y se convierta en 0
    }
}