﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.0
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Victop.Frame.DataChannel.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Victop.Frame.DataChannel.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 /*
        ///	Copyright (c) 2004-2011, The Dojo Foundation All Rights Reserved.
        ///	Available via Academic Free License &gt;= 2.1 OR the modified BSD license.
        ///	see: http://dojotoolkit.org/license for details
        ///*/
        ///
        ////*
        ///	This is an optimized version of Dojo, built for deployment and not for
        ///	development. To get sources and documentation, please visit:
        ///
        ///		http://dojotoolkit.org
        ///*/
        ///
        ///(function(
        ///	userConfig,
        ///	defaultConfig
        ///){
        ///	// summary:
        ///	//		This is the &quot;source loader&quot; and is the entry point for Dojo during deve [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string CheckDataAuthorityScript {
            get {
                return ResourceManager.GetString("CheckDataAuthorityScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 var result = (function (p_data, p_path) {
        ///                        if (!p_path || p_path.length === 0) {
        ///                        return null;
        ///                        }
        ///                if (p_data) {
        ///                for (var i = 0; i &lt; p_path.length; i++) {
        ///                if (i % 2 === 0) { //取对象根据对象返回dataArray(path偶数)
        ///                p_data = p_data[p_path[i]];
        ///                if (!p_data) {
        ///                    return null;
        ///                }
        ///                } else if (i % 2 === 1) { //取dataArray中n [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string GetDataByPathScript {
            get {
                return ResourceManager.GetString("GetDataByPathScript", resourceCulture);
            }
        }
    }
}
