//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "11.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Event_Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Event_Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Event_Messages", global::System.Reflection.Assembly.Load("App_GlobalResources"));
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
        ///   Looks up a localized string similar to The description is required..
        /// </summary>
        internal static string DESCRIPTION_REQUIRED {
            get {
                return ResourceManager.GetString("DESCRIPTION_REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The description is too long (max 1000 char)..
        /// </summary>
        internal static string DESCRIPTION_TOO_LONG {
            get {
                return ResourceManager.GetString("DESCRIPTION_TOO_LONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The end date is required..
        /// </summary>
        internal static string ENDDATE_REQUIRED {
            get {
                return ResourceManager.GetString("ENDDATE_REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name is required..
        /// </summary>
        internal static string NAME_REQUIRED {
            get {
                return ResourceManager.GetString("NAME_REQUIRED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name is too long (max 50 char)..
        /// </summary>
        internal static string NAME_TOO_LONG {
            get {
                return ResourceManager.GetString("NAME_TOO_LONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The end date must be greater than the start date..
        /// </summary>
        internal static string STARTDATE_GREATER_ENDDATE {
            get {
                return ResourceManager.GetString("STARTDATE_GREATER_ENDDATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The start date is required..
        /// </summary>
        internal static string STARTDATE_REQUIRED {
            get {
                return ResourceManager.GetString("STARTDATE_REQUIRED", resourceCulture);
            }
        }
    }
}
