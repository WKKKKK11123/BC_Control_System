using System;
using System.Windows.Markup;

namespace BC_Control_System.Command
{
    public class EnumBindingValueExtension : MarkupExtension
    {
        private Type _enumType;

        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (value != null)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    _enumType = value;
                }
            }
        }

        public int Value;

        public EnumBindingValueExtension() { }

        public EnumBindingValueExtension(Type enumType,int value)
        {
            EnumType = enumType;
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_enumType == null)
                throw new InvalidOperationException("The EnumType must be specified.");

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
                return enumValues;

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            tempArray.GetValue(Value);
            return tempArray;
        }
    }
}

