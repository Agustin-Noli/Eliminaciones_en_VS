using Com.Bgba.Arqtec.Contextdelivery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace EliminacionesWeb.Helpers
{
    public class ContextDeliveryHelper
    {
        private readonly IWebHostEnvironment _hostingEnviroment;
        private readonly string _ambiente;
        private readonly string _servidor;
        private readonly string _baseDeDatos;

        public ContextDeliveryHelper(IWebHostEnvironment hostingEnvironment, string servidor, string ambiente,  string baseDeDatos)
        {
            this._hostingEnviroment = hostingEnvironment;
            this._ambiente = ambiente;
            this._servidor = servidor;
            this._baseDeDatos = baseDeDatos;
        }

        public  string GetUserName ()
        {
            try
            {
                string AppPath = _hostingEnviroment.ContentRootPath + "\\Application.xml";
                ContextDelivery contextD = new ContextDelivery(AppPath);

                string ContextSet = this._ambiente;

                User usuario = contextD.GetUser("ELIMINIACIONESapp", "BD_ELIMINACIONES", ContextSet);

                return usuario.GetUserName();
            }
            catch(ContextDeliveryException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetUserPassword()
        {
            try
            {
                string AppPath = _hostingEnviroment.ContentRootPath + "/Application.xml";
                ContextDelivery context = new ContextDelivery(AppPath);

                string ContextSet = this._ambiente;

                User usuario = context.GetUser("ELIMINIACIONESapp", "BD_ELIMINACIONES", ContextSet);

                return usuario.GetPassword();
            }
            catch (ContextDeliveryException ex)
            {

                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public string ConnString()
        {
            StringBuilder connString = new StringBuilder();
            connString.Append("Server=");
            connString.Append(_servidor);
            connString.Append(";Database=");
            connString.Append(_baseDeDatos);
            connString.Append(";Trusted_Connection=False;Persist Security Info=False;User ID=");
            connString.Append(GetUserName());
            connString.Append(";");
            connString.Append("Password=");
            connString.Append(GetUserPassword());
            connString.Append(";");

            return connString.ToString();
        }


    }
}
