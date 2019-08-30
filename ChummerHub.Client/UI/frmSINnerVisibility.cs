using System;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class frmSINnerVisibility : Form
    {
        public SINners.Models.SINnerVisibility MyVisibility
        {
            get => ucSINnerVisibility1.MyVisibility;
            set => ucSINnerVisibility1.MyVisibility = value;
        }

        public frmSINnerVisibility()
        {
            InitializeComponent();
            DialogResult = DialogResult.Ignore;
            AcceptButton = bOk;
        }



        private void BOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
