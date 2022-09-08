using Classes.Common;

namespace Classes.Operations
{
    public class DATE : OperationImplementation
    {
        static OperationDescriptor descriptor = new OperationDescriptor("DATE",
            new Dictionary<string, DataObjectType> { },
            DataObjectType.DateTime);
        static DATE()
        {
            Register(new DATE());
        }
        public DATE()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            if (parameters.Count == 1 && parameters.First().Value == null)
            {
                return new DataObject(DateTime.Parse(parameters.First().Key));
            }
            else
            {
                return new DataObject("Error", DataObjectType.String, parameters.First().Value);
            }
        }
    }
}
