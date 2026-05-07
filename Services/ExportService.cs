using AirportFlightManagement.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirportFlightManagement.Services;

public class ExportService
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
            worksheet.Cell(1, 2).Value = "Destination";
            worksheet.Cell(1, 3).Value = "Day";
            worksheet.Cell(1, 4).Value = "Departure";
            worksheet.Cell(1, 5).Value = "Arrival";
            worksheet.Cell(1, 6).Value = "Seats";
            worksheet.Cell(1, 7).Value = "Available";
            worksheet.Cell(1, 8).Value = "Price (€)";
            worksheet.Cell(1, 9).Value = "Status";

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
                worksheet.Cell(row, 2).Value = flight.Destination;
                worksheet.Cell(row, 3).Value = flight.DepartureDate.ToString("dddd");
                worksheet.Cell(row, 4).Value = flight.DepartureTime.ToString("HH:mm");
                worksheet.Cell(row, 5).Value = flight.ArrivalTime.ToString("HH:mm");
                worksheet.Cell(row, 6).Value = flight.TotalSeats;
                worksheet.Cell(row, 7).Value = flight.AvailableSeats;
                worksheet.Cell(row, 8).Value = flight.TicketPrice;
                worksheet.Cell(row, 9).Value = flight.Status;

                // Alternate row colors
                if (row % 2 == 0)
                {
                    worksheet.Range($"A{row}:I{row}").Style.Fill.BackgroundColor = XLColor.FromArgb(0x1A1F3A);
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
}
