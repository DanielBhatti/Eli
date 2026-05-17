using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.BankOfAmerica;

public class BoaTransactionProcessor : TransactionProcessor<BoaTransaction>
{
    public List<BoaTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<BoaTransaction>();
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
            var record = new BoaTransaction(csv.GetField<DateOnly>("Date"), csv.GetField<string>("Description") ?? "", csv.GetField<string>("Location") ?? "", csv.GetField<decimal>("Amount"));
            results.Add(record);
        }

        return results;
    }
}
