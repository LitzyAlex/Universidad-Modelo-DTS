#include<iostream>
using namespace std;
int main(){
    int nVendedores;
    int nProductos;

    cout<<"Ingresa el numero de vendedores: ";
    cin>>nVendedores;
    cout<<"Ingresa el numero de productos: ";
    cin>>nProductos;

    string* vendedores = new string[nVendedores];
    for(int i=0; i<nVendedores; i++){
        cout<<"Ingresa el nombre del vendedor "<<i+1<<":";
        cin>>vendedores[i];
    }

    string* productos = new string[nProductos];
    for(int i=0; i<nProductos; i++){
        cout<<"Ingresa el producto "<<i+1<<":";
        cin>>productos[i];
    }

    int** cantidades= new int*[nVendedores];
    for(int i=0; i<nVendedores; i++){
        cantidades[i]= new int[nProductos];
    }

    int* total= new int[nVendedores];

    for (int i = 0; i < nVendedores; i++) {
        int suma = 0;
        for (int j = 0; j < nProductos; j++) {
            cout << "Introduce la cantidad que vendio " << vendedores[i]
                << " del producto " << productos[j] << ": ";
            cin >> cantidades[i][j];
            suma=suma+cantidades[i][j];
        }
        total[i] = suma;
    }

    cout<<"\nInformación\n";  

    for(int i=0;i<nVendedores;i++){
        cout<<"\nVendedor: " <<vendedores[i]<<endl;
            for (int j = 0; j < nProductos; j++) {
            cout<< productos[j] << ": " << cantidades[i][j] <<endl;
        }
        cout<<"Total vendido: " <<total[i]<<endl;
    }

    int mayor=0;
    for(int i=0;i<nVendedores;i++){
        for(int j=0;j<nProductos;j++){
            if(cantidades[i][j]>mayor){
                mayor=cantidades[i][j];
            }
        }
    }

    string* pro = new string[nProductos];  
    for (int i = 0; i < nProductos; i++) {
    pro[i] = "";  
    }
    cout << "\nEl producto mas vendido es ";
    for (int i = 0; i < nVendedores; i++) {
     for (int j = 0; j < nProductos; j++) {
        if (cantidades[i][j] == mayor && pro[j]!=productos[j]) {  
            pro[j] = productos[j];  
        cout << productos[j]<<", ";        
        }   
     }
    }

    int mayor2=0;
    for(int i=0;i<nVendedores;i++){
        if(total[i]>mayor2){
            mayor2=total[i];
        }
    }

cout<<"\nEl vendedor(es) que vendio mas productos en total fue ";
    for(int i=0; i<nVendedores;i++){
        if(mayor2==total[i]){
            cout<<vendedores[i]<<", ";
        }
    }

    cout<<"con un total vendido de "<<mayor2;

}