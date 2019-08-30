using NLog;
using SINners.Models;
using System;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class frmSINnerGroupEdit : Form
    {
        private Logger Log = NLog.LogManager.GetCurrentClassLogger();
        //public frmSINnerGroupEdit()
        //{
        //    InitializeComponent();
        //}

        public frmSINnerGroupEdit(SINnerGroup group, bool onlyPWHash)
        {
            InitializeComponent();
            MySINnerGroupCreate.MyGroup = group;
            if (group.Id == null || group.Id == Guid.Empty)
                MySINnerGroupCreate.EditMode = true;
            MySINnerGroupCreate.InitializeMe(onlyPWHash);
        }


        public ucSINnerGroupCreate MySINnerGroupCreate
        {
            get
            {
                return this.siNnerGroupCreate1;
            }
        }
    }
}
