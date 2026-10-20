using System;

namespace SimMarket
{
    public class Cliente
    {
        //Atributos constantes
        public int Id { get; }
        public bool EsPreference { get; }
        public int ArticulosTotales { get; }               // cuantos articulos va a recolectar
        public int IteracionesPorRecoleccion { get; }      // cada cuantas iteraciones agarra articulos
        public int ArticulosPorRecoleccion { get; }        // cuantos agarra cada vez
        public int IteracionLlegada { get; }

        // Atributos que van cambiando
        public EstadoCliente Estado { get; private set; }
        public int ArticulosRecolectados { get; private set; }
        public int ArticulosPorCobrar { get; private set; } // lo que le falta pagar en caja
        public int IteracionSalida { get; private set; }

        private int contadorEspera;                         // iteraciones transcurridas desde la ultima recoleccion

        //Esto se supone permite definir un conjunto de opciones con nombres(Estados/etapas del cliente)
        public enum EstadoCliente
        {
            Comprando,        // recorriendo el mercado, recolectando articulos
            ListoParaCaja,    // ya recolecto todo y se supone que se formara en la siguiente iteracion
            EnFila,           // formado en la fila de una caja(La queue)
            Terminado,        // ya pago y salio del mercado(lo matamos o asi)
            Abandono          // todas las cajas elegibles estaban llenas, se fue sin comprar
        }


        
        //Constructor
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

        // Avanza la recoleccion durante una iteracion
        // Devuelve true si termino de recoger todos sus articulos(Min es para no pasarse)
        public bool Recolectar()
        {
            if (Estado != EstadoCliente.Comprando) //Saber si esta comprando
            { 
                return false; 
            }
            contadorEspera++;

            if (contadorEspera < IteracionesPorRecoleccion) //Saber si en esa iteración debe agarrar articulos
            { 
                return false; 
            }
            contadorEspera = 0;

            int restantes = ArticulosTotales - ArticulosRecolectados;
            ArticulosRecolectados += Math.Min(ArticulosPorRecoleccion, restantes); //Aqui va a agarrar el numero menor entre estos dos para no pasarse

            if (ArticulosRecolectados >= ArticulosTotales)
            {
                Estado = EstadoCliente.ListoParaCaja; //Ahora se va a la caja
                return true;
            }
            return false;
        }

        //Marca que el cliente ya esta formado en una caja
        public void EntrarAFila()  //Creo que puede ponerse de forma bonita (lo de => )
        { 
            Estado = EstadoCliente.EnFila; 
        }

        // Registra cuantos articulos pudo cobrar la caja(ya que cobra cierta cantidad)
        public int Cobrar(int cantidad)
        {
            int cobrados = Math.Min(cantidad, ArticulosPorCobrar);
            ArticulosPorCobrar -= cobrados;
            return cobrados;
        }

        // Indica si al cliente ya le terminaron de cobrar
        public bool TerminoDePagar => ArticulosPorCobrar == 0;

        //Marca la salida del cliente
        public void Salir(int iteracionActual)
        {
            Estado = EstadoCliente.Terminado;
            IteracionSalida = iteracionActual;
        }


        //Marca que el cliente se fue sin pagar porque no habia cajas disponibles
        public void Abandonar(int iteracionActual)
        {
            Estado = EstadoCliente.Abandono;
            IteracionSalida = iteracionActual;
        }

        //Este es un metodo que ya existe en los objetos en C#
        //Es para obtener una representación en texto del objeto
        //Cuando no se modifica al hacer WriteLine(cliente) solo sale el nombre del objeto
        //Aqui lo sobreescribirmos para que funciona de esta manera:
        public override string ToString()
        {
            string pref = EsPreference ? " [PREFERENCE]" : "";
            return $"Cliente #{Id}{pref} | {Estado} | recolectados {ArticulosRecolectados}/{ArticulosTotales} |  Cada {IteracionesPorRecoleccion} iter. recoge {ArticulosPorRecoleccion} | por cobrar {ArticulosPorCobrar}";
        }
    }
}
