using Classes.Common;
using System.Collections.Generic;

namespace Classes.Operations
{
    public class SubString : OperationImplementation
    {
        public static readonly string STRING = "String";
        public static readonly string START_INDEX = "StartIndex";
        public static readonly string END_INDEX = "EndIndex";
        static OperationDescriptor descriptor = new OperationDescriptor("SubString",
            new Dictionary<string, DataObjectType> {
                { STRING, DataObjectType.String },
                { START_INDEX,DataObjectType.Number } ,
                { END_INDEX,DataObjectType.Number }
            },
            DataObjectType.DescriptorNodeList);
        static SubString()
        {
            Register(new SubString());
        }
        public SubString()
        {
            this.Descriptor = descriptor;
        }
        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            string str = OperationExecutor.operationImplementations[parameters[STRING].OperationName]
                .Evaluate(parameters[STRING].Arguments).GetString();

            int startIndex = ((int)OperationExecutor.operationImplementations[parameters[START_INDEX].OperationName]
                .Evaluate(parameters[START_INDEX].Arguments).GetNumber());
            int endIndex = ((int)OperationExecutor.operationImplementations[parameters[END_INDEX].OperationName]
                .Evaluate(parameters[END_INDEX].Arguments).GetNumber());
            return new DataObject(str.Substring(startIndex, endIndex));
        }
    }
}
