namespace StockDataViewer;

/// <summary>Chooses one or more stock files and their initial chart date range.</summary>
public partial class Form1 : Form
{
    /// <summary>Creates the starting form with the Project 1 file-dialog presets.</summary>
    public Form1()
    {
        InitializeComponent();
        // Enter opens the file dialog; the next Enter accepts NVDA_daily.
        AcceptButton = ButtonLoadStock;
        ActiveControl = ButtonLoadStock;
        OpenFileDialogStock.Title = "Select one or more stock CSV files";
        OpenFileDialogStock.DefaultExt = "csv";
        OpenFileDialogStock.FileName = "NVDA_daily";
        OpenFileDialogStock.Multiselect = true;
        OpenFileDialogStock.CheckFileExists = true;
        OpenFileDialogStock.Filter = "All stock files (*.csv)|*.csv|Daily (*_daily.csv)|*_daily.csv|Weekly (*_weekly.csv)|*_weekly.csv|Monthly (*_monthly.csv)|*_monthly.csv|Yearly (*_yearly.csv)|*_yearly.csv";
        OpenFileDialogStock.InitialDirectory = FindStockDataFolder();
    }

    /// <summary>Walks up from the executable to find the assignment's Stock Data folder.</summary>
    private string FindStockDataFolder()
    {
        // The same search works when launched from Visual Studio or a moved solution.
        DirectoryInfo? current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            string candidate = Path.Combine(current.FullName, "Stock Data");
            if (Directory.Exists(candidate)) return candidate;
            current = current.Parent;
        }
        return string.Empty;
    }

    /// <summary>Opens an independent chart for each selected CSV.</summary>
    private void ButtonLoadStock_Click(object? sender, EventArgs e)
    {
        DateTime start = DateTimePickerStartDate.Value.Date;
        DateTime end = DateTimePickerEndDate.Value.Date;
        if (start > end)
        {
            MessageBox.Show(this, "The start date must be on or before the end date.");
            return;
        }
        if (OpenFileDialogStock.ShowDialog(this) != DialogResult.OK) return;
        int opened = 0;
        foreach (string filePath in OpenFileDialogStock.FileNames)
        {
            // A bad file does not prevent the remaining selections from opening.
            try
            {
                FormStockChart chart = new FormStockChart(filePath, start, end);
                chart.Show(this);
                opened++;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is FormatException)
            {
                MessageBox.Show(this, Path.GetFileName(filePath) + ": " + ex.Message, "Could not load stock");
            }
        }
        LabelStatus.Text = "Opened " + opened + " stock window(s).";
    }
}
