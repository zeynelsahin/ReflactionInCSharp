namespace ReflectionSample;

public interface ITalk
{
    void Talk(string sentence);
}

public class EmployeeMarkerAttribute : Attribute
{
}

[EmployeeMarker]
public class Employee : Person
{
    public string Company { get; set; }
}

public class Alien : ITalk
{
    public void Talk(string sentence)
    {
        Console.WriteLine($"Alien talking : {sentence}");
    }
}

public class Person: ITalk
{
    public Person() 
    {
        Console.WriteLine("A person is being created");
    }

    public Person(string name)
    {
        Console.WriteLine($"A person with name {name} ,s being create");
        Name = name;
    }

    protected Person(string name, int age)
    {
        Console.WriteLine($"A person with name {name} ang age {age} is being created using a private constructor");
        Name = name;
        this.age = age;
    }
    public string Name { get; set; }
    public int age;
    private string _aPrivateField = "initial private field value";
    public void Talk(string sentence)
    {
        Console.WriteLine($"Talking : {sentence}");
    }

    protected void Yell(string sentence)
    {
        Console.WriteLine($"Yelling {sentence}");
    }

    public override string ToString()
    {
        return $"{Name} {age} {_aPrivateField}";
    }
}