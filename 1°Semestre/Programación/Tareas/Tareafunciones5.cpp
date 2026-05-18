#include <iostream>
using namespace std;

int esPrimo(int n){
    if(n <= 1){
        return 0; 
    }

    for(int i = 2; i < n; i++){
        if(n % i == 0){
            return 0; 
        }
    }

    return 1; 
}

int main(){
    int num;

    cout << "Ingrese un numero: ";
    cin >> num;

    int primo = esPrimo(num);

    if(primo == 1){
        cout << "\nEl numero " << num << " ES primo.";
    } else {
        cout << "\nEl numero " << num << " NO es primo.";
    }
}
