// AI disclosure: I have used Claude (Anthropic) and OpenAI Codex in creating this project.
// Peak and valley logic is in aSmartCandlestick; file reading and filtering are in CandlestickUtilities.

namespace StockDataViewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}