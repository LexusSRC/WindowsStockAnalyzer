namespace StockDataViewer;

/// <summary>
/// Main window: loads a stock CSV, filters it by date range, and shows it in a grid.
/// Microsoft Copilot was used to generate and autofill some of the code in this file.
public partial class Form1 : Form
{
    // Every row from the most recently loaded file (never filtered, so Update can re-filter it)
    private List<Candlestick> allCandlesticks = new List<Candlestick>();

    // Name of the loaded file, shown in the status label
    private string loadedFileName = "";

    /// <summary>
    /// Builds the form's controls and applies the OpenFileDialog presets.
    /// </summary>
    public Form1()
    {
        InitializeComponent();
        ConfigureOpenFileDialog();
    }

    /// <summary>
    /// Sets the dialog's default extension, starting folder, preset file name, and period filters.
    /// </summary>
    private void ConfigureOpenFileDialog()
    {
        OpenFileDialogStock.Title = "Select a stock CSV file";
        OpenFileDialogStock.DefaultExt = "csv";
        OpenFileDialogStock.FileName = "NVDA_daily";
        OpenFileDialogStock.CheckFileExists = true;

        // Each filter is "description|pattern"; the user picks the period from the file-type box
        OpenFileDialogStock.Filter =
            "All stock files (*.csv)|*.csv|" +
            "Daily (*_daily.csv)|*_daily.csv|" +
            "Weekly (*_weekly.csv)|*_weekly.csv|" +
            "Monthly (*_monthly.csv)|*_monthly.csv|" +
            "Yearly (*_yearly.csv)|*_yearly.csv";
        OpenFileDialogStock.FilterIndex = 1; // filter numbers start at 1, so this is "All"

        OpenFileDialogStock.InitialDirectory = FindStockDataFolder();
    }

    /// <summary>
    /// Walks up from the running program's folder until it finds a folder named "Stock Data".
    /// </summary>
    /// <returns>The full path of the Stock Data folder, or an empty string if not found.</returns>
    private string FindStockDataFolder()
    {
        // Start in the folder the .exe runs from (bin\Debug\net10.0-windows)
        DirectoryInfo? current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current != null)
        {
            string candidate = Path.Combine(current.FullName, "Stock Data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            // Move one folder up and look again
            current = current.Parent;
        }

        return string.Empty;
    }

    /// <summary>
    /// Lets the user pick a CSV file, loads it, and displays the rows in the chosen date range.
    /// </summary>
    private void ButtonLoadStock_Click(object sender, EventArgs e)
    {
        // Do nothing if the user cancels the dialog
        if (OpenFileDialogStock.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            StockDataLoader loader = new StockDataLoader();
            allCandlesticks = loader.LoadCandlesticks(OpenFileDialogStock.FileName);
            loadedFileName = Path.GetFileName(OpenFileDialogStock.FileName);
            DisplayCandlesticks();
        }
        catch (IOException ex)
        {
            // Happens if the file is open in another program or can't be read
            MessageBox.Show("Could not read the file: " + ex.Message);
        }
    }

    /// <summary>
    /// Re-applies the current start and end dates to the loaded data.
    /// </summary>
    private void ButtonUpdate_Click(object sender, EventArgs e)
    {
        // Nothing to filter until a file has been loaded
        if (allCandlesticks.Count == 0)
        {
            MessageBox.Show("Load a stock file first.");
            return;
        }

        DisplayCandlesticks();
    }

    /// <summary>
    /// Filters the loaded rows by the two date pickers and shows them in the grid.
    /// </summary>
    private void DisplayCandlesticks()
    {
        // .Date drops the time-of-day part so only calendar dates are compared
        DateTime startDate = DateTimePickerStartDate.Value.Date;
        DateTime endDate = DateTimePickerEndDate.Value.Date;

        if (startDate > endDate)
        {
            MessageBox.Show("The start date must be on or before the end date.");
            return;
        }

        // Keep only the rows whose date falls inside the range (inclusive)
        List<Candlestick> filtered = allCandlesticks
            .Where(c => c.Date.Date >= startDate && c.Date.Date <= endDate)
            .ToList();

        DataGridViewStockData.DataSource = filtered;
        FormatGridColumns();

        LabelStatus.Text = loadedFileName + " - showing " + filtered.Count +
            " of " + allCandlesticks.Count + " rows";
    }

    /// <summary>
    /// Makes the grid easier to read: short dates, two-decimal prices, comma-separated volume.
    /// </summary>
    private void FormatGridColumns()
    {
        foreach (DataGridViewColumn column in DataGridViewStockData.Columns)
        {
            if (column.Name == "Date")
            {
                column.DefaultCellStyle.Format = "yyyy-MM-dd";
            }
            else if (column.Name == "Volume")
            {
                column.DefaultCellStyle.Format = "N0";
            }
            else
            {
                column.DefaultCellStyle.Format = "F2";
            }
        }
    }
}
