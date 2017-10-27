﻿#pragma checksum "..\..\..\..\Pages\Setting\Currency.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7DAE75A8B2E594155849C2432B05708F"
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
    /// Currency
    /// </summary>
    public partial class Currency : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 10 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernProgressRing mpr;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grContent;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernButton muiBtnDelete;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernButton muiBtnAdd;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblTotal;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FirstFloor.ModernUI.Windows.Controls.ModernButton muibtnGetCurrent;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtgCurrency;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Pages\Setting\Currency.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkAll;
        
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
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/pages/setting/currency.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Setting\Currency.xaml"
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
            
            #line 8 "..\..\..\..\Pages\Setting\Currency.xaml"
            ((CashierRegister.Pages.Setting.Currency)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.mpr = ((FirstFloor.ModernUI.Windows.Controls.ModernProgressRing)(target));
            return;
            case 3:
            this.grContent = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.muiBtnDelete = ((FirstFloor.ModernUI.Windows.Controls.ModernButton)(target));
            
            #line 24 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.muiBtnDelete.Click += new System.Windows.RoutedEventHandler(this.muiBtnDelete_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.muiBtnAdd = ((FirstFloor.ModernUI.Windows.Controls.ModernButton)(target));
            
            #line 29 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.muiBtnAdd.Click += new System.Windows.RoutedEventHandler(this.muiBtnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tblTotal = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.muibtnGetCurrent = ((FirstFloor.ModernUI.Windows.Controls.ModernButton)(target));
            
            #line 36 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.muibtnGetCurrent.Click += new System.Windows.RoutedEventHandler(this.muibtnGetCurrent_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.dtgCurrency = ((System.Windows.Controls.DataGrid)(target));
            
            #line 43 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.dtgCurrency.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.dtgCurrency_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.chkAll = ((System.Windows.Controls.CheckBox)(target));
            
            #line 47 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.chkAll.Checked += new System.Windows.RoutedEventHandler(this.chkAll_Checked);
            
            #line default
            #line hidden
            
            #line 47 "..\..\..\..\Pages\Setting\Currency.xaml"
            this.chkAll.Unchecked += new System.Windows.RoutedEventHandler(this.chkAll_Unchecked);
            
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
            case 10:
            
            #line 55 "..\..\..\..\Pages\Setting\Currency.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.chkCheckDelete_Checked);
            
            #line default
            #line hidden
            
            #line 55 "..\..\..\..\Pages\Setting\Currency.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.chkCheckDelete_Unchecked);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 125 "..\..\..\..\Pages\Setting\Currency.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnEdit_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
