class Program {
public struct Empleado /*(struct/class)*/
{
    public double salarioBase;
    public double comision;

    public Empleado(double salarioBase, double comision)
    {
        this.salarioBase = salarioBase;
        this.comision = comision;
    }

    public override string ToString()
    {
        return $"Salario Base: {salarioBase}, Comisión: {comision}";
    }

    public void CambiarSalario(Empleado e, double incremento)
    {
        e.salarioBase += incremento;
        e.comision += incremento;
    }
}

    static void Main() {  
        Empleado empleado1 = new Empleado(1200,250);
        Console.WriteLine(empleado1.ToString());

        Empleado empleado2 = empleado1;

        empleado2.CambiarSalario(empleado2,100);
        Console.WriteLine(empleado1.ToString());
    }
}
