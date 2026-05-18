#include <iostream>
#include <ctime>
#include <cstdlib>
#include <string>
#include<vector>
#include<sstream>
#include <fstream>//Nuevo :c

using namespace std;

struct Personas{ //clase, o super variable
    string nombre;
    int edad;
};

void GetTexto(){
    ifstream archivo;
    vector<Personas> persona;
    string linea;

    while(getline(archivo,linea)){
        stringstream ss(linea); //leer cada linea que existe en el texto
        Personas p;
        getline(ss,p.nombre,',');
        ss >> p.edad;
        persona.push_back(p);
    }

    for(int i=0; i<persona.size(); i++){
        cout<<persona[i].nombre <<" - " <<persona[i].edad <<endl;
    }

}



void Escribir(){
    ofstream archivo;
    archivo.open("Data.txt", ios::out);

    if(archivo.fail()){ //medida de prevención
        cout<<"No pudo acceder al archivo";
        exit(1);
    }

    archivo <<"Hola mundo uwu";
    archivo.close();
}

void Leer(){
    ifstream archivo;
    string texto;

    archivo.open("Data.txt", ios::in);

    if(archivo.fail()){
        cout << "No puedo acceder al archivo";
        exit(1);
    }

    while(!archivo.eof()){
        getline(archivo, texto);
        cout << texto << endl;
    }

    archivo.close();
}

void Agregar(){
    ofstream archivo;
    archivo.open("Data.txt", ios::app);

    if(archivo.fail()){
   cout << "No puedo acceder al archivo";
    exit(1);
    }

    archivo << " , pero pronto estare listo";
    archivo.close();

}

int main()
{
   // Escribir();
    // Leer();
    //Agregar();
    GetTexto();
}