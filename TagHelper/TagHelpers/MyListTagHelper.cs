using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASPTagHelper.TagHelpers
{
    [HtmlTargetElement("mylist")]
    public class MyListTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        // thuộc tính sẽ là list-title
        // Chú ý đặt tên theo Pascal
        public string ListTitle { get; set; } = "Chưa thiết lập title";

        public List<string> ListItems { get; set; } = new List<string>();

        public string Author { get; set; } = "chưa thiết lập";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // ul sẽ thay cho mylist
            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;

            // Sử dụng bootstrap cho thẻ ul
            output.Attributes.SetAttribute("class", "list-group");

            // PreElement: chèn phần tử này trước khi sin ra <ul> <li>
            output.PreElement.AppendHtml($"<h2>{ListTitle}</h2>");
            output.PreElement.AppendHtml($"<p>Author: {Author}</p>");
            StringBuilder sb = new StringBuilder();
            foreach (string item in ListItems)
            {
                // Sử dụng bootstrap cho thẻ li 
                sb.Append($@"<li class=""list-group-item"">{item}</li>");
            }
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}