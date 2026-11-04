List<Animal> animales = new List<Animal>
{
    new Gato("Michi"),
    new Perro("Rex"),
    new Animal("Bicho")
};

foreach (Animal animal in animales)
{
    Console.WriteLine(animal.Nombre + " dice: " + animal.HacerSonido());
}

class Animal
{
    public string Nombre { get; }

    public Animal(string nombre)
    {
        Nombre = nombre;
    }

    public virtual string HacerSonido()
    {
        return "SIUUU";
    }
}

class Gato : Animal
{
    public Gato(string nombre) : base(nombre) { }

    public override string HacerSonido()
    {
        return "Miau";
    }
}

class Perro : Animal
{
    public Perro(string nombre) : base(nombre) { }

    public override string HacerSonido()
    {
        return "Guau";
    }
}