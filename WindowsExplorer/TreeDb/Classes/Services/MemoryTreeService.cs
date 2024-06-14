using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeDb.Classes;
using TreeDb.Classes.Persistence;
using TreeDb.Classes.MemoryDataStructures;
using System.Collections;

namespace TreeDb.Classes.Services
{
    public class MemoryTreeService
    {
        ViewFetchingService viewFetchingService;
        RecordsService recordsService;
        Queue<QueryReferenceMapping> queryReferenceMappings;
        FullNodeView rootNode;

        const string ROOT = "ROOT";

        public MemoryTreeService(ViewFetchingService viewFetchingService, RecordsService recordsService, int size)
        {
            this.viewFetchingService = viewFetchingService;
            this.recordsService = recordsService;
            queryReferenceMappings = new Queue<QueryReferenceMapping>(size);
            if (viewFetchingService.IfViewExistsForReference(ROOT))
            {
                rootNode = viewFetchingService.GetView(ROOT, null);
            }
            else
            {
                rootNode = new FullNodeView("root", null);
                viewFetchingService.SaveView(rootNode, ROOT);
            }
        }

        public TypeFileRecord Navigate(string query)
        {
            string reference = "";
            try
            {
                //Try Get from Tree view in memory. Works as Cache
                var res = rootNode.Query(query).First();
                if (res != null)
                    return res;
                //Try Get from this service
                if (queryReferenceMappings.Any(map => map.NormalizedQuery.Equals(QueryService.NormalizeQueryToViewReference(query))))
                {
                    reference = queryReferenceMappings.First(map => map.NormalizedQuery.Equals(QueryService.NormalizeQueryToViewReference(query))).Reference;
                    return viewFetchingService.GetView(reference, null);
                }
                throw new TableNotFoundException();
            }
            catch (Exception e)
            {

                throw new TableNotFoundException(e);
            }
        }
        public async void CreateView(string query)
        {
            rootNode.PutViewRecordRecursive(query, true);
        }

        public FullNodeView GetFromReference(string reference, FullNodeView parent)
        {
            return viewFetchingService.GetView(reference, parent);
        }
    }
}
