using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedRef.Domain;
using SharedRef.Extensions.ObjectHelper;
using SharedRef.Data;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using NLog;
using Spring.Caching;
using SharedRef.Domain.IoC;
using SharedRef.Extensions.ListOfSimpleItems;
using SharedRef.Helper;

namespace SharedRef
{
    internal class Manager : IManager
    {
        internal Manager()
        {  }

        public DependencyConteiner Conteiner {get; set;}

        static Logger _log = LogManager.GetCurrentClassLogger();

        #region Общие 

        public List<string> GetReferences()
        {
            return Conteiner.References.Select(r => r.Name).ToList();
        }

        public List<string> GetMatrixes()
        {
            return Conteiner.Matrixes.Select(r => r.Name).ToList();
        }

        public void Clear()
        {
            using (var uof = UnitOfWorkFactory.Data())
            {
                uof.Clear();
            }
        }
        #endregion

        #region GetValues 

        public List<SimpleItem> GetValues(string name, string parentId = null, string startWith = null, int count = 0, string sort = "")
        {
            var refer = Conteiner.GetReference(name);

            Assert.IsNotNull(refer, "Справочник {0} отсутствует", name);

            return refer.GetItems(parentId, startWith, count, sort);
        }

        [CacheResult("AspNetCache", "'XML N=' + #name + 'P=' + #parentId + #startWith + #count + #sort")]
        public string GetValuesXml(string name, string parentId, string startWith, int count, string sort)
        {
            var list = GetValues(name, parentId, startWith, count, sort);

            return list.ToXml(name);
        }

        [CacheResult("AspNetCache", "'JSON N=' + #name + 'P=' + #parentId + #startWith + #count + #sort")]
        public string GetValuesJson(string name, string parentId, string startWith, int count, string sort)
        {
            var list = GetValues(name, parentId, startWith, count, sort);

            return list.ToJson(name);
        }
        #endregion

        #region GetById

        public SimpleItem GetById(string name, string id)
        {
            var refer = Conteiner.GetReference(name);

            Assert.IsNotNull(refer, "Справочник {0} отсутствует", name);

            return refer.GetById(id);
        }

        [CacheResult("AspNetCache", "'Ref XML N=' + #name + 'I=' + #id")]
        public string GetByIdXml(string name, string id)
        {
            var item = GetById(name, id);

            return new List<SimpleItem>() { item }.ToXml(name);
        }

        [CacheResult("AspNetCache", "'Ref JSON N=' + #name + 'I=' + #id")]
        public string GetByIdJson(string name, string id)
        {
            var item = GetById(name, id);

            return new List<SimpleItem>() { item }.ToJson(name);

        }
        #endregion

        #region Matrixes

        public List<SimpleItem> GetMatrix(string name, string conditionals, string returns)
        {
            var matrix = Conteiner.GetMatrix(name);

            Assert.IsNotNull(matrix, "Справочник {0} отсутствует", name);

            return matrix.Get(conditionals, returns);
        }

        [CacheResult("AspNetCache", "'Matrix Xml=' + #name + #conditionals + #returns")]
        public string GetMatrixXml(string name, string conditionals, string returns)
        {
            var list = GetMatrix(name, conditionals, returns);

            return list.ToXml(name);
        }

        [CacheResult("AspNetCache", "'Matrix Json=' + #name + #conditionals + #returns")]
        public string GetMatrixJson(string name, string conditionals, string returns)
        {
            var list = GetMatrix(name, conditionals, returns);

            return list.ToJson(name);
        }
        #endregion
    }
}
