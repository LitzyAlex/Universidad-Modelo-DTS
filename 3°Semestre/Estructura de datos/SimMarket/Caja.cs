using System;
using QueueDev;  

namespace SimMarket
{
    public class Caja
    {
        // atributos CONSTANTES
        public int Id { get; }                      // identificador caja
        public bool EsRapida { get; }               // es caja rápida? true o false
        public int CapacidadCobro { get; }          // número de artículos que procesa p/iteración 
        public int LimiteArticulos { get; }         // (solo aplica a rápidas)
        public int MaxClientes { get; }             // máximo de clientes por caja incluyendo al que se atiende (MAX CUSTOMERS PER CASHIER)

        private readonly ArrayQueue<Cliente> fila; //readonly: la referencia no puede reasignarse una vez inicializado

        // atributos VARIABLES
        public int ClientesAtendidos { get; private set; }      // cuántos ya terminaron de pagar
        public int ArticulosProcesados { get; private set; }    // total de artículos que la caja ya ha cobrado


        // constructor
        public Caja(int id, bool esRapida, int capacidadCobro, int limiteArticulos, int maxClientes)
        {
            Id = id;
            EsRapida = esRapida;
            CapacidadCobro = capacidadCobro;
            LimiteArticulos = limiteArticulos;
            MaxClientes = maxClientes;
            fila = new ArrayQueue<Cliente>(maxClientes); // capacidad exacta, nunca se expande
        }

        // propiedades
            // 1. cuántos clientes hay en la fila incluyendo al que es atendido
        public int ClientesEnEspera => fila.Size;

            // 2. si el total de clientes en espera ya es el máx no puede recibir más
        public bool Llena => fila.Size >= MaxClientes;

            // 3. cuánto tardaría la caja en cobrar TODOS los artículos: suma de techo(articulos pendientes / capacidad)
        public int TiempoEsperaEstimado
        {
            get
            {
                int total = 0;  // inicia acumulador
                for (int i = 0; i < fila.Size; i++)     // recorre todos los clientes
                {
                           // cliente en la posición i, cuántos artículos le falta por cobrar?
                    int pendientes = fila.ElementAt(i).ArticulosPorCobrar;  
                                // redondea hacia arriba
                    total += (int)Math.Ceiling((double)pendientes / CapacidadCobro);
                }
                return total;   // tiempo total
            }
        }

            // 4. obtener a un cliente en cierta posición (0 = el que se atiende)
        public Cliente ClienteEnPosicion(int posicion) => fila.ElementAt(posicion);

            // 5. la cola está vacía? true
        public bool Desocupada => fila.Empty;

            // 6. el primer cliente
        public Cliente ClienteAlFrente => fila.Empty ? null : fila.Head();


        // método
        public bool PuedeAtender(Cliente c) // -> valida
        {
            if (!EsRapida)  // si es caja normal para cualquier cliente
            {
                return true;
            }

            // si es preferente o sus artículos no exceden el límite
            return c.EsPreference || c.ArticulosTotales <= LimiteArticulos;
        }

        public void Formar(Cliente c) // -> ya se forma al final
        {
            if (Llena) 
            {
                throw new InvalidOperationException($"La caja {Id} esta llena");
            }
            c.EntrarAFila(); // cambia el estado del cliente
            fila.Enqueue(c);
        }

        public Cliente ProcesarIteracion() // -> cobra los artículos
        {
            if (fila.Empty) // comprueba que hay clientes
            {
                return null;
            }

            Cliente actual = fila.Head();   // el primer cliente de la fila
            ArticulosProcesados += actual.Cobrar(CapacidadCobro);   // llama al método del cliente

            if (actual.TerminoDePagar)
            {
                fila.Dequeue(); // lo borra de la fila y el simulador del mercado
                ClientesAtendidos++;
                return actual;
            }
            return null;
        }

        // sobreescribir x2
        public override string ToString()
        {
            string tipo = EsRapida ? $"RÁPIDA (max {LimiteArticulos})" : "NORMAL";
            string frente = fila.Empty ? "vacía" : $"cobrando a #{fila.Head().Id} ({fila.Head().ArticulosPorCobrar} restantes)";
            return $"Caja {Id} {tipo} | cap {CapacidadCobro}/iter | en fila: {ClientesEnEspera}/{MaxClientes} | atendidos: {ClientesAtendidos} | articulos procesados: {ArticulosProcesados} |espera est.: {TiempoEsperaEstimado} iter | {frente}";
        }
    }
}
