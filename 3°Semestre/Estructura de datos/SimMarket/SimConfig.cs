using System;

namespace SimMarket
{
    // panel de ajustes
    public class SimConfig
    {
        // CAJAS
        public int NumCajas = 3;                 // total de cajas
        public int NumCajasRapidas = 2;          // cuántas son rápidas
        public int LimiteArticulosRapida = 5;    // máx de artículos p/caja rápida
        public int MinCapacidadCobro = 1;        // mín de artículos que una caja puede procesar (1 a CHECKOUT PRODUCTS)
        public int MaxCapacidadCobro = 5;        // máx que una caja procesa (CHECKOUT PRODUCTS)
        public int MaxClientesPorCaja = 2;       // máx de clientes por caja incluyendo al que se atiende (MAX CUSTOMERS PER CASHIER) 

        // CLIENTES
        public int MaxLlegadasPorIteracion = 2;         // clientes máx que pueden llegar p/iteración
        public int MinArticulos = 1;                    // rango de artículos que puede tener el cliente
        public int MaxArticulos = 20;
        public int MinIteracionesPorRecoleccion = 1;    // cada cuántas iteraciones agarra articulos un cliente (tiempo)
        public int MaxIteracionesPorRecoleccion = 3;
        public int MinArticulosPorRecoleccion = 1;      // cuántos artículos puede agarrar un cliente en cada iteración (cantidad)
        public int MaxArticulosPorRecoleccion = 4;
        public double ProbPreference = 0.10;            // 10% de clientes Preference dice el documento
        public int IteracionesLlegada = 10;             // cada cuántas iteraciones pueden entrar clientes nuevos (CUSTOMER ARRIVAL ITERATIONS)

        // SEMILLA
        public int? Semilla = null;              // int?: permite que el valor sea null
                                                 // Semilla: controla los números aleatorios
                                                 // = null: se especifica un número para resultados repetibles 

        // VALIDACIONES -> cajas
        public void Validar()
        {   
            // 1. existe mínimo una caja
            if (NumCajas <= 0)
            {
                throw new ArgumentException("Debe haber al menos una caja");
            }

            // 2. debe existir, al menos, una caja normal (0 <= rapidas < cajas)
            if (NumCajasRapidas < 0 || NumCajasRapidas >= NumCajas)
            {
                throw new ArgumentException("Las cajas rapidas no pueden ser negativas y debe existir al menos una caja normal");
            }

            // 3. el límite de artículos p/caja rápida no puede ser -
            if (LimiteArticulosRapida < 0)
            {
                throw new ArgumentException("El limite de articulos no puede ser negativo");
            }

            // 4. la capacidad mín no puede ser - ni superar la capacidad máx
            if (MinCapacidadCobro <= 0 || MinCapacidadCobro > MaxCapacidadCobro)
            {
                throw new ArgumentException("El rango de capacidad de cobro no es valido");
            }


        // VALIDACIONES -> clientes
            // 5. el máx de llegadas no puede ser -
            if (MaxLlegadasPorIteracion < 0)
            {
                throw new ArgumentException("Las llegadas maximas no pueden ser negativas");
            }

            // 6. el mín debe ser + pero no superar el máx de artículos
            if (MinArticulos <= 0 || MinArticulos > MaxArticulos)
            {
                throw new ArgumentException("El rango de articulos no es valido");
            }

            // 7. respetar el intervalo de mín y máx de iteraciones
            if (MinIteracionesPorRecoleccion <= 0 || MinIteracionesPorRecoleccion > MaxIteracionesPorRecoleccion)
            {
                throw new ArgumentException("El rango de iteraciones por recoleccion no es valido");
            }

            // 8. respetar el rango de mín y máx de artículos p/iteración
            if (MinArticulosPorRecoleccion <= 0 || MinArticulosPorRecoleccion > MaxArticulosPorRecoleccion)
            {
                throw new ArgumentException("El rango de articulos por recoleccion no es valido");
            }

            // 9. probabilidad entre 0% y 100%
            if (ProbPreference < 0 || ProbPreference > 1)
            {
                throw new ArgumentException("La probabilidad de Preference debe estar entre 0 y 1");
            }

            // 10. cada caja debe poder tener mínimo un cliente
            if (MaxClientesPorCaja <= 0)
            {
                throw new ArgumentException("Cada caja debe poder tener al menos un cliente");
            }

            // 11. mínimo una iteración para el tiempo de llegada
            if (IteracionesLlegada <= 0)
            {
                throw new ArgumentException("Debe haber al menos una iteracion de llegada de clientes");
            }
        }

        // presets uwu
        public static SimConfig TiendaChica() => new SimConfig  // static: llamar al método sin crear su objeto
        {                                                       // => new SimConfig: devuelve objeto de tipo SimConfig con los valores asignados
            NumCajas = 3,
            NumCajasRapidas = 1,
            MaxClientesPorCaja = 4,
            IteracionesLlegada = 10
        };

        public static SimConfig HoraPico() => new SimConfig
        {
            NumCajas = 5,
            NumCajasRapidas = 2,
            MaxClientesPorCaja = 3,
            MaxLlegadasPorIteracion = 3,
            IteracionesLlegada = 25
        };

        public static SimConfig Supermercado() => new SimConfig
        {
            NumCajas = 8,
            NumCajasRapidas = 3,
            MaxClientesPorCaja = 6,
            MaxLlegadasPorIteracion = 3,
            IteracionesLlegada = 30
        };


    }
}
