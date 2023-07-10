namespace AspComponentView.Components.MessagePage
{
    public class Message
    {
        public string Title { get; set; } = "Thông báo";

        public string HtmlContent { get; set; } = "";
        
        public string urlRedirect { get; set; } = "/";

        public int secondWait { get; set; } = 3; // Mặc định sau 3s thì chuyển trang
    }
}