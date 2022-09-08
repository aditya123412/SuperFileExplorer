using Classes.Common;
using Classes.Operations;
using Microsoft.AspNetCore.Mvc;

namespace FileServerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemController : ControllerBase
    {
        OperationExecutor executor = new OperationExecutor();
        ScriptExecutor scriptExecutor = new ScriptExecutor();

        private readonly ILogger<FileSystemController> _logger;

        public FileSystemController(ILogger<FileSystemController> logger)
        {
            _logger = logger;
            scriptExecutor.AddFileSystem();
        }

        //[HttpPost(Name = "ExecuteQuery")]
        //public DataObject ExecuteQuery(OperationExpression Operations)
        //{
        //    return OperationExecutor.operationImplementations[Operations.OperationName].Evaluate(Operations.Arguments);
        //}

        [HttpPost(Name = "ExecuteScript")]
        public object ExecuteScript(string script)
        {
            return ScriptExecutor.ExecuteScript(script, new DataObject[] { });
        }
    }
}