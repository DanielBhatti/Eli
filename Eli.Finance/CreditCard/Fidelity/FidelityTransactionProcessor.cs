using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.Fidelity;

public class FidelityTransactionProcessor : TransactionProcessor<FidelityTransaction>
{
    public List<FidelityTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<FidelityTransaction>();
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
            var record = new FidelityTransaction(
                Enum.Parse<FidelityTransactionType>(csv.GetField<string>("TransactionType") ?? "", ignoreCase: true),
                csv.GetField<string>("Category") ?? "",
                csv.GetField<string>("SubCategory") ?? "",
                csv.GetField<DateOnly>("TransactionDate"),
                csv.GetField<DateOnly>("PostedDate"),
                csv.GetField<string>("Merchant") ?? "",
                csv.GetField<decimal>("Amount"));

            results.Add(record);
        }

        return results;
    }
}
