﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECO.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ECO.Resources.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to The section &quot;eco&quot; was not found in .config file..
        /// </summary>
        internal static string CONFIG_NOT_FOUND {
            get {
                return ResourceManager.GetString("CONFIG_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type of the identity attribute is different from the type defined by the entity &apos;{0}&apos;..
        /// </summary>
        internal static string IDENTITY_TYPE_NOT_MATCHED {
            get {
                return ResourceManager.GetString("IDENTITY_TYPE_NOT_MATCHED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The persistent context &apos;{0}&apos; is not registered..
        /// </summary>
        internal static string PERSISTENCE_CONTEXT_NOT_FOUND {
            get {
                return ResourceManager.GetString("PERSISTENCE_CONTEXT_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The persistent unit &apos;{0}&apos; is not registered..
        /// </summary>
        internal static string PERSISTENCE_UNIT_NOT_FOUND {
            get {
                return ResourceManager.GetString("PERSISTENCE_UNIT_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The persistent class &apos;{0}&apos; is not related to any persistence context..
        /// </summary>
        internal static string PERSISTENT_CLASS_NOT_FOUND {
            get {
                return ResourceManager.GetString("PERSISTENT_CLASS_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error during the activation of the type &apos;{0}&apos;..
        /// </summary>
        internal static string TYPE_LOAD_EXCEPTION {
            get {
                return ResourceManager.GetString("TYPE_LOAD_EXCEPTION", resourceCulture);
            }
        }
    }
}
