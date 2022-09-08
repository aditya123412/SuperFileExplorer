
using Classes.Common;
using Jint;

namespace Classes.Operations
{
    public class ScriptExecutor
    {
        public Engine engine = new Engine(cfg => cfg.AllowClr());
        public ScriptExecutor()
        {
            AddFileSystem();
        }

        public void AddFileSystem()
        {
            engine.SetValue("getFiles", ProjectAll.EvaluateNative);
            engine.SetValue("getFilesAndFolders", ProjectAll.EvaluateNative);
        }
        public void RegisterOperation(OperationImplementation operation)
        {
            engine.SetValue(operation.Descriptor.OperationName, operation.Evaluate);
        }
        public object ExecuteScript(string script, IEnumerable<DataObject> initObjects)
        {
            engine.SetValue("context", "");
            foreach (var initObject in initObjects)
            {
                engine.SetValue(initObject.Name, initObject.Value);
            }
            var result = engine.Execute(script).GetValue("context").ToObject();
            return (List<IDescriptorNode>)result;
        }
        public void SetObject<T>(string objName, T value)
        {
            engine.SetValue(objName, value);
        }
        public object GetObject(string objName)
        {
            return engine.GetValue(objName);
        }
    }
}
