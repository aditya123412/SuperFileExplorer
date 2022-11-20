namespace TreeDb.Classes.Services
{
    public class QueryService
    {

        #region CONSTANTS
        public static readonly string PARENT = "parent";
        public static readonly string NAME = "name";
        public static readonly string ATTRIBUTE_QUERY = "@";
        public static readonly string TAG_QUERY = "?";
        public static readonly string PARENT_QUERY = "^";
        public static readonly string FOLDER_SEP = @"\";
        public static readonly string QUERY_SEP = @"\";
        public static readonly string WILD_CARD = @"*";
        public static readonly string QUERY_AS_FOLDER = "[]";
        public static readonly string SAVE_PATH = "save_path";
        public static readonly string SAVE_NAME = "save_save";
        #endregion

        public static string NormalizeQueryToViewReference(string query)
        {
            var lc_query = query.Trim().ToLower();
            lc_query.Replace(Path.DirectorySeparatorChar, '_');
            return lc_query.ToString();
        }

        public static (string command, string remainingQuery) GetFirstCommand(string Query, string QuerySeparator)
        {
            string remainingQuery = default;
            var commands = Query.Split(QuerySeparator);
            if (commands.Length == 1)
            {
                return (commands[0], default);
            }
            remainingQuery = String.Join(QuerySeparator, commands.Skip(1));
            return (commands[0], remainingQuery);
        }

    }
}
