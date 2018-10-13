using IProduct.Modules.Data;
using IProduct.Modules.Library.Base_Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IProduct.Modules
{
    public static class GlobalConfigration
    {
        public static string FileBasePath { get; set; }

        public static string ImageMapp { get; set; }

        public static LangaueSettings LangaueSettings;
        public static void LoadSettings(DbContext dbContext)
        {
            LangaueSettings = new LangaueSettings(dbContext);
        }
    }
}
