namespace StockDataViewer;

/// <summary>A candle with its zero-based position and peak/valley flags.</summary>
public class aSmartCandlestick : aCandlestick
{
    public int index;
    public bool isPeak;
    public bool isValley;

    /// <summary>Copies the original candle and records its position in the displayed list.</summary>
    public aSmartCandlestick(aCandlestick candle, int position)
    {
        // Keep the original prices; analysis adds information without changing them.
        Date = candle.Date;
        Open = candle.Open;
        High = candle.High;
        Low = candle.Low;
        Close = candle.Close;
        Volume = candle.Volume;
        index = position;
    }

    /// <summary>Sets isPeak: true when this candle's High is higher than both neighbors' Highs.</summary>
    /// <param name="candles">Every candle currently displayed, including this one.</param>
    public void CheckPeak(List<aSmartCandlestick> candles)
    {
        // The first and last candles have only one neighbor, so they cannot be peaks.
        if (index <= 0 || index >= candles.Count - 1)
        {
            isPeak = false;
            return;
        }

        // index is this candle's position in the list, so index - 1 and index + 1 are its neighbors.
        // Strictly higher than both: an equal High does not count as a peak.
        isPeak = High > candles[index - 1].High && High > candles[index + 1].High;
    }

    /// <summary>Sets isValley: true when this candle's Low is lower than both neighbors' Lows.</summary>
    /// <param name="candles">Every candle currently displayed, including this one.</param>
    public void CheckValley(List<aSmartCandlestick> candles)
    {
        // The first and last candles have only one neighbor, so they cannot be valleys.
        if (index <= 0 || index >= candles.Count - 1)
        {
            isValley = false;
            return;
        }

        // Strictly lower than both neighbors' Lows: an equal Low does not count as a valley.
        isValley = Low < candles[index - 1].Low && Low < candles[index + 1].Low;
    }
}
