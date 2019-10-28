/*
 *
 * Written by Starman. Was kinda lazy, so i did some copied some of my previous network code and pasted it.
 *
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WebmanMod
{
    /// <summary>
    /// Access the Playstation 3 Hard- and Software with the Webman Mod Plugin.
    /// </summary>
    public class Webman
    {
        private IPAddress ip;
        private const int stdport = 7887;
        private Socket ntwrk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public enum LedColor
        {
            Red = 0,
            Green = 1,
            Yellow = 2
        }

        public enum LedMode
        {
            On = 1,
            Off = 0,
            BlinkFast = 2,
            BlinkSlow = 3
        }

        public enum PowerFlags
        {
            QuickReboot,
            SoftReboot,
            HardReboot,
            Shutdown
        }


        public enum Syscall8Mode
        {
            FullyEnabled = 0,
            PartialDisable_KeepPS3MAPIandCobraMambaFeatures = 1,
            PartialDisable_KeepPS3MAPIFeatures = 2,
            FakeDisabled = 3,
            FullyDisabled = 4
        }


        public enum BuzzerMode
        {
            Single = 1,
            Double = 2,
            Triple = 3
        }

        public enum DiscOptions
        {
            EjectDisc,
            InsertDisc
        }


        /// <summary>
        /// Connects to Webman on PS3MAPI standard port
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool Connect(string ipAddress)
        {
            bool result = false;


            result = IPAddress.TryParse(ipAddress, out ip);
            if (result)
            {
                ntwrk.Connect(ip, stdport);
                if (ntwrk.Connected)
                    return true;
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// Connects to Webman
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(string ipAddress, int port)
        {
            bool result = false;

            result = IPAddress.TryParse(ipAddress, out ip);
            if (result)
            {
                ntwrk.Connect(ip, port);
                if (ntwrk.Connected)
                    return true;
                else return false;
            }
            else return false;
        }


        /// <summary>
        /// Disconnects from the target
        /// </summary>
        public void Disconnect()
        {
            bool cnncted = ntwrk.Connected;

            if (cnncted)
            {
                ntwrk.Disconnect(false);
            }
        }

        /// <summary>
        /// Sends a notify command to display notifications with custom text
        /// </summary>
        /// <param name="message"></param>
        public void Notify(string message)
        {
            WebRequest ntfy = WebRequest.Create("http://" + ip + "/notify.ps3mapi?msg=" + message);
            WebResponse responder = ntfy.GetResponse();

            using (Stream datastream = responder.GetResponseStream())
            {
                StreamReader strrd = new StreamReader(datastream);

                strrd.ReadToEnd();
            }
            responder.Close();
        }

        /// <summary>
        /// Will trigger the buzzer function on the PS3
        /// </summary>
        /// <param name="mode"></param>
        public void RingBuzzer(BuzzerMode mode)
        {
            WebRequest bzzer = WebRequest.Create("http://" + ip + "/buzzer.ps3mapi?mode=" + mode);
            WebResponse bzzerrsp = bzzer.GetResponse();

            using (Stream dataStream = bzzerrsp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                reader.ReadToEnd();

            }
            bzzerrsp.Close();
        }


        /// <summary>
        /// Will set a new Console ID and PSID on the PS3.
        /// </summary>
        /// <param name="CIDpart1"></param>
        /// <param name="CIDpart2"></param>
        /// <param name="PSIDpart1"></param>
        /// <param name="PSIDpart2"></param>
        public void SetConsoleIDandPSID(string CIDpart1, string CIDpart2, string PSIDpart1, string PSIDpart2)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/setidps.ps3mapi?idps1=" + CIDpart1 + "&idps2=" + CIDpart2 + "&psid1=" + PSIDpart1 + "&psid2=" + PSIDpart2);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
            rsp.Close();
        }


        /// <summary>
        /// Functions for Syscall 8
        /// </summary>
        /// <param name="mode"></param>
        public void DisableSyscall8(Syscall8Mode mode)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/syscall8.ps3mapi?mode=" + mode);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
            rsp.Close();
        }

        /// <summary>
        /// Power options for the PS3
        /// </summary>
        /// <param name="flag"></param>
        public void Power(PowerFlags flag)
        {
            if (flag == PowerFlags.QuickReboot)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/reboot.ps3?quick");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
            else if (flag == PowerFlags.SoftReboot)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/reboot.ps3?soft");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
            else if (flag == PowerFlags.HardReboot)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/reboot.ps3?hard");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
            else if (flag == PowerFlags.Shutdown)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/shutdown.ps3");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
        }


        /// <summary>
        /// Displays a notification on the PS3 containing the system informations
        /// </summary>
        public void ShowSystemInfo()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/cpursx.ps3");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
            rsp.Close();
        }



        /// <summary>
        /// Sets the fanspeed of the playstation 3, minimum speed must be 25 and maximum 95. Do not use % after the value.
        /// </summary>
        /// <param name="Percentage"></param>
        public void SetFanSpeed(string Percentage)
        {
            if (string.IsNullOrEmpty(Percentage) || Percentage.Length > 2)
                MessageBox.Show("Empty values or values with a length that is greater than 2 numbers are not valid.");
            else
            {
                int prc = Convert.ToInt32(Percentage);

                if (prc > 95 || prc < 25)
                    MessageBox.Show("Invalid fan speed value, select a value between 25 and 95");
                else
                {

                    WebRequest cidpsid = WebRequest.Create("http://" + ip + "/cpursx.ps3?fan=" + Percentage);
                    WebResponse rsp = cidpsid.GetResponse();

                    using (Stream dataStream = rsp.GetResponseStream())
                    {
                        StreamReader rd = new StreamReader(dataStream);
                        rd.ReadToEnd();
                    }
                    rsp.Close();
                }
            }
        }
        /// <summary>
        /// Sets the fanspeed of the playstation 3, minimum speed must be 25 and maximum 95. Do not use % after the value.
        /// </summary>
        /// <param name="Percentage"></param>
        public void SetFanSpeed(int Percentage)
        {
            if (Percentage > 95 || Percentage < 25)
                MessageBox.Show("Invalid fan speed settings.");
            else
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/cpursx.ps3?fan=" + Percentage);
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
        }

        public void GameDisc(DiscOptions options)
        {
            if (options == DiscOptions.InsertDisc)
            {

                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/insert.ps3");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
            else if (options == DiscOptions.EjectDisc)
            {

                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/eject.ps3");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
        }

        public void DownloadFileFromBrowser(string URL)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/download.ps3?url=" + URL);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void DownloadFileFromBrowser(string DownloadPath, string URL)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/download.ps3?to=" + DownloadPath + "&url=" + URL);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void OpenBrowserURL(string URL)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3?" + URL);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void ToggleVSHMenu()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$vsh_menu");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void ToggleSLaunchMenu()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$slaunch");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void DoScreenshot(string SavingPath)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$screenshot_xmb/" + SavingPath);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void ToggleInGameVideoRecord()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/videorec.ps3");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum SyscallTypes
        {
            AllCFWSyscalls,
            All_ExceptCCAPISyscall
        }

        public void DisableSyscalls(SyscallTypes types)
        {
            if (types == SyscallTypes.AllCFWSyscalls)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$disable_syscalls");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (types == SyscallTypes.All_ExceptCCAPISyscall)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$disable_syscalls?ccapi");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void ShowMinimumDowngradeVersion()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/rebuild.ps3");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void RebuildDatabase()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$disable_syscalls");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }


        /// <summary>
        /// Toggles the Cobra payload in Stage 2
        /// </summary>
        public void ToggleCobra()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$toggle_cobra");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }

        }

        public void TogglePS2Emu()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$toggle_ps2emu");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum PS3Mode
        {
            NormalMode,
            RebugMode
        }

        public void SwitchMode(PS3Mode mode)
        {
            if (mode == PS3Mode.NormalMode)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$toggle_normal_mode");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (mode == PS3Mode.RebugMode)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$toggle_rebug_mode");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void SwitchDebugMenu()
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$toggle_debug_menu");
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum ServerOption
        {
            BlockConnection,
            RestoreConnection
        }

        public void OnlineServers(ServerOption option)
        {
            if (option == ServerOption.BlockConnection)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$block_servers");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (option == ServerOption.RestoreConnection)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/browser.ps3$restore_servers");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }


        public enum Services
        {
            FTP,
            NetworkService,
            PS3MAPI,
            All
        }

        public void StopService(Services service)
        {
            if (service == Services.FTP)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/netstatus.ps3?stop-ftp");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (service == Services.NetworkService)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/netstatus.ps3?stop-netsrv");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (service == Services.PS3MAPI)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/netstatus.ps3?stop-ps3mapi");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (service == Services.All)
            {
                WebRequest cidpsid = WebRequest.Create("http://" + ip + "/netstatus.ps3?stop");
                WebResponse rsp = cidpsid.GetResponse();

                using (Stream dataStream = rsp.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
                rsp.Close();
            }
        }

        public void Led(LedColor color, LedMode mode)
        {
            WebRequest cidpsid = WebRequest.Create("http://" + ip + "/led.ps3mapi?color=" + color + "&mode=" + mode);
            WebResponse rsp = cidpsid.GetResponse();

            using (Stream dataStream = rsp.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
            rsp.Close();
        }

        public void InstallPackage()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/install.ps3");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum ScriptFileExtension
        {
            bat,
            txt
        }


        /// <summary>
        /// Executes a script command on the PS3, must have .txt or .bat extension
        /// </summary>
        /// <param name="ScriptName"></param>
        public void ExecuteScriptFile(string FilePath, string FileName, ScriptFileExtension extension)
        {
            if (extension == ScriptFileExtension.bat)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/play.ps3/" + FilePath + "/" + FileName + ".bat");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (extension == ScriptFileExtension.txt)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/play.ps3/" + FilePath + "/" + FileName + ".txt");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }


        }

        public enum SetFanMode
        {
            Dynamic,
            Manual
        }

        public void FanSettings(SetFanMode mode)
        {
            if (mode == SetFanMode.Dynamic)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/cpursx.ps3?dyn");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (mode == SetFanMode.Manual)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/cpursx.ps3?man");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void IncreaseFanSpeed()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/cpursx.ps3?up");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void DecreaseFanSpeed()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/cpursx.ps3?up");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void ShowIDPSandPSID()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/browser.ps3$show_idps");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum Data
        {
            ConsoleID,
            PSID,
            All
        }

        public enum ShowNetworkStatus
        {
            PS3MAPIServer,
            NetworkServer,
            FTPServer
        }

        public void NetworkStatus(ShowNetworkStatus status)
        {
            if (status == ShowNetworkStatus.PS3MAPIServer)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/netstatus.ps3?ps3mapi");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (status == ShowNetworkStatus.NetworkServer)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/netstatus.ps3?netsrv");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (status == ShowNetworkStatus.FTPServer)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/netstatus.ps3?ftp");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }



        }


        /// <summary>
        /// Dumps The Console ID/PSID with the act.dat file to dev_usb000 or dev_hdd000, dependant if dev_usb000 is available.
        /// </summary>
        /// <param name="data"></param>
        public void Dump(Data data)
        {
            if (data == Data.ConsoleID)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/idps.ps3");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (data == Data.PSID)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/psid.ps3");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (data == Data.All)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/consoleid.ps3");
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void SetMemory(string processID, string Offset, string Value)
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/setmem.ps3mapi?proc=" + processID + "&addr=" + Offset + "&val=" + Value);
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public enum MemoryType
        {
            LV1,
            LV2
        }

        public void Peek(MemoryType type, string address)
        {
            if (type == MemoryType.LV1)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/peek.lv1?" + address);
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (type == MemoryType.LV2)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/peek.lv2?" + address);
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void Poke(MemoryType type, string address, string value)
        {
            

            if (type == MemoryType.LV1)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/poke.lv1?" + address + "=" + value);
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
            else if (type == MemoryType.LV2)
            {
                WebRequest installpkg = WebRequest.Create("http://" + ip + "/poke.lv2?" + address + "=" + value);
                WebResponse webresponder = installpkg.GetResponse();

                using (Stream dataStream = webresponder.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(dataStream);
                    rd.ReadToEnd();
                }
            }
        }

        public void PauseRSX()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/browser.ps3$rsx_pause");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }
        }

        public void ContinueRSX()
        {
            WebRequest installpkg = WebRequest.Create("http://" + ip + "/browser.ps3$rsx_continue");
            WebResponse webresponder = installpkg.GetResponse();

            using (Stream dataStream = webresponder.GetResponseStream())
            {
                StreamReader rd = new StreamReader(dataStream);
                rd.ReadToEnd();
            }

        }
    }
}
