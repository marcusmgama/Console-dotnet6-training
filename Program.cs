// See https://aka.ms/new-console-template for more information

Console.WriteLine("\n------| PoupaDev |-------\n");


var objectives = new List<FinanceObjective>{

    new FinanceObjective ("Viagem a Orlando", 25000),
    new FinanceObjectiveWithDeadline(new DateTime(2023, 10, 1), "Viagem a Orlando com prazo", 25000)

};

foreach (var objective in objectives)
{
    objective.PrintSummary();
}

ShowMenu();

var option = Console.ReadLine();

while (option != "0")
{
    switch (option)
    {
        case "1":
            //AddObjective
            AddObjective();
            break;
        case "2":
            //Deposit
            DoOperation(OperationType.Deposit);
            break;
        case "3":
            //Withdraw
            DoOperation(OperationType.Withdraw);
            break;
        case "4":
            GetDetails();
            break;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }

    ShowMenu();
    option = Console.ReadLine();

}

#region Methods

void ShowMenu()
{
    Console.WriteLine("Digite 1 para Cadastro de Objetivo.");
    Console.WriteLine("Digite 2 para realizar um Depósito.");
    Console.WriteLine("Digite 3 para realizar um Levantamento.");
    Console.WriteLine("Digite 4 para Exibir detalhes de um Objetivo.");
    Console.WriteLine("Digite 0 para sair.\n");
}

void AddObjective()
{
    Console.Write("Digite um título: ");
    var title = Console.ReadLine();

    Console.Write("Digite um valor de objetivo: ");
    var valueReaded = Console.ReadLine();
    var value = decimal.Parse(valueReaded);


    var objective = new FinanceObjective(title, value);


    objectives.Add(objective);
    Console.WriteLine($"Objetivo ID: {objective.Id} foi criado com sucesso.");
}

void DoOperation(OperationType type)
{
    Console.Write("Digite o ID do Objetivo: ");
    var idReaded = Console.ReadLine();
    var id = int.Parse(idReaded);

    Console.Write("Digite o valor da operação: ");
    var readedValue = Console.ReadLine();
    var value = decimal.Parse(readedValue);

    var operation = new Operation(value, type, id);

    var objective = objectives.SingleOrDefault(o => o.Id == id);
    objective.Operations.Add(operation);

}

void GetDetails()
{
    Console.Write("Digite o ID do Objetivo: ");
    var idReaded = Console.ReadLine();
    var id = int.Parse(idReaded);

    var objective = objectives.SingleOrDefault(o => o.Id == id);

    objective.PrintSummary();
}

#endregion
#region Models

public class FinanceObjective
{
    public FinanceObjective(string? title, decimal? valueObjective)
    {
        Id = new Random().Next(0, 1000);
        Title = title;
        ValueObjective = valueObjective;

        Operations = new List<Operation>();
    }

    public int Id { get; private set; }
    public string? Title { get; private set; }
    public decimal? ValueObjective { get; private set; }
    public List<Operation> Operations { get; private set; }

    public decimal Balance => GetBalance();

    decimal GetBalance()
    {
        var totalDeposit = Operations
            .Where(o => o.Type == OperationType.Deposit)
            .Sum(o => o.Value);

        var totalWithdraw = Operations
            .Where(o => o.Type == OperationType.Withdraw)
            .Sum(o => o.Value);

        return totalDeposit - totalWithdraw;
    }
    public virtual void PrintSummary()
    {
        Console.WriteLine($"Objetivo: {Title}, Valor: R${ValueObjective}, com Saldo atual: R${Balance} ");
    }

}

public class FinanceObjectiveWithDeadline : FinanceObjective
{
    public FinanceObjectiveWithDeadline(DateTime deadline, string? title, decimal? valueObjective) : base(title, valueObjective)
    {
        Deadline = deadline;
    }

    public DateTime Deadline { get; private set; }

    public override void PrintSummary()
    {
        base.PrintSummary();

        var daysRemaining = (Deadline - DateTime.Now).TotalDays;
        var valueRemaining = ValueObjective - Balance;

        Console.WriteLine($"Faltam {daysRemaining} para o prazo de seu objetivo, e faltam R${valueRemaining} para completar.")
    }
}


public class Operation
{
    public Operation(decimal value, OperationType type, int idObjective)
    {
        Id = new Random().Next(0, 1000);
        Value = value;
        Type = type;
        IdObjective = idObjective;
    }

    public int Id { get; private set; }
    public decimal Value { get; private set; }
    public OperationType Type { get; private set; }
    public int IdObjective { get; private set; }
}
public enum OperationType
{
    Withdraw = 0,
    Deposit = 1
}

#endregion
