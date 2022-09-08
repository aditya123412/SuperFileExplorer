using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Classes.Common;
using Force.Crc32;

namespace Classes.Operations
{
    public class FilenameDescriptorMatcherExtensionMethods : IDescriptorMatcherExtensionMethods<uint>
    {
        private Dictionary<uint, IDescriptorNode> _descriptorNode;

        public async void getDescriptorsInFolder(string path)
        {
            var serializedJSON = await System.IO.File.ReadAllTextAsync(path);
            _descriptorNode = JsonSerializer.Deserialize<Dictionary<uint, IDescriptorNode>>(serializedJSON);
        }

        public FilenameDescriptorMatcherExtensionMethods()
        {
            _descriptorNode = new Dictionary<uint, IDescriptorNode>();
        }
        public IDescriptorNode getDescriptorForActualFile(string path)
        {
            uint hash = this.getHashCKey(path);
            return getNode(hash);
        }
        public IDescriptorNode getNode(uint hash)
        {
            return _descriptorNode?.GetValueOrDefault(hash, null);
        }
        public uint getHashCKey(string path)
        {
            return Crc32Algorithm.ComputeAndWriteToEnd(System.IO.File.ReadAllBytes(path));
        }

    }
}
