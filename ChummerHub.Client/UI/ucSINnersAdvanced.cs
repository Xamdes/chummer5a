using System;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class ucSINnersAdvanced : UserControl
    {
        public ucSINnersUserControl MySINnersUsercontrol
        {
            get; private set;
        }

        public ucSINnersAdvanced() => SINnersAdvancedConstructor(null);

        public ucSINnersAdvanced(ucSINnersUserControl parent) => SINnersAdvancedConstructor(parent);

        private void SINnersAdvancedConstructor(ucSINnersUserControl parent)
        {
            InitializeComponent();
            Name = "SINnersAdvanced";
            AutoSize = true;
            cbSINnerUrl.SelectedIndex = 0;
            MySINnersUsercontrol = parent;

            //TreeNode root = null;
            //MySINnersUsercontrol.MyCE.PopulateTree(ref root, null, null);
            //MyTagTreeView.Nodes.Add(root);
        }



        private void cmdPopulateTags_Click(object sender, EventArgs e) => PopulateTags();

        private void PopulateTags()
        {
            MyTagTreeView.Nodes.Clear();
            MySINnersUsercontrol.MyCE.MySINnerFile.SiNnerMetaData.Tags = MySINnersUsercontrol.MyCE.PopulateTags();
            TreeNode root = null;
            MySINnersUsercontrol.MyCE.PopulateTree(ref root, null, null);
            MyTagTreeView.Nodes.Add(root);
        }


        private void cmdPrepareModel_Click(object sender, EventArgs e) => MySINnersUsercontrol.MyCE.PrepareModel();

        private async void cmdPostSINnerMetaData_Click(object sender, EventArgs e) => await ChummerHub.Client.Backend.Utils.PostSINnerAsync(MySINnersUsercontrol.MyCE);

        private void MyTagTreeView_VisibleChanged(object sender, EventArgs e)
        {
            MyTagTreeView.Nodes.Clear();
            //if (MySINnersUsercontrol.MyCE?.MySINnerFile?.SiNnerMetaData?.Tags?.Any(a => a.TagName == "Reflection") == false)
            PopulateTags();
            //MySINnersUsercontrol.MyCE.PopulateTree(ref root, null, null);
            //MyTagTreeView.Nodes.Add(root);
        }

        private async void cmdUploadChummerFile_Click(object sender, EventArgs e) => await ChummerHub.Client.Backend.Utils.UploadChummerFileAsync(MySINnersUsercontrol.MyCE);





    }
}
