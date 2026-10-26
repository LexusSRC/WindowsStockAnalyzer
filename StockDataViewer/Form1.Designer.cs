#nullable disable
namespace StockDataViewer;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        ButtonLoadStock = new Button();
        LabelStatus = new Label();
        LabelStartDate = new Label();
        DateTimePickerStartDate = new DateTimePicker();
        LabelEndDate = new Label();
        DateTimePickerEndDate = new DateTimePicker();
        ButtonUpdate = new Button();
        DataGridViewStockData = new DataGridView();
        OpenFileDialogStock = new OpenFileDialog();
        ((System.ComponentModel.ISupportInitialize)DataGridViewStockData).BeginInit();
        SuspendLayout();
        // 
        // ButtonLoadStock (TabIndex 0 so it has focus when the app starts)
        // Microsoft Copilot was used to generate and autofill some of the code in this file.
        //
        ButtonLoadStock.Location = new Point(12, 12);
        ButtonLoadStock.Name = "ButtonLoadStock";
        ButtonLoadStock.Size = new Size(110, 30);
        ButtonLoadStock.TabIndex = 0;
        ButtonLoadStock.Text = "Load Stock";
        ButtonLoadStock.UseVisualStyleBackColor = true;
        ButtonLoadStock.Click += ButtonLoadStock_Click;
        // 
        // LabelStatus
        // 
        LabelStatus.AutoSize = true;
        LabelStatus.Location = new Point(135, 19);
        LabelStatus.Name = "LabelStatus";
        LabelStatus.Size = new Size(97, 15);
        LabelStatus.TabIndex = 5;
        LabelStatus.Text = "No file loaded yet";
        // 
        // LabelStartDate
        // 
        LabelStartDate.AutoSize = true;
        LabelStartDate.Location = new Point(12, 62);
        LabelStartDate.Name = "LabelStartDate";
        LabelStartDate.Size = new Size(62, 15);
        LabelStartDate.TabIndex = 6;
        LabelStartDate.Text = "Start date:";
        // 
        // DateTimePickerStartDate (preset to June 1, 2026)
        // 
        DateTimePickerStartDate.Format = DateTimePickerFormat.Short;
        DateTimePickerStartDate.Location = new Point(85, 58);
        DateTimePickerStartDate.Name = "DateTimePickerStartDate";
        DateTimePickerStartDate.Size = new Size(110, 23);
        DateTimePickerStartDate.TabIndex = 1;
        DateTimePickerStartDate.Value = new DateTime(2026, 6, 1, 0, 0, 0, 0);
        // 
        // LabelEndDate
        // 
        LabelEndDate.AutoSize = true;
        LabelEndDate.Location = new Point(220, 62);
        LabelEndDate.Name = "LabelEndDate";
        LabelEndDate.Size = new Size(56, 15);
        LabelEndDate.TabIndex = 7;
        LabelEndDate.Text = "End date:";
        // 
        // DateTimePickerEndDate (preset to September 18, 2026)
        // 
        DateTimePickerEndDate.Format = DateTimePickerFormat.Short;
        DateTimePickerEndDate.Location = new Point(285, 58);
        DateTimePickerEndDate.Name = "DateTimePickerEndDate";
        DateTimePickerEndDate.Size = new Size(110, 23);
        DateTimePickerEndDate.TabIndex = 2;
        DateTimePickerEndDate.Value = new DateTime(2026, 9, 18, 0, 0, 0, 0);
        // 
        // ButtonUpdate
        // 
        ButtonUpdate.Location = new Point(415, 54);
        ButtonUpdate.Name = "ButtonUpdate";
        ButtonUpdate.Size = new Size(90, 30);
        ButtonUpdate.TabIndex = 3;
        ButtonUpdate.Text = "Update";
        ButtonUpdate.UseVisualStyleBackColor = true;
        ButtonUpdate.Click += ButtonUpdate_Click;
        // 
        // DataGridViewStockData
        // 
        DataGridViewStockData.AllowUserToAddRows = false;
        DataGridViewStockData.AllowUserToDeleteRows = false;
        DataGridViewStockData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DataGridViewStockData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DataGridViewStockData.Location = new Point(12, 96);
        DataGridViewStockData.Name = "DataGridViewStockData";
        DataGridViewStockData.ReadOnly = true;
        DataGridViewStockData.Size = new Size(860, 453);
        DataGridViewStockData.TabIndex = 4;
        // 
        // OpenFileDialogStock (settings are applied in Form1.cs)
        // 
        OpenFileDialogStock.FileName = "openFileDialog1";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 561);
        Controls.Add(DataGridViewStockData);
        Controls.Add(ButtonUpdate);
        Controls.Add(DateTimePickerEndDate);
        Controls.Add(LabelEndDate);
        Controls.Add(DateTimePickerStartDate);
        Controls.Add(LabelStartDate);
        Controls.Add(LabelStatus);
        Controls.Add(ButtonLoadStock);
        Name = "Form1";
        Text = "Stock Data Viewer";
        ((System.ComponentModel.ISupportInitialize)DataGridViewStockData).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button ButtonLoadStock;
    private Label LabelStatus;
    private Label LabelStartDate;
    private DateTimePicker DateTimePickerStartDate;
    private Label LabelEndDate;
    private DateTimePicker DateTimePickerEndDate;
    private Button ButtonUpdate;
    private DataGridView DataGridViewStockData;
    private OpenFileDialog OpenFileDialogStock;
}
