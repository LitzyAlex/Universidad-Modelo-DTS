#include <iostream>
using namespace std;

int verificarLargo(string pass){
    if(pass.length() > 8){
        return 1; 
    }
    return 0; 
}


int verificarMayuscula(string pass){
    for(int i = 0; i < pass.length(); i++){
        if(pass[i] >= 'A' && pass[i] <= 'Z'){
            return 1; 
        }
    }
    return 0; 
}


int verificarNumero(string pass){
    for(int i = 0; i < pass.length(); i++){
        if(pass[i] >= '0' && pass[i] <= '9'){
            return 1;
        }
    }
    return 0; 
}

int main(){
    string password;

    cout << "Ingrese una contrasena para validar: ";
    cin >> password;

    int largo = verificarLargo(password);
    int mayus = verificarMayuscula(password);
    int numero = verificarNumero(password);

    if(largo == 1 && mayus == 1 && numero == 1){
        cout << "\nLa contrasena ES segura.";
    } else {
        cout << "\nLa contrasena NO es segura.";
        cout << "\nRequisitos:\n";
        cout << "- Mas de 8 caracteres\n";
        cout << "- Al menos una letra mayuscula\n";
        cout << "- Al menos un numero\n";
    }

  
}
