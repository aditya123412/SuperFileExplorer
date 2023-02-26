namespace WindowsExplorer_WPF_NET.Misc.Data
{
    public class QueryObject
    {
        public FieldName FieldName { get; set; }
        public FieldType FieldType { get; set; }
    }
    public enum FieldName { Name, Created, Extension, DefaultIcon, Thumbnail, Size, FullPath,Location, IsFolder, ThumbnailLoaded, SupportsThumbnail, LastModified, LastAccessed, LastUpdatedBy, LastAccessedBy, Type }
    public enum FieldType { String, Number, DateTime, Picture, Type, File, Folder }
}