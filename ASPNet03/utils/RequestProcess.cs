using System.Text;
using Microsoft.Extensions.Primitives;

namespace ASPNet03.utils
{
    public static class RequestProcess
    {
        public static string RequestInfo(HttpRequest request)
        {
            var sb = new StringBuilder();

            // Lấy http scheme
            var scheme = request.Scheme;
            // td, tr là các phương thức extension method tạo ra thẻ td, tr
            sb.Append(("Scheme".td() + scheme.td()).tr());

            // Host header
            var host = request.Host.HasValue ? request.Host.Value : "no host";
            sb.Append(("Host".td() + host.td()).tr());

            // Lấy pathbase (URL Path cho Map)
            var pathbase = request.PathBase.ToString();
            sb.Append(("Pathbase".td() + pathbase.td()).tr());

            // Lấy pathbase (URL Path cho Map)
            var path = request.Path.ToString();
            sb.Append(("Path".td() + path.td()).tr());

            // Lấy chuỗi query
            var queryString = request.QueryString.HasValue ? request.QueryString.Value : "no query string";
            sb.Append(("QueryString".td() + queryString.td()).tr());

            // Lấy phương thức
            var method = request.Method;
            sb.Append(("Method".td() + method.td()).tr());

            // Lấy giao thức
            var Protocol = request.Protocol;
            sb.Append(("Protocol".td() + Protocol.td()).tr());

            // Lấy ContentType
            var ContentType = request.ContentType;
            sb.Append(("ContentType".td() + ContentType.td()).tr());

            // Tham số là 1 delegate có 1 tham số là KeyValuePair<string,Microsoft.Extensions.Primitives.StringValues> và trả về 1 string
            var listheaderString = request.Headers.Select((header) =>
            {
                return $"{header.Key}: {header.Value}".HtmlTag("li");
            });
            var headerHtml = string.Join("", listheaderString).HtmlTag("ul");
            sb.Append(("Header".td() + headerHtml.td()).tr());

            var listCookiesString = request.Cookies.Select((header) =>
            {
                return $"{header.Key}: {header.Value}".HtmlTag("li");
            });
            var cookiesHtml = string.Join("", listCookiesString).HtmlTag("ul");
            sb.Append(("Cookies".td() + cookiesHtml.td()).tr());

            // Lấy tên và giá trí query
            var listQuery = request.Query.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
            var queryHtml = string.Join("", listQuery).HtmlTag("ul");
            sb.Append(("Các Query".td() + queryHtml.td()).tr());

            // Kiểm tra thử query có biến tên abc không?
            StringValues abcParameter;
            bool existAbcParam = request.Query.TryGetValue("abc", out abcParameter);
            string queryVal = existAbcParam ? abcParameter.FirstOrDefault() : "không có giá trị";
            sb.Append(("abc query".td() + queryVal.ToString().td()).tr());

            string info = "Thông tin request".HtmlTag("h2") + sb.ToString().HtmlTag("table", "table table-sm table-bordered");
            return info;
        }

        public static async Task<string> ProcessSubmitFormAsync(HttpRequest request)
        {
            //Xử lý đọc dữ liệu Form - khi post - dữ liệu này trình  bày trên Form
            string hovaten = "";
            bool luachon = false;
            string email = "";
            string password = "";
            string thongbao = "";

            if (request.Method == "POST")
            {
                IFormCollection form = request.Form;
                hovaten = form["hovaten"].FirstOrDefault() ?? "";
                email = form["email"].FirstOrDefault() ?? "";
                password = form["password"].FirstOrDefault() ?? "";
                luachon = form["luachon"].FirstOrDefault() == "on";

                // Thêm @ để có thể xuống dòng viết
                thongbao = @$"Dữ liệu post - email: {email} - hovaten: {hovaten}
                -password: {password} - luachon: {luachon}";
            }


            // Đọc toàn bộ nội dung HTML
            // Đường dẫn tính từ thư mục chứa project hiện tại, đây đang đọc file chứ không phải href mà dùng
            // đường dẫn tuyệt đối hay tương đối của url
            var format = await File.ReadAllTextAsync("./FormSubmit.html");
            var html = string.Format(format, hovaten, email, luachon ? "checked" : "").HtmlTag("div", "container") + thongbao.HtmlTag("div","container");
            return html;
        }
    }
}