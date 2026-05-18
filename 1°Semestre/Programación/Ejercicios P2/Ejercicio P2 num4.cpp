#include <iostream>
using namespace std;
//Encontrar el maximo y el minimo
int main()
{
	int array[10];
	int suma=0;
	int mayor=0;
	int menor=0;
	cout<<"Ingresa los valores del arreglo:\n";
	for(int i=0; i<10; i++) {
		cin>>array[i];
	}

	for(int i=0; i<10; i++) {
		if(array[i]>mayor) {
			mayor=array[i];
		}
	}
	if(array[0]<=mayor) {
		menor=array[0];
	}
	for(int i=0; i<10; i++) {
		if(array[i]<menor) {
			menor=array[i];
		}
	}
	cout<<"El numero mayor es " <<mayor;
	cout<<"\nEl numero menor es " <<menor;
}
