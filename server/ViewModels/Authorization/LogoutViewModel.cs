using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicroSB.Server.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
