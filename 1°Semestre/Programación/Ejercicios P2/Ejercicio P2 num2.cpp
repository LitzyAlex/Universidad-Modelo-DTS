#include <iostream>
using namespace std;
//Suma de elementos
int main()
{
	int array[10];
	int suma=0;
	cout<<"Ingresa los valores del arreglo:\n";
	for(int i=0; i<10; i++) {
		cin>>array[i];
	}

	cout<<"\nLa suma de esos valores es de ";
	for(int i=0; i<10; i++) {
		suma=suma+array[i];
	}
	cout<<suma;
}