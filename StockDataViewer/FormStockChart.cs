using System.Windows.Forms.DataVisualization.Charting;

namespace StockDataViewer;

/// <summary>One stock's price, volume and P/V annotations with independent date controls.</summary>
public class FormStockChart : Form
{
    private readonly List<aCandlestick> allCandlesticks;
    private readonly Chart ChartStock = new Chart { Name = "ChartStock", Dock = DockStyle.Fill };
    private readonly DateTimePicker DateTimePickerStartDate = new DateTimePicker { Name = "DateTimePickerStartDate", Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly DateTimePicker DateTimePickerEndDate = new DateTimePicker { Name = "DateTimePickerEndDate", Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly Button ButtonUpdate = new Button { Name = "ButtonUpdate", Text = "Refresh", AutoSize = true };
    private readonly Label LabelStatus = new Label { Name = "LabelStatus", Dock = DockStyle.Bottom, Height = 26, TextAlign = ContentAlignment.MiddleLeft };

    /// <summary>Reads the supplied CSV and displays the starting form's chosen date range.</summary>
    public FormStockChart(string filePath, DateTime startDate, DateTime endDate)
    {
        // Read before showing the form so the caller can report a file error.
        allCandlesticks = CandlestickUtilities.ReadCandlesticks(filePath);
        Text = Path.GetFileNameWithoutExtension(filePath) + " - Stock Chart";
        Name = "FormStockChart";
        ClientSize = new Size(1040, 720);
        MinimumSize = new Size(740, 500);
        DateTimePickerStartDate.Value = startDate;
        DateTimePickerEndDate.Value = endDate;
        FlowLayoutPanel controls = new FlowLayoutPanel { Name = "PanelDateRange", Dock = DockStyle.Top, Height = 48, Padding = new Padding(8), WrapContents = false };
        controls.Controls.AddRange(new Control[] {
            new Label { Name = "LabelStartDate", Text = "Start date:", AutoSize = true, Padding = new Padding(0, 5, 0, 0) }, DateTimePickerStartDate,
            new Label { Name = "LabelEndDate", Text = "End date:", AutoSize = true, Padding = new Padding(10, 5, 0, 0) }, DateTimePickerEndDate, ButtonUpdate });
        Controls.Add(ChartStock);
        Controls.Add(LabelStatus);
        Controls.Add(controls);
        ButtonUpdate.Click += ButtonUpdate_Click;
        AcceptButton = ButtonUpdate;
        ConfigureChart();
        DisplayCandlesticks();
    }

    /// <summary>Creates aligned chart areas for candlesticks and trading volume.</summary>
    private void ConfigureChart()
    {
        ChartArea prices = new ChartArea("Prices");
        prices.Position = new ElementPosition(0, 0, 100, 70);
        prices.AxisY.IsStartedFromZero = false;
        prices.AxisY.Title = "Price";
        prices.AxisY.LabelStyle.Format = "0.00";
        prices.AxisX.LabelStyle.Format = "MMM dd";
        prices.AxisX.MajorGrid.Enabled = false;
        prices.AxisY.MajorGrid.LineColor = Color.Gainsboro;
        ChartArea volume = new ChartArea("Volume");
        volume.Position = new ElementPosition(0, 70, 100, 30);
        volume.AlignWithChartArea = "Prices";
        volume.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
        volume.AlignmentStyle = AreaAlignmentStyles.All;
        volume.AxisX.LabelStyle.Format = "MMM dd";
        volume.AxisX.MajorGrid.Enabled = false;
        volume.AxisY.LabelStyle.Format = "N0";
        volume.AxisY.Title = "Volume";
        volume.AxisY.MajorGrid.LineColor = Color.Gainsboro;
        ChartStock.ChartAreas.Add(prices);
        ChartStock.ChartAreas.Add(volume);
        // The chart requires High, Low, Open, Close as the four Y values, in that order.
        Series candles = new Series("Candlesticks") { ChartArea = "Prices", ChartType = SeriesChartType.Candlestick, YValuesPerPoint = 4, XValueType = ChartValueType.Date, IsXValueIndexed = true, Color = Color.Black };
        candles["PriceUpColor"] = "ForestGreen";
        candles["PriceDownColor"] = "Firebrick";
        candles["OpenCloseStyle"] = "Triangle";
        candles["ShowOpenClose"] = "Both";
        candles["PointWidth"] = "0.7";
        candles.ToolTip = "#VALX{yyyy-MM-dd}\nHigh: #VALY1\nLow: #VALY2\nOpen: #VALY3\nClose: #VALY4";
        ChartStock.Series.Add(candles);
        ChartStock.Series.Add(new Series("Volume") { ChartArea = "Volume", ChartType = SeriesChartType.Column, XValueType = ChartValueType.Date, IsXValueIndexed = true, Color = Color.SteelBlue });
    }

    /// <summary>Validates the selected range before redrawing this window.</summary>
    private void ButtonUpdate_Click(object? sender, EventArgs e)
    {
        // Keep the previous display when the user reverses the two dates.
        if (DateTimePickerStartDate.Value.Date > DateTimePickerEndDate.Value.Date)
        {
            MessageBox.Show(this, "The start date must be on or before the end date.");
            return;
        }
        DisplayCandlesticks();
    }

    /// <summary>Filters the cached history and redraws candles, volume and turning points.</summary>
    private void DisplayCandlesticks()
    {
        List<aCandlestick> filtered = CandlestickUtilities.FilterCandlesticks(allCandlesticks, DateTimePickerStartDate.Value, DateTimePickerEndDate.Value);
        List<aSmartCandlestick> smart = CandlestickUtilities.ConvertToSmartCandlesticks(filtered);
        int peaks = CandlestickUtilities.FindPeaks(smart).Count;
        int valleys = CandlestickUtilities.FindValleys(smart).Count;
        ChartStock.Annotations.Clear();
        foreach (Series series in ChartStock.Series) series.Points.Clear();
        ChartArea prices = ChartStock.ChartAreas["Prices"];
        prices.AxisY.Minimum = double.NaN;
        prices.AxisY.Maximum = double.NaN;
        foreach (aSmartCandlestick candle in smart)
        {
            int position = ChartStock.Series[0].Points.AddXY(candle.Date, candle.High, candle.Low, candle.Open, candle.Close);
            ChartStock.Series[1].Points.AddXY(candle.Date, candle.Volume);
            if (candle.isPeak) AddTurningPoint(candle, position, true);
            if (candle.isValley) AddTurningPoint(candle, position, false);
        }
        // Leave space around the wicks for labels, even for a single flat candle.
        if (smart.Count > 0)
        {
            double low = (double)smart.Min(candle => candle.Low);
            double high = (double)smart.Max(candle => candle.High);
            double padding = Math.Max((high - low) * 0.14, Math.Max(Math.Abs(high) * 0.01, 0.01));
            prices.AxisY.Minimum = low - padding;
            prices.AxisY.Maximum = high + padding;
        }
        LabelStatus.Text = smart.Count == 0 ? "No candlesticks in this date range." : smart.Count + " candlesticks, " + peaks + " peaks (P), " + valleys + " valleys (V)";
        ChartStock.Invalidate();
    }

    /// <summary>Points a P or V callout at the candle's actual high or low wick.</summary>
    private void AddTurningPoint(aSmartCandlestick candle, int position, bool peak)
    {
        // Indexed chart X coordinates start at one; candle indexes start at zero.
        ChartArea area = ChartStock.ChartAreas["Prices"];
        CalloutAnnotation annotation = new CalloutAnnotation {
            Name = (peak ? "Peak" : "Valley") + candle.index,
            Text = peak ? "P" : "V", AxisX = area.AxisX, AxisY = area.AxisY,
            AnchorX = position + 1, AnchorY = (double)(peak ? candle.High : candle.Low),
            AnchorAlignment = peak ? ContentAlignment.BottomCenter : ContentAlignment.TopCenter,
            AnchorOffsetY = peak ? -2 : 2, CalloutStyle = CalloutStyle.SimpleLine,
            ForeColor = peak ? Color.DarkGreen : Color.DarkRed,
            LineColor = peak ? Color.DarkGreen : Color.DarkRed,
            BackColor = Color.Transparent, ClipToChartArea = "Prices",
            ToolTip = candle.Date.ToString("yyyy-MM-dd") + (peak ? " peak" : " valley")
        };
        ChartStock.Annotations.Add(annotation);
    }
}
