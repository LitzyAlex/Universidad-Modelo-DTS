using System;
using QueueDev;  

namespace SimMarket
{
    public class Caja
    {
        //Atributos fijos
        public int Id { get; }
        public bool EsRapida { get; }
        public int CapacidadCobro { get; }          // articulos que procesa por iteracion 
        public int LimiteArticulos { get; }         // solo aplica a rapidas
        public int MaxClientes { get; }             // MAX CUSTOMERS PER CASHIER (incluye al que se atiende)

        private readonly ArrayQueue<Cliente> fila;

        //Atributos que cambian
        public int ClientesAtendidos { get; private set; }
        public int ArticulosProcesados { get; private set; }

        //Constructor

        public Caja(int id, bool esRapida, int capacidadCobro, int limiteArticulos, int maxClientes)
        {
            Id = id;
            EsRapida = esRapida;
            CapacidadCobro = capacidadCobro;
            LimiteArticulos = limiteArticulos;
            MaxClientes = maxClientes;
            fila = new ArrayQueue<Cliente>(maxClientes); // capacidad exacta, nunca se expande
        }

        //Propiedades

        //Cuantos clientes hay formados (mas el actual)
        public int ClientesEnEspera => fila.Size;

        //Ya tiene el maximo de clientes asignados (no puede recibir mas)
        public bool Llena => fila.Size >= MaxClientes;

        //Tiempo de espera estimado en iteraciones: suma de techo(articulos pendientes / capacidad)
        //de cada cliente que ya esta en la fila (incluyendo el que se esta atendiendo)
        //Es la formula pedida por el profe
        public int TiempoEsperaEstimado
        {
            get
            {
                int total = 0;
                for (int i = 0; i < fila.Size; i++)
                {
                    int pendientes = fila.ElementAt(i).ArticulosPorCobrar;
                    total += (int)Math.Ceiling((double)pendientes / CapacidadCobro); // techo de la division
                }
                return total;
            }
        }

        //Cliente en la posicion indicada de la fila (0 = el que se atiende)
        public Cliente ClienteEnPosicion(int posicion) => fila.ElementAt(posicion);

        //Caja desocupada
        public bool Desocupada => fila.Empty;

        //Cliente al frente de la fila o nada si no hay
        public Cliente ClienteAlFrente => fila.Empty ? null : fila.Head();

        //Métodos

        //Este cliente puede formarse aqui
        //Normal: se      Rapida: si es Preference o no excede el limite de articulos
        public bool PuedeAtender(Cliente c)
        {
            if (!EsRapida)
            {
                return true;
            }

            return c.EsPreference || c.ArticulosTotales <= LimiteArticulos;
        }

        //Forma al cliente al final de la fila
        public void Formar(Cliente c)
        {
            if (Llena)
            {
                throw new InvalidOperationException($"La caja {Id} esta llena");
            }
            c.EntrarAFila();
            fila.Enqueue(c);
        }

        // Procesa una iteracion de cobro solo se hace a el cliente que ests al frente
        // Si termina de pagar se saca de la fila >:) y el simulador lo saca del mercado
        public Cliente ProcesarIteracion()
        {
            if (fila.Empty)
            {
                return null;
            }

            Cliente actual = fila.Head();
            ArticulosProcesados += actual.Cobrar(CapacidadCobro);

            if (actual.TerminoDePagar)
            {
                fila.Dequeue();
                ClientesAtendidos++;
                return actual;
            }
            return null;
        }

        //La otra fumada para sobreescribir
        public override string ToString()
        {
            string tipo = EsRapida ? $"RÁPIDA (max {LimiteArticulos})" : "NORMAL";
            string frente = fila.Empty ? "vacía" : $"cobrando a #{fila.Head().Id} ({fila.Head().ArticulosPorCobrar} restantes)";
            return $"Caja {Id} {tipo} | cap {CapacidadCobro}/iter | en fila: {ClientesEnEspera}/{MaxClientes} | atendidos: {ClientesAtendidos} | articulos procesados: {ArticulosProcesados} |espera est.: {TiempoEsperaEstimado} iter | {frente}";
        }
    }
}
