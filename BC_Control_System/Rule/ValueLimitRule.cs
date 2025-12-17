using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace BC_Control_System.Rule
{
    public class ValidationBehavior : Behavior<TextBox>
    {
        // 错误回调属性（绑定到Model的方法）
        public static readonly DependencyProperty ErrorHandlerProperty =
            DependencyProperty.Register(
                "ErrorHandler",
                typeof(Action<string, string, bool>), // 增加bool参数表示是否错误
                typeof(ValidationBehavior)
            );

        public Action<string, string, bool> ErrorHandler
        {
            get => (Action<string, string, bool>)GetValue(ErrorHandlerProperty);
            set => SetValue(ErrorHandlerProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            // 监听验证状态变化
            Validation.AddErrorHandler(AssociatedObject, OnValidationError);
            AssociatedObject.LostFocus += OnLostFocus; // 确保焦点离开时更新
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            HandleValidation(e.Error, e.Action == ValidationErrorEventAction.Added);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            // 焦点离开时强制验证
            if (AssociatedObject is UIElement element && Validation.GetHasError(element))
            {
                var errors = Validation.GetErrors(element);
                if (errors.Count > 0)
                    HandleValidation(errors[0], true);
            }
        }

        private void HandleValidation(ValidationError error, bool isError)
        {
            if (error.BindingInError is BindingExpression binding)
            {
                string propertyName = binding.ResolvedSourcePropertyName;
                string errorMessage = isError ? error.ErrorContent?.ToString() : null;

                // 通知Model（无论是否有错误）
                ErrorHandler?.Invoke(propertyName, errorMessage, isError);
            }
        }

        protected override void OnDetaching()
        {
            Validation.RemoveErrorHandler(AssociatedObject, OnValidationError);
            AssociatedObject.LostFocus -= OnLostFocus;
            base.OnDetaching();
        }
    }

    public class ValueLimitRule : ValidationRule
    {
        public float? MaxValue { get; set; }
        public float? MinValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            float f = 0;
            if (value == null)
                return new ValidationResult(false, "value");
            bool result = float.TryParse(value.ToString(), out f);
            if (!result)
                return new ValidationResult(false, "需要输入值");
            if (MaxValue != null && f > MaxValue)
            {
                return new ValidationResult(false, "输入值超过最大值");
            }
            if (MinValue != null && f < MinValue)
            {
                return new ValidationResult(false, "输入值小于最小值");
            }
            return ValidationResult.ValidResult;
        }
    }
}
