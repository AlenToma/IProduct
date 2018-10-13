using EntityWorker.Core.InterFace;
using IProduct.Modules.Library;
using System;
using System.IO;

namespace IProduct.Modules.Rules
{
    public class FileRules : EntityWorker.Core.Interface.IDbRuleTrigger<Files>
    {
        public void AfterSave(IRepository repository, Files itemDbEntity, object objectId)
        {
            var mapp = repository.Get<Mapps>().Where(x => x.Id == itemDbEntity.Mapp_Id).ExecuteFirstOrDefault();
            var path = Path.Combine(GlobalConfigration.FileBasePath, mapp.Name, itemDbEntity.FilePath);
            var thumpFullPath = Path.Combine(GlobalConfigration.FileBasePath, "Thumps", itemDbEntity.FilePath);

            if (Actions.IsImage(path) && !Actions.IsImage(itemDbEntity.FileBytes))
                throw new Exception("the Image file is not supported");

            if (!File.Exists(path))
                using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var file = itemDbEntity.FileBytes;
                    fs.Write(file, 0, file.Length);
                }

            if (!Directory.Exists(Path.Combine(GlobalConfigration.FileBasePath, "Thumps")))
                Directory.CreateDirectory(Path.Combine(GlobalConfigration.FileBasePath, "Thumps"));
                
            if (Actions.IsImage(path) && !File.Exists(thumpFullPath))
            {
                using (var fs = new FileStream(thumpFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var file = Actions.GenerateThumbImage(itemDbEntity.FileBytes, 300, 150);
                    fs.Write(file, 0, file.Length);
                }
            }
        }

        public void BeforeSave(IRepository repository, Files itemDbEntity)
        {
            if (string.IsNullOrEmpty(itemDbEntity.FriendlyName))
                throw new Exception("FriendlyName cant be empty");

            if (itemDbEntity.FileBytes == null)
                throw new Exception("File is empty please upload a file");

            if (!itemDbEntity.Id.HasValue)
            {
                var mapp = repository.Get<Mapps>().Where(x => x.Id == itemDbEntity.Mapp_Id).ExecuteFirstOrDefault();
                var path = Path.Combine(GlobalConfigration.FileBasePath, mapp.Name, itemDbEntity.FilePath);
                if (File.Exists(path))
                    throw new Exception("FriendlyName already exist in the current directory, please choose another FriendlyName or another Directory");
            }
        }
    }
}
