using Chummer;
using Chummer.Plugins;
using ChummerHub.Client.Backend;
using ChummerHub.Client.Model;
using NLog;
using SINners;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class ucSINnersUserControl : UserControl
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private CharacterShared _mySINner = null;
        private ucSINnersBasic TabSINnersBasic;

        public CharacterShared MySINner => _mySINner;

        public ucSINnersAdvanced TabSINnersAdvanced = null;

        public CharacterExtended MyCE
        {
            get; set;
        }

        public Character CharacterObject => MySINner.CharacterObject;



        public async Task<CharacterExtended> SetCharacterFrom(CharacterShared mySINner)
        {
            InitializeComponent();
            _mySINner = mySINner;
            MyCE = new CharacterExtended(mySINner.CharacterObject, null, PluginHandler.MySINnerLoading);
            MyCE.ZipFilePath = await MyCE.PrepareModel();


            TabSINnersBasic = new ucSINnersBasic(this)
            {
                Visible = true
            };
            TabSINnersAdvanced = new ucSINnersAdvanced(this)
            {
                Visible = true
            };


            tabPageBasic.Controls.Add(TabSINnersBasic);
            tabPageAdvanced.Controls.Add(TabSINnersAdvanced);

            AutoSize = true;

            if ((ucSINnersOptions.UploadOnSave == true))
            {
                try
                {
                    mySINner.CharacterObject.OnSaveCompleted = null;
                    mySINner.CharacterObject.OnSaveCompleted += PluginHandler.MyOnSaveUpload;
                }
                catch (Exception e)
                {
                    Log.Warn(e);
                }

            }
            //MyCE.MySINnerFile.SiNnerMetaData.Tags = MyCE.PopulateTags();
            return MyCE;
        }





        public async Task RemoveSINnerAsync()
        {
            try
            {
                SINnersClient client = StaticUtils.GetClient();
                await client.DeleteAsync(MyCE.MySINnerFile.Id.Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }


    }
}
