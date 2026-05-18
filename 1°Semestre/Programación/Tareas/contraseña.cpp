#include <iostream>
#include <conio.h>
#include <string>  // ✅ Necesario para string
using namespace std;

int main()
{
    string username;
    string password;
    
    cout << "Iniciar Sesion\n";
    cout << "Username: ";
    cin >> username;
    
    cout << "Password: ";
    password = "";
    char ch;
    
    while ((ch = _getch()) != 13 && ch != 10) { // 13 = Enter
        if (ch == 8) { // Backspace
            if (!password.empty()) {
                password.pop_back();
                cout << "\b \b";
            }
        } else {
            password += ch;
            cout << '*';
        }
    }
    cout << endl;
    
    // ✅ Para verificar que funcionó
    cout << "Usuario: " << username << endl;
    cout << "Password ingresada: " << password << endl;
    

}