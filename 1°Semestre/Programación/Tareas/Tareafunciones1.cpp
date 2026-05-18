#include<iostream>
#include <cctype>
using namespace std;
 
int main(){
char frase[100], subcadena[50];
char carac;
bool encontrada=false;
cout<<"Ingresa la frase: \n";
cin.getline(frase,100);

cout<<"\nQue caracter le gustaria buscar: ";
cin>>carac;
int cont=0;
    for (int i =0; frase[i]!= '\0'; i++) {
        if (tolower(frase[i]) == tolower(carac)) {
            cont++;
        }
    }
cout<<"\nLa letra "<<carac<<" aparece "<<cont<<" veces en la frase";

int cont2=1;
char espacio=' ';
for(int i=0;frase[i]!='\0';i++){
    if(frase[i]==espacio){
        cont2++;
    }
}

cout<<"\nLa frase tiene "<<cont2<<" palabras";

  cout << "Escribe la subcadena que desa buscar en la frase principal: ";
    cin.getline(subcadena, 50);
for (int i = 0; frase[i] != '\0'; i++) {
        int j = 0;
        while (frase[i + j] != '\0' && subcadena[j] != '\0' && frase[i + j] == subcadena[j]) {
            j++;
        }
   if (subcadena[j] == '\0') {
            encontrada = true;
            break;
        }
    }

    if (encontrada){
        cout << "La subcadena SÍ se encuentra en la frase." << endl;}
    else{
        cout << "La subcadena NO se encuentra en la frase." << endl;
    }

}

