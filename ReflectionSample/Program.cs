// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using ReflectionSample;

var _networkMonitorSettings = new NetworkMonitorSettings();
Type _warninServiceType;
MethodInfo _warningServiceMethod;
object _warningService = null;
List<object> _warningServiceParameterValues;


BootStrapFromConfiguration();

Console.WriteLine("Monitoring network something went wrong");
Warn();

void Warn()
{
    if (_warningService==null)
    {
        _warningService = Activator.CreateInstance(_warninServiceType);
    }

    var parameters = _networkMonitorSettings.PropertyBag.Select(propertyBagItem => propertyBagItem.Value).ToList();

    _warningServiceMethod.Invoke(_warningService, parameters.ToArray());
    
}

void BootStrapFromConfiguration()
{
   
    
    var appSettingConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json", true
        , true).Build();
    appSettingConfig.Bind("NetworkMonitorSettings",_networkMonitorSettings);
    _warninServiceType = Assembly.GetExecutingAssembly().GetType(_networkMonitorSettings.WarningService);
    if (_warninServiceType==null)
    {
        throw new Exception("Configuration is invalid - warning service not found");
    }

    _warningServiceMethod = _warninServiceType.GetMethod(_networkMonitorSettings.MethodToExecute);
    if (_warningServiceMethod==null)
    {
        throw new Exception("Configuration is invalid - method to execute on warning service not found");
    }

    foreach (var parameter in _warningServiceMethod.GetParameters())
    {
        if (!_networkMonitorSettings.PropertyBag.TryGetValue(parameter.Name, out object parameterValue))
        {
            throw new Exception($"Configuration is invalid - parameter {parameter.Name} not found");
        }

        _warningServiceParameterValues = new List<object>();
        try
        {
            var typedValue = Convert.ChangeType(parameterValue, parameter.ParameterType);
            _warningServiceParameterValues.Add(typedValue);
        }
        catch
        {
            throw new Exception($"Configuration is invalid - parameter {parameter.Name} cannot be converted to expected type {parameter.ParameterType}");
        }

    }

}

// ManipulationObject();
// MetaDataWorking();
void MetaDataWorking()
{
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

    var modulesFromExternalAssembly = externalAssembly.GetModules(); //Tüm moddüller
    var oneModuleFromExternalAssembly = externalAssembly.GetModule("System.Text.Json.dll"); //Tek module dll

    var typesFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetTypes();
    var oneTypeFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetType("System.Text.Json.JsonProperty");

    var constructors = oneTypeFromCurrentAssembly.GetConstructors();
    foreach (var constructor in oneTypeFromCurrentAssembly.GetConstructors()) //Person clasının  constructorları
    {
        Console.WriteLine(constructor);
    }

// foreach (var method in oneTypeFromCurrentAssembly.GetMethods())
// {
//     Console.WriteLine(method);
// }
//

    foreach (var method in oneTypeFromCurrentAssembly.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
        Console.WriteLine($"{method} , public : {method.IsPublic}");
    }

    foreach (var method in oneTypeFromCurrentAssembly.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
    {
        Console.WriteLine($"{method} , public : {method.IsPublic}");
    }
}

void ManipulationObject()
{
    var _typeOfConfiguration = "ReflectionSample.Alien";

    var personType = typeof(Person);
    var personConstructor = personType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    foreach (var constructorInfo in personConstructor)
    {
        Console.WriteLine(constructorInfo);
    }

    var privatePersonConstructor = personType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(int) }, null);
    Console.WriteLine(privatePersonConstructor);

    var person1 = personConstructor[0].Invoke(null);
    var person2 = personConstructor[1].Invoke(new object[] { "Zeynel" });
    var person3 = personConstructor[2].Invoke(new object[] { "Zeynel", 23 });

    var person4 = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person").Unwrap();
    var person5 = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person",
        true, BindingFlags.Instance | BindingFlags.Public, null, new object?[] { "Zeynel" }, null, null);

    var personTypeFromString = Type.GetType("ReflectionSample.Person");
    var person6 = Activator.CreateInstance(personTypeFromString, new object[]
    {
        "Zeynel"
    });

    var person7 = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person", true, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[]
    {
        "Zeynel", 23
    }, null, null);

    var assembly = Assembly.GetExecutingAssembly();
    var person8 = assembly.CreateInstance("ReflectionSample.Person");


    var actualTypeFromConfiguration = Type.GetType(_typeOfConfiguration);
    var iTalkInstance = Activator.CreateInstance(actualTypeFromConfiguration) as ITalk;
    iTalkInstance.Talk("Naber");

    dynamic dynamicITalkInstance = Activator.CreateInstance(actualTypeFromConfiguration);
    dynamicITalkInstance.Talk("Zeynelll"); // Error show not compile time  Error show run time

    var personForManipulation = Activator.CreateInstance("ReflectionSample", "ReflectionSample.Person", true,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null, new object[] { "Zeynel", 23 }, null, null)
        ?.Unwrap();
    var nameProperty = personForManipulation.GetType().GetProperty("Name");
    nameProperty?.SetValue(personForManipulation, "Yavuz");

    var ageField = personForManipulation.GetType().GetField("age");
    ageField?.SetValue(personForManipulation, 24);

    var privateField = personForManipulation.GetType().GetField("_aPrivateField", BindingFlags.Instance | BindingFlags.NonPublic);
    privateField.SetValue(personForManipulation, "update value");

    personForManipulation.GetType().InvokeMember("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, null, personForManipulation, new[] { "Şahin" });
    personForManipulation.GetType().InvokeMember("_aPrivateField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, personForManipulation, new[] { "second update" });
    Console.WriteLine(personForManipulation);

    var talkMethod = personForManipulation.GetType().GetMethod("Talk");
    talkMethod.Invoke(personForManipulation, new[] { "Naber 2" });

    person1.GetType().InvokeMember("Yell", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, personForManipulation, new[] { " yell" });
}