using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class frmSINnerResponse : Form
    {
        public frmSINnerResponse()
        {
            InitializeComponent();
        }

        public ucSINnerResponseUI SINnerResponseUI
        {
            get
            {
                return this.siNnerResponseUI1;
            }
        }
    }
}
