using Chummer;
using Chummer.Plugins;
using NLog;
using SINners.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class ucSINnerVisibility : UserControl
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private SINnerVisibility _mySINnerVisibility;
        public SINnerVisibility MyVisibility
        {
            get => _mySINnerVisibility;
            set
            {
                _mySINnerVisibility = value;
                FillVisibilityListBox();
            }
        }

        public CheckedListBox MyCheckBoxList => clbVisibilityToUsers;

        public ucSINnerVisibility()
        {
            InitializeComponent();
            MyVisibility = new SINnerVisibility()
            {
                UserRights = new List<SINnerUserRight>()
            };
            clbVisibilityToUsers.ItemCheck += clbVisibilityToUsers_ItemCheck;
        }

        public ucSINnerVisibility(SINnerVisibility vis)
        {
            MyVisibility = vis;
            InitializeComponent();
            clbVisibilityToUsers.ItemCheck += clbVisibilityToUsers_ItemCheck;
        }

        private void FillVisibilityListBox() => PluginHandler.MainForm.DoThreadSafe(new Action(() =>
                                              {
                                                  try
                                                  {
                                                      //((ListBox)clbVisibilityToUsers).DataSource = null;
                                                      if (MyVisibility != null)
                                                      {
                                                          ((ListBox)clbVisibilityToUsers).DataSource = MyVisibility.UserRightsObservable;
                                                      }
                                                      ((ListBox)clbVisibilityToUsers).DisplayMember = "EMail";
                                                      ((ListBox)clbVisibilityToUsers).ValueMember = "CanEdit";
                                                      for (int i = 0; i < clbVisibilityToUsers.Items.Count; i++)
                                                      {
                                                          SINnerUserRight obj = (SINnerUserRight)clbVisibilityToUsers.Items[i];
                                                          clbVisibilityToUsers.SetItemChecked(i, obj.CanEdit != null && obj.CanEdit.Value);
                                                      }
                                                      clbVisibilityToUsers.Refresh();
                                                  }
                                                  catch (Exception e)
                                                  {
                                                      Log.Error(e);
                                                      throw;
                                                  }

                                              }));

        private void bVisibilityAddEmail_Click(object sender, EventArgs e)
        {
            string email = tbVisibilityAddEmail.Text;
            MyVisibility.AddVisibilityForEmail(email);
            FillVisibilityListBox();
        }



        private void clbVisibilityToUsers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(clbVisibilityToUsers);
            selectedItems = clbVisibilityToUsers.SelectedItems;

            if (clbVisibilityToUsers.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    SINnerUserRight userright = selectedItems[i] as SINnerUserRight;
                    if (userright != null)
                    {
                        if (e.NewValue == CheckState.Checked)
                        {
                            userright.CanEdit = true;
                        }
                        else
                        {
                            userright.CanEdit = false;
                        }
                    }
                }
            }
            else
            {
                Program.MainForm.ShowMessageBox("No email selected!");
            }
        }

        private void bVisibilityRemove_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(clbVisibilityToUsers);
            selectedItems = clbVisibilityToUsers.SelectedItems;

            if (clbVisibilityToUsers.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    SINnerUserRight userright = selectedItems[i] as SINnerUserRight;
                    MyVisibility.UserRightsObservable.Remove(userright);
                }
                FillVisibilityListBox();
            }
            else
            {
                Program.MainForm.ShowMessageBox("No email selected!");
            }
        }


    }
}
