﻿#pragma checksum "..\..\..\..\Pages\Setting\RestoreGDrive.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "19D5577579EA84AD53F3031150042161"
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
    /// RestoreGDrive
    /// </summary>
    public partial class RestoreGDrive : FirstFloor.ModernUI.Windows.Controls.ModernDialog, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblConfirm;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernProgressRing mpr;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button muiBtnOK;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button muiBtnCancel;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/pages/setting/restoregdrive.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
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
            
            #line 9 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
            ((CashierRegister.Pages.Setting.RestoreGDrive)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ModernDialog_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tblConfirm = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.mpr = ((FirstFloor.ModernUI.Windows.Controls.ModernProgressRing)(target));
            return;
            case 4:
            this.muiBtnOK = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
            this.muiBtnOK.Click += new System.Windows.RoutedEventHandler(this.muiBtnOK_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.muiBtnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\Pages\Setting\RestoreGDrive.xaml"
            this.muiBtnCancel.Click += new System.Windows.RoutedEventHandler(this.muiBtnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tblNotification = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
