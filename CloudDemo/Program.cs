using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase;
using SuperSocket.Common;
using System.IO;
using System.Reflection;
using SuperWebSocket;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

using System.Data;

namespace CloudDemo
{
    static class Program
    {
       
        static void Main(string[] args)
        {
            if ((!Platform.IsMono && !Environment.UserInteractive) || (Platform.IsMono && !AppDomain.CurrentDomain.FriendlyName.Equals(Path.GetFileName(Assembly.GetEntryAssembly().CodeBase))))
            {
                Program.RunAsService();
            }
            else
            {
                if (args != null && args.Length > 0)
                {
                    if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                    {
                        SelfInstaller.InstallMe();
                    }
                    else
                    {
                        if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                        {
                            SelfInstaller.UninstallMe();
                        }
                        else
                        {
                            Console.WriteLine("Invalid argument!");
                        }
                    }
                }
                else
                {
                    Program.RunAsConsole();
                }
            }
        }
        #region windows服务
        private static void RunAsService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
			{
				new WebSocketService()
			};
            ServiceBase.Run(servicesToRun);
        }
        #endregion
        #region 控制台的方式
        private static bool setConsoleColor;
        private static void SetConsoleColor(ConsoleColor color)
        {
            if (setConsoleColor)
            {
                Console.ForegroundColor = color;
            }
        }
        private static void CheckCanSetConsoleColor()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.ResetColor();
                setConsoleColor = true;
            }
            catch
            {
                setConsoleColor = false;
            }
        }
        /// <summary>
        /// 新的会话链接
        /// </summary>
        /// <param name="session"></param>
        private static void wsServer_NewSessionConnected(WebSocketSession session)
        {

            Console.WriteLine("Starting..." + session.RemoteEndPoint);
            LogHelper.Receive("NewConnected[" + session.RemoteEndPoint + "]");
        }
        private static void RunAsConsole()
        {
            int startTime;
            int endTime;
            int runTime;
            List<string> tasks = new List<string>();
            //BackgroundWorker receiveWorker;
            CheckCanSetConsoleColor();
            Console.WriteLine("Press any key to start the SuperSocket ServiceEngine!");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Initializing...");

            

            IBootstrap bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                SetConsoleColor(ConsoleColor.Red);
                Console.WriteLine("Failed to initialize SuperSocket ServiceEngine! Please check error log for more information!");
                Console.ReadKey();
            }
            else
            {
                //var socketServer = bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("SuperWebSocket")) as WebSocketServer;
               var secureSocketServer = bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("SecureSuperWebSocket")) as WebSocketServer;

                

               secureSocketServer.NewSessionConnected += wsServer_NewSessionConnected;

                ///////////////////////////////database load
                DataSet dsEnrolls;
                EnrollData ed = new EnrollData();
                ed.New("./");
                dsEnrolls = EnrollData.DataModule.GetEnrollDatas();
                //////////////////////////////////

                Console.WriteLine("Starting...");
                StartResult result = bootstrap.Start();
                Console.WriteLine("-------------------------------------------------------------------");
                foreach (IWorkItem server in bootstrap.AppServers)
                {
                    //装载事件
                    WebSocketLoader.Setup(server);
                    if (server.State == ServerState.Running)
                    {
                      
                        SetConsoleColor(ConsoleColor.Green);
                        Console.WriteLine("- {0} has been started", server.Name);
                    }
                    else
                    {
                        SetConsoleColor(ConsoleColor.Red);
                        Console.WriteLine("- {0} failed to start", server.Name);
                    }
                }
                Console.ResetColor();
                Console.WriteLine("-------------------------------------------------------------------");
                switch (result)
                {
                    case StartResult.None:
                        SetConsoleColor(ConsoleColor.Red);
                        Console.WriteLine("No server is configured, please check you configuration!");
                        Console.ReadKey();
                        return;

                    case StartResult.Success:
                        Console.WriteLine("The SuperSocket ServiceEngine has been started!");
                        break;

                    case StartResult.PartialSuccess:
                        SetConsoleColor(ConsoleColor.Red);
                        Console.WriteLine("Some server instances were started successfully, but the others failed! Please check error log for more information!");
                        break;

                    case StartResult.Failed:
                        SetConsoleColor(ConsoleColor.Red);
                        Console.WriteLine("Failed to start the SuperSocket ServiceEngine! Please check error log for more information!");
                        Console.ReadKey();
                        return;
                }
               
                Console.ResetColor();
                Console.WriteLine("Press key 'q' to stop the ServiceEngine.");
                ///////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////
                //////////////below test the demo :server active send the command to terminal
                string str;
                bool i = true;
                while (i==true)
                {
                    str = Console.ReadLine(); 
                    switch (str)
                    {
                        case "stop":
                            i=false;
                            break;
                        case "getuserlist":
                            WebSocketLoader.getuserlist(WebSocketLoader.g_now_sn);
                            break;
                        case "getuserinfo":
                            WebSocketLoader.getuserinfo(WebSocketLoader.g_now_sn, 1, 0);
                                break;
                        case "setpwd":
                                WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, 1, "邹春庆", 10, 0, 123456, null);
                                break;
                        case "setcard":
                                WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, 1, "邹春庆", 11, 0, 2352253, null);
                                break;
                        case "setfp":
                               //thbio 1.0 
                               // WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, 1,"邹春庆",0, 0,0,"cb09194bbe53ba6845befe6ecd9d0272ab7b0c76bb77147a97ef9c7eb5e1a482abc7d90de8936707d2c5ce00c26c0f02139efc0720b87c0e2f48bc02cefeec0551199a06d3abe862d047a966546b7b3ae496f198f4943796db30b166f91e5ff1177be39676ecaf2793c8a696e94f601b95dc6b4b5230e3c0cd336de4a438ce82d5d6a61f197090ed0b7ffeee4b09022f(100)0100320001(3)83604024cb091a4ea6578a6a05be9e6e0d9d62722a7bcc763a77d47a94effc7e34e1c48228c7a90d0897671ed246eb02006d66040198ec1e41b8dc03ec58c4646ede3c2a9339dfa88da91ee8774389e6d6676ba08c8c70509f8cbf1a9438f3fd719f3200065bb97eeaed6e07e3d4641bc40b6a27735056b41324528bca33503745285aafd3ffa427d93bb6ff55ee9b647130022f4be30c6f(108)");
                               //thbio 3.0
                                WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, 1,"邹春庆",0, 0,0,"c51a6b00d9860af9ff3e19c769889355d08fa7c958877bc1d80bb8c36b8963d2789388d51c8b0bfdf97839048b8a8c16300b6a8749884c35c0d1c9082889fc71e1640b112c898c85d95a0b8f348924a5d8d5d98b4286f4cee0480a0438887ce1d8ce09893f87d501d08a19032f893576e1080c454486b58de7c0294446897c41b0e19ba6928b7c0d1fffd744268c74b5ff79fcc4358b94e5ff33fa87a78b5d26ef7fc94b5a857b95e7c5fa42bd85fd65ff7e188336885631d905fb86a589ae3116be1fc23289de3df83bffc340876e82cfc1fd45(272)28471547254213a3263581334462457f348445f41673f191352f3328f4152540893663833243454f2f2637f9f44262(65)146bd91d1191ae92382630a18b92185203a39cb3bc061020d9b9975601b199b4bb2214709590160a32d2de924e47443320c1f8053391ce8580155431cea1456210e1c8a119163682bc800c593076b292806c056416914b1b4061aba1c23c1614349102584104dc7159514293ceb12e578443d1f0f56544c87af0(60)090b080f070c060d030a05021304120e10161718(10)8eb0");
                                break;
                        case "getname":
                                WebSocketLoader.getusername(WebSocketLoader.g_now_sn, 1);
                                break;
                        case "setname":
                                WebSocketLoader.setusername(WebSocketLoader.g_now_sn);
                                break;
                        case "deleteuser":
                                WebSocketLoader.deleteuser(WebSocketLoader.g_now_sn, 1,13); //0~9 :fp  10 pwd ;11: card ;12: all fp ;13 :all(fp pwd card name)
                                break;
                        case "cleanuser":
                               WebSocketLoader.cleanuser(WebSocketLoader.g_now_sn);
                               break;
                        case "getnewlog":
                                WebSocketLoader.getnewlog(WebSocketLoader.g_now_sn);
                                break;
                        case "getalllog":
                           WebSocketLoader.getalllog(WebSocketLoader.g_now_sn);                         
                            break;
                        case "cleanlog":
                            WebSocketLoader.cleanlog(WebSocketLoader.g_now_sn);
                            break;
                        case "initsys":
                            WebSocketLoader.initsys(WebSocketLoader.g_now_sn);
                            break; 
                        case "cleanadmin":
                            WebSocketLoader.cleanadmin(WebSocketLoader.g_now_sn);
                            break;
                        case "setdevinfo":
                            WebSocketLoader.setdevinfo(WebSocketLoader.g_now_sn);
                            break;
                        case "getdevinfo":
                            WebSocketLoader.getdevinfo(WebSocketLoader.g_now_sn);
                            break;
                        case "opendoor":
                            WebSocketLoader.opendoor(WebSocketLoader.g_now_sn);
                            break;
                        case "setdevlock":
                            WebSocketLoader.setdevlock(WebSocketLoader.g_now_sn);
                            break;
                        case "getdevlock":
                            WebSocketLoader.getdevlock(WebSocketLoader.g_now_sn);
                            break;
                        case "setuserlock":
                            WebSocketLoader.setuserlock(WebSocketLoader.g_now_sn);
                            break;
                        case "getuserlock":
                            WebSocketLoader.getuserlock(WebSocketLoader.g_now_sn,2);
                            break; 
                        case "deleteuserlock":
                            WebSocketLoader.deleteuserlock(WebSocketLoader.g_now_sn, 1);
                            break;
                        case "cleanuserlock":
                            WebSocketLoader.cleanuserlock(WebSocketLoader.g_now_sn);
                            break;
                        case "reboot":
                            WebSocketLoader.reboot(WebSocketLoader.g_now_sn);
                            break;
                        case "settime":
                            WebSocketLoader.settime(WebSocketLoader.g_now_sn);
                            break;
                        case "disabledevice":
                             WebSocketLoader.disabledevice(WebSocketLoader.g_now_sn);
                            break;
                        case "enabledevice":
                            WebSocketLoader.enabledevice(WebSocketLoader.g_now_sn);
                            break;
                        ////////////////////////////////////////////for debug
                        case "uploadalluser":
                             DataTable dbEnrollTble;
                             DataRow dbRow;
                             DataSet dsChange;
                             bool doubleid = false;

                             dbEnrollTble = dsEnrolls.Tables[0];

                            int startalltime = System.Environment.TickCount;
                            int errorcount = 0;
                            WebSocketLoader.disablereturn = false;
                            WebSocketLoader.disabledevice(WebSocketLoader.g_now_sn);
                            while (!WebSocketLoader.disablereturn);
                            WebSocketLoader.getuserlistreturn = false;
                            WebSocketLoader.userlistindex = 0;
                            WebSocketLoader.getuserlist(WebSocketLoader.g_now_sn);
                            while (!WebSocketLoader.getuserlistreturn);
                            int a = 0;
                            while(a < WebSocketLoader.userlistindex)
                            {
                                errorcount = 0;
                            getagain:
                                WebSocketLoader.getuserinfoflag = false;
                                SetConsoleColor(ConsoleColor.Green);
                                Console.WriteLine("index:"+a+"==>getuser:" + WebSocketLoader.str_userlist[a].enrollid + ";backupnum:" + WebSocketLoader.str_userlist[a].backupnum);
                                CheckCanSetConsoleColor();
                                startTime = System.Environment.TickCount;  
                                WebSocketLoader.getuserinfo(WebSocketLoader.g_now_sn,WebSocketLoader.str_userlist[a].enrollid, WebSocketLoader.str_userlist[a].backupnum);
                                while (!WebSocketLoader.getuserinfoflag && System.Environment.TickCount - startTime<10000) ;
                               if( System.Environment.TickCount - startTime>=10000)
                               {
                                   if (errorcount > 3)
                                   {
                                       Console.WriteLine("error!!!!!!!!!!!!!!!!!!!");
                                       goto getend;
                                   }
                                   else
                                       goto getagain;
                               }

                                endTime = System.Environment.TickCount;
                                runTime=endTime-startTime;
                                SetConsoleColor(ConsoleColor.Red);
                                Console.WriteLine("time=" + runTime+"ms");
                                CheckCanSetConsoleColor();
                                ////////////////////////////save to database
                                doubleid = false;
                                foreach (DataRow dbRow1 in dbEnrollTble.Rows)
                                {
                                    if ((int)dbRow1["EnrollNumber"] == WebSocketLoader.tmpuserinfo.enrollid)
                                    {
                                        if ((int)dbRow1["FingerNumber"] == WebSocketLoader.tmpuserinfo.backupnum)
                                        {
                                            doubleid = true;
                                            break;
                                        }
                                    }
                                }
                                if (doubleid == false)
                                {
                                dbRow = dbEnrollTble.NewRow();
                                dbRow["EnrollNumber"] = WebSocketLoader.tmpuserinfo.enrollid;
                                dbRow["FingerNumber"] = WebSocketLoader.tmpuserinfo.backupnum;
                                dbRow["Privilige"] = WebSocketLoader.tmpuserinfo.admin;
                                dbRow["Username"] = WebSocketLoader.tmpuserinfo.name;                                
                                if (WebSocketLoader.tmpuserinfo.backupnum >= 10)
                                {
                                    dbRow["Password1"] = WebSocketLoader.tmpuserinfo.password;
                                }                              
                                else
                                {
                                    dbRow["Password1"] = 0;
                                    dbRow["FPdata"] = WebSocketLoader.tmpuserinfo.fpdata;

                                }
                                dbEnrollTble.Rows.Add(dbRow);
                                }
                                a++;
                                ////////////////////////////////
                            }
                             WebSocketLoader.enablereturn = false;
                            WebSocketLoader.enabledevice(WebSocketLoader.g_now_sn);
                            while (!WebSocketLoader.enablereturn) ;
                    getend:
                            dsChange = dsEnrolls.GetChanges();
                            EnrollData.DataModule.SaveEnrolls(dsEnrolls);
                            SetConsoleColor(ConsoleColor.Red);
                            Console.WriteLine("alltimes=" + (System.Environment.TickCount - startalltime) + "ms");
                            CheckCanSetConsoleColor();
                            break;
                     case "downloadalluser":
                            int vEnrollNumber;
                            int vFingerNumber;
                            int vPrivilege;
                            int glngEnrollPData;
                            string username;
                            string fpdata;

                            errorcount = 0;
                            startalltime = System.Environment.TickCount;
                            WebSocketLoader.disablereturn = false;
                            WebSocketLoader.disabledevice(WebSocketLoader.g_now_sn);
                            while (!WebSocketLoader.disablereturn) ;
                            dbEnrollTble = dsEnrolls.Tables[0];
                            if (dbEnrollTble.Rows.Count == 0)
                            {
                                SetConsoleColor(ConsoleColor.Red);
                                Console.WriteLine("no data in database!");
                                CheckCanSetConsoleColor();
                                break;
                            }
                            Console.WriteLine("allcount=" + dbEnrollTble.Rows.Count);
                            a = 1;
                            foreach (DataRow dbRow2 in dbEnrollTble.Rows)
                            {
                                errorcount = 0;
                                vEnrollNumber = (int)dbRow2["EnrollNumber"];
                                vFingerNumber = (int)dbRow2["FingerNumber"];
                                vPrivilege = (int)dbRow2["Privilige"];
                                glngEnrollPData = (int)dbRow2["Password1"];
                                username = (string)dbRow2["Username"];
                                fpdata = (string)dbRow2["FPdata"];
                            sendagain:
                                SetConsoleColor(ConsoleColor.Green);
                                Console.WriteLine("index:"+a+":enrollid:" + vEnrollNumber + ",backnum=" + vFingerNumber+",name="+username);
                                CheckCanSetConsoleColor();
                                WebSocketLoader.setuserinfoflag = false;
                                startTime = System.Environment.TickCount;
                                WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, vEnrollNumber, username, vFingerNumber, vPrivilege, glngEnrollPData, fpdata);
                                while (!WebSocketLoader.setuserinfoflag && System.Environment.TickCount - startTime < 10000) ;
                                if (System.Environment.TickCount - startTime >= 10000)
                                {
                                    errorcount++;
                                    if (errorcount > 3)
                                    {
                                        Console.WriteLine("error!!!!!!!!!!!!!!!!!!!");
                                        goto sendend;
                                    }
                                    else
                                        goto sendagain;
                                }
                                endTime = System.Environment.TickCount;
                                runTime = endTime - startTime;
                                SetConsoleColor(ConsoleColor.Red);
                                Console.WriteLine("time=" + runTime + "ms");
                                CheckCanSetConsoleColor();
                                a++;

                            }                           
                            WebSocketLoader.enablereturn = false;
                            WebSocketLoader.enabledevice(WebSocketLoader.g_now_sn);
                            while (!WebSocketLoader.enablereturn) ;
                    sendend:
                             SetConsoleColor(ConsoleColor.Red);
                            Console.WriteLine("alltimes=" + (System.Environment.TickCount - startalltime) + "ms");
                            CheckCanSetConsoleColor();
                            break;
                        case "cleandb":
                            EnrollData.DataModule.DeleteDB();
                            break;
                        default:
                            Console.WriteLine("can not find this command!");
                            break;
                    }                    
                }
                bootstrap.Stop();
                Console.WriteLine();
                Console.WriteLine("The SuperSocket ServiceEngine has been stopped!");
            }
        }
        #endregion



    }
}
