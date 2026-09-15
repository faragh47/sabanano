using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using Data.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoSaba.Analyzes;
using NanoSaba.Utility;
using Newtonsoft.Json;

namespace NanoSaba.Controllers
{
    public class RegisterOrderController : Controller
    {
        private readonly IMediator _mediator;
        private AnalyzeConverter _converter;

        public RegisterOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NextLevel(string button)
        {
            var registerOrder = initOrderDto();
            var analyzesJson = TempData["RegisterOrder"] as string;
            registerOrder = JsonConvert.DeserializeObject<RegisterOrderPageDto>(analyzesJson);
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
            return View("~/Views/Order/RegisterOrderLevel2.cshtml", registerOrder);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddSample(string button)
        {
            var registerOrder = initOrderDto();
            string actionType = button;
            var id = Request.Form["analyzeId"].ToString();
            if (string.IsNullOrEmpty(actionType))
            {
                actionType = Request.Form["actionType"].ToString();
            }

            if (actionType.Equals("addSample"))
            {
                var editDto =
                    registerOrder.OrderAnalyzes.FirstOrDefault(x => x.Name.Equals(registerOrder.OrderAnalyze.Name));
                if (editDto is null)
                    registerOrder.OrderAnalyzes.Add(registerOrder.OrderAnalyze);
                else
                {
                    //edit
                    registerOrder.OrderAnalyzes.Remove(editDto);
                    registerOrder.OrderAnalyzes.Add(registerOrder.OrderAnalyze);
                }

                registerOrder.OrderAnalyze = new();
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                registerOrder.IsCreatable = true;
                return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
            }
            else if (actionType.Equals("nextLevel"))
            {
                var analyzesJson = TempData["RegisterOrder"] as string;
                registerOrder = JsonConvert.DeserializeObject<RegisterOrderPageDto>(analyzesJson);
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                return View("~/Views/Order/RegisterOrderLevel2.cshtml", registerOrder);
            }
            else if (actionType.Equals("edit"))
            {
                var editDto = registerOrder.OrderAnalyzes.FirstOrDefault(x => x.Name.Equals(id));
                registerOrder.OrderAnalyze = editDto;
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                registerOrder.IsEditable = true;
                return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
            }
            else if (actionType.Equals("delete"))
            {
                var editDto = registerOrder.OrderAnalyzes.FirstOrDefault(x => x.Name.Equals(id));
                registerOrder.OrderAnalyzes.Remove(editDto);
                registerOrder.OrderAnalyze = new();
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
            }
            else if (actionType.Equals("view"))
            {
                var editDto = registerOrder.OrderAnalyzes.FirstOrDefault(x => x.Name.Equals(id));
                registerOrder.OrderAnalyze = editDto;
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                registerOrder.IsEditable = true;
                foreach (var item in registerOrder.OrderAnalyzes)
                {
                    item.IsView = true;
                }

                return View("~/Views/Order/RegisterOrderView.cshtml", registerOrder);
            }

            return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NextLevel()
        {
            var analyzesJson = TempData["RegisterOrder"] as string;
            var registerOrder = JsonConvert.DeserializeObject<RegisterOrderPageDto>(analyzesJson);
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
            return View("~/Views/Order/RegisterOrderLevel2.cshtml", registerOrder);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddSample1()
        {
            var registerOrder = initOrderDto();
            return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
        }

        private RegisterOrderPageDto initOrderDto()
        {
            var analyzesJson = TempData["RegisterOrder"] as string;
            var registerOrder = JsonConvert.DeserializeObject<RegisterOrderPageDto>(analyzesJson);
            var orderAnalyzeCommand = GetOrderAnalyze();
            registerOrder.OrderAnalyze = orderAnalyzeCommand;
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
            return registerOrder;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(string myParams)
        {
            TempData["AnalyzeName"] = myParams;
            var pagedto = new RegisterOrderPageDto()
            {
                Name = myParams,
                OrderAnalyze = new(),
                IsCreatable = true
            };
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(pagedto);
            return View("~/Views/Order/RegisterOrder.cshtml", pagedto);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetOrder(string item, bool isEdit)
        {
            var analyze = new AnalyzePageDto();
            var resultOrder = await _mediator.Send(new GetRegisterOrderQuery()
            {
                Id = Convert.ToInt32(item),
            }, CancellationToken.None);
            resultOrder.IsEditable = isEdit;
            resultOrder.IsView = !isEdit;
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(resultOrder);
            TempData["AnalyzeName"] = resultOrder.Name;
            return View("~/Views/Order/RegisterOrder.cshtml", resultOrder);
        }

        [AllowAnonymous]
        public async Task<IActionResult> OrderView(string item)
        {
            var analyze = new AnalyzePageDto();
            var resultOrder = await _mediator.Send(new GetRegisterOrderQuery()
            {
                Id = Convert.ToInt32(item),
            }, CancellationToken.None);
            resultOrder.IsEditable = false;
            resultOrder.IsView = true;
            TempData["RegisterOrder"] = JsonConvert.SerializeObject(resultOrder);
            TempData["AnalyzeName"] = resultOrder.Name;
            return View("~/Views/Order/RegisterOrderView.cshtml", resultOrder);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterOrder(string button)
        {
            var analyzesJson = TempData["RegisterOrder"] as string;
            var registerOrder = JsonConvert.DeserializeObject<RegisterOrderPageDto>(analyzesJson);
            if (button.Equals("back"))
            {
                TempData["RegisterOrder"] = JsonConvert.SerializeObject(registerOrder);
                return View("~/Views/Order/RegisterOrder.cshtml", registerOrder);
            }

            var grant = GetGrant();
            var company = GetCompany();
            if (grant.Item1)
                registerOrder.Grant = grant.Item2;
            else
            {
                registerOrder.Grant = null;
            }

            if (company.Item1)
                registerOrder.Company = company.Item2;
            else
            {
                registerOrder.Company = null;
            }

            registerOrder.IsRequireHeader = FormAttributesUtility.GetBoolByKey("isRequireHeader", Request);
            registerOrder.Description = Request.Form["description"].ToString();
            ;
            registerOrder.IsRequireToReturnSample =
                FormAttributesUtility.GetBoolByKey("isRequireToReturnSample", Request);
            if (registerOrder.OrderAnalyze.Id is 0)
            {
                var result = await _mediator.Send(new CreateOrderCommand()
                {
                    OrderAnalyzes = registerOrder.OrderAnalyzes,
                    Grant = registerOrder.Grant,
                    Company = registerOrder.Company,
                    IsRequireToReturnSample = registerOrder.IsRequireToReturnSample,
                    IsRequireHeader = registerOrder.IsRequireHeader,
                    Description = registerOrder.Description
                });
            }
            else
            {
                var result = await _mediator.Send(new UpdateOrderCommand()
                {
                    Id = registerOrder.OrderAnalyze.Id,
                    OrderAnalyzes = registerOrder.OrderAnalyzes,
                    Grant = registerOrder.Grant,
                    Company = registerOrder.Company,
                    IsRequireToReturnSample = registerOrder.IsRequireToReturnSample,
                    IsRequireHeader = registerOrder.IsRequireHeader,
                    Description = registerOrder.Description
                });
            }

            return RedirectToAction("Index", "OrderCart"); // Redirects to OrderCart's Index action
        }

        private OrderAnalyzeCommand GetOrderAnalyze()
        {
            OrderAnalyzeCommand formAttributes = new();
            formAttributes.IsSensitiveToLight = FormAttributesUtility.GetBoolByKey("isSensitiveToLight", Request);
            formAttributes.IsSensitiveToHumidity = FormAttributesUtility.GetBoolByKey("isSensitiveToHumidity", Request);
            formAttributes.IsExplosive = FormAttributesUtility.GetBoolByKey("isExplosive", Request);
            formAttributes.HasNoSafety = FormAttributesUtility.GetBoolByKey("hasNoSafety", Request);
            formAttributes.IsPoisonous = FormAttributesUtility.GetBoolByKey("isPoisonous", Request);
            formAttributes.IsEscapable = FormAttributesUtility.GetBoolByKey("isEscapable", Request);
            formAttributes.IsFlammable = FormAttributesUtility.GetBoolByKey("isFlammable", Request);
            formAttributes.IsBadForBreathing = FormAttributesUtility.GetBoolByKey("isBadForBreathing", Request);
            formAttributes.HasNotAnyCondition = FormAttributesUtility.GetBoolByKey("hasNotAnyCondition", Request);
            formAttributes.IsSickness = FormAttributesUtility.GetBoolByKey("isSickness", Request);
            formAttributes.IsAdsorbBySkin = FormAttributesUtility.GetBoolByKey("isAdsorbBySkin", Request);
            formAttributes.SpeceficTemperture = FormAttributesUtility.GetDoubleByKey("speceficTemperture", Request);
            formAttributes.Id = FormAttributesUtility.GetLongByKey("id", Request);
            formAttributes.Description = Request.Form["description"].ToString();
            formAttributes.AdditionalDescription = Request.Form["additionalDescription"].ToString();
            formAttributes.Name = Request.Form["name"].ToString();
            string key = "";
            key = Request.Form["key"].ToString();
            _converter = new(Request);
            _converter.GetAnalyzeByKey(formAttributes, key);
            formAttributes.Key = key;
            return formAttributes;
        }

        private Tuple<bool, CreateGrantCommand> GetGrant()
        {
            var hasGrant = FormAttributesUtility.GetBoolByKey("HasGrant", Request);
            var grant = new CreateGrantCommand
            {
                NationalCode = Request.Form["nationalCode"].ToString(),
                FirstName = Request.Form["firstName"].ToString(),
                LastName = Request.Form["lastName"].ToString(),
                UniversityName = Request.Form["universityName"].ToString(),
                TelNumber = Request.Form["telNumber"].ToString()
            };
            return new Tuple<bool, CreateGrantCommand>(hasGrant, grant);
        }

        private Tuple<bool, CreateCompanyCommand> GetCompany()
        {
            var hasCompany = FormAttributesUtility.GetBoolByKey("HasCompany", Request);
            var company = new CreateCompanyCommand()
            {
                NationalCode = Request.Form["companyNationalCode"].ToString(),
                RegisterCode = Request.Form["registerCode"].ToString(),
                EconomicNumber = Request.Form["economicNumber"].ToString(),
                MobileNumber = Request.Form["mobileNumber"].ToString()
            };
            return new Tuple<bool, CreateCompanyCommand>(hasCompany, company);
        }
    }
}