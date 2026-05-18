#include <iostream>
using namespace std;
//Eliminar un elemento
int main()
{
	int array[10]= {1,2,3,4,5,6,7,8,9,1};
	int error=0;
	cout<<"Array=";
	for(int i=0; i<10; i++) {
		cout<<array[i]<<", ";
	}

	cout<<"\nQue valor quieres eliminar: ";
	cin>>error;
	for(int i=0; i<10; i++) {
		if(error==array[i]) {
			array[i]=0;
		}
	}

	cout<<"El array es: ";
	for(int i=0; i<10; i++) {
		cout<<array[i]<<", ";
	}
}
