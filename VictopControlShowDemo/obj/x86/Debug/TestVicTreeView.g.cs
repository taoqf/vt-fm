﻿#pragma checksum "..\..\..\TestVicTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4BD33CF3C78EC421E97E0A1FA7D52001"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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
using Victop.Wpf.Controls;


namespace VictopControlShowDemo {
    
    
    /// <summary>
    /// TestVicTreeView
    /// </summary>
    public partial class TestVicTreeView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\TestVicTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddBrother;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\TestVicTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddChildren;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\TestVicTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Victop.Wpf.Controls.VicTreeView tree;
        
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
            System.Uri resourceLocater = new System.Uri("/VictopControlShowDemo;component/testvictreeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\TestVicTreeView.xaml"
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
            this.btnAddBrother = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\TestVicTreeView.xaml"
            this.btnAddBrother.Click += new System.Windows.RoutedEventHandler(this.btnAddBrother_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnAddChildren = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\TestVicTreeView.xaml"
            this.btnAddChildren.Click += new System.Windows.RoutedEventHandler(this.btnAddChildren_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tree = ((Victop.Wpf.Controls.VicTreeView)(target));
            
            #line 15 "..\..\..\TestVicTreeView.xaml"
            this.tree.Loaded += new System.Windows.RoutedEventHandler(this.tree_Loaded);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\TestVicTreeView.xaml"
            this.tree.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.tree_SelectedItemChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

