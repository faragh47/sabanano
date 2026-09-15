using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;
using MediatR;
using QuestPDF.Fluent;

public class FormCreatorController : Controller
{
    private readonly IMediator _mediator;

    public FormCreatorController(IMediator mediator)
    {
        _mediator = mediator;
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    public async Task<IActionResult> GeneratePdf(string orderId)
    {
        var result = await _mediator.Send(new GetOrderForDocumentQuery()
        {
            Id =Convert.ToInt64(orderId) 
        });
        var document = new FormPdfDocument(result);
        var pdfStream = new MemoryStream();
        document.GeneratePdf(pdfStream);
        pdfStream.Position = 0;

        return File(pdfStream, "application/pdf", "FTIRForm.pdf");
    }
}