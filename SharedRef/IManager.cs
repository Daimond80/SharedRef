using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context.Support;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using SharedRef.Helper;
using SharedRef.Domain.IoC;
using NLog;
using Spring.Aop.Framework;
using SharedRef.AOP;
using SharedRef.Domain;
using Spring.Caching;

namespace SharedRef
{
    public interface IManager
    {
        DependencyConteiner Conteiner { get; set; }

        List<string> GetReferences();

        List<string> GetMatrixes();

        void Clear();
        
        List<SimpleItem> GetValues(string name, string parentId = null, string startWith = null, int count = 0, string sort = "");

        string GetValuesXml(string name, string parentId, string startWith, int count, string sort);

        string GetValuesJson(string name, string parentId, string startWith, int count, string sort);

        SimpleItem GetById(string name, string id);

        string GetByIdXml(string name, string id);

        string GetByIdJson(string name, string id);

        string GetMatrixXml(string name, string conditionals, string returns);

        string GetMatrixJson(string name, string conditionals, string returns);
    }
}
