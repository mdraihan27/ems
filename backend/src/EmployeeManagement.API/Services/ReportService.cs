using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.API.Services;

/// <summary>
/// Service that generates PDF reports using QuestPDF.
/// </summary>
public class ReportService : IReportService
{
    /// <inheritdoc />
    public byte[] GenerateEmployeeListReport(List<Employee> employees)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Text("Employee Management System")
                            .FontSize(18).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text("Employee List Report")
                            .FontSize(14).SemiBold();
                        col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC")
                            .FontSize(8).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Blue.Darken2);
                    });
                });

                page.Content().Element(content =>
                {
                    if (employees.Count == 0)
                    {
                        content.PaddingTop(20).Text("No employees found matching the criteria.")
                            .FontSize(12).Italic().FontColor(Colors.Grey.Darken1);
                        return;
                    }

                    content.Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2.5f);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1.5f);
                        });

                        // Header row
                        table.Header(header =>
                        {
                            foreach (var h in new[] { "#", "Name", "NID", "Department", "Phone", "Salary" })
                            {
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text(h).FontColor(Colors.White).Bold().FontSize(9);
                            }
                        });

                        // Data rows
                        for (int i = 0; i < employees.Count; i++)
                        {
                            var emp = employees[i];
                            var bgColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                            foreach (var val in new[] { (i + 1).ToString(), emp.Name, emp.NID, emp.Department, emp.Phone, $"৳{emp.BasicSalary:N2}" })
                            {
                                table.Cell().Background(bgColor).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(4).Text(val).FontSize(9);
                            }
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    /// <inheritdoc />
    public byte[] GenerateEmployeeCvReport(Employee employee)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(header =>
                {
                    header.Column(col =>
                    {
                        col.Item().Text("Employee CV Report")
                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC")
                            .FontSize(8).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Blue.Darken2);
                    });
                });

                page.Content().Element(content =>
                {
                    content.Column(col =>
                    {
                        // Employee Details
                        col.Item().PaddingBottom(10).Text("Personal Information")
                            .FontSize(14).Bold().FontColor(Colors.Blue.Darken1);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            AddInfoRow(table, "Name", employee.Name);
                            AddInfoRow(table, "NID", employee.NID);
                            AddInfoRow(table, "Phone", employee.Phone);
                            AddInfoRow(table, "Department", employee.Department);
                            AddInfoRow(table, "Basic Salary", $"৳{employee.BasicSalary:N2}");
                        });

                        col.Item().PaddingTop(15).PaddingBottom(5)
                            .LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                        // Spouse Section
                        col.Item().PaddingBottom(10).Text("Spouse Information")
                            .FontSize(14).Bold().FontColor(Colors.Blue.Darken1);

                        if (employee.Spouse != null)
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                });

                                AddInfoRow(table, "Spouse Name", employee.Spouse.Name);
                                AddInfoRow(table, "Spouse NID", employee.Spouse.NID);
                            });
                        }
                        else
                        {
                            col.Item().PaddingLeft(10).Text("No spouse")
                                .Italic().FontColor(Colors.Grey.Darken1);
                        }

                        col.Item().PaddingTop(15).PaddingBottom(5)
                            .LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                        // Children Section
                        col.Item().PaddingBottom(10).Text("Children Information")
                            .FontSize(14).Bold().FontColor(Colors.Blue.Darken1);

                        if (employee.Children.Count != 0)
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    foreach (var h in new[] { "#", "Name", "Date of Birth" })
                                    {
                                        header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                            .Text(h).FontColor(Colors.White).Bold().FontSize(9);
                                    }
                                });

                                for (int i = 0; i < employee.Children.Count; i++)
                                {
                                    var child = employee.Children.ElementAt(i);
                                    var bgColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                                    foreach (var val in new[] { (i + 1).ToString(), child.Name, child.DateOfBirth.ToString("yyyy-MM-dd") })
                                    {
                                        table.Cell().Background(bgColor).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                            .Padding(4).Text(val).FontSize(9);
                                    }
                                }
                            });
                        }
                        else
                        {
                            col.Item().PaddingLeft(10).Text("No children")
                                .Italic().FontColor(Colors.Grey.Darken1);
                        }
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    /// <summary>
    /// Adds a label-value row to an info table.
    /// </summary>
    private static void AddInfoRow(TableDescriptor table, string label, string value)
    {
        table.Cell().Padding(4).Text(label).Bold().FontSize(10);
        table.Cell().Padding(4).Text(value).FontSize(10);
    }
}
