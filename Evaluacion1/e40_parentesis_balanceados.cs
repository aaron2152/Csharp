string[] pruebas = { "((a+b)*c)", "(a+b))(", "{[()]}", "{[(])}", "" };

foreach (string texto in pruebas)
{
    string resultado = EstaBalanceado(texto) ? "balanceado" : "NO balanceado";
    Console.WriteLine("\"" + texto + "\" -> " + resultado);
}

bool EstaBalanceado(string texto)
{
    Stack<char> pila = new Stack<char>();

    foreach (char c in texto)
    {
        if (c == '(' || c == '[' || c == '{')
        {
            pila.Push(c);
        }
        else if (c == ')' || c == ']' || c == '}')
        {
            if (pila.Count == 0) return false;

            char abierto = pila.Pop();
            if ((c == ')' && abierto != '(') ||
                (c == ']' && abierto != '[') ||
                (c == '}' && abierto != '{')) return false;
        }
    }

    return pila.Count == 0;
}