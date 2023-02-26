using Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsExplorer_WPF.Misc;

namespace Classes.Operations
{
    public class DataObject
    {
        public DataObject(string name, DataObjectType type, Object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }
        public DataObject(string value, string name = "value")
        {
            Type = DataObjectType.String;
            Name = name;
            Value = value;
        }
        public DataObject(float value, string name = "value")
        {
            Type = DataObjectType.Number;
            Name = name;
            Value = value;
        }
        public DataObject(bool value, string name = "value")
        {
            Type = DataObjectType.Boolean;
            Name = name;
            Value = value;
        }
        public DataObject(Object value, string name = "value")
        {
            Type = DataObjectType.BLOB;
            Name = name;
            Value = value;
        }
        public DataObject(FFBase singleFileOrFolder, string name = "value")
        {
            Type = DataObjectType.SingleFile;
            Name = name;
            Value = singleFileOrFolder;
        }
        public DataObject(List<FFBase> descriptorList, string name = "value")
        {
            Type = DataObjectType.DescriptorNodeList;
            Name = name;
            Value = descriptorList;
        }
        public DataObject(ICollection<string> stringList, string name = "value")
        {
            Type = DataObjectType.StringList;
            Name = name;
            Value = stringList;
        }
        public DataObject(Dictionary<string, List<FFBase>> fileGroupsList, string name = "value")
        {
            Type = DataObjectType.FileGroupsList;
            Name = name;
            Value = fileGroupsList;
        }
        public DataObject(DateTime dateTime, string name = "value")
        {
            Type = DataObjectType.DateTime;
            Name = name;
            Value = dateTime;
        }
        public DataObjectType Type { get; set; }
        public string Name { get; set; }
        public Object Value { get; set; }
        public static List<FFBase> NewFileList() => new List<FFBase>();
        public static Dictionary<string, List<FFBase>> NewFileGroup() => new Dictionary<string, List<FFBase>>();
        public static List<(string, string)> NewOperationResultList() => new List<(string, string)>();

        #region Getters
        public List<FFBase> GetDescriptorNodes() => (List<FFBase>)EvaluateExpression();
        public FFBase GetSingleFile() => (FFBase)EvaluateExpression();
        public Dictionary<string, List<FFBase>> GetFileGroupsList() => (Dictionary<string, List<FFBase>>)EvaluateExpression();
        public bool GetBoolean() => (bool)EvaluateExpression();
        public string GetString() => (string)EvaluateExpression();
        public DateTime GetDateTime() => (DateTime)EvaluateExpression();
        public string[] GetStringList() => (string[])EvaluateExpression();
        public float GetNumber() => (float)EvaluateExpression();
        public OperationExpression GetExpression() => (OperationExpression)EvaluateExpression();
        public Object GetBLOB() => EvaluateExpression();
        #endregion
        public Object EvaluateExpression()
        {
            if (Type == DataObjectType.Expression)
            {
                var expression = Value as OperationExpression;
                return OperationExecutor.operationImplementations[expression.OperationName].Evaluate(expression.Arguments).Value;
            }
            else
            {
                return Value;
            }
        }
    }
    public enum DataObjectType
    {
        DescriptorNodeList = 0,
        OperationResultList = 1,
        FileGroupsList = 2,
        StringList = 3,
        String = 4,
        Number = 5,
        Boolean = 6,
        DateTime = 7,
        none = 8,
        SingleFile = 9,
        Expression = 10,
        BLOB = 11
    }
}
