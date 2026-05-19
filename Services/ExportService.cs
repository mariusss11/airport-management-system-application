using AirportFlightManagement.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using Stimulsoft.Report;
using Stimulsoft.Report.Viewer.Avalonia.Viewer;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer.Avalonia.Viewer;
using System.Data;
using Avalonia.Controls;
using Avalonia;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace AirportFlightManagement.Services;

public partial class ExportService
{
    public string ExportToExcel(List<Flight> flights, string reportName)
    {
        string fileName = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        string filePath = Path.Combine(Path.GetTempPath(), fileName);

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add(reportName);

            // Add headers
            worksheet.Cell(1, 1).Value = "Flight Code";
            worksheet.Cell(1, 4).Value = "Departure";
            worksheet.Cell(1, 2).Value = "Destination";
            worksheet.Cell(1, 5).Value = "Departure Date";
            worksheet.Cell(1, 5).Value = "Departure Time";
            worksheet.Cell(1, 5).Value = "Arrival Time";
            worksheet.Cell(1, 6).Value = "Total Seats";
            worksheet.Cell(1, 7).Value = "Available Seats";
            worksheet.Cell(1, 8).Value = "Price ($)";

            // Style header row
            var headerRange = worksheet.Range("A1:I1");
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(0xDC143C);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;

            // Add data
            int row = 2;
            foreach (var flight in flights)
            {
                worksheet.Cell(row, 1).Value = flight.FlightCode;
                worksheet.Cell(row, 2).Value = flight.DepartureAirport.Name;
                worksheet.Cell(row, 3).Value = flight.DestinationAirport.Name;
                worksheet.Cell(row, 4).Value = flight.DepartureDate.ToString("dddd");
                worksheet.Cell(row, 5).Value = flight.DepartureTime.ToString("HH:mm");
                worksheet.Cell(row, 6).Value = flight.ArrivalTime.ToString("HH:mm");
                worksheet.Cell(row, 7).Value = flight.TotalSeats;
                worksheet.Cell(row, 8).Value = flight.AvailableSeats;
                worksheet.Cell(row, 9).Value = flight.TicketPrice;

                // Alternate row colors
                if (row % 2 == 0)
                {
                    worksheet.Range($"A{row}:I{row}").Style.Fill.BackgroundColor = XLColor.Gray;
                }

                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
        }

        return filePath;
    }

    public string ExportToWord(List<Flight> flights, string reportName)
    {
        // For now, we'll export as CSV text file (.txt) which is simpler
        string fileName = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        string filePath = Path.Combine(Path.GetTempPath(), fileName);

        using (var writer = new System.IO.StreamWriter(filePath))
        {
            writer.WriteLine(reportName);
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            writer.WriteLine();
            writer.WriteLine(string.Join("\t", "Flight Code", "Destination", "Day", "Departure", "Arrival", "Seats", "Available", "Price", "Status"));

            foreach (var flight in flights)
            {
                writer.WriteLine(string.Join("\t",
                    flight.FlightCode,
                    flight.Destination,
                    flight.DepartureDate.ToString("dddd"),
                    flight.DepartureTime.ToString("HH:mm"),
                    flight.ArrivalTime.ToString("HH:mm"),
                    flight.TotalSeats,
                    flight.AvailableSeats,
                    flight.TicketPrice.ToString("F2"),
                    flight.Status));
            }
        }

        return filePath;
    }
    
    public string ExportToStiReport(List<Flight> flights, string reportName)
{
    // 1. Create DataTable from flights
    DataTable flightTable = new DataTable("Flights");
    flightTable.Columns.Add("FlightCode", typeof(string));
    flightTable.Columns.Add("Destination", typeof(string));
    flightTable.Columns.Add("DepartureDate", typeof(DateTime));
    flightTable.Columns.Add("DepartureTime", typeof(TimeSpan));
    flightTable.Columns.Add("ArrivalTime", typeof(TimeSpan));
    flightTable.Columns.Add("TicketPrice", typeof(decimal));
    flightTable.Columns.Add("TotalSeats", typeof(int));
    flightTable.Columns.Add("AvailableSeats", typeof(int));
    flightTable.Columns.Add("Status", typeof(string));

    foreach (var f in flights)
    {
        flightTable.Rows.Add(f.FlightCode, f.Destination, f.DepartureDate, f.DepartureTime,
            f.ArrivalTime, f.TicketPrice, f.TotalSeats, f.AvailableSeats, f.Status);
    }

    // 2. Create DataSet and register
    DataSet flightDataSet = new DataSet();
    flightDataSet.Tables.Add(flightTable);

    var report = new StiReport();
    report.RegData(flightDataSet);
    report.Dictionary.Synchronize();

    var page = report.Pages[0];

    // 3. Create a simple DataBand
    var dataBand = new StiDataBand
    {
        DataSourceName = "Flights",
        Height = 0.5,
        Name = "DataBand"
    };
    page.Components.Add(dataBand);

    var dataText = new StiText(new RectangleD(0, 0, 10, 0.5))
    {
        Text = "{Line}. {Flights.FlightCode} | {Flights.Origin} -> {Flights.Destination} | {Flights.DepartureDate} {Flights.DepartureTime} - {Flights.ArrivalTime} | ${Flights.TicketPrice}",
        Name = "DataText"
    };
    dataBand.Components.Add(dataText);

    // 4. Footer with total count
    var footerBand = new StiFooterBand { Height = 0.5, Name = "FooterBand" };
    page.Components.Add(footerBand);
    var footerText = new StiText(new RectangleD(0, 0, 10, 0.5))
    {
        Text = "Count - {Count()}",
        HorAlignment = StiTextHorAlignment.Right
    };
    footerBand.Components.Add(footerText);

    // 5. Save as .mrt file in temp (optional)
    string fileName = $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}.mrt";
    string filePath = Path.Combine(Path.GetTempPath(), fileName);
    report.Save(filePath);

    return filePath;
}
}
