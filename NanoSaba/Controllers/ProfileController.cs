using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.People.Commands.UpdateOrder;
using CleanArchitecture.Application.People.Commands.UpdatePerson;
using CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Infrastructure.Common;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoSaba.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using static CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination.OrderBriefDto;

namespace NanoSaba.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var analyzePrices = await _mediator.Send(new GetAnalyzerDevicePricesQuery());
            var dashboard = await _mediator.Send(new GetOrderDashboardQuery());
            var result = await _mediator.Send(new GetProfileQuery());
            var resultArticle = await _mediator.Send(new GetArticleWithPaginationQuery()
            {
                RecordsPerPage = 3
            }, CancellationToken.None);

            var pagedto = new ProfilePageDto()
            {
                Person = result,
                Articles = resultArticle.Items,
                Dashboard = dashboard,
                AnalyzePrices = analyzePrices
            };

            return View("~/Views/Home/Profile.cshtml", pagedto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice()
        {
            bool isSuccess = true;
            string message = "";
            long result = 0;

            var id = FormAttributesUtility.GetLongByKey("id", Request);
            var price = FormAttributesUtility.GeDecimalByKey("price", Request);

            if (id <= 0 || price <= 0)
            {
                isSuccess = false;
                message = "Invalid id or price.";
            }
            else
            {
                try
                {
                    result = await _mediator.Send(new UpdateAnalyzerDevicePriceCommand()
                    {
                        Id = (int)id,
                        Price = price
                    }, CancellationToken.None);

                    if (result <= 0)
                    {
                        isSuccess = false;
                        message = "Update failed.";
                    }
                }
                catch (AppException exp)
                {
                    isSuccess = false;
                    message = exp.Message;
                }
            }

            return await Index();
        }

        [HttpGet]
        public async Task<IActionResult> ExportOrdersToExcel()
        {
            var exportDto = await _mediator.Send(new GetOrderExportQuery());

            var bytes = BuildExcelWithHeadersAndRows(exportDto, dto => new object?[]
            {
                dto.RowIndex,
                dto.SampleCode,
                dto.CustomerName,
                dto.Date,
                dto.Analysis,
                dto.TypeNormalUrgent,
                dto.Count,
                dto.AnalysisCost,
                dto.SetadPaid,
                dto.TotalCost,
                dto.Tax,
                dto.NetAfterGrantTax,
                dto.TotalPaid,
                dto.ResultSent,
                dto.StatusPaid,
                dto.PartnerLab,
                dto.PartnerPaid,
                dto.PaidOrNot,
                dto.NationalId,
                dto.Phone,
                dto.Email,
                dto.Description,
                dto.DocCode
            });

            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Report_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(bytes, contentType, fileName);
        }

        private static byte[] BuildExcelWithHeadersAndRows<T>(IEnumerable<T> items, Func<T, object?[]> mapRow)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string[] headers =
            {
                "ردیف",
                "کد نمونه",
                "نام مشتری/ سازمان",
                "تاریخ",
                "آنالیز",
                "نوع (عادی- فوری)",
                "تعداد",
                "هزینه آنالیز (تومان)",
                "هزینه پرداختی ستاد (تومان)",
                "هزینه کل (تومان)",
                "مالیات (تومان)",
                "هزینه کل پس از کسر گرنت و افزودن مالیات (تومان)",
                "جمع کل پرداختی",
                "نتیجه (ارسال شده- ارسال نشده)",
                "وضعیت (پرداخت شده- پرداخت نشده)",
                "آزمایشگاه همکار",
                "پرداختی همکار",
                "پرداخت شده- نشده",
                "کد ملی",
                "شماره تلفن",
                "ایمیل",
                "توضیحات",
                "کد مدرک: F-46/00"
            };

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("فروردین");
            ws.View.RightToLeft = true;

            // هدرها
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cells[1, c + 1].Value = headers[c];
                ws.Cells[1, c + 1].Style.Font.Bold = true;
            }

            // ردیف‌ها
            int r = 2;
            foreach (var item in items)
            {
                var values = mapRow(item);
                for (int c = 0; c < headers.Length && c < values.Length; c++)
                {
                    ws.Cells[r, c + 1].Value = values[c];
                }
                r++;
            }

            // استایل‌های ساده
            ws.Cells[1, 1, 1, headers.Length].AutoFilter = true;
            ws.View.FreezePanes(2, 1);
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditProfile()
        {
            bool isSuccess = true;
            string message = "";
            long result = 0;
            var id = FormAttributesUtility.GetLongByKey("id", Request);
            var firstName = Request.Form["FirstName"].ToString();
            var lastName = Request.Form["LastName"].ToString();
            var companyName = Request.Form["CompanyName"].ToString();
            var mobilenumber = Request.Form["Mobile"].ToString();
            var email = Request.Form["Email"].ToString();
            var password = Request.Form["Password"].ToString();
            var address = Request.Form["Address"].ToString();

            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(mobilenumber) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(password))
            {
                isSuccess = false;
            }
            else
            {
                try
                {
                    result = await _mediator.Send(new UpdatePersonProfileCommand()
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        MobileNumber = mobilenumber,
                        Address = address,
                        Email = email,
                        CompanyName = companyName,
                        Password = password
                    }, CancellationToken.None);
                    if (result <= 0)
                        isSuccess = false;

                    //var resultGenerateCode = await _userCodeService.GenerateCode(mobilenumber, CancellationToken.None);
                }
                catch (AppException exp)
                {
                    isSuccess = false;
                    message = exp.Message;
                }
            }

            return await Index();
        }

        [HttpPost]
        public async Task<IActionResult> UploadComplaint(IFormFile UploadedFile)
        {
            var TrackingCodes = Request.Form["TrackingCodes"].ToString();
            var OrderDescription = Request.Form["OrderDescription"].ToString();
            var Description = Request.Form["Description"].ToString();
            var Recomendation = Request.Form["Recomendation"].ToString();
            long imageId = 0;
            if (UploadedFile != null && UploadedFile.Length != 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var image = new CreateImageCommand()
                {
                    ImageTitle = UploadedFile.FileName,
                    ImageLatinTitle = "test",
                    ContextId = 1,
                    ImageFile = UploadedFile,
                    FolderPath = filePath
                };
                imageId = await _mediator.Send(image);
            }
            var result = await _mediator.Send(new CreateComplaintCommand()
            {
                ImageId = imageId,
                TrackingCodes = TrackingCodes,
                OrderDescription = OrderDescription,
                Description = Description,
                Recomendation = Recomendation,
            });
            return await Index();
        }

        [HttpPost]
        public async Task<IActionResult> UploadRecommendation()
        {
            var Description = Request.Form["Description"].ToString();
            var result = await _mediator.Send(new CreateRecommendationCommand()
            {
                Description = Description,
            });

            return await Index();
        }
    }
}