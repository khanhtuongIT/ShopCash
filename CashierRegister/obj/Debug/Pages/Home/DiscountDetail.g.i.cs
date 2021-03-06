﻿#pragma checksum "..\..\..\..\Pages\Home\DiscountDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "217F2800B5A635F5BC6ED27F4AB42A8F"
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
    /// DiscountDetail
    /// </summary>
    public partial class DiscountDetail : FirstFloor.ModernUI.Windows.Controls.ModernDialog, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAmount;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPercent;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblAmountPercent;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblDiscountDetailType;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbAmountPercent;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOK;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/pages/home/discountdetail.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
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
            
            #line 9 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            ((CashierRegister.Pages.Home.DiscountDetail)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ModernDialog_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnAmount = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.btnAmount.Click += new System.Windows.RoutedEventHandler(this.btnAmount_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnPercent = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.btnPercent.Click += new System.Windows.RoutedEventHandler(this.btnPercent_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tblAmountPercent = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tblDiscountDetailType = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txbAmountPercent = ((System.Windows.Controls.TextBox)(target));
            
            #line 66 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.txbAmountPercent.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txbAmountPercent_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 66 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.txbAmountPercent.KeyDown += new System.Windows.Input.KeyEventHandler(this.txbAmountPercent_KeyDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnOK = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.btnOK.Click += new System.Windows.RoutedEventHandler(this.btnOK_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\..\Pages\Home\DiscountDetail.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.tblNotification = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

