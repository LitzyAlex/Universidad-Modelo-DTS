#include <iostream>
using namespace std;
//Ordenar de manera alternativa
int main()
{
	int array[10]= {10,9,8,7,6,5,4,3,2,1};
	int array2[10]= {0,0,0,0,0,0,0,0,0,0};
	int cont=0;
	int atras=9;
	int adelante=0;

	for(int i=0; i<9; i++) {
		for(int j=i+1; j<10; j++) {
			if(array[i]>array[j]) {
				cont=array[i];
				array[i]=array[j];
				array[j]=cont;
			}
		}
	}

	for(int i=0; i<10; i++) {
		if(i%2==0) {
			array2[i]=array[atras];
			atras--;
		}
	}

	for(int i=0; i<10; i++) {
		if(i%2==1) {
			array2[i]=array[adelante];
			adelante++;
		}
	}

	cout<<"\nLos valores ordenados alternativamente son: ";
	for(int i=0; i<10; i++) {
		cout<<array2[i]<<", ";
	}
}