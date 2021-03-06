﻿#pragma checksum "..\..\..\..\Pages\Home\FindCustomer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6A773858E448B1367C7DF087AD2885E5"
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


namespace CashierRegister.Pages.Home {
    
    
    /// <summary>
    /// FindCustomer
    /// </summary>
    public partial class FindCustomer : FirstFloor.ModernUI.Windows.Controls.ModernDialog, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbCustomer;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbPhone;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernButton muiBtnSearch;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtgCustomer;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spPaging;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button muiBtnAdd;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Pages\Home\FindCustomer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button muiBtnClose;
        
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
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/pages/home/findcustomer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Home\FindCustomer.xaml"
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
            
            #line 9 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            ((CashierRegister.Pages.Home.FindCustomer)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ModernDialog_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txbCustomer = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.txbCustomer.KeyDown += new System.Windows.Input.KeyEventHandler(this.txbCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txbPhone = ((System.Windows.Controls.TextBox)(target));
            
            #line 36 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.txbPhone.KeyDown += new System.Windows.Input.KeyEventHandler(this.txbPhone_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.muiBtnSearch = ((FirstFloor.ModernUI.Windows.Controls.ModernButton)(target));
            
            #line 38 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.muiBtnSearch.Click += new System.Windows.RoutedEventHandler(this.muiBtnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dtgCustomer = ((System.Windows.Controls.DataGrid)(target));
            
            #line 45 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.dtgCustomer.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.dtgCustomer_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.spPaging = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.muiBtnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.muiBtnAdd.Click += new System.Windows.RoutedEventHandler(this.muiBtnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.muiBtnClose = ((System.Windows.Controls.Button)(target));
            
            #line 81 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            this.muiBtnClose.Click += new System.Windows.RoutedEventHandler(this.muiBtnClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 60 "..\..\..\..\Pages\Home\FindCustomer.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSelect_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

