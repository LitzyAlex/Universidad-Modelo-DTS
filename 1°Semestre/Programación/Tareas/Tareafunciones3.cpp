#include <iostream>
using namespace std;

float calcularIVA(float totalCompra){
    float iva = totalCompra * 0.19;
    return iva;
}

int main(){
    float compra;

    cout << "Ingrese el valor total de la compra: ";
    cin >> compra;

    float iva = calcularIVA(compra);

    cout << "\nEl IVA (19%) de la compra es: " << iva << endl;
    cout << "El total a pagar con IVA incluido es: " << compra + iva << endl;

}
