#include<iostream>
using namespace std;

int main(){
    int nfila;
    int ncol;
    cout<<"Ingresa el numero de filas: ";
    cin>>nfila;
    cout<<"Ingresa el numero de columnas: ";
    cin>>ncol;
    //reservar memoria
    int**puntero_matriz = new int*[nfila];
    for(int i=0;i<nfila;i++){
        puntero_matriz[i]= new int[ncol];
    }
    //Elementos de la matriz
    for(int i=0;i<nfila;i++){
        for(int j=0;j<ncol;j++){
            cout<<"Digite un numero ["<<i<<"]["<<j<<"]: ";
            cin>>puntero_matriz[i][j];
        }
    }

        for(int i=0;i<nfila;i++){
        for(int j=0;j<ncol;j++){
            cout<<puntero_matriz[i][j];
        }
        cout<<"\n";
    }
}


