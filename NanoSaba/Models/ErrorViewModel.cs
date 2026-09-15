using CleanArchitecture.Infrastructure.Common;

namespace NanoSaba.Models;

public class ErrorViewModel:BaseViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

