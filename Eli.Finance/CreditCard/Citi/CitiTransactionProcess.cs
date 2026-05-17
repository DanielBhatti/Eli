using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.Citi;

public class CitiTransactionProcessor : TransactionProcessor<CitiTransaction>
{
    public List<CitiTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<CitiTransaction>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        _ = csv.Read();
        _ = csv.ReadHeader();

        while(csv.Read())
        {
            var record = new CitiTransaction(
                DateOnly.Parse(csv.GetField<string>("Date") ?? "", CultureInfo.InvariantCulture),
                csv.GetField<string>("Description") ?? "",
                GetDecimalOrDefault(csv.GetField<string>("Debit")),
                GetDecimalOrDefault(csv.GetField<string>("Credit")),
                csv.GetField<string>("Category") ?? "");

            results.Add(record);
        }

        return results;
    }

    private static decimal GetDecimalOrDefault(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0m;
        }

        return decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
    }
}
