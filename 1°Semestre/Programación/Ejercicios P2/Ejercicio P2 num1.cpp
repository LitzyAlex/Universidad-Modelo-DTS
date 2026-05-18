#include <iostream>
using namespace std;
//Ingresar y mostrar un arreglo
int main()
{
	int array[5];
	cout<<"Ingresa los valores del arreglo:\n";
	for(int i=0; i<5; i++) {
		cin>>array[i];
	}

	cout<<"Los valores ingresados fueron: ";
	for(int i=0; i<5; i++) {
		cout<<array[i]<<", ";
	}
}