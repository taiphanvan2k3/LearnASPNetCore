using Microsoft.AspNetCore.Mvc;

namespace AspComponentView.Components.MessagePage
{
    public class MessagePage : ViewComponent
    {
        public MessagePage()
        {

        }

        public IViewComponentResult Invoke(Message message)
        {
            // Thiết lập header của HTTP response chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH", $"{message.secondWait};URL={message.urlRedirect}");
            return View(message);
        }
    }
}