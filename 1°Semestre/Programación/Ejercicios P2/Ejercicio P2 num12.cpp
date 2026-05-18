#include <iostream>
using namespace std;
//Elminar duplicados
int main()
{
	int array[10];
	int array2[10];
	int n=0;
	int repetido=0;
	cout<<"Ingrese los valores del arreglo:\n";
	for(int i=0; i<10; i++) {
		cin>>array[i];
	}

	cout<<"\nArreglo: ";
	for(int i=0; i<10; i++) {
		cout<<array[i]<<", ";
	}

	for(int i=0; i<10; i++) {
		repetido=0;
		for(int j=0; j<n; j++) {
			if(array[i]==array2[j]) {
				repetido=1;
				break;
			}
		}
		if(repetido==0) {
			array2[n]=array[i];
			n++;
		}
	}
	cout<<"\nArreglo sin repeticiones: ";
	for(int i=0; i<n; i++) {
		cout<<array2[i] <<", ";
	}
}