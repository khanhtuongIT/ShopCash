﻿#pragma checksum "..\..\..\UserControls\UCConnectServer.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E4404EFEE542E2AFA561C93C8F2F9F94"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace CashierRegister.UserControls {
    
    
    /// <summary>
    /// UCConnectServer
    /// </summary>
    public partial class UCConnectServer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spSQLServer;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar pgbSQLServer;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbServerName;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboAuthentication;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbUserName;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwbPassword;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConnect;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDisconnect;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\UserControls\UCConnectServer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblNotificationSQLServer;
        
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
            System.Uri resourceLocater = new System.Uri("/Cash Register;component/usercontrols/ucconnectserver.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\UCConnectServer.xaml"
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
            this.spSQLServer = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.pgbSQLServer = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 3:
            this.txbServerName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cboAuthentication = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.txbUserName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.pwbPassword = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 7:
            this.btnConnect = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.btnDisconnect = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.tblNotificationSQLServer = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
