using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace StockDataViewer;

/// <summary>Reads, filters and analyzes stock candles without depending on a form.</summary>
public static class CandlestickUtilities
{
    /// <summary>Reads named CSV columns and returns valid candles in date order.</summary>
    public static List<aCandlestick> ReadCandlesticks(string filePath)
    {
        List<aCandlestick> candles = new List<aCandlestick>();
        // TextFieldParser is part of .NET and handles quoted fields and embedded commas.
        using TextFieldParser parser = new TextFieldParser(filePath);
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        string[] headers = parser.ReadFields() ?? Array.Empty<string>();
        string[] required = { "Date", "Open", "High", "Low", "Close", "Volume" };
        int[] columns = required.Select(name => Array.FindIndex(headers,
            header => string.Equals(header.Trim().Trim('\uFEFF'), name,
                StringComparison.OrdinalIgnoreCase))).ToArray();
        if (columns.Any(column => column < 0))
            throw new FormatException("The CSV needs Date, Open, High, Low, Close and Volume columns.");

        while (!parser.EndOfData)
        {
            string[]? fields;
            try { fields = parser.ReadFields(); }
            catch (MalformedLineException) { continue; }
            if (fields == null || fields.Length <= columns.Max()) continue;
            if (!DateTime.TryParse(fields[columns[0]], CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime date)) continue;

            decimal[] values = new decimal[5];
            bool valid = true;
            for (int i = 0; i < values.Length; i++)
                valid &= decimal.TryParse(fields[columns[i + 1]], NumberStyles.Number,
                    CultureInfo.InvariantCulture, out values[i]);
            // Reject impossible OHLC ordering and negative volume instead of charting bad rows.
            if (!valid || values[1] < Math.Max(values[0], values[3]) ||
                values[2] > Math.Min(values[0], values[3]) || values[1] < values[2] || values[4] < 0)
                continue;
            candles.Add(new aCandlestick { Date = date.Date, Open = values[0],
                High = values[1], Low = values[2], Close = values[3], Volume = values[4] });
        }
        return candles.OrderBy(candle => candle.Date).ToList();
    }

    /// <summary>Returns candles inside both calendar-date boundaries, including the endpoints.</summary>
    public static List<aCandlestick> FilterCandlesticks(List<aCandlestick> candles,
        DateTime startDate, DateTime endDate)
    {
        // A reversed range is an input error, rather than an empty stock history.
        if (startDate.Date > endDate.Date) throw new ArgumentException("The start date must be on or before the end date.");
        return candles.Where(candle => candle.Date.Date >= startDate.Date &&
            candle.Date.Date <= endDate.Date).OrderBy(candle => candle.Date).ToList();
    }

    /// <summary>Copies the displayed candles and gives them consecutive zero-based indexes.</summary>
    public static List<aSmartCandlestick> ConvertToSmartCandlesticks(List<aCandlestick> candles)
    {
        // New objects keep flags from an old display out of a refreshed range.
        return candles.Select((candle, position) => new aSmartCandlestick(candle, position)).ToList();
    }

    /// <summary>Asks every candle whether it is a peak and returns the candles flagged as peaks.</summary>
    public static List<aSmartCandlestick> FindPeaks(List<aSmartCandlestick> candles)
    {
        // The rule for what counts as a peak is defined in aSmartCandlestick.CheckPeak.
        foreach (aSmartCandlestick candle in candles) candle.CheckPeak(candles);
        return candles.Where(candle => candle.isPeak).ToList();
    }

    /// <summary>Asks every candle whether it is a valley and returns the candles flagged as valleys.</summary>
    public static List<aSmartCandlestick> FindValleys(List<aSmartCandlestick> candles)
    {
        // The rule for what counts as a valley is defined in aSmartCandlestick.CheckValley.
        foreach (aSmartCandlestick candle in candles) candle.CheckValley(candles);
        return candles.Where(candle => candle.isValley).ToList();
    }
}
