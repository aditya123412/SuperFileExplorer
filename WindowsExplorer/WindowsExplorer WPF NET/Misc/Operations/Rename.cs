using Classes.Common;
using System;
using System.Collections.Generic;

namespace Classes.Operations
{
    public class Rename : OperationImplementation
    {
        public static readonly string filePath = "File";
        public static readonly string newName = "NewName";
        static OperationDescriptor descriptor = new OperationDescriptor("Rename",
            new Dictionary<string, DataObjectType> { { filePath, DataObjectType.String }, { newName, DataObjectType.String } },
            DataObjectType.String);
        static Rename()
        {
            Register(new Rename());
        }
        public Rename()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var path = OperationExecutor.Evaluate(parameters[filePath]).GetString();
            var newName = OperationExecutor.Evaluate(parameters[Rename.newName]).GetString();
            try
            {
                System.IO.File.Move(path, newName);
                return new DataObject(true);
            }
            catch (Exception)
            {
                return new DataObject(false);
            }
        }
    }
}
