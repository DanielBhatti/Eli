using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.Td;

public class TdTransactionProcessor : TransactionProcessor<TdTransaction>
{
    public List<TdTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<TdTransaction>();
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
            var amount = ParseAmount(csv.GetField<string>("Amount") ?? "");
            var record = new TdTransaction(
                csv.GetField<DateOnly>("Date"),
                csv.GetField<DateOnly>("Posted Date"),
                NormalizeReferenceNumber(csv.GetField<string>("Reference Number") ?? ""),
                csv.GetField<string>("Activity Type") ?? "",
                csv.GetField<string>("Status") ?? "",
                csv.GetField<string>("Card Number") ?? "",
                csv.GetField<string>("Merchant Category") ?? "",
                csv.GetField<string>("Merchant Name") ?? "",
                csv.GetField<string>("Merchant City") ?? "",
                csv.GetField<string>("Merchant State/Province") ?? "",
                csv.GetField<string>("Merchant Postal Code") ?? "",
                amount,
                GetIntOrDefault(csv.GetField<string>("Rewards")),
                amount < 0m ? TdTransactionType.Payment : TdTransactionType.Purchase);

            results.Add(record);
        }

        return results;
    }

    private static string NormalizeReferenceNumber(string value) => value.Trim('"');

    private static decimal ParseAmount(string value) => decimal.Parse(value, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));

    private static int GetIntOrDefault(string? value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        return int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);
    }
}
