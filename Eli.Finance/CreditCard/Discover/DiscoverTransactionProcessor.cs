using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.Discover;

public class DiscoverTransactionProcessor : TransactionProcessor<DiscoverTransaction>
{
    public List<DiscoverTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<DiscoverTransaction>();
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

        while (csv.Read())
        {
            var record = new DiscoverTransaction(
                csv.GetField<DateOnly>("Trans. Date"),
                csv.GetField<string>("Description") ?? "",
                csv.GetField<decimal>("Amount"),
                csv.GetField<string>("Category") ?? "");

            results.Add(record);
        }

        return results;
    }
}
