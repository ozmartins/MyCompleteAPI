using Hard.Business.Interfaces;
using Hard.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace API.Controllers
{
    public abstract class MainController : Controller
    {
        private readonly INotifier _notifier;

        protected MainController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new { Success = true, Data = result });
            }
            else
            {
                return BadRequest(new { Success = false, Errors = _notifier.GetNotifications().Select(x => x.Message) });
            }
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelStateDictionary)
        {
            if (!modelStateDictionary.IsValid) NotifyInvalidModel(modelStateDictionary);
            return CustomResponse();
        }

        protected void NotifyInvalidModel(ModelStateDictionary modelStateDictionary)
        {
            var erros = modelStateDictionary.Values.SelectMany(x => x.Errors);
            foreach (var erro in erros)
            {
                var errorMessage = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string errorMessage)
        {
            _notifier.Handle(new Notification(errorMessage));
        }
    }
}
