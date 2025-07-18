﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace Azuro.CRM.SharePointIntegration.SharePointView {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ViewsSoap", Namespace="http://schemas.microsoft.com/sharepoint/soap/")]
    public partial class Views : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetViewOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetViewHtmlOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteViewOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddViewOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetViewCollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateViewOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateViewHtmlOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateViewHtml2OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Views() {
            this.Url = global::Azuro.CRM.SharePointIntegration.Properties.Settings.Default.Azuro_CRM_SharePointIntegration_ViewsWS_Views;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetViewCompletedEventHandler GetViewCompleted;
        
        /// <remarks/>
        public event GetViewHtmlCompletedEventHandler GetViewHtmlCompleted;
        
        /// <remarks/>
        public event DeleteViewCompletedEventHandler DeleteViewCompleted;
        
        /// <remarks/>
        public event AddViewCompletedEventHandler AddViewCompleted;
        
        /// <remarks/>
        public event GetViewCollectionCompletedEventHandler GetViewCollectionCompleted;
        
        /// <remarks/>
        public event UpdateViewCompletedEventHandler UpdateViewCompleted;
        
        /// <remarks/>
        public event UpdateViewHtmlCompletedEventHandler UpdateViewHtmlCompleted;
        
        /// <remarks/>
        public event UpdateViewHtml2CompletedEventHandler UpdateViewHtml2Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetView", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetView(string listName, string viewName) {
            object[] results = this.Invoke("GetView", new object[] {
                        listName,
                        viewName});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetViewAsync(string listName, string viewName) {
            this.GetViewAsync(listName, viewName, null);
        }
        
        /// <remarks/>
        public void GetViewAsync(string listName, string viewName, object userState) {
            if ((this.GetViewOperationCompleted == null)) {
                this.GetViewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetViewOperationCompleted);
            }
            this.InvokeAsync("GetView", new object[] {
                        listName,
                        viewName}, this.GetViewOperationCompleted, userState);
        }
        
        private void OnGetViewOperationCompleted(object arg) {
            if ((this.GetViewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetViewCompleted(this, new GetViewCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetViewHtml", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetViewHtml(string listName, string viewName) {
            object[] results = this.Invoke("GetViewHtml", new object[] {
                        listName,
                        viewName});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetViewHtmlAsync(string listName, string viewName) {
            this.GetViewHtmlAsync(listName, viewName, null);
        }
        
        /// <remarks/>
        public void GetViewHtmlAsync(string listName, string viewName, object userState) {
            if ((this.GetViewHtmlOperationCompleted == null)) {
                this.GetViewHtmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetViewHtmlOperationCompleted);
            }
            this.InvokeAsync("GetViewHtml", new object[] {
                        listName,
                        viewName}, this.GetViewHtmlOperationCompleted, userState);
        }
        
        private void OnGetViewHtmlOperationCompleted(object arg) {
            if ((this.GetViewHtmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetViewHtmlCompleted(this, new GetViewHtmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/DeleteView", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteView(string listName, string viewName) {
            this.Invoke("DeleteView", new object[] {
                        listName,
                        viewName});
        }
        
        /// <remarks/>
        public void DeleteViewAsync(string listName, string viewName) {
            this.DeleteViewAsync(listName, viewName, null);
        }
        
        /// <remarks/>
        public void DeleteViewAsync(string listName, string viewName, object userState) {
            if ((this.DeleteViewOperationCompleted == null)) {
                this.DeleteViewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteViewOperationCompleted);
            }
            this.InvokeAsync("DeleteView", new object[] {
                        listName,
                        viewName}, this.DeleteViewOperationCompleted, userState);
        }
        
        private void OnDeleteViewOperationCompleted(object arg) {
            if ((this.DeleteViewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteViewCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/AddView", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode AddView(string listName, string viewName, System.Xml.XmlNode viewFields, System.Xml.XmlNode query, System.Xml.XmlNode rowLimit, string type, bool makeViewDefault) {
            object[] results = this.Invoke("AddView", new object[] {
                        listName,
                        viewName,
                        viewFields,
                        query,
                        rowLimit,
                        type,
                        makeViewDefault});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void AddViewAsync(string listName, string viewName, System.Xml.XmlNode viewFields, System.Xml.XmlNode query, System.Xml.XmlNode rowLimit, string type, bool makeViewDefault) {
            this.AddViewAsync(listName, viewName, viewFields, query, rowLimit, type, makeViewDefault, null);
        }
        
        /// <remarks/>
        public void AddViewAsync(string listName, string viewName, System.Xml.XmlNode viewFields, System.Xml.XmlNode query, System.Xml.XmlNode rowLimit, string type, bool makeViewDefault, object userState) {
            if ((this.AddViewOperationCompleted == null)) {
                this.AddViewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddViewOperationCompleted);
            }
            this.InvokeAsync("AddView", new object[] {
                        listName,
                        viewName,
                        viewFields,
                        query,
                        rowLimit,
                        type,
                        makeViewDefault}, this.AddViewOperationCompleted, userState);
        }
        
        private void OnAddViewOperationCompleted(object arg) {
            if ((this.AddViewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddViewCompleted(this, new AddViewCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetViewCollection", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetViewCollection(string listName) {
            object[] results = this.Invoke("GetViewCollection", new object[] {
                        listName});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetViewCollectionAsync(string listName) {
            this.GetViewCollectionAsync(listName, null);
        }
        
        /// <remarks/>
        public void GetViewCollectionAsync(string listName, object userState) {
            if ((this.GetViewCollectionOperationCompleted == null)) {
                this.GetViewCollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetViewCollectionOperationCompleted);
            }
            this.InvokeAsync("GetViewCollection", new object[] {
                        listName}, this.GetViewCollectionOperationCompleted, userState);
        }
        
        private void OnGetViewCollectionOperationCompleted(object arg) {
            if ((this.GetViewCollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetViewCollectionCompleted(this, new GetViewCollectionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateView", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode UpdateView(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit) {
            object[] results = this.Invoke("UpdateView", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateViewAsync(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit) {
            this.UpdateViewAsync(listName, viewName, viewProperties, query, viewFields, aggregations, formats, rowLimit, null);
        }
        
        /// <remarks/>
        public void UpdateViewAsync(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit, object userState) {
            if ((this.UpdateViewOperationCompleted == null)) {
                this.UpdateViewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateViewOperationCompleted);
            }
            this.InvokeAsync("UpdateView", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit}, this.UpdateViewOperationCompleted, userState);
        }
        
        private void OnUpdateViewOperationCompleted(object arg) {
            if ((this.UpdateViewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateViewCompleted(this, new UpdateViewCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateViewHtml", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode UpdateViewHtml(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode toolbar, System.Xml.XmlNode viewHeader, System.Xml.XmlNode viewBody, System.Xml.XmlNode viewFooter, System.Xml.XmlNode viewEmpty, System.Xml.XmlNode rowLimitExceeded, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit) {
            object[] results = this.Invoke("UpdateViewHtml", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        toolbar,
                        viewHeader,
                        viewBody,
                        viewFooter,
                        viewEmpty,
                        rowLimitExceeded,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateViewHtmlAsync(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode toolbar, System.Xml.XmlNode viewHeader, System.Xml.XmlNode viewBody, System.Xml.XmlNode viewFooter, System.Xml.XmlNode viewEmpty, System.Xml.XmlNode rowLimitExceeded, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit) {
            this.UpdateViewHtmlAsync(listName, viewName, viewProperties, toolbar, viewHeader, viewBody, viewFooter, viewEmpty, rowLimitExceeded, query, viewFields, aggregations, formats, rowLimit, null);
        }
        
        /// <remarks/>
        public void UpdateViewHtmlAsync(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode toolbar, System.Xml.XmlNode viewHeader, System.Xml.XmlNode viewBody, System.Xml.XmlNode viewFooter, System.Xml.XmlNode viewEmpty, System.Xml.XmlNode rowLimitExceeded, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit, object userState) {
            if ((this.UpdateViewHtmlOperationCompleted == null)) {
                this.UpdateViewHtmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateViewHtmlOperationCompleted);
            }
            this.InvokeAsync("UpdateViewHtml", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        toolbar,
                        viewHeader,
                        viewBody,
                        viewFooter,
                        viewEmpty,
                        rowLimitExceeded,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit}, this.UpdateViewHtmlOperationCompleted, userState);
        }
        
        private void OnUpdateViewHtmlOperationCompleted(object arg) {
            if ((this.UpdateViewHtmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateViewHtmlCompleted(this, new UpdateViewHtmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateViewHtml2", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode UpdateViewHtml2(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode toolbar, System.Xml.XmlNode viewHeader, System.Xml.XmlNode viewBody, System.Xml.XmlNode viewFooter, System.Xml.XmlNode viewEmpty, System.Xml.XmlNode rowLimitExceeded, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit, string openApplicationExtension) {
            object[] results = this.Invoke("UpdateViewHtml2", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        toolbar,
                        viewHeader,
                        viewBody,
                        viewFooter,
                        viewEmpty,
                        rowLimitExceeded,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit,
                        openApplicationExtension});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateViewHtml2Async(string listName, string viewName, System.Xml.XmlNode viewProperties, System.Xml.XmlNode toolbar, System.Xml.XmlNode viewHeader, System.Xml.XmlNode viewBody, System.Xml.XmlNode viewFooter, System.Xml.XmlNode viewEmpty, System.Xml.XmlNode rowLimitExceeded, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, System.Xml.XmlNode aggregations, System.Xml.XmlNode formats, System.Xml.XmlNode rowLimit, string openApplicationExtension) {
            this.UpdateViewHtml2Async(listName, viewName, viewProperties, toolbar, viewHeader, viewBody, viewFooter, viewEmpty, rowLimitExceeded, query, viewFields, aggregations, formats, rowLimit, openApplicationExtension, null);
        }
        
        /// <remarks/>
        public void UpdateViewHtml2Async(
                    string listName, 
                    string viewName, 
                    System.Xml.XmlNode viewProperties, 
                    System.Xml.XmlNode toolbar, 
                    System.Xml.XmlNode viewHeader, 
                    System.Xml.XmlNode viewBody, 
                    System.Xml.XmlNode viewFooter, 
                    System.Xml.XmlNode viewEmpty, 
                    System.Xml.XmlNode rowLimitExceeded, 
                    System.Xml.XmlNode query, 
                    System.Xml.XmlNode viewFields, 
                    System.Xml.XmlNode aggregations, 
                    System.Xml.XmlNode formats, 
                    System.Xml.XmlNode rowLimit, 
                    string openApplicationExtension, 
                    object userState) {
            if ((this.UpdateViewHtml2OperationCompleted == null)) {
                this.UpdateViewHtml2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateViewHtml2OperationCompleted);
            }
            this.InvokeAsync("UpdateViewHtml2", new object[] {
                        listName,
                        viewName,
                        viewProperties,
                        toolbar,
                        viewHeader,
                        viewBody,
                        viewFooter,
                        viewEmpty,
                        rowLimitExceeded,
                        query,
                        viewFields,
                        aggregations,
                        formats,
                        rowLimit,
                        openApplicationExtension}, this.UpdateViewHtml2OperationCompleted, userState);
        }
        
        private void OnUpdateViewHtml2OperationCompleted(object arg) {
            if ((this.UpdateViewHtml2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateViewHtml2Completed(this, new UpdateViewHtml2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetViewCompletedEventHandler(object sender, GetViewCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetViewCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetViewCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetViewHtmlCompletedEventHandler(object sender, GetViewHtmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetViewHtmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetViewHtmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void DeleteViewCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void AddViewCompletedEventHandler(object sender, AddViewCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddViewCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddViewCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetViewCollectionCompletedEventHandler(object sender, GetViewCollectionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetViewCollectionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetViewCollectionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateViewCompletedEventHandler(object sender, UpdateViewCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateViewCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateViewCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateViewHtmlCompletedEventHandler(object sender, UpdateViewHtmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateViewHtmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateViewHtmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateViewHtml2CompletedEventHandler(object sender, UpdateViewHtml2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateViewHtml2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateViewHtml2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591