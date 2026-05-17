using System.Diagnostics;

namespace Delegates;

#region Basic

public static class DelegateBasic
{
    /*
        Delegate customizado.

        Ele representa QUALQUER método que:
        - recebe 2 int
        - retorna int
    */

    public static void Run()
    {
        /*
            Estamos armazenando um método
            dentro do delegate.
        */

        Operation sum = Sum;
        Operation subtract = Subtract;

        Console.WriteLine(sum(10, 5)); // 15
        Console.WriteLine(subtract(10, 5)); // 5

        Console.WriteLine(Execute(10, 5, sum)); // 15
        Console.WriteLine(Execute(10, 5, subtract)); // 5
    }

    public static void RunExercise()
    {
        Operation multiply = Multiply;

        Console.WriteLine(Execute(10, 5, multiply));

        Console.WriteLine("Digite a operação (sum, subtract, multiply):");
        var operationInput = Console.ReadLine();

        Operation selectedOperation;

        if (operationInput == "sum")
        {
            selectedOperation = Sum;
        }
        else if (operationInput == "subtract")
        {
            selectedOperation = Subtract;
        }
        else if (operationInput == "multiply")
        {
            selectedOperation = Multiply;
        }
        else
        {
            Console.WriteLine("Operação inválida.");
            return;
        }

        var result = Execute(10, 5, selectedOperation);

        Console.WriteLine($"Resultado final: {result}");
    }

    public static int Execute(int left, int right, Operation operation)
    {
        Console.WriteLine("Iniciando operação...");

        var result = operation(left, right);

        Console.WriteLine("Finalizando operação...");

        return result;
    }

    public delegate int Operation(int left, int right);

    private static int Sum(int left, int right) => left + right;

    private static int Subtract(int left, int right) => left - right;

    private static int Multiply(int left, int right) => left * right;
}

#endregion

#region Action

public static class DelegateAction
{
    /*
        Action é um delegate pré-definido
        que representa um método que NÃO RETORNA NADA (void).
    */
    public static void Run()
    {
        Action<string> print = Print;
        // ou pode ser assim: Action<string> print = message => Console.WriteLine(message);
        print("Hello, Action!");

        Execute(print);
    }

    public static void RunExercise()
    {
        Action<string> print = Print;

        print("Logging Error");
        Execute(print);

        ExecuteWithTimer(print);

        List<Action<string>> pipelines =
        [
            Validate,
            Transform,
            Persist,
            Notify
        ];

        ExecutePipeline("pedido criado", pipelines);
        ExecuteMulticast("pedido criado");
    }

    private static void Execute(Action<string> action)
    {
        Console.WriteLine("Iniciando execução...");

        action("Executando Action");

        Console.WriteLine("Finalizando execução...");
    }

    private static void ExecutePipeline(
        string message,
        IEnumerable<Action<string>> pipeline)
    {
        Console.WriteLine("Iniciando pipeline...");

        foreach (var step in pipeline)
        {
            step(message);
        }

        Console.WriteLine("Finalizando pipeline...");
    }

    private static void ExecuteMulticast(string message)
    {
        Action<string> process = Validate;

        process += Transform;
        process += Persist;
        process += Notify;

        process(message);
    }

    private static void ExecuteWithTimer(Action<string> action)
    {
        Console.WriteLine("Iniciando execução...");
        var stopwatch = Stopwatch.StartNew();
        action("Executando Action");
        stopwatch.Stop();
        Console.WriteLine($"Finalizando execução... Tempo: {stopwatch.ElapsedMilliseconds} ms");
    }

    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private static void Validate(string message)
    {
        Console.WriteLine($"[VALIDATE] {message}");
    }

    private static void Transform(string message)
    {
        Console.WriteLine($"[TRANSFORM] {message.ToUpper()}");
    }

    private static void Persist(string message)
    {
        Console.WriteLine($"[PERSIST] {message}");
    }

    private static void Notify(string message)
    {
        Console.WriteLine($"[NOTIFY] {message}");
    }
}

#endregion

#region Func

public static class DelegateFunc
{
    /*
        Func é um delegate pré-definido
        que representa um método que RETORNA VALOR.

        O último tipo genérico sempre representa o retorno.

        Func<int, int>
        - recebe int
        - retorna int

        Func<int, int, int>
        - recebe int
        - recebe int
        - retorna int
    */

    public static void Run()
    {
        Func<int, int, int> sum = (a, b) => a + b;
        Console.WriteLine(sum(10, 5)); // 15
    }

    public static void RunExercise()
    {
        var price = 100m;

        Func<int, int, int> sum = (a, b) => a + b;
        Func<int, int, int> subtract = (a, b) => a - b;
        Func<int, int, int> multiply = (a, b) => a * b;
        Console.WriteLine(Execute(10, 5, sum)); // 15
        Console.WriteLine(Execute(10, 5, subtract)); // 5
        Console.WriteLine(Execute(10, 5, multiply)); // 50

        List<Func<decimal, decimal>> pipelines =
        [
            ApplyDiscount,
            ApplyTax,
            Round
        ];

        var finalPrice = ExecutePipeline(price, pipelines);
        Console.WriteLine($"Preço final: {finalPrice}");
    }

    private static int Execute(int left, int right, Func<int, int, int> operation)
    {
        Console.WriteLine("Iniciando operação...");
        var result = operation(left, right);
        Console.WriteLine("Finalizando operação...");
        return result;
    }

    private static T ExecutePipeline<T>(
        T input,
        IEnumerable<Func<T, T>> pipeline)
    {
        var current = input;

        foreach (var step in pipeline)
        {
            current = step(current);
        }

        return current;
    }

    private static decimal ApplyDiscount(decimal price)
    {
        return price * 0.9m;
    }

    private static decimal ApplyTax(decimal price)
    {
        return price * 1.1m;
    }

    private static decimal Round(decimal price)
    {
        return Math.Round(price, 2);
    }
}

#endregion

#region Predicate

public static class DelegatePredicate
{
    /*
        Predicate<T>

        Recebe T
        Retorna bool

        Ele representa:
        - regras
        - filtros
        - validações
        - condições
    */

    public static void Run()
    {
        Console.WriteLine(IsEven(10)); // True
        Console.WriteLine(IsEven(5)); // False

        List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        List<int> evenNumbers = Filter(numbers, IsEven);
        Console.WriteLine("Números pares:");
        evenNumbers.ForEach(Console.WriteLine);
    }

    public static void RunExercise()
    {
        List<User> users =
        [
            new("Rafael", 25, true),
            new("João", 17, true),
            new("Maria", 30, false),
            new("Pedro", 40, true)
        ];

        Predicate<User> isAdult = user => user.Age >= 18;
        Predicate<User> isActive = user => user.IsActive;

        List<User> activeAdults = Filter(users, user => isAdult(user) && isActive(user));
        Console.WriteLine("Usuários ativos e adultos:");
        activeAdults.ForEach(user => Console.WriteLine(user.Name));
    }

    private static List<T> Filter<T>(
        IEnumerable<T> items,
        Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in items)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    private static bool IsEven(int value)
    {
        return value % 2 == 0;
    }

    public sealed class User
    {
        public User(
            string name,
            int age,
            bool isActive)
        {
            Name = name;
            Age = age;
            IsActive = isActive;
        }

        public string Name { get; }
        public int Age { get; }
        public bool IsActive { get; }
    }
}

#endregion