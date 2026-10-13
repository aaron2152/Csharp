Console.OutputEncoding = System.Text.Encoding.UTF8;
string negocio = "Panadería La Espiga";
Console.WriteLine("=== " + negocio + " ===");
Console.Write("¿Cómo se llama? ");
string cliente = Console.ReadLine();
DateTime ahora = DateTime.Now;
Console.WriteLine("Hola, " + cliente + ". Son las " + ahora.ToString("HH:mm") + " del " + ahora.ToString("dd/MM/yyyy" + "."));
Console.WriteLine("Gracias por visitarnos en " + negocio + ".");