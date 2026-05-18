#include <iostream>
using namespace std;
//Promedio de elementos
int main()
{
	int array[10];
	int suma=0;
	float promedio=0;
	cout<<"Ingresa los valores del arreglo:\n";
	for(int i=0; i<10; i++) {
		cin>>array[i];
	}

	for(int i=0; i<10; i++) {
		suma=suma+array[i];
	}
	promedio=suma/10;
	cout<<"El promedio de esos numeros es de " <<promedio;
}