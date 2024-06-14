using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System;
using Microsoft.WindowsAPICodePack.Shell;
using WindowsExplorer_WPF.Misc;
using WindowsExplorer_WPF.Misc.Helpers;

namespace WindowsExplorer_WPF
{
    public static class WindowHelpers
    {

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        public static T FindChildren<T>(DependencyObject parent, string childName)
           where T : List<DependencyObject>
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            var foundChildren = new List<DependencyObject>();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    var foundChilds = FindChildren<T>(child, childName);

                    // If the child is found, add to the return collection
                    foundChildren.AddRange(foundChilds);
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChildren.Add(child);
                    }
                }
            }

            return (T)foundChildren;
        }
        public static T FindParent<T>(DependencyObject child, string parentName) where T : DependencyObject
        {
            if (child == null) return null;
            FrameworkElement parent = VisualTreeHelper.GetParent(child) as FrameworkElement;
            if (parent != null)
            {
                if (parent.Name.Equals(parentName))
                {
                    return parent as T;
                }
                else
                {
                    return FindParent<T>(parent, parentName);
                }
            }
            return null;
        }
        public static void AssignEventHandlersToAllChildNodes(Control depObj, Action<Control> assignPropsAndEvents, int depth)
        {
            if (depObj == null)
                return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                Control ithChild = VisualTreeHelper.GetChild(depObj, i) as Control;
                assignPropsAndEvents(ithChild);
                if (depth > 0)
                {
                    AssignEventHandlersToAllChildNodes(ithChild, assignPropsAndEvents, depth--);
                }
            }
        }
        public static System.Windows.Media.Imaging.BitmapSource GetBitmapSourceFromPath(string path)
        {
            ShellObject shellObject = ShellObject.FromParsingName(path);
            var bitmapSource = MainViewDataHelpers.Bitmap2BitmapImage(shellObject.Thumbnail.Bitmap);
            return bitmapSource;
        }
    }
}