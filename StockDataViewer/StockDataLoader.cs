using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace StockDataViewer
{
    /// Reads a CSV file and turns each row into a Candlestick.
    /// Microsoft Copilot was used to generate and autofill some of the code in this file.
    public class StockDataLoader
    {
        /// Loads every Row of the CSV file into a Candlestick object, skipping any rows that are invalid.
        /// <param name="filePath">Full path to the CSV file.</param>
        /// <returns>A list of Candlestick objects, one per valid row.</returns>
        public List<Candlestick> LoadCandlesticks(string filePath)
        {
            List<Candlestick> candlesticks = new List<Candlestick>();
            string[] lines = File.ReadAllLines(filePath);

            // Start at 1 because line 0 is the header row (Date,Open,High,...)
            for (int i = 1; i < lines.Length; i++)
            {
                // Skip blank lines
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                string[] parts = lines[i].Split(',');

                // A valid row needs all six fields
                if (parts.Length < 6)
                {
                    continue;
                }

                // TryParse returns true/false instead of crashing on bad data
                bool dateOk = DateTime.TryParse(parts[0].Trim('"'), CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime date);
                bool openOk = decimal.TryParse(parts[1].Trim('"'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal open);
                bool highOk = decimal.TryParse(parts[2].Trim('"'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal high);
                bool lowOk = decimal.TryParse(parts[3].Trim('"'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal low);
                bool closeOk = decimal.TryParse(parts[4].Trim('"'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal close);
                bool volumeOk = decimal.TryParse(parts[5].Trim('"'), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal volume);

                // Only keep the row if every field parsed
                if (dateOk && openOk && highOk && lowOk && closeOk && volumeOk)
                {
                    Candlestick c = new Candlestick();
                    c.Date = date;
                    c.Open = open;
                    c.High = high;
                    c.Low = low;
                    c.Close = close;
                    c.Volume = volume;
                    candlesticks.Add(c);
                }
            }

            return candlesticks;
        }
    }
}


