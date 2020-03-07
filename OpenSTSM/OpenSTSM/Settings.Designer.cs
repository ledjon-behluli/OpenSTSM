﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenSTSM {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MiddlePointDistanceThreshold {
            get {
                return ((int)(this["MiddlePointDistanceThreshold"]));
            }
            set {
                this["MiddlePointDistanceThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("80")]
        public int NumberOfRegionProposals {
            get {
                return ((int)(this["NumberOfRegionProposals"]));
            }
            set {
                this["NumberOfRegionProposals"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int OuterSelectionThreshold {
            get {
                return ((int)(this["OuterSelectionThreshold"]));
            }
            set {
                this["OuterSelectionThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int DecimalPointProbabilityRounding {
            get {
                return ((int)(this["DecimalPointProbabilityRounding"]));
            }
            set {
                this["DecimalPointProbabilityRounding"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int RegionProposalsMultiplicity {
            get {
                return ((int)(this["RegionProposalsMultiplicity"]));
            }
            set {
                this["RegionProposalsMultiplicity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int SpatialDistanceOfCoordinatePointsThreshold {
            get {
                return ((int)(this["SpatialDistanceOfCoordinatePointsThreshold"]));
            }
            set {
                this["SpatialDistanceOfCoordinatePointsThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://github.com/ledjon-behluli/OpenSTSM")]
        public string GitHubUri {
            get {
                return ((string)(this["GitHubUri"]));
            }
            set {
                this["GitHubUri"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20.00")]
        public decimal LeafProbabilityThreshold {
            get {
                return ((decimal)(this["LeafProbabilityThreshold"]));
            }
            set {
                this["LeafProbabilityThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UseGpuAcceleration {
            get {
                return ((bool)(this["UseGpuAcceleration"]));
            }
            set {
                this["UseGpuAcceleration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int NumberOfResultsPerElement {
            get {
                return ((int)(this["NumberOfResultsPerElement"]));
            }
            set {
                this["NumberOfResultsPerElement"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string NN_ModelPath {
            get {
                return ((string)(this["NN_ModelPath"]));
            }
            set {
                this["NN_ModelPath"] = value;
            }
        }
    }
}
