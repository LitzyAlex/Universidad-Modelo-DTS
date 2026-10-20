using System;
using ListDev;    // ArrayList<T> (lista del equipo)

namespace SimMarket
{
    // Este es como el controlador de todo, el que tiene las reglas
    // Unity (o la consola) solo llama a EjecutarIteracion() y lee el estado.
    public class Simulador
    {
        //readonly es un modificador que se utiliza para declarar campos de solo lectura
        //Es como una const pero se puede calcular o asignar al crear el objeto sin tener que saber el valor fijo al inicio
        private readonly SimConfig config;
        private readonly Random rnd;

        //Listas
        private readonly ArrayList<Cliente> clientesEnMercado = new ArrayList<Cliente>(); // comprando o listos para caja
        private readonly ArrayList<Caja> cajas = new ArrayList<Caja>();

        private int siguienteIdCliente = 1;

        //Atributos visibles y propiedades
        public int Iteracion { get; private set; }
        public int TotalLlegados { get; private set; }
        public int TotalAtendidos { get; private set; }
        public int ClientesAbandonaron { get; private set; } // se fueron porque todas las cajas elegibles estaban llenas
        public int ClientesComprando
        {
            get
            {
                int total = 0;
                for (int i = 0; i < clientesEnMercado.Size; i++)
                {
                    if (clientesEnMercado[i].Estado == Cliente.EstadoCliente.Comprando)
                    {
                        total++;
                    }
                }
                return total;
            }
        }

        public int ClientesEnFilas 
        { 
            get 
            { 
                int n = 0;
                for (int i = 0; i < cajas.Size; i++)
                {
                    n += cajas[i].ClientesEnEspera;
                }
                return n; 
            } 
        }
        public int ClientesEnTienda => clientesEnMercado.Size + ClientesEnFilas; // incluye a los que ya terminaron de recolectar y esperan formarse

        //La simulacion termina cuando ya paso el periodo de llegadas y no queda nadie en el mercado ni en las filas
        public bool Terminada => Iteracion >= config.IteracionesLlegada && clientesEnMercado.Empty && ClientesEnFilas == 0;
        public int NumCajas => cajas.Size;
        public Caja ObtenerCaja(int i) => cajas[i];
        public int ClientesEnMercado => clientesEnMercado.Size; // comprando o listos para formarse
        public Cliente ObtenerClienteEnMercado(int i) => clientesEnMercado[i];

        //Cosas de Unity que quizas ocupare pero por si acaso andan por aca
        //Son eventos que igual ocupo en program para poner mensajes
        public event Action<Cliente> ClienteLlego;
        public event Action<Cliente, Caja> ClienteSeFormo;
        public event Action<Cliente, Caja> ClienteSalio;
        public event Action<Cliente> ClienteAbandono;
        
        //Constructor

        public Simulador(SimConfig config)
        {
            this.config = config;
            this.config.Validar();
            rnd = config.Semilla.HasValue ? new Random(config.Semilla.Value) : new Random(); // Aqui vemos de utilizar la semilla si tiene
            CrearCajas();
        }

        //Metodos
      
        // Crea NumCajas cajas. Las primeras NumCajasRapidas son rapidas el resto normales
        // A cada una se le asigna al azar (una sola vez) su capacidad de cobro por iteracion
        private void CrearCajas()
        {
            for (int i = 0; i < config.NumCajas; i++)
            {
                bool rapida = i < config.NumCajasRapidas;
                int cap = rnd.Next(config.MinCapacidadCobro, config.MaxCapacidadCobro + 1); //Genera añeatoriamente la capacidad
                cajas.Add(new Caja(i + 1, rapida, cap, config.LimiteArticulosRapida, config.MaxClientesPorCaja));
            }
        }

        // Genera un cliente con datos al azar
        // checa igual si es Preference

        private Cliente GenerarCliente()
        {
            //Okey se puso hacia abajo por cuestiones de espacio hacia el lado, se veia muy feito
            return new Cliente(
                siguienteIdCliente++,
                rnd.Next(config.MinArticulos, config.MaxArticulos + 1),
                rnd.Next(config.MinIteracionesPorRecoleccion, config.MaxIteracionesPorRecoleccion + 1),
                rnd.Next(config.MinArticulosPorRecoleccion, config.MaxArticulosPorRecoleccion + 1),
                rnd.NextDouble() < config.ProbPreference, Iteracion); // Genera un valor decimal entre 0 y 1 y comprueba si es menor que 0.10
        }

        // LA ITERACION
        public void EjecutarIteracion()
        {
            Iteracion++;

            ProcesarCajas();
            AsignarClientesACajas();
            RecolectarArticulos();
            GenerarLlegadas();
        }

        //Cada uno de estos metodos tiene un invoke al final, hay una razon
        //Lo ocupo para el Unity, el simulador puede saber algo pero mi Unity ocupa enterarse para mostrarlo
        //Entonces ocupamos eventos, el simulador anuncia que algo pas y las clases interesadas pueden escuchar ese avis
        //Entonces invoke hace eso, es como si alguien escucha esto, que se entere que algo paso
        //Solo es para Unity, entonces no preocuparse

        //Paso 1: cada caja cobra a su cliente al frente los que terminan se largan
        private void ProcesarCajas()
        {
            for (int i = 0; i < cajas.Size; i++)
            {
                Caja caja = cajas[i];
                Cliente terminado = caja.ProcesarIteracion();
                if (terminado != null)
                {
                    terminado.Salir(Iteracion);
                    TotalAtendidos++;
                    ClienteSalio?.Invoke(terminado, caja);
                }
            }
        }

        // Paso 2: recorre la lista de clientes y forma en caja a los que estan ListoParaCaja
        // Al formarse se quitan de la lista del mercado (ya estan en una cola de una caja)
        // Si no hay ninguna caja elegible con espacio, el cliente abandona y tambien se quita de la lista

        private void AsignarClientesACajas()
        {
            // Se recorre en orden de llegada para que los que llegaron antes elijan caja primero
            int i = 0;
            while (i < clientesEnMercado.Size)
            {
                Cliente c = clientesEnMercado[i];
                if (c.Estado != Cliente.EstadoCliente.ListoParaCaja)
                {
                    i++;
                    continue;
                }

                // Sea que se forme o que abandone, sale de la lista del mercado
                // (no se incrementa i porque RemoveAt recorre los demas una posicion)
                clientesEnMercado.RemoveAt(i);

                Caja destino = SeleccionarCaja(c);
                if (destino == null)
                {
                    // Todas las cajas elegibles estan llenas: abandona la compra
                    c.Abandonar(Iteracion);
                    ClientesAbandonaron++;
                    ClienteAbandono?.Invoke(c);
                    continue;
                }

                destino.Formar(c);
                ClienteSeFormo?.Invoke(c, destino);
            }
        }

        //Paso 3: los que estan Comprando agarran articulos, si terminan quedan ListoParaCaja
        private void RecolectarArticulos()
        {
            for (int i = 0; i < clientesEnMercado.Size; i++)
            {
                clientesEnMercado[i].Recolectar();
            }
        }

        //Paso 4: llegan de 0 a MaxLlegadasPorIteracion clientes nuevos
        private void GenerarLlegadas()
        {
            // Pasado el periodo de llegadas ya no entra nadie, la simulacion sigue hasta vaciarse
            if (Iteracion > config.IteracionesLlegada)
            {
                return;
            }

            int n = rnd.Next(0, config.MaxLlegadasPorIteracion + 1);
            for (int i = 0; i < n; i++)
            {
                Cliente c = GenerarCliente();
                clientesEnMercado.Add(c);
                TotalLlegados++;
                ClienteLlego?.Invoke(c);
            }
        }

        //Reglas de las cajas
        private Caja SeleccionarCaja(Cliente c)
        {
            Caja mejor = null;
            int mejorTiempo = 0;

            for (int i = 0; i < cajas.Size; i++)
            {
                Caja candidata = cajas[i];

                if (!candidata.PuedeAtender(c) || candidata.Llena)
                {
                    continue;
                }

                int tiempo = candidata.TiempoEsperaEstimado;

                if (mejor == null || tiempo < mejorTiempo || (tiempo == mejorTiempo && candidata.EsRapida && !mejor.EsRapida))
                {
                    mejor = candidata;
                    mejorTiempo = tiempo;
                }
            }

            return mejor;
        }

    }
}
