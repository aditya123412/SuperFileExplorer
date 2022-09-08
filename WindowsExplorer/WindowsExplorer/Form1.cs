using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsExplorer
{
    public partial class Form1 : Form
    {
        TreeNode root = new TreeNode("Desktop");
        TreeNode doc = new TreeNode("My Documents");
        TreeNode comp = new TreeNode("My Computer ");
        TreeNode drivenode;
        TreeNode filenode;         
        DirectoryInfo dir;
        string path = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.LabelEdit = true;
            listView1.FullRowSelect = true;
            listView1.Sorting = SortOrder.Ascending;            
            treeView1.Nodes.Add(root);
            doc.ImageIndex = 5;
            comp.ImageIndex = 4;
            treeView1.Nodes.Add(doc);
            treeView1.Nodes.Add(comp);            
            GetDrives();
        }
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
                        getFilesAndDir(dirnode, di);
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
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                TreeNode selectednode = e.Node;
                treeView1.SelectedNode.ImageIndex = e.Node.ImageIndex;
                selectednode.Expand();
                textBox1.Text = selectednode.FullPath;
                if (selectednode.Nodes.Count > 0)
                {
                    foreach (TreeNode n in selectednode.Nodes)
                    {
                        ListViewItem lst = new ListViewItem(n.Text, n.ImageIndex);
                        lst.Name = n.FullPath.Substring(13);                        
                        listView1.Items.Add(lst);
                    }
                }
                else
                {
                    listView1.Items.Add(selectednode.FullPath, selectednode.Text, selectednode.ImageIndex);
                }
            }
            catch (Exception e1)
            {
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected==true)
                {
                        path = listView1.Items[i].Name;
                        textBox1.Text = path;   
                        listView1.Items.Clear();
                        LoadFilesAndDir(path);
                }
            }
        }
        private void LoadFilesAndDir(string address)
        {            
            DirectoryInfo di = new DirectoryInfo(address);
            try
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    listView1.Items.Add(fi.Name, filenode.ImageIndex);
                }
                try
                {
                    foreach(DirectoryInfo listd in di.GetDirectories())
                    {
                        listView1.Items.Add(listd.FullName,listd.Name, 2);
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
    }
}