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
    public class DependencyConteiner
    {
        static Logger _log = LogManager.GetCurrentClassLogger();

        public static XmlApplicationContext Context = null;

        public IList<Reference> References = new List<Reference>();

        public IList<Matrix> Matrixes = new List<Matrix>();

        static IManager _manager = null;

        public IManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = (IManager)Context.GetObject("Manager");
                    _manager.Conteiner = this;
                }
                return _manager;
            }
        }

        public void Clear()
        {
            Manager.Clear();
            _manager = null;
        }

        public Reference GetReference(string name)
        {
            return References.Where(r => r.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public Matrix GetMatrix(string name)
        {
            return Matrixes.Where(r => r.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public DependencyConteiner()
        {
            if (Context == null)
                Reload();
        }

        private void Reload()
        {
            var files = new List<string>();

            GetConfigsFiles(AppSettings.ConfigsPath, ref files);

            _log.Info("Чтение контекста");

            Context = new XmlApplicationContext(files.ToArray());

            Context.GetObjectNamesForType(typeof(INamed)).ToList()
                .ForEach(id => ((INamed)Context.GetObject(id)).Name = id);

            foreach (var id in Context.GetObjectNamesForType(typeof(IInitializing)))
            {
                ((IInitializing)Context.GetObject(id)).AfterPropertiesSet();
            }

            foreach (var id in Context.GetObjectNamesForType(typeof(Reference)))
            {
                References.Add((Reference)Context.GetObject(id));
            }

            foreach (var id in Context.GetObjectNamesForType(typeof(Matrix)))
            {
                Matrixes.Add((Matrix)Context.GetObject(id));
            }
        }

        private FileInfo GetFileInfoForObject(string id)
        {
            var def = Context.GetObjectDefinition(id);

            string fileName = Regex.Match(def.ResourceDescription, @"file \[(.*)\] line").Groups[1].Value;

            return new FileInfo(fileName);
        }

        private static void GetConfigsFiles(string path, ref List<string> files)
        {
            files.AddRange(
                Directory.GetFiles(path, "*.xml")
                    .Where(f => f.EndsWith(".hbm.xml") == false && f.EndsWith(".cfg.xml") == false));

            foreach (var dir in Directory.GetDirectories(path))
            {
                GetConfigsFiles(dir, ref files);
            }
        }
    }
}
