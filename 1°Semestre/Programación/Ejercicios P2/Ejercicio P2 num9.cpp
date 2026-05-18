#include <iostream>
using namespace std;
//Contar ocurrencias
int main()
{
	int array[10]= {1,2,3,4,5,6,7,8,9,1};
	int valor=0;
	int cont=0;
	cout<<"Ingresa un valor para ver cuantas veces aparece en el array: ";
	cin>>valor;
	for(int i=0; i<10; i++) {
		if(array[i]==valor) {
			cont++;
		}
	}
	cout<<"\nEl "<<valor <<" aparece "<<cont <<" veces";
}