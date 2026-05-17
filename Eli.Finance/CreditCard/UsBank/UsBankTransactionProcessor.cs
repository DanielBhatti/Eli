using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.UsBank;

public class UsBankTransactionProcessor : TransactionProcessor<UsBankTransaction>
{
    public List<UsBankTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<UsBankTransaction>();
        var statementYear = GetStatementYear(filePath);
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
            var record = new UsBankTransaction(
                ParseStatementDate(csv.GetField<string>("Post Date") ?? "", statementYear),
                ParseStatementDate(csv.GetField<string>("Trans Date") ?? "", statementYear),
                csv.GetField<string>("Ref #") ?? "",
                csv.GetField<string>("Transaction Description") ?? "",
                amount,
                amount < 0m ? UsBankTransactionType.Payment : UsBankTransactionType.Purchase);

            results.Add(record);
        }

        return results;
    }

    private static int GetStatementYear(string filePath)
    {
        var fileName = Path.GetFileName(filePath);

        if(fileName.Length < 4 || !int.TryParse(fileName[..4], NumberStyles.None, CultureInfo.InvariantCulture, out var year))
        {
            throw new ArgumentException($"US Bank transaction file name must begin with a four-digit statement year, for example '2025_us_bank_transactions.csv'. Actual file name: '{fileName}'.", nameof(filePath));
        }

        return year;
    }

    private static DateOnly ParseStatementDate(string value, int statementYear)
    {
        var date = DateTime.ParseExact(value, "MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        return new DateOnly(statementYear, date.Month, date.Day);
    }

    private static decimal ParseAmount(string value)
    {
        var isCredit = value.EndsWith("CR", StringComparison.OrdinalIgnoreCase);
        var normalized = value.Replace("CR", "", StringComparison.OrdinalIgnoreCase);
        var amount = decimal.Parse(normalized, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));

        return isCredit ? -amount : amount;
    }
}
