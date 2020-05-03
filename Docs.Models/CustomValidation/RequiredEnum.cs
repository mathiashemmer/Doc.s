using System;
using System.ComponentModel.DataAnnotations;

namespace Docs.Models.CustomValidation
{
    public class RequiredEnumAttribute : RequiredAttribute
    {

        int minValue { get; set; }
        int maxValue { get; set; }
        bool hasRange { get; set; } = false;
        public RequiredEnumAttribute() : base() { }
        public RequiredEnumAttribute(int minValue, int maxValue) : base() {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.hasRange = true;
        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var type = value.GetType();
            if (!type.IsEnum && Enum.IsDefined(type, value)) return false;

            if (hasRange)
            {
                return ((int)value >= this.minValue && (int)value <= this.maxValue);
            }
            return true;
        }
    }
}
