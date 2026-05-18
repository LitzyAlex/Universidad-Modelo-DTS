#include <iostream>
using namespace std;

int pedirNumero(string texto){
    int n;
    cout << texto;
    cin >> n;
    return n;
}

void leerEstudiantes(string estudiantes[], int n){
    for (int i = 0; i < n; i++) {
        cout << "Introduce el nombre del estudiante " << i + 1 << ": ";
        cin >> estudiantes[i];
    }
}

void leerMaterias(string materias[], int n){
    for (int i = 0; i < n; i++) {
        cout << "Introduce el nombre de la materia " << i + 1 << ": ";
        cin >> materias[i];
    }
}

float** crearMatriz(int nEst, int nMat){
    float** matriz = new float*[nEst];
    for (int i = 0; i < nEst; i++){
        matriz[i] = new float[nMat];
    }
    return matriz;
}

void llenarCalificacionesYPromedios(float** calificaciones, float promedios[],string estudiantes[],string materias[], int nEst, int nMat){
    for (int i = 0; i < nEst; i++){
        float suma = 0;
        for (int j = 0; j < nMat; j++){
            cout << "Introduce la calificacion del estudiante " 
                 << estudiantes[i] << " en la materia " << materias[j] << ": ";
            cin >> calificaciones[i][j];
            suma += calificaciones[i][j];
        }
        promedios[i] = suma / nMat;
    }
}

void mostrarResultados(float** calificaciones, float promedios[],string estudiantes[],string materias[],int nEst,int nMat){
    cout << "\nRESULTADOS\n";
    for (int i = 0; i < nEst; i++){
        cout << "\nEstudiante: " << estudiantes[i];
        for (int j = 0; j < nMat; j++){
            cout << "\n  " << materias[j] << ": " << calificaciones[i][j];
        }
    cout << "\nPromedio: " << promedios[i];
    }
}


int main() {
    int nEstudiantes = pedirNumero("Introduce el numero de estudiantes: ");
    int nMaterias = pedirNumero("Introduce el numero de materias: ");

    string* estudiantes = new string[nEstudiantes];
    string* materias = new string[nMaterias];

    leerEstudiantes(estudiantes, nEstudiantes);
    leerMaterias(materias, nMaterias);

    float** calificaciones = crearMatriz(nEstudiantes, nMaterias);
    float* promedios = new float[nEstudiantes];

    llenarCalificacionesYPromedios(calificaciones, promedios,estudiantes, materias,nEstudiantes, nMaterias);

    mostrarResultados(calificaciones, promedios,estudiantes, materias,nEstudiantes, nMaterias);

}
