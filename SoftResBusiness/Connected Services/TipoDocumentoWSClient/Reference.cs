﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoftResBusiness.TipoDocumentoWSClient {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://services.softres.pucp.edu.pe/")]
    public partial class IOException : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://services.softres.pucp.edu.pe/")]
    public partial class tipoDocumentoDTO : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int idTipoDocumentoField;
        
        private bool idTipoDocumentoFieldSpecified;
        
        private string nombreField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int idTipoDocumento {
            get {
                return this.idTipoDocumentoField;
            }
            set {
                this.idTipoDocumentoField = value;
                this.RaisePropertyChanged("idTipoDocumento");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idTipoDocumentoSpecified {
            get {
                return this.idTipoDocumentoFieldSpecified;
            }
            set {
                this.idTipoDocumentoFieldSpecified = value;
                this.RaisePropertyChanged("idTipoDocumentoSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
                this.RaisePropertyChanged("nombre");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://services.softres.pucp.edu.pe/")]
    public partial class InterruptedException : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://services.softres.pucp.edu.pe/", ConfigurationName="TipoDocumentoWSClient.TipoDocumento")]
    public interface TipoDocumento {
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/eliminarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/eliminarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.IOException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/eliminar/Fault/IOException", Name="IOException")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.InterruptedException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/eliminar/Fault/InterruptedExcep" +
            "tion", Name="InterruptedException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftResBusiness.TipoDocumentoWSClient.eliminarResponse eliminar(SoftResBusiness.TipoDocumentoWSClient.eliminarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/eliminarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/eliminarResponse")]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.eliminarResponse> eliminarAsync(SoftResBusiness.TipoDocumentoWSClient.eliminarRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorIdRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorIdResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.IOException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorId/Fault/IOException", Name="IOException")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.InterruptedException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorId/Fault/InterruptedE" +
            "xception", Name="InterruptedException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse obtenerPorId(SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorIdRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/obtenerPorIdResponse")]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse> obtenerPorIdAsync(SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/modificarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/modificarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.IOException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/modificar/Fault/IOException", Name="IOException")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.InterruptedException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/modificar/Fault/InterruptedExce" +
            "ption", Name="InterruptedException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftResBusiness.TipoDocumentoWSClient.modificarResponse modificar(SoftResBusiness.TipoDocumentoWSClient.modificarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/modificarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/modificarResponse")]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.modificarResponse> modificarAsync(SoftResBusiness.TipoDocumentoWSClient.modificarRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/insertarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/insertarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.IOException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/insertar/Fault/IOException", Name="IOException")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.InterruptedException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/insertar/Fault/InterruptedExcep" +
            "tion", Name="InterruptedException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftResBusiness.TipoDocumentoWSClient.insertarResponse insertar(SoftResBusiness.TipoDocumentoWSClient.insertarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/insertarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/insertarResponse")]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.insertarResponse> insertarAsync(SoftResBusiness.TipoDocumentoWSClient.insertarRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/listarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/listarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.IOException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/listar/Fault/IOException", Name="IOException")]
        [System.ServiceModel.FaultContractAttribute(typeof(SoftResBusiness.TipoDocumentoWSClient.InterruptedException), Action="http://services.softres.pucp.edu.pe/TipoDocumento/listar/Fault/InterruptedExcepti" +
            "on", Name="InterruptedException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftResBusiness.TipoDocumentoWSClient.listarResponse listar(SoftResBusiness.TipoDocumentoWSClient.listarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://services.softres.pucp.edu.pe/TipoDocumento/listarRequest", ReplyAction="http://services.softres.pucp.edu.pe/TipoDocumento/listarResponse")]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.listarResponse> listarAsync(SoftResBusiness.TipoDocumentoWSClient.listarRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="eliminar", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class eliminarRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0;
        
        public eliminarRequest() {
        }
        
        public eliminarRequest(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="eliminarResponse", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class eliminarResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int @return;
        
        public eliminarResponse() {
        }
        
        public eliminarResponse(int @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtenerPorId", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class obtenerPorIdRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int arg0;
        
        public obtenerPorIdRequest() {
        }
        
        public obtenerPorIdRequest(int arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtenerPorIdResponse", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class obtenerPorIdResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO @return;
        
        public obtenerPorIdResponse() {
        }
        
        public obtenerPorIdResponse(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="modificar", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class modificarRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0;
        
        public modificarRequest() {
        }
        
        public modificarRequest(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="modificarResponse", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class modificarResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int @return;
        
        public modificarResponse() {
        }
        
        public modificarResponse(int @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="insertar", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class insertarRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0;
        
        public insertarRequest() {
        }
        
        public insertarRequest(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="insertarResponse", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class insertarResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int @return;
        
        public insertarResponse() {
        }
        
        public insertarResponse(int @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="listar", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class listarRequest {
        
        public listarRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="listarResponse", WrapperNamespace="http://services.softres.pucp.edu.pe/", IsWrapped=true)]
    public partial class listarResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://services.softres.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO[] @return;
        
        public listarResponse() {
        }
        
        public listarResponse(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO[] @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TipoDocumentoChannel : SoftResBusiness.TipoDocumentoWSClient.TipoDocumento, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TipoDocumentoClient : System.ServiceModel.ClientBase<SoftResBusiness.TipoDocumentoWSClient.TipoDocumento>, SoftResBusiness.TipoDocumentoWSClient.TipoDocumento {
        
        public TipoDocumentoClient() {
        }
        
        public TipoDocumentoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TipoDocumentoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TipoDocumentoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TipoDocumentoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftResBusiness.TipoDocumentoWSClient.eliminarResponse SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.eliminar(SoftResBusiness.TipoDocumentoWSClient.eliminarRequest request) {
            return base.Channel.eliminar(request);
        }
        
        public int eliminar(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.eliminarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.eliminarRequest();
            inValue.arg0 = arg0;
            SoftResBusiness.TipoDocumentoWSClient.eliminarResponse retVal = ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).eliminar(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.eliminarResponse> SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.eliminarAsync(SoftResBusiness.TipoDocumentoWSClient.eliminarRequest request) {
            return base.Channel.eliminarAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.eliminarResponse> eliminarAsync(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.eliminarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.eliminarRequest();
            inValue.arg0 = arg0;
            return ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).eliminarAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.obtenerPorId(SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest request) {
            return base.Channel.obtenerPorId(request);
        }
        
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO obtenerPorId(int arg0) {
            SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest();
            inValue.arg0 = arg0;
            SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse retVal = ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).obtenerPorId(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse> SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.obtenerPorIdAsync(SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest request) {
            return base.Channel.obtenerPorIdAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdResponse> obtenerPorIdAsync(int arg0) {
            SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.obtenerPorIdRequest();
            inValue.arg0 = arg0;
            return ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).obtenerPorIdAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftResBusiness.TipoDocumentoWSClient.modificarResponse SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.modificar(SoftResBusiness.TipoDocumentoWSClient.modificarRequest request) {
            return base.Channel.modificar(request);
        }
        
        public int modificar(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.modificarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.modificarRequest();
            inValue.arg0 = arg0;
            SoftResBusiness.TipoDocumentoWSClient.modificarResponse retVal = ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).modificar(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.modificarResponse> SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.modificarAsync(SoftResBusiness.TipoDocumentoWSClient.modificarRequest request) {
            return base.Channel.modificarAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.modificarResponse> modificarAsync(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.modificarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.modificarRequest();
            inValue.arg0 = arg0;
            return ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).modificarAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftResBusiness.TipoDocumentoWSClient.insertarResponse SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.insertar(SoftResBusiness.TipoDocumentoWSClient.insertarRequest request) {
            return base.Channel.insertar(request);
        }
        
        public int insertar(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.insertarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.insertarRequest();
            inValue.arg0 = arg0;
            SoftResBusiness.TipoDocumentoWSClient.insertarResponse retVal = ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).insertar(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.insertarResponse> SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.insertarAsync(SoftResBusiness.TipoDocumentoWSClient.insertarRequest request) {
            return base.Channel.insertarAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.insertarResponse> insertarAsync(SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO arg0) {
            SoftResBusiness.TipoDocumentoWSClient.insertarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.insertarRequest();
            inValue.arg0 = arg0;
            return ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).insertarAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftResBusiness.TipoDocumentoWSClient.listarResponse SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.listar(SoftResBusiness.TipoDocumentoWSClient.listarRequest request) {
            return base.Channel.listar(request);
        }
        
        public SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO[] listar() {
            SoftResBusiness.TipoDocumentoWSClient.listarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.listarRequest();
            SoftResBusiness.TipoDocumentoWSClient.listarResponse retVal = ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).listar(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.listarResponse> SoftResBusiness.TipoDocumentoWSClient.TipoDocumento.listarAsync(SoftResBusiness.TipoDocumentoWSClient.listarRequest request) {
            return base.Channel.listarAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftResBusiness.TipoDocumentoWSClient.listarResponse> listarAsync() {
            SoftResBusiness.TipoDocumentoWSClient.listarRequest inValue = new SoftResBusiness.TipoDocumentoWSClient.listarRequest();
            return ((SoftResBusiness.TipoDocumentoWSClient.TipoDocumento)(this)).listarAsync(inValue);
        }
    }
}
