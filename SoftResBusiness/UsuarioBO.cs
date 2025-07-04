﻿using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class UsuarioBO
    {
        private UsuarioClient usuarioClienteSOAP;

        public UsuarioBO()
        {
            // Usa la configuración declarada en web/app.config (name="UsuarioPort")
            this.usuarioClienteSOAP = new UsuarioClient();
        }

        public int Insertar(usuariosDTO usuario)
        {
            return this.usuarioClienteSOAP.insertar(usuario);
        }

        public usuariosDTO ObtenerPorID(int usuarioID)
        {
            return this.usuarioClienteSOAP.obtenerPorId(usuarioID);
        }

        public int Modificar(usuariosDTO usuario)
        {
            return this.usuarioClienteSOAP.modificar(usuario);
        }
        public int CambiarContraseña(usuariosDTO usuario)
        {
            return this.usuarioClienteSOAP.cambiarcontrasena(usuario);
        }

        public int Eliminar(usuariosDTO usuario)
        {
            return this.usuarioClienteSOAP.eliminar(usuario);
        }

        public BindingList<usuariosDTO> Listar(usuariosParametros parametros)
        {
            var lista = this.usuarioClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<usuariosDTO>();   // lista vacía si el servicio devuelve null

            return new BindingList<usuariosDTO>(lista);
        }
        public usuariosDTO login (credencialesDTO credenciales)
        {
            usuariosDTO usuario = this.usuarioClienteSOAP.login(credenciales);
            if(usuario == null) return null;
            return usuario;
        }
        public bool ValidarDocumentoUnico(string numDocumento)
        {
            return this.usuarioClienteSOAP.existeDoc(numDocumento);
        }
        public bool ValidarEmailUnico(string email)
        {
            return this.usuarioClienteSOAP.existeEmail(email);
        }
    }
}
