#include<iostream>
using namespace std;

int suma(int a, int b){
    int suma=a+b;
    return suma;
}
void mult(int a, int b){
    int mult=a*b;
    cout<<mult;
}

int main(){
    int num1=1;
    int num2=2;
    int num3=3;
    int num4=4;
   cout<< suma(num1,num2)<<endl;
    cout<<suma(num3,num4)<<endl;
    mult(num1,num2);
}