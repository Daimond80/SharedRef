using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Web.Caching;

namespace SharedRef.Web
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Json : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetReferences()
        {
            return String.Join(Environment.NewLine, new DependencyConteiner().Manager.GetReferences());
        }

        [WebMethod]
        public string GetMatrixes()
        {
            return String.Join(Environment.NewLine, new DependencyConteiner().Manager.GetMatrixes());
        }

        [WebMethod]
        public string GetValues(string name, string parentId, string startWith, int count, string sort)
        {
            return new DependencyConteiner().Manager.GetValuesJson(name, parentId, startWith, count, sort);
        }

        [WebMethod]
        public string GetById(string name, string id)
        {
            return new DependencyConteiner().Manager.GetByIdJson(name, id);
        }

        [WebMethod]
        public string GetMatrixValues(string name, string conditionals, string returns)
        {
            return new DependencyConteiner().Manager.GetMatrixJson(name, conditionals, returns);
        }

        [WebMethod]
        public string Clear()
        {
            new DependencyConteiner().Clear();

            return "OK";
        }
    }
}