#include <iostream>
using namespace std;

int obtenerDias(int totalSeg){
    return totalSeg / 86400; 
}

int obtenerHoras(int totalSeg){
    return (totalSeg % 86400) / 3600; 
}

int obtenerMinutos(int totalSeg){
    int resto = totalSeg % 3600;
    return resto / 60; 
}


int obtenerSegundos(int totalSeg){
    return totalSeg % 60;
}

int main(){
    int segundos;

    cout << "Ingrese los segundos totales: ";
    cin >> segundos;

    int dias = obtenerDias(segundos);
    int horas = obtenerHoras(segundos);
    int minutos = obtenerMinutos(segundos);
    int segRest = obtenerSegundos(segundos);

    cout << "\nEquivalencia:\n";
    cout << dias << " dias\n";
    cout << horas << " horas\n";
    cout << minutos << " minutos\n";
    cout << segRest << " segundos\n";

}
