namespace StockDataViewer;

/// One date's prices and trading volume read from a stock CSV.
public class aCandlestick
{
    public DateTime Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
}
