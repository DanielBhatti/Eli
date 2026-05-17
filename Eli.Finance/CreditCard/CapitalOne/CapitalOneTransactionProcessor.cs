using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Eli.Finance.CreditCard.CapitalOne;

public class CapitalOneTransactionProcessor : TransactionProcessor<CapitalOneTransaction>
{
    public List<CapitalOneTransaction> ParseTransactionsFromFile(string filePath)
    {
        var results = new List<CapitalOneTransaction>();
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

        var hasNewDateColumns = HasHeader(csv, "Trans Date") && HasHeader(csv, "Post Date");
        var hasOldDateColumn = HasHeader(csv, "Date");

        if(!hasNewDateColumns && !hasOldDateColumn)
        {
            throw new InvalidDataException("Capital One transaction file must contain either 'Date' or both 'Trans Date' and 'Post Date' columns.");
        }

        while(csv.Read())
        {
            var amount = ParseAmount(csv.GetField<string>("Amount") ?? "");
            var transDate = hasNewDateColumns
                ? ParseStatementDate(csv.GetField<string>("Trans Date") ?? "", statementYear)
                : ParseStatementDate(csv.GetField<string>("Date") ?? "", statementYear);
            var postDate = hasNewDateColumns
                ? ParseStatementDate(csv.GetField<string>("Post Date") ?? "", statementYear)
                : (DateOnly?)null;

            var record = new CapitalOneTransaction(
                transDate,
                postDate,
                csv.GetField<string>("Description") ?? "",
                amount,
                amount < 0m ? CapitalOneTransactionType.Payment : CapitalOneTransactionType.Purchase);

            results.Add(record);
        }

        return results;
    }

    private static bool HasHeader(CsvReader csv, string header) => csv.HeaderRecord?.Contains(header, StringComparer.OrdinalIgnoreCase) == true;

    private static int GetStatementYear(string filePath)
    {
        var fileName = Path.GetFileName(filePath);

        if(fileName.Length < 4 || !int.TryParse(fileName[..4], NumberStyles.None, CultureInfo.InvariantCulture, out var year))
        {
            throw new ArgumentException($"Capital One transaction file name must begin with a four-digit statement year, for example '2021_capital_one_transactions.csv'. Actual file name: '{fileName}'.", nameof(filePath));
        }

        return year;
    }

    private static DateOnly ParseStatementDate(string value, int statementYear)
    {
        var date = DateTime.ParseExact(value, "MMM d", CultureInfo.InvariantCulture, DateTimeStyles.None);
        return new DateOnly(statementYear, date.Month, date.Day);
    }

    private static decimal ParseAmount(string value)
    {
        var normalized = value.Replace(" ", "", StringComparison.OrdinalIgnoreCase);
        return decimal.Parse(normalized, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
    }
}
