# Stock Data Viewer - Project 2

This extends LexusSRC/WindowsStockAnalyzer Project 1 using its namespace, .NET 10 target, simple Windows Forms controls, naming and comment conventions. AI disclosure: Claude (Anthropic) and OpenAI Codex were used in creating this project.

## Run

Use a Visual Studio version supporting .NET 10 with the desktop development workload, or the .NET 10 SDK. Open StockDataViewer.sln (StockDataViewer.slnx also works in compatible Visual Studio versions), restore NuGet packages and run. The only package is WinForms.DataVisualization 1.10.2, a continuation of the Windows Forms Chart control. CSV reading uses .NET's TextFieldParser, not a third-party CSV library.

Place the existing Stock Data folder beside the solution folder, for example:

    Parent/
      Stock Data/NVDA_daily.csv
      WindowsStockAnalyzer/StockDataViewer.sln
      WindowsStockAnalyzer/StockDataViewer/StockDataViewer.csproj

Press Enter twice to load NVDA_daily with June 1, 2026 through September 18, 2026 preset. Ctrl-select or Shift-select CSVs in the dialog to open more than one chart. Every chart has its own start/end dates and Refresh button. A Refresh changes only that window and uses its cached full history.

## Peak and valley rule

A candle is a peak when its High is strictly higher than the Highs of the candles immediately before and after it, and a valley when its Low is strictly lower than both neighbors' Lows. The first and last candles of the displayed range have only one neighbor, so they can never be a peak or valley. Equal values do not count, and one candle can be both a peak and a valley. The rule is defined in aSmartCandlestick (CheckPeak and CheckValley, which use the candle's index to find its neighbors); CandlestickUtilities.FindPeaks and FindValleys ask every candle and return the flagged ones. Analysis runs on the filtered list, so a Refresh with new dates recomputes the flags for that range.

## Implementation

- aCandlestick stores OHLC, date and volume; aSmartCandlestick derives from it and adds int index and bool isPeak/isValley.
- CandlestickUtilities handles file reading, inclusive date filtering, smart conversion and peak/valley checks. The specification spells this name two ways; this uses CandlestickUtilities from the numbered requirements.
- Form1 selects files and initial dates. FormStockChart accepts the filename and dates in its constructor, then displays candlesticks and volume with actual P/V CalloutAnnotation objects.
- CSV headers are mapped by name, including when an extra Adj Close column is present. Quoted comma-containing values are supported. Invalid rows and impossible OHLC ordering are skipped; missing required headers produce a useful message.
- CSV values used in application checks are test inputs; no claim is made that local sample prices are authentic market observations.

## Submission

The supplied source ZIP contains the complete cleaned solution folder. Stock Data, bin, obj, Git metadata, verification harness and binaries are excluded. Submit this solution ZIP; keep Stock Data in its parent folder when running locally.
