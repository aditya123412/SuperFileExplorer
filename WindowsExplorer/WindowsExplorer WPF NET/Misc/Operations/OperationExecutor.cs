using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes.Operations
{
    public class OperationExecutor
    {
        public static Dictionary<string, OperationDescriptor> operations = new Dictionary<string, OperationDescriptor>();
        public static Dictionary<string, OperationImplementation> operationImplementations = new Dictionary<string, OperationImplementation>();

        public OperationExecutor()
        {

        }

        public void RegisterAllImplementations()
        {
            var implementations = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(OperationImplementation).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select<Type, object>((type, outputObject) =>
                 {
                     type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                     return null;
                 });
        }

        public static void RegisterOperationDescription(OperationDescriptor Operation)
        {
            var operationName = Operation.OperationName;
            if (operations.ContainsKey(operationName.ToLower()))
            {
                operations.Add(operationName.ToLower(), Operation);
            }
        }
        public static void RegisterOperationImplementation(OperationImplementation Operation)
        {
            var operationName = Operation.Descriptor.OperationName;
            if (!operationImplementations.ContainsKey(operationName.ToLower()))
            {
                operationImplementations.Add(operationName.ToLower(), Operation);
            }
        }

        public static OperationImplementation getImplementation(string name)
        {
            return operationImplementations[name.ToLower()];
        }

        public static DataObject Evaluate(OperationExpression Operations)
        {
            return operationImplementations[Operations.OperationName].Evaluate(Operations.Arguments);
        }

        //public void ExecuteScript(string scrip)
        //{
        //    ScriptEngine engine = new ScriptEngine();
        //}
    }
}
