using Chummer;
using Chummer.Plugins;
using ChummerHub.Client.Backend;
using Microsoft.Rest;
using NLog;
using SINners;
using SINners.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class ucSINnersBasic : UserControl
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public ucSINnersUserControl myUC
        {
            get; private set;
        }

        public ucSINnersBasic() => SINnersBasicConstructor(null);

        public ucSINnersBasic(ucSINnersUserControl parent) => SINnersBasicConstructor(parent);

        private bool _inConstructor = false;

        private void SINnersBasicConstructor(ucSINnersUserControl parent)
        {
            _inConstructor = true;
            InitializeComponent();


            TagValueArchetype.DataSource = ContactControl.ContactArchetypes;
            Name = "SINnersBasic";
            bGroupSearch.Enabled = false;
            AutoSize = true;
            myUC = parent;
            myUC.MyCE = parent.MyCE;
            if (myUC.MyCE?.MySINnerFile?.Id != null)
            {
                tbID.Text = myUC.MyCE?.MySINnerFile?.Id?.ToString();
            }

            string tip =
                "Assigning this SINner a new Id enables you to save multiple versions of this chummer on SINnersHub." +
                Environment.NewLine;
            tip += "";
            bGenerateNewId.SetToolTip(tip);
            CheckSINnerStatus().ContinueWith(a =>
            {
                if (!a.Result)
                {
                    Log.Error("somehow I couldn't check the onlinestatus of " +
                                                        myUC.MyCE.MySINnerFile.Id);
                }

            });
            foreach (object cb in gpTags.Controls)
            {
                if ((cb is Control cont))
                {
                    cont.Click += OnGroupBoxTagsClick;
                }

                if ((cb is CheckBox cccb))
                {
                    cccb.CheckedChanged += OnGroupBoxTagsClick;
                    cccb.CheckStateChanged += OnGroupBoxTagsClick;
                }

                if ((cb is ComboBox ccb))
                {
                    ccb.TextChanged += OnGroupBoxTagsClick;
                }

                if ((cb is TextBox ctb))
                {
                    ctb.TextChanged += OnGroupBoxTagsClick;
                }
            }

            _inConstructor = false;
        }

        public async Task<bool> CheckSINnerStatus()
        {

            try
            {
                if ((myUC?.MyCE?.MySINnerFile?.Id == null) || (myUC.MyCE.MySINnerFile.Id == Guid.Empty))
                {
                    PluginHandler.MainForm.DoThreadSafe(() =>
                    {

                        bUpload.Text = "SINless Character/Error";

                    });
                    return false;
                }

                using (new CursorWait(true, this))
                {
                    SINnersClient client = StaticUtils.GetClient();
                    HttpOperationResponse<ResultSinnerGetSINnerGroupFromSINerById> response = await client.GetSINnerGroupFromSINerByIdWithHttpMessagesAsync(myUC.MyCE.MySINnerFile.Id.Value);
                    PluginHandler.MainForm.DoThreadSafe(() =>
                    {
                        if (response.Response.StatusCode == HttpStatusCode.OK)
                        {
                            myUC.MyCE.MySINnerFile.MyGroup = response.Body.MySINnerGroup;
                            bUpload.Text = "Remove from SINners";
                            bGroupSearch.Enabled = true;
                            lUploadStatus.Text = "online";
                            bUpload.Enabled = true;
                        }
                        else if (response.Response.StatusCode == HttpStatusCode.NotFound)
                        {
                            myUC.MyCE.MySINnerFile.MyGroup = null;
                            lUploadStatus.Text = "not online";
                            bGroupSearch.Enabled = false;
                            bGroupSearch.SetToolTip(
                                "SINner needs to be uploaded first, before he/she can join a group.");
                            bUpload.Enabled = true;
                            bUpload.Text = "Upload";
                        }
                        else if (response.Response.StatusCode == HttpStatusCode.NoContent)
                        {
                            myUC.MyCE.MySINnerFile.MyGroup = null;
                            lUploadStatus.Text = "Statuscode: " + response.Response.StatusCode;
                            bGroupSearch.Enabled = true;
                            bGroupSearch.SetToolTip(
                                "SINner does not belong to a group.");
                            bUpload.Text = "Remove from SINners";
                            lUploadStatus.Text = "online";
                            bUpload.Enabled = true;
                        }
                        cbTagCustom.Enabled = false;
                        TagValueCustomName.Enabled = false;
                    });
                    PluginHandler.MainForm.DoThreadSafe(() => { UpdateTags(); });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                PluginHandler.MainForm.DoThreadSafe(() =>
                {
                    bUpload.Text = "unknown Status";
                });
                return false;
            }
            return true;
        }

        private void UpdateTags()
        {
            IEnumerable<string> archetypeseq = from a in StaticUtils.UserRoles
                                               where a.ToLowerInvariant() == "ArchetypeAdmin".ToLowerInvariant()
                                               select a;
            if (archetypeseq.Any())
            {
                cbTagCustom.Enabled = true;
                TagValueCustomName.Enabled = true;
            }
            if (myUC?.MyCE?.MySINnerFile?.MyGroup != null)
            {
                lGourpForSinner.Text = myUC.MyCE.MySINnerFile.MyGroup.Groupname;
            }

            List<Control> gpControlSeq = (from a in GetAllControls(gpTags, new List<Control>())
                                          where a.Name.Contains("cbTag")
                                          select a).ToList();

            List<Control> gpControlValueSeq = (from a in GetAllControls(gpTags, new List<Control>())
                                               where a.Name.Contains("TagValue")
                                               select a).ToList();

            foreach (Control cb in gpControlSeq)
            {
                if (cb is CheckBox cbTag)
                {
                    cbTag.CheckState = CheckState.Unchecked;
                }
            }

            foreach (Tag tag in myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.ToList())
            {
                if (tag == null)
                {
                    continue;
                }
                //search for a CheckBox that is named like the tag
                string checkBoxKey = "cbTag" + tag.TagName;
                List<Control> tagsCB = (from a in gpControlSeq where a.Name == checkBoxKey select a).ToList();
                if (!tagsCB.Any())
                {
                    continue;
                }

                if (!(tagsCB.FirstOrDefault() is CheckBox cbTag))
                {
                    throw new ArgumentNullException("Control " + checkBoxKey + " is NOT a Checkbox!");
                }

                if (bool.TryParse(tag.TagValue, out bool value))
                {
                    cbTag.Checked = value;
                }
                else
                {
                    cbTag.Checked = false;
                }
                //search for the value-control (whatever that may be)
                string tagValueControlKey = "TagValue" + tag.TagName;
                List<Control> tagValueControlSeq = (from a in gpControlValueSeq where a.Name == tagValueControlKey select a).ToList();
                if (!tagValueControlSeq.Any())
                {
                    continue;
                }

                cbTag.Checked = true;
                if (tagValueControlSeq.FirstOrDefault() is TextBox tbTagValue)
                {
                    tbTagValue.Text = tag.TagValue;
                }
                else if (tagValueControlSeq.FirstOrDefault() is ComboBox comboTagValue)
                {
                    if (tag.TagValue != null)
                    {
                        if (!comboTagValue.Items.Contains(tag.TagValue))
                        {
                            comboTagValue.Items.Add(tag.TagValue);
                        }

                        comboTagValue.SelectedItem = tag.TagValue;
                    }
                }
                else if (tagValueControlSeq.FirstOrDefault() is NumericUpDown upDownTagValue)
                {
                    if (decimal.TryParse(tag.TagValue, out decimal val))
                    {
                        upDownTagValue.Value = val;
                    }
                }
            }
        }

        private void OnGroupBoxTagsClick(object sender, object eventargs) =>
            // Call the base class
            //base.OnClick(eventargs);
            SaveTagsToSinner();

        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                list.Add(c);
                if (c.Controls.Count > 0)
                {
                    list = GetAllControls(c, list);
                }
            }

            return list;
        }

        private void SaveTagsToSinner()
        {
            if (_inConstructor)
            {
                return;
            }

            if (myUC == null)
            {
                return;
            }

            List<Control> gpControlSeq = (from a in GetAllControls(gpTags, new List<Control>())
                                          where a.Name.Contains("cbTag")
                                          select a).ToList();

            List<Control> gpControlValueSeq = (from a in GetAllControls(gpTags, new List<Control>())
                                               where a.Name.Contains("TagValue")
                                               select a).ToList();

            myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags = myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.Where(a => a != null).ToList();

            foreach (Control cb in gpControlSeq)
            {
                if (!(cb is CheckBox cbTag))
                {
                    continue;
                }

                string tagName = cbTag.Name.Substring("cbTag".Length);
                Tag tag = null;
                if (myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.All(a => a != null && a.TagName != tagName))
                {
                    tag = new Tag(true)
                    {
                        SiNnerId = myUC.MyCE.MySINnerFile.Id,
                        TagName = tagName
                    };

                }
                else
                {
                    tag = myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.FirstOrDefault(a => a != null && a.TagName == tagName);
                }
                if (tag == null)
                {
                    continue;
                }

                if (myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.Contains(tag))
                {
                    myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.Remove(tag);
                }

                if (cbTag.Checked == false)
                {
                    continue;
                }
                else if (cbTag.CheckState == CheckState.Checked)
                {
                    tag.TagValue = (true).ToString();
                }
                myUC.MyCE.MySINnerFile.SiNnerMetaData.Tags.Add(tag);
                //search for the value
                string tagValueControlKey = "TagValue" + tag.TagName;
                List<Control> tagValueControlSeq =
                    (from a in gpControlValueSeq where a.Name == tagValueControlKey select a).ToList();
                if (!tagValueControlSeq.Any())
                {
                    continue;
                }

                if (tagValueControlSeq.FirstOrDefault() is TextBox tbTagValue)
                {
                    tag.TagValue = tbTagValue.Text;
                }
                else if (tagValueControlSeq.FirstOrDefault() is ComboBox comboTagValue)
                {
                    tag.TagValue = comboTagValue.SelectedItem?.ToString();
                }
                else if (tagValueControlSeq.FirstOrDefault() is NumericUpDown upDownTagValue)
                {
                    tag.TagValue = upDownTagValue.Value.ToString(CultureInfo.InvariantCulture);
                }

            }
        }


        private async void bUpload_Click(object sender, EventArgs e)
        {
            using (new CursorWait(true, this))
            {
                try
                {
                    if (bUpload.Text.Contains("Upload"))
                    {
                        lUploadStatus.Text = "uploading";
                        await myUC.MyCE.Upload();
                    }
                    else
                    {
                        lUploadStatus.Text = "removing";
                        await myUC.RemoveSINnerAsync();
                    }

                }
                catch (Exception exception)
                {
                    Program.MainForm.ShowMessageBox(exception.Message);
                }
            }
            await CheckSINnerStatus();


        }

        private void bGroupSearch_Click(object sender, EventArgs e)
        {
            frmSINnerGroupSearch gs = new frmSINnerGroupSearch(myUC.MyCE, this);
            gs.MySINnerGroupSearch.OnGroupJoinCallback += (o, group) =>
            {
                PluginHandler.MainForm.DoThreadSafe(() =>
                {
                    PluginHandler.MainForm.CharacterRoster.LoadCharacters(false, false, false, true);
                });
            };
            DialogResult res = gs.ShowDialog();

        }

        private async void BVisibility_Click(object sender, EventArgs e)
        {
            frmSINnerVisibility visfrm = new frmSINnerVisibility();
            using (new CursorWait(true, this))
            {
                if (!myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility.UserRights.Any())
                {
                    SINnersClient client = Backend.StaticUtils.GetClient();
                    HttpOperationResponse<ResultSinnerGetSINnerVisibilityById> res =
                        await client.GetSINnerVisibilityByIdWithHttpMessagesAsync(
                            myUC.MyCE.MySINnerFile.Id.Value);
                    object obj = await Backend.Utils.HandleError(res, res.Body);
                    if (res?.Body?.CallSuccess == true)
                    {
                        myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility.UserRights = res.Body.UserRights;
                    }
                }
            }

            visfrm.MyVisibility = myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility;
            DialogResult result = visfrm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility = visfrm.MyVisibility;
            }

        }

        private void BGenerateNewId_Click(object sender, EventArgs e)
        {
            Guid? oldId = myUC.MyCE.MySINnerFile.Id;
            myUC.MyCE.MySINnerFile.Id = Guid.NewGuid();
            myUC.MyCE.MySINnerFile.SiNnerMetaData.Id = Guid.NewGuid();
            myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility.Id = Guid.NewGuid();
            foreach (SINnerUserRight user in myUC.MyCE.MySINnerFile.SiNnerMetaData.Visibility.UserRights)
            {
                user.Id = Guid.NewGuid();
            }
            myUC.MyCE.PopulateTags();
            //this.myUC.MyCE.MySINnerFile.MyExtendedAttributes.Id = Guid.NewGuid();
            if (oldId != null)
            {
                myUC.CharacterObject.FileName = myUC.CharacterObject.FileName.Replace(oldId.ToString(), myUC.MyCE.MySINnerFile.Id.ToString());
            }
            myUC.CharacterObject.Save(myUC.MyCE.MySINnerFile.Id + ".chum5", false, true);
            tbID.Text = myUC.MyCE.MySINnerFile.Id.ToString();
        }

        private void TlpTags_MouseLeave(object sender, EventArgs e) => SaveTagsToSinner();
    }
}
