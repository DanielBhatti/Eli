using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.Aaa;

public class AaaTransactionProcessor : TransactionProcessor
{
    public List<MinDetailTransaction> ParseMinDetailTransactionsFromFile(string filePath)
    {
        var results = new List<MinDetailTransaction>();
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
            var record = new MinDetailTransaction(csv.GetField<DateOnly>("TRANS DATE"), csv.GetField<string>("TRANSACTION DESCRIPTION/LOCATION") ?? "", csv.GetField<decimal>("AMOUNT"));
            results.Add(record);
        }

        return results;
    }
}
