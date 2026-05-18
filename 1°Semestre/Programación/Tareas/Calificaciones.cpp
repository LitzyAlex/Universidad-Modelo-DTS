#include <iostream>
using namespace std;

int main() { 
    int nEstudiantes;
    cout << "Introduce el numero de estudiantes: "; 
    cin >> nEstudiantes;

    int nMaterias;
    cout << "Introduce el numero de materias: ";
    cin >> nMaterias;

    string* estudiantes = new string[nEstudiantes];
    for (int i = 0; i < nEstudiantes; i++) {
        cout << "Introduce el nombre del estudiante " << i + 1 << ": ";
        cin >> estudiantes[i];
    }

    string* materias = new string[nMaterias];
    for (int i = 0; i < nMaterias; i++) {
        cout << "Introduce el nombre de la materia " << i + 1 << ": "; 
        cin >> materias[i];
    }

    float** calificaciones = new float* [nEstudiantes];
    for (int i = 0; i < nEstudiantes; i++) {
        calificaciones[i] = new float[nMaterias];
    }

    float* promedios = new float[nEstudiantes];

    for (int i = 0; i < nEstudiantes; i++) {
        float suma = 0;
        for (int j = 0; j < nMaterias; j++) {
            cout << "Introduce la calificacion del estudiante " << estudiantes[i]
                << " en la materia " << materias[j] << ": ";
            cin >> calificaciones[i][j];
            suma += calificaciones[i][j];
        }
        promedios[i] = suma / nMaterias;
    }

    cout << "\nRESULTADOS\n";

    for (int i = 0; i < nEstudiantes; i++) {
        cout << "\nEstudiante: " << estudiantes[i] << endl;
        for (int j = 0; j < nMaterias; j++) {
            cout << "  " << materias[j] << ": " << calificaciones[i][j] << endl;
        }
        cout << "  Promedio: " << promedios[i];
    }

}