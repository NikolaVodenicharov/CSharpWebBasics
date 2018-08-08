namespace SimpleMvc.Framework.Attributes.Validation
{
    using System;

    public class NumberRangeAttribute : PropertyValidationAttribute
    {
        private readonly double minimum;
        private readonly double maximum;

        public NumberRangeAttribute(double minimum, double maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public override bool IsValid(object value)
        {
            double? valueAsDouble = value as double?;

            if (valueAsDouble == null)
            {
                return true;
            }

            return
                valueAsDouble >= this.minimum &&
                valueAsDouble <= this.maximum;
        }
    }
}
