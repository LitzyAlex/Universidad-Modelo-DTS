#include <iostream>
#include <ctime>
using namespace std;

const int nfila=4;
const int ncol=4; 
int**matriz;

void azar() {
    for(int i=0; i<nfila; i++) {
        for(int j=0; j<ncol; j++) {
            matriz[i][j]= 1 + (rand() % 100);
        }
    }
} 

void mostrar() {
    for(int i=0; i<nfila; i++){
        for(int j=0; j<ncol; j++){
            cout<<matriz[i][j] <<" ";
        }
        cout<<"\n";
    }
}

void mayor(){
    int mayor=matriz[0][0];
    for(int i=0; i<nfila; i++) {
        for(int j=0; j<ncol; j++){
            if(matriz[i][j] > mayor){
                mayor=matriz[i][j];
            }
        }
    }
    cout<<"\nEl numero mayor es " <<mayor;
}

void menor(){
    int menor=matriz[0][0];
    for(int i=0; i<nfila; i++){
        for (int j=0; j<ncol; j++) {
            if (matriz[i][j] < menor) {
                menor = matriz[i][j];
            }
        }
    }
    cout<<"\nEl numero menor es " <<menor;
}

int main() {
    srand(time(0));
    matriz= new int*[nfila];
    for(int i=0; i<nfila; i++) {
        matriz[i]= new int[ncol];
    }

azar();
mostrar();
mayor();
menor();


}
