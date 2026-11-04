using System;

CuentaBancaria cuenta = new CuentaBancaria("Lucia", 100000);

cuenta.Depositar(50000);
cuenta.Retirar(30000);
cuenta.Retirar(500000);
cuenta.Mostrar();

class CuentaBancaria
{
    public string Titular { get; }
    public decimal Saldo { get; private set; }

    public CuentaBancaria(string titular, decimal saldoIncial)
    {
        Titular = titular;
        Saldo = saldoIncial;
    }

    public void Depositar(decimal monto)
    {
        if (monto <= 0)
        {
            Console.WriteLine("El monto debe ser positivo.");
            return;
        }
        Saldo += monto;
    }

    public void Retirar(decimal monto)
    {
        if (monto > Saldo)
        {
            Console.Write("Saldo insuficiente para retirar " + monto + ".");
            return;
        }
        Saldo -= monto;
    }

    public void Mostrar()
    {
        Console.WriteLine(Titular + " tiene: " + Saldo + " Gs.");
    }
}