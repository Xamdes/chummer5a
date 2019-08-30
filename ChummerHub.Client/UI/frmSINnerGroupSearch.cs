using Chummer;
using Chummer.Plugins;
using ChummerHub.Client.Model;
using NLog;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class frmSINnerGroupSearch : Form
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private CharacterExtended MyCE
        {
            get;
        }
        public ucSINnersBasic MyParentForm
        {
            get;
        }

        public ucSINnerGroupSearch MySINnerGroupSearch => siNnerGroupSearch1;
        public frmSINnerGroupSearch(CharacterExtended ce, ucSINnersBasic parentBasic)
        {
            MyCE = ce;
            MyParentForm = parentBasic;
            InitializeComponent();
            siNnerGroupSearch1.MyCE = ce;
            siNnerGroupSearch1.MyParentForm = this;
            VisibleChanged += (sender, args) =>
            {
                if (Visible == true)
                {
                    ReallyCenterToScreen();
                }
            };

        }
        protected void ReallyCenterToScreen()
        {
            Screen screen = Screen.FromControl(PluginHandler.MainForm);

            Rectangle workingArea = screen.WorkingArea;
            Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - Height) / 2)
            };
        }

        private void FrmSINnerGroupSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PluginHandler.MainForm.CharacterRoster.DoThreadSafe(() =>
                {
                    PluginHandler.MainForm.CharacterRoster.LoadCharacters(false, false, false, true);
                });
            }
            catch (Exception exception)
            {
                Log.Warn(exception);
            }

        }
    }
}
