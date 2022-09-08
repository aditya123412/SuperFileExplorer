using System.Windows.Forms;

namespace FileServerWebAPI.Services
{
    public class FileSystemService
    {
        TreeNode root = new TreeNode("Desktop");
        TreeNode doc = new TreeNode("My Documents");
        TreeNode comp = new TreeNode("My Computer ");
        TreeNode drivenode;
        TreeNode filenode;
        DirectoryInfo dir;
        string path = "";
        private void GetDrives()
        {
            DriveInfo[] drive = DriveInfo.GetDrives();
            foreach (DriveInfo d in drive)
            {
                drivenode = new TreeNode(d.Name);
                dir = d.RootDirectory;
                comp.Nodes.Add(drivenode);
                switch (d.DriveType)
                {
                    case DriveType.CDRom:
                        drivenode.ImageIndex = 8;
                        break;
                    case DriveType.Fixed:
                        drivenode.ImageIndex = 1;
                        break;
                    case DriveType.Removable:
                        drivenode.ImageIndex = 1;
                        break;
                    case DriveType.NoRootDirectory:

                        drivenode.ImageIndex = 5;
                        break;
                    case DriveType.Network:
                        drivenode.ImageIndex = 1;
                        break;
                    case DriveType.Unknown:
                        drivenode.ImageIndex = 2;
                        break;
                }
                getFilesAndDir(drivenode, dir);
            }
        }
        private void getFilesAndDir(TreeNode node, DirectoryInfo dirname)
        {
            try
            {
                foreach (FileInfo fi in dirname.GetFiles())
                {
                    filenode = new TreeNode(fi.Name);
                    filenode.Name = fi.FullName;
                    getFileExtension(filenode.Name);
                    node.Nodes.Add(filenode);
                }
                try
                {
                    foreach (DirectoryInfo di in dirname.GetDirectories())
                    {
                        TreeNode dirnode = new TreeNode(di.Name);
                        dirnode.ImageIndex = 2;
                        dirnode.Name = di.FullName;
                        node.Nodes.Add(dirnode);
                        getFilesAndDir(dirnode, di); //Recursive Functioning
                    }
                }
                catch (Exception e1)
                {
                }
            }
            catch (Exception e1)
            {
            }
        }
        private void getFileExtension(string filename)
        {
            filenode = new TreeNode();
            switch (Path.GetExtension(filename))
            {
                case ".txt":
                case ".rtf":
                    filenode.ImageIndex = 6;
                    break;
                case ".doc":
                case ".docx":
                    filenode.ImageIndex = 0;
                    break;
                case ".html":
                case ".htm":
                    filenode.ImageIndex = 3;
                    break;
                case ".rar":
                case ".zip":
                    filenode.ImageIndex = 9;
                    break;
                case ".java":
                    filenode.ImageIndex = 10;
                    break;
                default:
                    filenode.ImageIndex = 7;
                    break;
            }
        }
    }
}
