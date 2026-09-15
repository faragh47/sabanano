using Common.Utilities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class FormPdfDocument : IDocument
{
    private readonly LabAnalysisForm Model;

    public FormPdfDocument(LabAnalysisForm model)
    {
        Model = model;
    }

    public void Compose(IDocumentContainer container)
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDoc.png");
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape()); // Set the page size to A4 in landscape orientation
            page.Margin(2, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(6).FontFamily("B Nazanin"));

            // page.Size(PageSizes.A4);
            // page.Margin(2, Unit.Centimetre);
            // page.PageColor(Colors.White);
            // page.DefaultTextStyle(x => x.FontSize(8).FontFamily("B Nazanin"));
            page.Content().Padding(8).Column(column =>
            {
                column.Item().Border(1).Background(Colors.White).Row(row =>
                {
                    row.RelativeItem(2).Column(header =>
                    {
                        header.Item().BorderRight(1).Padding(2).Row(row =>
                        {
                            row.RelativeItem().Text(Model.DocumentNumber).FontSize(6).AlignRight();
                            row.RelativeItem().Text(":کد مدرک").FontSize(6).Bold().AlignRight(); //DOCUMENT NUMBER
                        });
                        header.Item().BorderRight(1).BorderBottom(1).Padding(2).Row(row =>
                        {
                            row.RelativeItem().Text(Model.TrackingCode).FontSize(8).AlignRight();
                            row.RelativeItem().Text(":شماره سفارش").FontSize(8).Bold().AlignRight(); //TrackingCode
                        });
                        header.Item().BorderRight(1).Padding(2).Row(row =>
                        {
                            row.RelativeItem().Text(Model.AcceptanceDate.ToPersianDate()).FontSize(6)
                                .AlignRight();
                            row.RelativeItem().Text(":تاریخ پذیرش").FontSize(6).Bold().AlignRight(); //DateTimeNow
                        });
                    });
                    row.RelativeItem(4).AlignCenter().Column(header =>
                    {
                        header.Item().BorderBottom(1).Padding(2).Text("آزمایشگاه تحقیقاتی شناسایی نانو مواد صبا")
                            .FontSize(8).Bold().AlignCenter();

                        header.Item().Padding(2).Text(Model.FormTitle) //SampleNumber
                            .FontSize(8).Bold().AlignCenter();
                    });

                    row.RelativeItem()
                        .AlignRight()
                        .AlignTop().Height(50).Width(50)
                        .Element(e => e.Image(imagePath));

                    // row.RelativeItem()
                    //     .AlignRight()
                    //     .AlignTop()
                    //     .Element(e => e.Image(imagePath).FitArea());
                });
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // First column (labels or text)
                        columns.RelativeColumn(3); // Second column (values or inputs)
                        columns.RelativeColumn(3); // Third column (values or inputs)
                    });

                    // Header Row
                    table.Cell().ColumnSpan(3)
                        .Background(Colors.Blue.Medium)
                        .Border(1)
                        .Padding(2)
                        .Text("اطلاعات مشتری")
                        .FontColor(Colors.Black)
                        .FontSize(8)
                        .Bold()
                        .AlignCenter();

                    table.Cell().Border(1).Padding(2)
                        .Text($"شماره تماس: {Model.Customer.MobileNumber}")
                        .FontSize(8)
                        .AlignRight(); // MobileNumber

                    table.Cell().Border(1).Padding(2)
                        .Text($"{Model.Customer.EmailAddress}:آدرس ایمیل")
                        .FontSize(8)
                        .AlignRight(); // Email

                    table.Cell().Border(1).Padding(2)
                        .Text($"نام و نام خانوادگی: {Model.Customer.FullName}")
                        .FontSize(8)
                        .AlignRight(); // FirstName

                    table.Cell().Border(1).Padding(2)
                        .Text($"آدرس: {Model.Customer.Address}")
                        .FontSize(8)
                        .AlignRight(); // Address

                    table.Cell().Border(1).Padding(2)
                        .Text($"نام دانشگاه/سازمان/شرکت: {Model.Customer.OrganizationName}")
                        .FontSize(8)
                        .AlignRight(); // CompanyName

                    if (Model.Grant?.HasGrant is true)
                    {
                        table.Cell().Border(1).Padding(2)
                            .Text("گرنت: دارد")
                            .FontSize(8)
                            .AlignRight(); // Grant available

                        table.Cell().Border(1).Padding(2)
                            .Text($"کد ملی صاحب گرنت: {Model?.Grant?.NationalCode}")
                            .FontSize(8)
                            .AlignRight(); // Grant NationalCode
                    }
                    else
                    {
                        table.Cell().Border(1).Padding(2)
                            .Text("گرنت: ندارد")
                            .FontSize(8)
                            .AlignRight(); // Grant not available
                    }

                    table.Cell().Border(1).Padding(2)
                        .Text($"شماره اقتصادی: {Model?.Company?.EconomicNumber}")
                        .FontSize(8)
                        .AlignRight(); // Economic Number

                    table.Cell().Border(1).Padding(2)
                        .Text($"شماره همراه رابط: {Model?.Company?.MobileNumber}")
                        .FontSize(8)
                        .AlignRight(); // Mobile Number

                    table.Cell().Border(1).Padding(2)
                        .Text($"شناسه ملی شرکت: {Model?.Company?.NationalCode}")
                        .FontSize(8)
                        .AlignRight(); // Company National Code


                    table.Cell().Border(1).Padding(2)
                        .Text("")
                        .FontSize(8)
                        .AlignRight(); // Grant NationalCode

                    table.Cell().Border(1).Padding(2)
                        .Text("")
                        .FontSize(8)
                        .AlignRight(); // Grant NationalCode


                    table.Cell().Border(1).Padding(2)
                        .Text($"شماره ثبت: {Model?.Company?.RegisterCode}")
                        .FontSize(8)
                        .AlignRight(); // Register Code
                });
                column.Item().Border(1)
                    .Background(Colors.Blue.Medium)
                    .Padding(2)
                    .Text("مشخصات اقلام آزمون")
                    .FontColor(Colors.Black)
                    .FontSize(8)
                    .Bold()
                    .AlignCenter();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(15); // Description
                        columns.RelativeColumn(15); // State
                        columns.RelativeColumn(4); // Code
                        columns.RelativeColumn(5); // Sample Name
                        columns.RelativeColumn(1); // Row Number
                    });
                    // Table Header Row
                    table.Header(header =>
                    {
                        header.Cell().Border(1).Background(Colors.Blue.Lighten3).Text("توضیحات").AlignCenter();
                        header.Cell().Border(1).Background(Colors.Blue.Lighten3).Text("شرایط").AlignCenter();
                        header.Cell().Border(1).Background(Colors.Blue.Lighten3).Text("کد").AlignCenter();
                        header.Cell().Border(1).Background(Colors.Blue.Lighten3).Text("نام نمونه").AlignCenter();
                        header.Cell().Border(1).Background(Colors.Blue.Lighten3).Text("ردیف").AlignCenter();
                    });
                    foreach (var item in Model.Analyzes)
                    {
                        table.Cell().Border(1).AlignCenter().Text(item.Description).AlignCenter();
                        table.Cell().Border(1).AlignCenter().Text(item.State).AlignCenter();
                        table.Cell().Border(1).AlignCenter().Text(item.SampleCode).AlignCenter();
                        table.Cell().Border(1).AlignCenter().Text(item.SampleName).AlignCenter();
                        table.Cell().Border(1).AlignCenter().Text(item.RowNumber.ToString()).AlignCenter();
                    }
                });

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => { columns.RelativeColumn(1); });
                    table.Cell()
                        .Border(1)
                        .Height(15)
                        .Text($"{Model.AdditionalComments} :توضیحات ")
                        .FontColor(Colors.Black)
                        .FontSize(8)
                        .Bold()
                        .AlignRight();
                });

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(4); // Label column
                        columns.RelativeColumn(1); // Value column
                    });

                    // Table header row
                    table.Cell().ColumnSpan(2)
                        .Background(Colors.Blue.Medium)
                        .Border(1)
                        .Padding(2)
                        .Text("هزینه‌ها و پرداخت")
                        .FontColor(Colors.Black).Bold().AlignCenter();

                    table.Cell().Border(1).AlignRight().Padding(2)
                        .Text($" ریال {Model.PaymentDetails?.AnalyzePrice:N2}  ").FontSize(8);
                    table.Cell().Border(1).Padding(2).Text("تعرفه آزمون").AlignRight().FontSize(8);

                    table.Cell().Border(1).AlignRight().Padding(2)
                        .Text($" ریال {Model.PaymentDetails?.GrantPrice:N2}  ").FontSize(8);
                    table.Cell().Border(1).Padding(2).Text("سهم شبکه آزمایشگاهی").AlignRight().FontSize(8);

                    table.Cell().Border(1).AlignRight().Padding(2)
                        .Text($" ریال{Model.PaymentDetails?.Tax:N2}  ").FontSize(8);
                    table.Cell().Border(1).Padding(2).Text("مالیات بر ارزش افزوده").AlignRight().FontSize(8);

                    table.Cell().Border(1).AlignRight().Padding(2)
                        .Text($"ریال {Model.PaymentDetails?.TotalPayablePrice:N2}").Bold().FontSize(10);
                    table.Cell().Border(1).Padding(2).Text("جمع مبلغ قابل پرداخت")
                        .AlignRight().Bold().FontSize(9);
                });

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Label column
                        columns.RelativeColumn(1); // Checkbox or value column
                    });

                    // Header row for the section
                    table.Cell().ColumnSpan(2)
                        .Background(Colors.Blue.Medium)
                        .Border(1)
                        .Padding(2)
                        .Text("این قسمت توسط آزمایشگاه تکمیل می گردد")
                        .FontColor(Colors.Black)
                        .Bold()
                        .AlignCenter();

                    table.Cell().ColumnSpan(2)
                        .Background(Colors.White)
                        .Border(1)
                        .Height(15)
                        .Padding(2)
                        .Text($"نظر کارشناس آزمون: {Model.LabCompletionDetails?.ExpertOpinion}")
                        .FontColor(Colors.Black)
                        .Bold()
                        .AlignRight();


                    if (Model.LabCompletionDetails?.CanPerformTest is true)
                        table.Cell().ColumnSpan(2)
                            .Background(Colors.White)
                            .Border(1)
                            .Height(15)
                            .Padding(2)
                            .Text("امکان انجام آزمون: دارد")
                            .FontColor(Colors.Black)
                            .Bold()
                            .AlignRight();
                    else
                        table.Cell().ColumnSpan(2)
                            .Background(Colors.White)
                            .Border(1)
                            .Height(15)
                            .Padding(2)
                            .Text("امکان انجام آزمون: ندارد")
                            .FontColor(Colors.Black)
                            .Bold()
                            .AlignRight();
                    // Header row for "نظر کارشناس آزمون"
                });
                column.Item().Text(Model.Address)
                    .FontSize(8).AlignRight();
                column.Item().Text(Model.ContactNumber)
                    .FontSize(8).AlignRight();
                column.Item().Text("www.nanomavadsaba.com :آدرس سایت")
                    .FontSize(8).AlignRight();
                column.Item().Text("Info.saba.analyze@gmail.com :آدرس ایمیل")
                    .FontSize(8).AlignRight();
            });
        });
    }
}