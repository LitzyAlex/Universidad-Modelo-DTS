public class Espectador
{
    public string Nombre;
    public int Edad;
    public TarjetaClub MiTarjeta;

    public Espectador(string nombre, int edad)
    {
        Nombre= nombre;
        Edad= edad;
        MiTarjeta= new TarjetaClub();
    }

    public Espectador(string nombre)
    {
        Nombre = nombre;
        Edad = 18;
        MiTarjeta = new TarjetaClub();
    }

}