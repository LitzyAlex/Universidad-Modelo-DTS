#include <iostream>
#include <ctime>
using namespace std;

int alumnos = 10;
int materias = 6;
float** calificaciones;
float promedios[10]; 

void calazar() {
    for (int i = 0; i < alumnos; i++) {
        for (int j = 0; j < materias; j++) {
            calificaciones[i][j] = 4 + (rand() % 10); 
        }
    }
}


void mostrarcal() {
    cout << "Calificaciones:\n";
    for (int i = 0; i < alumnos; i++) {
        cout << "Alumno " << i + 1 << ": ";
        for (int j = 0; j < materias; j++) {
            cout << calificaciones[i][j]<<", ";
        }
        cout << "\n";
    }
}

void calcularPromedio() {
    for (int i = 0; i < alumnos; i++) {
        int suma = 0;
        for (int j = 0; j < materias; j++) {
            suma=suma+ calificaciones[i][j];
        }
        promedios[i] = suma/materias;
    }
}


void mostrarPromedios() {
    cout << "\nPromedios:\n";
    for (int i = 0; i < alumnos; i++) {
        cout << "Alumno " << i + 1 << ": " << promedios[i] << "\n";
    }
}

void beca() {
    cout << "\nCandidatos beca:\n";
    for (int i = 0; i < alumnos; i++) {
        int reprobo=0;
        for (int j = 0; j < materias; j++) {
            if (calificaciones[i][j]<=5) {
                reprobo++;
            }
        }

        if (promedios[i]>= 8 && reprobo==0) {
            cout << "Alumno " << i + 1;
        }
        cout<<"\n";
    }
}

int main() {
    srand(time(0));
    calificaciones = new float*[alumnos];
    for (int i = 0; i < alumnos; i++) {
        calificaciones[i] = new float[materias];
    }


    calazar();
    mostrarcal();
    calcularPromedio();
    mostrarPromedios();
    beca();
}
