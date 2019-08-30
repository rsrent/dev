// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ModuleLibraryiOS.Search
{
    [Register ("SearchCell")]
    partial class SearchCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }
        }
    }
}