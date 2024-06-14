using Classes.Common;
using System;
using System.Collections.Generic;

namespace Classes.Operations
{
    public class Move : OperationImplementation
    {
        public static readonly string Source = "Source";
        public static readonly string Dest = "Dest";
        static OperationDescriptor descriptor = new OperationDescriptor("Move",
            new Dictionary<string, DataObjectType> { { Source, DataObjectType.String }, { Dest, DataObjectType.String } },
            DataObjectType.Boolean);
        static Move()
        {
            Register(new Move());
        }
        public Move()
        {
            this.Descriptor = descriptor;
        }

        public override DataObject Evaluate(Dictionary<string, OperationExpression> parameters)
        {
            var sourcePath = OperationExecutor.Evaluate(parameters[Source]).GetString();
            var destPath = OperationExecutor.Evaluate(parameters[Dest]).GetString();
            try
            {
                System.IO.File.Move(sourcePath, destPath);
                return new DataObject(true);
            }
            catch (Exception)
            {
                return new DataObject(false);
            }
        }
    }
}
