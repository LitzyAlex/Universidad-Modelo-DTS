#include <iostream>
#include <cctype>
using namespace std;

void limpiarFrase(char original[], char limpia[]){
    int j = 0;
    for(int i = 0; original[i] != '\0'; i++){
        if(original[i] != ' '){
            limpia[j] = tolower(original[i]);
            j++;
        }
    }
    limpia[j] = '\0';
}

int esPalindroma(char texto[]){
    int inicio = 0;
    
    int fin = 0;
    while(texto[fin] != '\0'){
        fin++;
    }
    fin--; 

    while(inicio < fin){
        if(texto[inicio] != texto[fin]){
            return 0; 
        }
        inicio++;
        fin--;
    }
    return 1; 
}

int main(){
    char frase[100];
    char fraseLimpia[100];

    cout << "Ingrese una palabra o frase: ";
    cin.getline(frase, 100);

    limpiarFrase(frase, fraseLimpia);

    int resultado = esPalindroma(fraseLimpia);

    if(resultado == 1){
        cout << "\nLa frase ES palindroma.";
    } else {
        cout << "\nLa frase NO es palindroma.";
    }

}
