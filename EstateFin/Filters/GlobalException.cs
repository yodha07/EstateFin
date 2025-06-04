using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EstateFin.Filters
{
    public class GlobalException : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string logmsg = $"[{DateTime.Now}] This is what caused the exception:  {context.Exception.Message} \n";

            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            string logPath = Path.Combine(logDir, "log.txt");

            File.AppendAllText(logPath, logmsg);

            var result = new ViewResult { ViewName = "Error" };

            //context.Result = result;

            context.Result = new RedirectToActionResult("Error", "Home", new { msg = logmsg });

            context.ExceptionHandled = true;
        }
    }
}
