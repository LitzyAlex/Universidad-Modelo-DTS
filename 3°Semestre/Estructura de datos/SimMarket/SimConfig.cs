using System;

namespace SimMarket
{
    // Datos de la simulacion, todo lo que puede cambiar anda por aca
    // En Unity se puede llenar desde el Inspector y pasarlo al Simulador
    public class SimConfig
    {
        // Cajas
        public int NumCajas = 3;                 // total de cajas(rapidas + normales)
        public int NumCajasRapidas = 2;          // cuantas de las anteriores son rapidas
        public int LimiteArticulosRapida = 5;    // maximo de articulos para usar caja rapida
        public int MinCapacidadCobro = 1;        // articulos que procesa una caja (minimo, el documento dice de 1 a CHECKOUT PRODUCTS)
        public int MaxCapacidadCobro = 5;        // articulos que procesa una caja (maximo) = CHECKOUT PRODUCTS
        public int MaxClientesPorCaja = 2;       // MAX CUSTOMERS PER CASHIER: maximo de clientes por caja incluyendo al que se atiende

        // Clientes
        public int MaxLlegadasPorIteracion = 2;  // cada iteracion llegan de 0 a este numero
        public int MinArticulos = 1;
        public int MaxArticulos = 20;
        public int MinIteracionesPorRecoleccion = 1; // cada cuantas iteraciones agarra articulos
        public int MaxIteracionesPorRecoleccion = 3;
        public int MinArticulosPorRecoleccion = 1;   // cuantos agarra cada vez
        public int MaxArticulosPorRecoleccion = 4;
        public double ProbPreference = 0.10;         // 10% de clientes Preference dice el documento
        public int IteracionesLlegada = 10;          // CUSTOMER ARRIVAL ITERATIONS: iteraciones durante las que pueden entrar clientes nuevos

        // Aleatorio
        //Expliacion
        //Ese ? en int? permite que el valor sea null y Semilla controla los numeros aleatorios
        //Usando una semilla fija se puede repetir una secuencia de numeros aleatorios(literalmente como en los juegos)
        //Esto no funciona solito en realidad, atento a eso
        public int? Semilla = null;              // poner un numero para resultados repetibles 

        //Para no validar todo de uno en uno(tecnicamente es de uno en uno tambien aca pero mas bonito, no se si falta alguna validacion)
        public void Validar()
        {
            if (NumCajas <= 0)
            {
                throw new ArgumentException("Debe haber al menos una caja");
            }

            if (NumCajasRapidas < 0 || NumCajasRapidas > NumCajas)
            {
                throw new ArgumentException("El numero de cajas rapidas no es valido");
            }

            if (LimiteArticulosRapida < 0)
            {
                throw new ArgumentException("El limite de articulos no puede ser negativo");
            }

            if (MinCapacidadCobro <= 0 || MinCapacidadCobro > MaxCapacidadCobro)
            {
                throw new ArgumentException("El rango de capacidad de cobro no es valido");
            }

            if (MaxLlegadasPorIteracion < 0)
            {
                throw new ArgumentException("Las llegadas maximas no pueden ser negativas");
            }

            if (MinArticulos <= 0 || MinArticulos > MaxArticulos)
            {
                throw new ArgumentException("El rango de articulos no es valido");
            }

            if (MinIteracionesPorRecoleccion <= 0 || MinIteracionesPorRecoleccion > MaxIteracionesPorRecoleccion)
            {
                throw new ArgumentException("El rango de iteraciones por recoleccion no es valido");
            }

            if (MinArticulosPorRecoleccion <= 0 || MinArticulosPorRecoleccion > MaxArticulosPorRecoleccion)
            {
                throw new ArgumentException("El rango de articulos por recoleccion no es valido");
            }

            if (ProbPreference < 0 || ProbPreference > 1)
            {
                throw new ArgumentException("La probabilidad de Preference debe estar entre 0 y 1");
            }

            if (MaxClientesPorCaja <= 0)
            {
                throw new ArgumentException("Cada caja debe poder tener al menos un cliente");
            }

            if (IteracionesLlegada <= 0)
            {
                throw new ArgumentException("Debe haber al menos una iteracion de llegada de clientes");
            }

            if (NumCajasRapidas >= NumCajas)
            {
                throw new ArgumentException("Debe existir al menos una caja normal");
            }
        }



    }
}
