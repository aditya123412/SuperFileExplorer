using Classes.Operations;

namespace Classes
{
    public abstract class IFileSystem
    {
        public List<IFileSystem> Files { get; set; }
        public abstract void SetContext(string path);
        public abstract DataObject Execute(OperationDescriptor opertion);
    }

}