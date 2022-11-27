// See https://aka.ms/new-console-template for more information


using System.Reflection;

var name = "Zeynel";
// var nameType = name.GetType();
var nameType = typeof(string);
Console.WriteLine(nameType);

var currentAssembly = Assembly.GetExecutingAssembly();
var typesFromCurrentAssembly = currentAssembly.GetTypes();
foreach (var type in typesFromCurrentAssembly)
{
    Console.WriteLine(type.Name);
}
Console.WriteLine("-------------");
var oneTypeFromCurrentAssembly = currentAssembly.GetType("ReflectionSample.Person");
Console.WriteLine(oneTypeFromCurrentAssembly.Name);

var externalAssembly = Assembly.Load("System.Text.Json");
var typesFromExternalAssembly = externalAssembly.GetTypes();
var oneTypeFromExternalAssembly = externalAssembly.GetType("Sytem.Text.Json.JsonProperty");

var modulesFromExternalAssembly = externalAssembly.GetModules();//Tüm moddüller
var oneModuleFromExternalAssembly = externalAssembly.GetModule("System.Text.Json.dll");//Tek module dll

var typesFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetTypes();
var oneTypeFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetType("System.Text.Json.JsonProperty");

var constructors = oneTypeFromCurrentAssembly.GetConstructors();
foreach (var constructor in oneTypeFromCurrentAssembly.GetConstructors())//Person clasının  constructorları
{
    Console.WriteLine(constructor);
}

// foreach (var method in oneTypeFromCurrentAssembly.GetMethods())
// {
//     Console.WriteLine(method);
// }
//

foreach (var method in oneTypeFromCurrentAssembly.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic))
{
    Console.WriteLine($"{method} , public : {method.IsPublic}");
}

foreach (var method in oneTypeFromCurrentAssembly.GetFields(BindingFlags.Instance|BindingFlags.NonPublic))
{
    Console.WriteLine($"{method} , public : {method.IsPublic}");
}