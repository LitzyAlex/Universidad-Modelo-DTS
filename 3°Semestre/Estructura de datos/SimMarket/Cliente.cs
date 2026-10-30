using System;

namespace SimMarket
{
    public class Cliente
    {
        // atributos CONSTANTES
        public int Id { get; }                             // identificador del cliente
        public bool EsPreference { get; }                  // preferente? true o false
        public int ArticulosTotales { get; }               // total de artículos que recogerá
        public int IteracionesPorRecoleccion { get; }      // cada cuántas iteraciones agarra artículos
        public int ArticulosPorRecoleccion { get; }        // cuántos artículos agarra p/iteración
        public int IteracionLlegada { get; }               // en qué iteración llegó al supermercado

        // atributos VARIABLES
        public EstadoCliente Estado { get; private set; }       
        public int ArticulosRecolectados { get; private set; }  // cantidad actual de artículos recogidos
        public int ArticulosPorCobrar { get; private set; }     // artículos pendientes en caja
        public int IteracionSalida { get; private set; }        // en qué iteración salió del supermercado
            
        private int contadorEspera;                         // cuántas iteraciones pasaron desde la última recolección

        public enum EstadoCliente   // estados = etapas del cliente
        {
            Comprando,        // recolectando artículos
            ListoParaCaja,    // terminó de recolectar, se forma en la siguiente iteración
            EnFila,           // esperando a que le cobren (Queue)
            Terminado,        // pagó y salió
            Abandono          // cajas elegibles llenas, se fue sin comprar
        }


        
        // constructor
        public Cliente(int id, int articulosTotales, int iteracionesPorRecoleccion,int articulosPorRecoleccion, bool esPreference, int iteracionLlegada)
        {
            Id = id;
            ArticulosTotales = articulosTotales;
            IteracionesPorRecoleccion = iteracionesPorRecoleccion;
            ArticulosPorRecoleccion = articulosPorRecoleccion;
            EsPreference = esPreference;
            IteracionLlegada = iteracionLlegada;

            Estado = EstadoCliente.Comprando;
            ArticulosRecolectados = 0;
            ArticulosPorCobrar = articulosTotales;
            contadorEspera = 0;
        }

        // métodos
        public bool Recolectar() //true si termina de recoger todos sus artículos
        {
            if (Estado != EstadoCliente.Comprando) // el cliente debe seguir comprando
            { 
                return false; 
            }
            contadorEspera++;

            if (contadorEspera < IteracionesPorRecoleccion) // en esa iteración debe agarrar artículos?
            { 
                return false; 
            }
            contadorEspera = 0; // se reinicia cuando al fin agarra artículo

            int restantes = ArticulosTotales - ArticulosRecolectados; 
            ArticulosRecolectados += Math.Min(ArticulosPorRecoleccion, restantes); // elige el menor para agarrar la cantidad exacta de artículos

            if (ArticulosRecolectados >= ArticulosTotales)  // ya recogió todos los artículos? sí:
            {
                Estado = EstadoCliente.ListoParaCaja;
                return true;
            }
            return false;
        }

        public void EntrarAFila() 
        { 
            Estado = EstadoCliente.EnFila;
        }

        public int Cobrar(int cantidad)
        {                           // el menor entre la cantidad que puede cobrar la caja y los artículos pendientes
            int cobrados = Math.Min(cantidad, ArticulosPorCobrar); 
            ArticulosPorCobrar -= cobrados; // actualiza faltante por cobrar
            return cobrados;    // int de artículos pagados
        }

        // si ya no hay artículos por cobrar es true
        public bool TerminoDePagar => ArticulosPorCobrar == 0;

        // marca la salida del cliente
        public void Salir(int iteracionActual)
        {
            Estado = EstadoCliente.Terminado;
            IteracionSalida = iteracionActual;
        }

        // el cliente se fue sin pagar pq no habían cajas disponibles
        public void Abandonar(int iteracionActual)
        {
            Estado = EstadoCliente.Abandono;
            IteracionSalida = iteracionActual;
        }

        // método que ya existe en los objetos en C#
        // para obtener una representación en texto del objeto
        // sino al hacer "WriteLine(cliente)" solo saldría el nombre del objeto
        // aqui se sobreescribe para que imprima de esta manera:
        public override string ToString()
        {
            string pref = EsPreference ? " [PREFERENCE]" : "";  // es preferente? sí: imprime la etiqueta, no:  
            return $"Cliente #{Id}{pref} | {Estado} | recolectados {ArticulosRecolectados}/{ArticulosTotales} |  Cada {IteracionesPorRecoleccion} iter. recoge {ArticulosPorRecoleccion} | por cobrar {ArticulosPorCobrar}";
            // ej. Cliente #7 [PREFERENCE] | Comprando | recolectados 6/10 | Cada 2 iter. recoge 3 | por cobrar 10
        }
    }
}
