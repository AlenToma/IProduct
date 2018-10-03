using EntityWorker.Core.InterFace;
using System.Collections.Generic;
using System.Linq;

namespace IProduct.Modules.Library.Base_Entity
{
    public class LangaueSettings
    {
        private IRepository _repository;

        private string _languageCode;

        private Dictionary<string, ColumnValue> _columns;
        public LangaueSettings(IRepository repository, string languageCode = "en-US")
        {
            _repository = repository;
            _languageCode = languageCode;
            Load();
        }

        private void Load()
        {
            _columns = _repository.Get<Column>().Where(x => x.ColumnValues.Any(a => a.Country.CountryCode == _languageCode)).LoadChildren().Execute().ToDictionary(x => x.Key, x => x.ColumnValues.FirstOrDefault());
        }

        public void Refresh()
        {
            Load();
        }

        public string this[string key]
        {
            get
            {
                return _columns[key]?.Value;
            }
        }
    }
}
