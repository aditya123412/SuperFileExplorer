using Classes.Common;

namespace Classes.Operations
{
    public class Number : OperationImplementation
    {
        static OperationDescriptor descriptor = new OperationDescriptor("NUMBER",
            new Dictionary<string, DataObjectType> { },
            DataObjectType.Number);
        static Number()
        {
            Register(new Number());
        }
        public Number()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            if (parameters.Count == 1 && parameters.First().Value == null)
            {
                return new DataObject(float.Parse(parameters.First().Key));
            }
            else
            {
                return new DataObject("Error", DataObjectType.String, parameters.First().Value);
            }
        }
    }
}
