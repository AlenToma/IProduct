using IProduct.Modules.Data;
using IProduct.Modules.Library.Base_Entity;
using System;
using System.IO;

namespace IProduct.Modules.Library
{
    public class Files : Entity
    {
        public string FilePath { get; set; }

        public string FriendlyName { get; set; }

        public string Type { get; set; }

        public Guid Mapp_Id { get; set; }

        private string _fileFullPath;
        public string FileFullPath
        {
            get
            {
                if (_fileFullPath == null)
                    LoadPaths();
                return _fileFullPath;

            }

            set
            {
                _fileFullPath = value;
            }
        }

        private string _fileThumpFullPath;
        public string FileThumpFullPath
        {
            get
            {
                if (_fileThumpFullPath == null)
                    LoadPaths();
                return _fileThumpFullPath;

            }
            set
            {
                _fileThumpFullPath = value;
            }
        }

        private byte[] _file;
        public byte[] FileBytes
        {
            get
            {
                if (_file == null)
                    LoadFile();
                return _file;
            }
            set
            {
                _file = value;
            }
        }

        private void LoadPaths()
        {
            string basePath = GlobalConfigration.ImageMapp;
            using (var rep = new DbContext())
            {
                var mapp = rep.Get<Mapps>().Where(x => x.Id == Mapp_Id).ExecuteFirstOrDefault();
                _fileFullPath = "/" + Path.Combine(basePath, mapp.Name, FilePath).Replace("\\", "/");
                _fileThumpFullPath = "/" + Path.Combine(basePath, "Thumps", FilePath).Replace("\\", "/"); ;
            }
        }
        private byte[] LoadFile()
        {
            string basePath = GlobalConfigration.FileBasePath;
            if (_file != null)
                return _file;
            if (!string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(basePath))
            {
                using (var rep = new DbContext())
                {
                    var mapp = rep.Get<Mapps>().Where(x => x.Id == Mapp_Id).ExecuteFirstOrDefault();
                    var path = Path.Combine(basePath, mapp.Name, FilePath);
                    _file = File.ReadAllBytes(path);
                }
            }

            return _file;
        }
    }
}
