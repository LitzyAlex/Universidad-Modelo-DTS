#include<iostream>
using namespace std;

float promedio(float a, float b, float c){
    float promedio=(a+b+c)/3;
    return promedio;
}

int main(){
    string nombre;
    float cal[3];
    cout<<"Ingrese el nombre del estudiantes: ";
    cin>>nombre;
    cout<<"\nIngrese las 3 calificaciones del estudiante: \n";
    for(int i=0;i<3;i++){
        cout<<"Calificacion " <<i+1 <<": ";
        cin>>cal[i];
    }

    cout<<"El promedio de " <<nombre << " es de ";
    cout<<promedio(cal[0],cal[1],cal[2]);
}