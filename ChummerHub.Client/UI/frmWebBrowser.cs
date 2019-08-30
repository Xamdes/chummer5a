using Chummer;
using ChummerHub.Client.Backend;
using Newtonsoft.Json;
using NLog;
using SINners.Models;
using System;
using System.Windows.Forms;

namespace ChummerHub.Client.UI
{
    public partial class frmWebBrowser : Form
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public frmWebBrowser() => InitializeComponent();





        private string LoginUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.SINnerUrl))
                {
                    Properties.Settings.Default.SINnerUrl = "https://sinners.azurewebsites.net/";
                    string msg = "if you are (want to be) a Beta-Tester, change this to http://sinners-beta.azurewebsites.net/!";
                    Log.Warn(msg);
                    Properties.Settings.Default.Save();
                }
                string path = Properties.Settings.Default.SINnerUrl.TrimEnd('/');

                path += "/Identity/Account/Login?returnUrl=/Identity/Account/Manage";
                return path;
            }
        }



        private void frmWebBrowser_Load(object sender, EventArgs e) => Invoke((Action)(() =>
                                                                       {
                                                                           SuspendLayout();
                                                                           webBrowser2.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(webBrowser2_Navigated);
                                                                           webBrowser2.ScriptErrorsSuppressed = true;
                                                                           webBrowser2.Navigate(LoginUrl);
                                                                           BringToFront();
                                                                       })
            );

        private bool login = false;

        private async void webBrowser2_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.AbsoluteUri == LoginUrl)
            {
                return;
            }

            if ((e.Url.AbsoluteUri.Contains("/Identity/Account/Logout")))
            {
                //maybe we are logged in now
                GetCookieContainer();
            }
            else if (e.Url.AbsoluteUri.Contains("/Identity/Account/Manage"))
            {
                try
                {
                    //we are logged in!
                    GetCookieContainer();
                    SINners.SINnersClient client = StaticUtils.GetClient();
                    if (client == null)
                    {
                        Log.Error("Cloud not create an instance of SINnersclient!");
                        return;
                    }
                    Microsoft.Rest.HttpOperationResponse<ResultAccountGetUserByAuthorization> user = await client.GetUserByAuthorizationWithHttpMessagesAsync();
                    if (user.Body?.CallSuccess == true)
                    {
                        if (user.Body != null)
                        {
                            login = true;
                            SINnerVisibility tempvis;
                            if (!string.IsNullOrEmpty(Properties.Settings.Default.SINnerVisibility))
                            {
                                tempvis = JsonConvert.DeserializeObject<SINnerVisibility>(Properties.Settings.Default
                                    .SINnerVisibility);
                            }
                            else
                            {
                                tempvis = new SINnerVisibility()
                                {
                                    IsGroupVisible = true,
                                    IsPublic = true
                                };
                            }

                            tempvis.AddVisibilityForEmail(user.Body.MyApplicationUser?.Email);
                            Close();
                        }
                        else
                        {
                            login = false;
                        }
                    }

                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                    throw;
                }

            }
        }

        private void GetCookieContainer()
        {
            try
            {
                using (new CursorWait(true, this))
                {
                    Properties.Settings.Default.CookieData = null;
                    Properties.Settings.Default.Save();
                    System.Net.CookieCollection cookies =
                        StaticUtils.AuthorizationCookieContainer?.GetCookies(new Uri(Properties.Settings.Default
                            .SINnerUrl));
                    SINners.SINnersClient client = StaticUtils.GetClient(true);
                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }

        }

        private void FrmWebBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (login == false)
            {
                GetCookieContainer();
            }
        }
    }
}
