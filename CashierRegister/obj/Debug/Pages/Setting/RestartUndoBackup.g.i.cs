﻿#pragma checksum "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5E0918D1CE9F05D9AC98745EF40B7D1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CashierRegister.Pages.Setting {
    
    
    /// <summary>
    /// RestartUndoBackup
    /// </summary>
    public partial class RestartUndoBackup : FirstFloor.ModernUI.Windows.Controls.ModernDialog, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRestart;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernProgressRing mpr;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUndo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblNotification;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/pages/setting/restartundobackup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnRestart = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
            this.btnRestart.Click += new System.Windows.RoutedEventHandler(this.btnRestart_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.mpr = ((FirstFloor.ModernUI.Windows.Controls.ModernProgressRing)(target));
            return;
            case 3:
            this.btnUndo = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\Pages\Setting\RestartUndoBackup.xaml"
            this.btnUndo.Click += new System.Windows.RoutedEventHandler(this.btnUndo_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tblNotification = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

