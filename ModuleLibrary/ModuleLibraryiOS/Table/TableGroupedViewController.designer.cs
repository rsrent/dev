// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    [Register ("TableGroupedViewController")]
    partial class TableGroupedViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableViewGrouped { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TableViewGrouped != null) {
                TableViewGrouped.Dispose ();
                TableViewGrouped = null;
            }
        }
    }
}