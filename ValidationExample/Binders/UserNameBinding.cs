using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValidationExample.Binders
{
    /// <summary>
    /// Lớp này để custom ại quá trình binding vào property UserName
    /// <br/> 
    /// Nhiệm vụ:
    /// <br/>
    /// - Chuyển tên thành in hoa <br/>
    /// - Tên không được chứa xxx <br/>
    /// - Cắt khoảng trắng ở đầu và cuối
    /// </summary>
    public class UserNameBinding : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("Đang null");
            }

            // ModelName trong ví dụ này: CustomerInfo.CustomerName
            string modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // Giá trị gửi đến cho modelName có nhiều giá trị vì ở 1 key nào đó có thể đến
            // từ submit form, query, header,....
            string value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                // Không thực hiện binding và trả về tác vụ đã hoàn thành
                return Task.CompletedTask;
            }

            // Thực hiện binding
            string result = value.ToLower();
            if (result.Contains("xxx"))
            {
                // Thực hiện gán giá trị đã nhập vào property nhưng gán đây để nó có giá trị
                // thôi chứ thực chất thì ModelState vẫn cho nó là Invalid
                // Nếu không có thực hiện SetModelValue thì giả sử dữ liệu sai thì nó không gán giá trị vào
                // cho property và lúc trả về Page thì ô input đó rỗng, người dùng sẽ không biết mình sai gì
                bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
                bindingContext.ModelState.TryAddModelError(modelName, "Lỗi do UserName chứa xxx");
                return Task.CompletedTask;
            }
            
            result = result.Trim();
            bindingContext.ModelState.SetModelValue(modelName, result, result);

            // Đây là lúc thật sự gán giá trị cho property
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}