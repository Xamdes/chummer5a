using Chummer;
using System;
using System.Collections.Generic;
using System.IO;

namespace SINners.Models
{
    public partial class SINner
    {
        public DateTime DownloadedFromSINnersTime
        {
            get;
            set;
        }


        public string ZipFilePath
        {
            get
            {
                if (Id == null)
                {
                    return null;
                }

                string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "SINner", Id.Value.ToString());
                return path;
            }
        }

        public string FilePath
        {
            get
            {
                string loadFilePath = null;
                if (Directory.Exists(ZipFilePath))
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(ZipFilePath, "*.chum5", SearchOption.TopDirectoryOnly);
                    foreach (string file in files)
                    {
                        DateTime lastwrite = File.GetLastWriteTime(file);
                        if ((lastwrite >= LastChange)
                            || LastChange == null)
                        {
                            loadFilePath = file;
                            return loadFilePath;
                        }
                        File.Delete(file);
                    }
                }
                return loadFilePath;
            }
        }

        public frmCharacterRoster.CharacterCache GetCharacterCache()
        {
            if (FilePath != null)
            {
                frmCharacterRoster.CharacterCache ret = new frmCharacterRoster.CharacterCache(FilePath);
                return ret;
            }

            return null;
        }

    }
}
