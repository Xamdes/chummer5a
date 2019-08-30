using Chummer;
using Chummer.Plugins;
using ChummerHub.Client.Backend;
using ChummerHub.Client.Model;
using SINners;
using SINners.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class ucSINnerShare : UserControl
    {
        private NLog.Logger Log = LogManager.GetCurrentClassLogger();
        public frmSINnerShare MyFrmSINnerShare;

        public frmCharacterRoster.CharacterCache MyCharacterCache
        {
            get; set;
        }
        public Func<Task<MyUserState>> DoWork
        {
            get;
        }
        //public Func<int, Task<MyUserState>> ReportProgress { get; }
        public Action<MyUserState> RunWorkerCompleted
        {
            get;
        }
        public Action<int, MyUserState> ReportProgress
        {
            get;
        }

        //public event DoWorkEventHandler DoWork;
        //public event ProgressChangedEventHandler ProgressChanged;
        //public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        //public BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        public ucSINnerShare()
        {
            InitializeComponent();
            DoWork += ShareChummer_DoWork;
            ReportProgress += ShareChummer_ProgressChanged;
            RunWorkerCompleted += ShareChummer_RunWorkerCompleted;  //Tell the user how the process went
        }

        public class MyUserState
        {
            public string LinkText
            {
                get; set;
            }
            public string StatusText
            {
                get; set;
            }
            public ucSINnerShare myWorker
            {
                get; set;
            }

            public MyUserState(ucSINnerShare worker)
            {
                myWorker = worker;
            }
            public int CurrentProgress
            {
                get; internal set;
            }
            /// <summary>
            /// 5 Steps are made in total!
            /// </summary>
            public int ProgressSteps
            {
                get; internal set;
            }
        }

        private async Task<MyUserState> ShareChummer_DoWork()
        {
            string hash = "";
            try
            {
                using (CustomActivity op_shareChummer = Timekeeper.StartSyncron("Share Chummer", null,
                    CustomActivity.OperationType.DependencyOperation, MyCharacterCache?.FilePath))
                {
                    MyUserState myState = new MyUserState(this);
                    CharacterExtended ce = null;
                    SINnersClient client = StaticUtils.GetClient();
                    string sinnerid = "";
                    Guid SINid = Guid.Empty;

                    async Task<CharacterExtended> GetCharacterExtended(CustomActivity parentActivity)
                    {
                        using (CustomActivity op_prepChummer = Timekeeper.StartSyncron("Loading Chummerfile", parentActivity,
                            CustomActivity.OperationType.DependencyOperation, MyCharacterCache?.FilePath))
                        {
                            Character c = new Character()
                            {
                                FileName = MyCharacterCache.FilePath
                            };
                            List<Character> foundchar = (from a in PluginHandler.MainForm.OpenCharacters
                                                         where a.FileName == MyCharacterCache.FilePath
                                                         select a).ToList();
                            if (foundchar?.Any() == true)
                                c = foundchar?.FirstOrDefault();
                            else
                            {
                                using (frmLoading frmLoadingForm = new frmLoading
                                {
                                    CharacterFile = MyCharacterCache.FilePath
                                })
                                {
                                    frmLoadingForm.Reset(36);
                                    frmLoadingForm.TopMost = true;
                                    frmLoadingForm.Show();
                                    myState.StatusText = "Loading chummer file...";
                                    myState.CurrentProgress += 10;
                                    ReportProgress(myState.CurrentProgress, myState);
                                    await c.Load(frmLoadingForm, false);
                                }
                            }

                            if (c == null)
                                throw new ArgumentNullException("Could not load Character file " +
                                                                MyCharacterCache.FilePath +
                                                                ".");
                            ce = new CharacterExtended(c, null, null, MyCharacterCache);
                            if (ce?.MySINnerFile?.Id != null)
                                sinnerid = ce.MySINnerFile.Id.ToString();
                            hash = ce?.MySINnerFile?.MyHash;
                            return ce;
                        }
                    }



                    if (MyCharacterCache.MyPluginDataDic.TryGetValue("SINnerId", out object sinneridobj))
                    {
                        sinnerid = sinneridobj?.ToString();
                    }
                    else
                    {
                        ce = await GetCharacterExtended(op_shareChummer);
                        sinnerid = ce.MySINnerFile.Id.ToString();
                        hash = ce?.MySINnerFile?.MyHash;
                    }



                    if ((string.IsNullOrEmpty(sinnerid)
                         || (!Guid.TryParse(sinnerid, out SINid))))
                    {
                        myState.StatusText = "SINner Id is unknown or not issued!";
                        ReportProgress(30, myState);
                    }
                    else
                    {
                        myState.StatusText = "SINner Id is " + SINid + ".";
                        myState.CurrentProgress = 30;
                        ReportProgress(myState.CurrentProgress, myState);
                    }


                    HttpOperationResponse<ResultSinnerGetSINById> checkresult = null;
                    //check if char is already online and updated
                    using (CustomActivity op_checkOnlineVersionChummer = Timekeeper.StartSyncron(
                        "check if online", op_shareChummer,
                        CustomActivity.OperationType.DependencyOperation, MyCharacterCache?.FilePath))
                    {
                        checkresult = await client.GetSINByIdWithHttpMessagesAsync(SINid);
                        if (checkresult == null)
                            throw new ArgumentException("Could not parse result from SINners Webservice!");
                        if (checkresult.Response.StatusCode != HttpStatusCode.NotFound)
                        {
                            if (checkresult.Body.CallSuccess != true)
                            {
                                if (checkresult.Body.MyException is Exception myException)
                                    throw new ArgumentException(
                                        "Error from SINners Webservice: " + checkresult.Body.ErrorText,
                                        myException);
                                else
                                    throw new ArgumentException("Error from SINners Webservice: " +
                                                                checkresult.Body.ErrorText);
                            }
                            else
                            {
                                hash = checkresult.Body.MySINner.MyHash;
                            }
                        }
                    }


                    DateTime lastWriteTimeUtc = System.IO.File.GetLastWriteTimeUtc(MyCharacterCache.FilePath);
                    if (checkresult.Response.StatusCode == HttpStatusCode.NotFound
                        || (checkresult.Body.MySINner.LastChange < lastWriteTimeUtc))
                    {
                        if (ce == null)
                        {
                            myState.StatusText = "The Chummer is newer and has to be uploaded again.";
                            myState.CurrentProgress = 30;
                            ReportProgress(myState.CurrentProgress, myState);
                            ce = await GetCharacterExtended(op_shareChummer);
                        }

                        using (CustomActivity op_uploadChummer = Timekeeper.StartSyncron(
                            "Uploading Chummer", op_shareChummer,
                            CustomActivity.OperationType.DependencyOperation, MyCharacterCache?.FilePath))
                        {

                            myState.StatusText = "Checking SINner availability (and if necessary upload it).";
                            myState.CurrentProgress = 35;
                            ReportProgress(myState.CurrentProgress, myState);
                            myState.ProgressSteps = 10;
                            bool uploadtask = await ce.Upload(myState, op_uploadChummer);
                            SINid = ce.MySINnerFile.Id.Value;
                            var result = await client.GetSINByIdWithHttpMessagesAsync(SINid);
                            if (result == null)
                                throw new ArgumentException("Could not parse result from SINners Webservice!");
                            if (result.Body?.CallSuccess != true)
                            {
                                if (result.Body?.MyException is Exception myException)
                                    throw new ArgumentException(
                                        "Error from SINners Webservice: " + result.Body?.ErrorText,
                                        myException);
                                else
                                    throw new ArgumentException(
                                        "Error from SINners Webservice: " + result.Body?.ErrorText);
                            }
                            else
                            {
                                hash = result.Body.MySINner.MyHash;
                            }
                        }
                    }

                    myState.StatusText = "SINner is online available.";
                    myState.CurrentProgress = 90;
                    ReportProgress(myState.CurrentProgress, myState);

                    string url = client.BaseUri + "O";
                    url += "/" + hash;
                    myState.LinkText = url;
                    ReportProgress(100, myState);
                    RunWorkerCompleted(myState);
                    return myState;
                }
            }
            catch (Exception exception)
            {
                Log.Warn(exception);
                throw;
            }
        }

        private void ShareChummer_ProgressChanged(int progress, MyUserState e)
        {
            pgbStatus.DoThreadSafe(() =>
            {
                pgbStatus.Value = e.CurrentProgress;
            });

            if (e is MyUserState us)
            {
                tbStatus.DoThreadSafe(() =>
                {
                    if (!tbStatus.Text.Contains(us.StatusText))
                        tbStatus.Text += us.StatusText + Environment.NewLine;
                });
                tbLink.DoThreadSafe(() =>
                {
                    if (!string.IsNullOrEmpty(us.LinkText) && (us.LinkText != tbLink.Text))
                    {
                        tbLink.Text = us.LinkText;
                    }
                });
            }
        }
        private void ShareChummer_RunWorkerCompleted(MyUserState us)
        {
            pgbStatus.Value = 100;
            tbLink.Text = us.LinkText;
            tbStatus.Text += "Link copied to clipboard." + Environment.NewLine;
            Clipboard.SetText(us.LinkText);
            tbStatus.Text += "Process was completed" + Environment.NewLine;
        }

        private void BOk_Click(object sender, EventArgs e)
        {
            MyFrmSINnerShare.Close();
        }
    }
}
