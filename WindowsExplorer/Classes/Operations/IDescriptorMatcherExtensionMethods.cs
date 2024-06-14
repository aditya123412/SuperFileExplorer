using Classes.Common;

namespace Classes.Operations
{
    public interface IDescriptorMatcherExtensionMethods<TKey>
    {
        IDescriptorNode getDescriptorForActualFile(string path);
        TKey getHashCKey(string path);
        IDescriptorNode getNode(TKey hash);
    }
}