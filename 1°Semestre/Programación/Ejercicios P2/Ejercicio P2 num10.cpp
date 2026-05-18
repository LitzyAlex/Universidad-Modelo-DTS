#include <iostream>
using namespace std;
//Combinar 2 arreglos
int main()
{
	int array1[10];
	int array2[10];
	int array3[20];
	int cont=0;
	cout<<"Ingresa los valores del arreglo 1:\n";
	for(int i=0; i<10; i++) {
		cin>>array1[i];
	}

	cout<<"Ingresa los valores del arreglo 2:\n";
	for(int i=0; i<10; i++) {
		cin>>array2[i];
	}

	for(int i=0; i<20; i++) {
		if(cont<12) {
			array3[i]=array1[i];
			cont++;
		}
		if(cont>=11) {
			array3[i]=array2[i-10];
		}
	}
	cout<<"\nLa combinacion de los array es: ";
	for(int i=0; i<20; i++) {
		cout<<array3[i]<<", ";
	}
}





