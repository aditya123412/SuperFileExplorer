using Classes.Common;
using System;
using System.Collections.Generic;

namespace Classes.Operations
{
    public class Delete : OperationImplementation
    {
        public static readonly string File = "File";
        public static readonly string DELETE = "Delete";
        static OperationDescriptor descriptor = new OperationDescriptor(DELETE,
            new Dictionary<string, DataObjectType> { { File, DataObjectType.SingleFile } },
            DataObjectType.String);
        static Delete()
        {
            Register(new Delete());
        }
        public Delete()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var fileObject = OperationExecutor.Evaluate(parameters[File]).ToString();
            
            try
            {
                System.IO.File.Delete(fileObject);
                return new DataObject(true);
            }
            catch (Exception)
            {
                System.IO.Directory.Delete(fileObject);
                return new DataObject(true);
            }
            return new DataObject(false);
        }
    }
}
