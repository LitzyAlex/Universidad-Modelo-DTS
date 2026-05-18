#include <iostream>
using namespace std;
//Buscar un elemento
int main()
{
	int array[10]= {1,10,20,30,40,50,60,70,80,100};
	int buscar=0;
	int cont=0;
	cout<<"Ingresa el valor que quieres buscar: ";
	cin>>buscar;
	for(int i=0; i<10; i++) {
		if(array[i]==buscar) {
			cout<<"\nEl numero " <<array[i] <<" si se encuentra en el array";
		}
		else {
			cont=cont+1;
		}
	}
	if(cont!=9) {
		cout<<"\nEl numero que buscas no esta en el array";
	}
}