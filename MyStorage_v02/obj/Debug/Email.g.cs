﻿#pragma checksum "..\..\Email.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AFAD09145AD35D708E9495BC47C03BCCF33E1A0BFF9198B199554D29E254E80F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MyStorage_v02;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace MyStorage_v02 {
    
    
    /// <summary>
    /// Email
    /// </summary>
    public partial class Email : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 93 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MyStorage_v02.LoadControl load_img;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ToolBar;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon Plg;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image line;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image cross;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbCode;
        
        #line default
        #line hidden
        
        
        #line 169 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txt_validation;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\Email.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSendAgain;
        
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
            System.Uri resourceLocater = new System.Uri("/MyStorage_v02;component/email.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Email.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 15 "..\..\Email.xaml"
            ((MyStorage_v02.Email)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.Window_SizeChanged);
            
            #line default
            #line hidden
            
            #line 15 "..\..\Email.xaml"
            ((MyStorage_v02.Email)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.load_img = ((MyStorage_v02.LoadControl)(target));
            return;
            case 3:
            this.ToolBar = ((System.Windows.Controls.Grid)(target));
            
            #line 99 "..\..\Email.xaml"
            this.ToolBar.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ToolBar_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Plg = ((System.Windows.Shapes.Polygon)(target));
            return;
            case 5:
            this.line = ((System.Windows.Controls.Image)(target));
            
            #line 102 "..\..\Email.xaml"
            this.line.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.line_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cross = ((System.Windows.Controls.Image)(target));
            
            #line 121 "..\..\Email.xaml"
            this.cross.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.cross_MouseDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.tbCode = ((System.Windows.Controls.TextBox)(target));
            
            #line 155 "..\..\Email.xaml"
            this.tbCode.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.tbCode_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txt_validation = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.btnSendAgain = ((System.Windows.Controls.Button)(target));
            
            #line 172 "..\..\Email.xaml"
            this.btnSendAgain.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 173 "..\..\Email.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

