namespace StockDataViewer;

partial class Form1
{
    private Button ButtonLoadStock = null!;
    private Label LabelStatus = null!;
    private DateTimePicker DateTimePickerStartDate = null!;
    private DateTimePicker DateTimePickerEndDate = null!;
    private OpenFileDialog OpenFileDialogStock = null!;

    /// <summary>Releases the file dialog along with the starting window.</summary>
    protected override void Dispose(bool disposing)
    {
        // The dialog is not a child control and needs explicit disposal.
        if (disposing) OpenFileDialogStock?.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>Creates the same simple controls and initial dates as Project 1.</summary>
    private void InitializeComponent()
    {
        SuspendLayout();
        ButtonLoadStock = new Button { Name = "ButtonLoadStock", Text = "Load Stock", Location = new Point(12, 12), Size = new Size(110, 30), TabIndex = 0 };
        ButtonLoadStock.Click += ButtonLoadStock_Click;
        LabelStatus = new Label { Name = "LabelStatus", Text = "Select one or more stock CSV files.", AutoSize = true, Location = new Point(135, 20) };
        DateTimePickerStartDate = new DateTimePicker { Name = "DateTimePickerStartDate", Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 6, 1), Location = new Point(85, 62), Width = 120, TabIndex = 1 };
        DateTimePickerEndDate = new DateTimePicker { Name = "DateTimePickerEndDate", Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 18), Location = new Point(300, 62), Width = 120, TabIndex = 2 };
        OpenFileDialogStock = new OpenFileDialog();
        // Keep the start window focused on selection; charts appear in separate forms.
        Controls.AddRange(new Control[] { ButtonLoadStock, LabelStatus,
            new Label { Name = "LabelStartDate", Text = "Start date:", AutoSize = true, Location = new Point(12, 66) }, DateTimePickerStartDate,
            new Label { Name = "LabelEndDate", Text = "End date:", AutoSize = true, Location = new Point(230, 66) }, DateTimePickerEndDate });
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 110);
        MinimumSize = new Size(620, 155);
        Name = "Form1";
        Text = "Stock Data Viewer";
        StartPosition = FormStartPosition.CenterScreen;
        ResumeLayout(false);
        PerformLayout();
    }
}
