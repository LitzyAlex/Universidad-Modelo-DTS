#include <iostream>
using namespace std;
//Ordenar el arreglo
int main()
{
	int array[5]={3,1,5,4,2};
	int cont=0;

for(int i=0; i<4; i++) {
    for(int j=i+1;j<5;j++){
    if(array[i]>array[j]){
        cont=array[i];
        array[i]=array[j];
        array[j]=cont;
    }
    }
}
cout<<"Los valores ordenados son: ";
	for(int i=0; i<5; i++) {
		cout<<array[i]<<", ";
	}
}

