using Newtonsoft.Json.Linq;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static Dictionary<string, string> _registeredDevices;
        private static WebSocketServer wsServer;
        public static string g_now_sn = "ZXNC08289238";

        public static bool disablereturn;
        public static bool enablereturn;
        public static bool getuserlistreturn;
        public static bool getuserinfoflag;

        public static bool setuserlistreturn;
        public static bool setuserinfoflag;

        public struct struct_userlist
        {
            public int enrollid;
            public int backupnum;
        }

        public static struct_userlist[] str_userlist = new struct_userlist[2000];
        public static int userlistindex;

        public struct struct_userinfo
        {
            public int enrollid;
            public int backupnum;
            public int admin;
            public string name;
            public Int32 password;
            public string fpdata;
        }

        public static struct_userinfo tmpuserinfo;


        public static WebSocketSession GetSessionByID(string sn)
        {
            if (_registeredDevices.ContainsKey(sn))
            {
                return wsServer.GetAppSessionByID(_registeredDevices[sn]);
            }
            else
                return null;
        }
        static void Main(string[] args)
        {
            _registeredDevices = new Dictionary<string, string>();
            _registeredDevices.Clear();
            wsServer = new WebSocketServer();
            int port = 7788;
            wsServer.Setup(port);
            wsServer.NewSessionConnected += new SessionHandler<WebSocketSession>(WsServer_NewSessionConnected);
            wsServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(WsServer_NewMessageReceived);
            wsServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(WsServer_SessionClosed);



            wsServer.Start();
            Console.WriteLine("Server is running on port " + port + ". Press ENTER to exit....");
            string str;
            bool i = true;
            while (i == true)
            {
                str = Console.ReadLine();
                switch (str)
                {
                    case "stop":
                        i = false;
                        break;
                    case "getuserlist":
                        getuserlist(g_now_sn);
                        break;
                    case "getuserinfo":
                        getuserinfo(g_now_sn, 1, 0);
                        break;
                    case "setpwd":
                        setuserinfo(g_now_sn, 1, "邹春庆", 10, 0, 123456, null);
                        break;
                    case "setcard":
                        setuserinfo(g_now_sn, 1, "邹春庆", 11, 0, 2352253, null);
                        break;
                    case "setfp":
                        //thbio 1.0 
                        // WebSocketLoader.setuserinfo(WebSocketLoader.g_now_sn, 1,"邹春庆",0, 0,0,"cb09194bbe53ba6845befe6ecd9d0272ab7b0c76bb77147a97ef9c7eb5e1a482abc7d90de8936707d2c5ce00c26c0f02139efc0720b87c0e2f48bc02cefeec0551199a06d3abe862d047a966546b7b3ae496f198f4943796db30b166f91e5ff1177be39676ecaf2793c8a696e94f601b95dc6b4b5230e3c0cd336de4a438ce82d5d6a61f197090ed0b7ffeee4b09022f(100)0100320001(3)83604024cb091a4ea6578a6a05be9e6e0d9d62722a7bcc763a77d47a94effc7e34e1c48228c7a90d0897671ed246eb02006d66040198ec1e41b8dc03ec58c4646ede3c2a9339dfa88da91ee8774389e6d6676ba08c8c70509f8cbf1a9438f3fd719f3200065bb97eeaed6e07e3d4641bc40b6a27735056b41324528bca33503745285aafd3ffa427d93bb6ff55ee9b647130022f4be30c6f(108)");
                        //thbio 3.0
                        setuserinfo(g_now_sn, 1, "邹春庆", 0, 0, 0, "c51a6b00d9860af9ff3e19c769889355d08fa7c958877bc1d80bb8c36b8963d2789388d51c8b0bfdf97839048b8a8c16300b6a8749884c35c0d1c9082889fc71e1640b112c898c85d95a0b8f348924a5d8d5d98b4286f4cee0480a0438887ce1d8ce09893f87d501d08a19032f893576e1080c454486b58de7c0294446897c41b0e19ba6928b7c0d1fffd744268c74b5ff79fcc4358b94e5ff33fa87a78b5d26ef7fc94b5a857b95e7c5fa42bd85fd65ff7e188336885631d905fb86a589ae3116be1fc23289de3df83bffc340876e82cfc1fd45(272)28471547254213a3263581334462457f348445f41673f191352f3328f4152540893663833243454f2f2637f9f44262(65)146bd91d1191ae92382630a18b92185203a39cb3bc061020d9b9975601b199b4bb2214709590160a32d2de924e47443320c1f8053391ce8580155431cea1456210e1c8a119163682bc800c593076b292806c056416914b1b4061aba1c23c1614349102584104dc7159514293ceb12e578443d1f0f56544c87af0(60)090b080f070c060d030a05021304120e10161718(10)8eb0");
                        break;
                    case "getname":
                        getusername(g_now_sn, 1);
                        break;
                    case "setname":
                        setusername(g_now_sn);
                        break;
                    case "deleteuser":
                        deleteuser(g_now_sn, 1, 13); //0~9 :fp  10 pwd ;11: card ;12: all fp ;13 :all(fp pwd card name)
                        break;
                    case "cleanuser":
                        cleanuser(g_now_sn);
                        break;
                    case "getnewlog":
                        getnewlog(g_now_sn);
                        break;
                    case "getalllog":
                        getalllog(g_now_sn);
                        break;
                    case "cleanlog":
                        cleanlog(g_now_sn);
                        break;
                    case "initsys":
                        initsys(g_now_sn);
                        break;
                    case "cleanadmin":
                        cleanadmin(g_now_sn);
                        break;
                    case "setdevinfo":
                        setdevinfo(g_now_sn);
                        break;
                    case "getdevinfo":
                        getdevinfo(g_now_sn);
                        break;
                    case "opendoor":
                        opendoor(g_now_sn);
                        break;
                    case "setdevlock":
                        setdevlock(g_now_sn);
                        break;
                    case "getdevlock":
                        getdevlock(g_now_sn);
                        break;
                    case "setuserlock":
                        setuserlock(g_now_sn);
                        break;
                    case "getuserlock":
                        getuserlock(g_now_sn, 2);
                        break;
                    case "deleteuserlock":
                        deleteuserlock(g_now_sn, 1);
                        break;
                    case "cleanuserlock":
                        cleanuserlock(g_now_sn);
                        break;
                    case "reboot":
                        reboot(g_now_sn);
                        break;
                    case "settime":
                        settime(g_now_sn);
                        break;
                    case "disabledevice":
                        disabledevice(g_now_sn);
                        break;
                    case "enabledevice":
                        enabledevice(g_now_sn);
                        break;
                }
            }
            Console.ReadKey();
            // wsServer.Setup()
        }

        private static void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("SessionClosed");
        }

        private static void WsServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("NewDataReceived");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, string message)
        {
            try
            {
                JObject jsonMsg = JObject.Parse(message);
                var cmd = jsonMsg.Value<string>("cmd");
                var ret = jsonMsg.Value<string>("ret");
                string strRespone = string.Empty;
                if (string.IsNullOrEmpty(cmd) == false) //client active send data
                {
                    var enrollid = 0;
                    switch (cmd)
                    {

                        case "reg":
                            var sn = jsonMsg.Value<string>("sn");
                            var jobjectdevinfo = jsonMsg["devinfo"];
                            var modelname = jobjectdevinfo.Value<string>("modelname");
                            var usersize = jobjectdevinfo.Value<Int32>("usersize");
                            var fpsize = jobjectdevinfo.Value<Int32>("fpsize");
                            var cardsize = jobjectdevinfo.Value<Int32>("cardsize");
                            var pwdsize = jobjectdevinfo.Value<Int32>("pwdsize");
                            var logsize = jobjectdevinfo.Value<Int32>("logsize");
                            var useduser = jobjectdevinfo.Value<Int32>("useduser");
                            var usedfp = jobjectdevinfo.Value<Int32>("usedfp");
                            var usedcard = jobjectdevinfo.Value<Int32>("usedcard");
                            var usedpwd = jobjectdevinfo.Value<Int32>("usedpwd");
                            var usedlog = jobjectdevinfo.Value<Int32>("usedlog");
                            var usednewlog = jobjectdevinfo.Value<Int32>("usednewlog");
                            var fpalgo = jobjectdevinfo.Value<string>("fpalgo");
                            var firmware = jobjectdevinfo.Value<string>("firmware");
                            var devicetime = jobjectdevinfo.Value<string>("time");
                            DateTime devicetime2 = DateTime.Parse(devicetime);
                            DateTime server = DateTime.Now;  //return the servertime to synchronizes the client device time
                            strRespone = "{\"ret\":\"reg\",\"result\":true,\"cloudtime\":\"" + server.ToString() + "\"}";
                            session.Send(strRespone);
                            var nowsessionid = session.SessionID; //add session id to list      
                            g_now_sn = sn;
                            _registeredDevices.Remove(sn);
                            _registeredDevices.Add(sn, nowsessionid);
                            break;
                        case "sendlog":
                            var count = jsonMsg.Value<string>("count");
                            var attRecords = jsonMsg["record"];
                            foreach (var ss in attRecords)
                            {
                                enrollid = ss.Value<Int32>("enrollid");
                                var time = ss.Value<string>("time");
                                var mode = ss.Value<Int32>("mode");
                                var inout = ss.Value<Int32>("inout");
                                var ievent = ss.Value<Int32>("event");
                                DateTime timelog = DateTime.Parse(time);
                            }
                            server = DateTime.Now;  //return the servertime to synchronizes the client device time
                            strRespone = "{\"ret\":\"sendlog\",\"result\":true,\"cloudtime\":\"" + server.ToString() + "\"}";
                            session.Send(strRespone);
                            break;
                        case "senduser":
                            enrollid = jsonMsg.Value<Int32>("enrollid");
                            var username = jsonMsg.Value<string>("name");  //version v1.1 add 
                            var backupnum = jsonMsg.Value<Int32>("backupnum");
                            var admin = jsonMsg.Value<Int32>("admin");
                            if (backupnum > 0 && backupnum < 10) //is fp
                            {
                                var fpdata = jsonMsg.Value<string>("record");
                            }
                            else if (backupnum == 10) //card
                            {
                                var carddata = jsonMsg.Value<Int32>("record");
                            }
                            else if (backupnum == 11) //pwd
                            {
                                var pwddata = jsonMsg.Value<Int32>("record");
                            }
                            /////////////////
                            server = DateTime.Now;  //return the servertime to synchronizes the client device time
                            strRespone = "{\"ret\":\"senduser\",\"result\":true,\"cloudtime\":\"" + server.ToString() + "\"}";
                            session.Send(strRespone);
                            break;
                        default:
                            break;
                    }
                }
                else if (string.IsNullOrEmpty(ret) == false) //server send cmd and rec data
                {
                    switch (ret)
                    {
                        case "getuserlist":
                            var result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var count = jsonMsg.Value<Int32>("count");
                                var indexfrom = jsonMsg.Value<Int32>("from");
                                var indexto = jsonMsg.Value<Int32>("to");
                                var attRecords = jsonMsg["record"];
                                foreach (var ss in attRecords)
                                {
                                    var enrollid = ss.Value<Int32>("enrollid");
                                    var admin = ss.Value<Int32>("admin");
                                    var backupnum = ss.Value<Int32>("backupnum");
                                    str_userlist[userlistindex].enrollid = enrollid;
                                    str_userlist[userlistindex].backupnum = backupnum;
                                    userlistindex++;
                                }
                                if (indexto < count)
                                {
                                    string cmdstring;
                                    cmdstring = "{\"cmd\":\"getuserlist\",\"stn\":false}";
                                    session.Send(cmdstring);
                                }
                                else
                                {
                                    getuserlistreturn = true;
                                }

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getuserinfo":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                tmpuserinfo.enrollid = jsonMsg.Value<Int32>("enrollid");
                                tmpuserinfo.name = jsonMsg.Value<string>("name"); //add version 1.1
                                tmpuserinfo.backupnum = jsonMsg.Value<Int32>("backupnum");
                                tmpuserinfo.admin = jsonMsg.Value<Int32>("admin");

                                if (tmpuserinfo.backupnum >= 0 && tmpuserinfo.backupnum < 10) //is fp
                                {
                                    tmpuserinfo.fpdata = jsonMsg.Value<string>("record");
                                }
                                else if (tmpuserinfo.backupnum == 10) //card
                                {
                                    tmpuserinfo.password = jsonMsg.Value<Int32>("record");
                                }
                                else if (tmpuserinfo.backupnum == 11) //pwd
                                {
                                    tmpuserinfo.password = jsonMsg.Value<Int32>("record");
                                }
                                getuserinfoflag = true;

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "setuserinfo":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                setuserinfoflag = true;
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "deleteuser":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "cleanuser":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getusername":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var nameutf8 = jsonMsg.Value<string>("record");
                                //utf8 change to gb2310 or ascii (option) maybe you can directly save utf8 char
                                //because http format usually is utf8
                                //var namegb2312 = LogHelper.utf8_gb2312(nameutf8);
                                Console.WriteLine(nameutf8);
                                ////////////////////////////////////////////
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "setusername":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getnewlog":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var count = jsonMsg.Value<Int32>("count");
                                var indexfrom = jsonMsg.Value<Int32>("from");
                                var indexto = jsonMsg.Value<Int32>("to");
                                var attRecords = jsonMsg["record"];
                                foreach (var ss in attRecords)
                                {
                                    var enrollid = ss.Value<Int32>("enrollid");
                                    var time = ss.Value<string>("time");
                                    var mode = ss.Value<Int32>("mode");
                                    var inout = ss.Value<Int32>("inout");
                                    var ievent = ss.Value<Int32>("event");
                                }
                                if (indexto < count)
                                {
                                    string cmdstring;
                                    cmdstring = "{\"cmd\":\"getnewlog\",\"stn\":false}";
                                    session.Send(cmdstring);
                                }

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getalllog":
                            Console.WriteLine(message);
                            break;
                        case "cleanlog":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "initsys":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "cleanadmin":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "setdevinfo":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getdevinfo":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var deviceid = jsonMsg.Value<Int32>("deviceid");
                                var language = jsonMsg.Value<Int32>("language");
                                var volume = jsonMsg.Value<Int32>("volume");
                                var screensaver = jsonMsg.Value<Int32>("screensaver");
                                var verifymode = jsonMsg.Value<Int32>("verifymode");
                                var sleep = jsonMsg.Value<Int32>("sleep");
                                var userfpnum = jsonMsg.Value<Int32>("userfpnum");
                                var loghint = jsonMsg.Value<Int32>("loghint");
                                var reverifytime = jsonMsg.Value<Int32>("reverifytime");
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "opendoor":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "setdevlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getdevlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var opendelay = jsonMsg.Value<Int32>("opendelay");
                                var doorsensor = jsonMsg.Value<Int32>("doorsensor");
                                var alarmdelay = jsonMsg.Value<Int32>("alarmdelay");
                                var threat = jsonMsg.Value<Int32>("threat");
                                var InputAlarm = jsonMsg.Value<Int32>("InputAlarm");
                                var antpass = jsonMsg.Value<Int32>("antpass");
                                var interlock = jsonMsg.Value<Int32>("interlock");
                                var mutiopen = jsonMsg.Value<Int32>("mutiopen");
                                var tryalarm = jsonMsg.Value<Int32>("tryalarm");
                                var tamper = jsonMsg.Value<Int32>("tamper");
                                var wgformat = jsonMsg.Value<Int32>("wgformat");
                                var wgoutput = jsonMsg.Value<Int32>("wgoutput");
                                var cardoutput = jsonMsg.Value<Int32>("cardoutput");
                                ///////////////// 
                                var dayzones = jsonMsg["dayzone"];
                                foreach (var days in dayzones)
                                {
                                    JArray day = days.Value<JArray>("day");
                                    foreach (var sections in day)
                                    {
                                        var section = sections.Value<string>("section");
                                        //save the message
                                    }
                                }
                                /////////////////////////////
                                var weekzones = jsonMsg["weekzone"];
                                foreach (var weeks in weekzones)
                                {
                                    JArray week = weeks.Value<JArray>("week");
                                    foreach (var days in week)
                                    {
                                        var day = days.Value<Int32>("day");
                                        ///////
                                        ///save the message
                                    }
                                }

                                var lockgroup = jsonMsg["lockgroup"]; //group //锁组合
                                foreach (var groups in lockgroup)
                                {
                                    var group = groups.Value<Int32>("group");
                                    ///////
                                    ///save the message
                                }
                                /////////////////////////////
                                var nopentimes = jsonMsg["nopentime"]; //normal opn time zone //
                                foreach (var days in nopentimes)
                                {
                                    var nday = days.Value<Int32>("day");
                                    ///////
                                    ///save the message
                                }

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "getuserlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                var enrollid = jsonMsg.Value<Int32>("enrollid");
                                var weekzone = jsonMsg.Value<Int32>("weekzone");
                                var group = jsonMsg.Value<Int32>("group");
                                var startime = jsonMsg.Value<string>("starttime");
                                var endtime = jsonMsg.Value<string>("endtime");
                                DateTime startime2 = DateTime.Parse(startime);
                                DateTime endtime2 = DateTime.Parse(endtime);
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "setuserlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "deleteuserlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "cleanuserlock":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {

                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "disabledevice":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                disablereturn = true;
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        case "enabledevice":
                            result = jsonMsg.Value<bool>("result");
                            if (result == true)
                            {
                                enablereturn = true;
                            }
                            else if (result == false)
                            {
                                var reasoncode = jsonMsg.Value<Int32>("reason");
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session)
        {

            Console.WriteLine("NewSessionConnected" + session.RemoteEndPoint);
            // getuserlist();
        }
        public static void getuserlist(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;

            string cmdstring;
            cmdstring = "{\"cmd\":\"getuserlist\",\"stn\":true}";
            session.Send(cmdstring);
        }
        public static void getuserinfo(string sn, int enrollid, int backupnum)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;

            string cmdstring;
            cmdstring = "{\"cmd\":\"getuserinfo\",\"enrollid\":" + enrollid + ",\"backupnum\":" + backupnum + "}";
            session.Send(cmdstring);
        }
        public static void setuserinfo(string sn, int enrollid, string username, int backupnum, int admin, int carddata, string fpdata)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            ////////////////////////name mustbe utf8  ,if your saved name format is utf8,you can directly send 
            string usernameutf8 = LogHelper.gb2312_utf8(username);
            // string usernameutf8 =username;
            //////////////////////////////////////option end
            if (backupnum >= 0 && backupnum < 10) //is fp
                cmdstring = "{\"cmd\":\"setuserinfo\",\"enrollid\":" + enrollid + ",\"name\":\"" + usernameutf8 + "\",\"backupnum\":" + backupnum + ",\"admin\":" + admin + ",\"record\":\"" + fpdata + "\"}";
            else if (backupnum == 10) //password
                cmdstring = "{\"cmd\":\"setuserinfo\",\"enrollid\":" + enrollid + ",\"name\":\"" + usernameutf8 + "\",\"backupnum\":" + backupnum + ",\"admin\":" + admin + ",\"record\":" + carddata + "}";
            else if (backupnum == 11) //card
                cmdstring = "{\"cmd\":\"setuserinfo\",\"enrollid\":" + enrollid + ",\"name\":\"" + usernameutf8 + "\",\"backupnum\":" + backupnum + ",\"admin\":" + admin + ",\"record\":" + carddata + "}";
            else
                cmdstring = null;
            Console.WriteLine("setuserinfo len=" + cmdstring.Length);
            session.Send(cmdstring);
        }
        public static void deleteuser(string sn, int enrollid, int backupnum)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"deleteuser\",\"enrollid\":" + enrollid + ",\"backupnum\":" + backupnum + "}";
            session.Send(cmdstring);
        }
        public static void cleanuser(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"cleanuser\"}";
            session.Send(cmdstring);
        }
        public static void getusername(string sn, int enrollid)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getusername\",\"enrollid\":" + enrollid + "}";
            session.Send(cmdstring);
        }
        public static void setusername(string sn) //max send cout <=50
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            ////////////////////////name mustbe utf8  ,if your saved name format is utf8,you can directly send 
            // string usernameutf8 = LogHelper.gb2312_utf8(username);
            //////////////////////////////////////option end
            cmdstring = "{\"cmd\":\"setusername\",\"count\":3,\"record\":[{\"enrollid\":1,\"name\":\"" + LogHelper.gb2312_utf8("邹春庆") + "\"},{\"enrollid\":2,\"name\":\"" + LogHelper.gb2312_utf8("周结官") + "\"},{\"enrollid\":3,\"name\":\"" + LogHelper.gb2312_utf8("吉川野子") + "\"}]}";
            //cmdstring = "{\"cmd\":\"setusername\",\"count\":1,\"record\":[{\"enrollid\":1,\"name\":\"" + "Türkçe" + "\"}]}";
            //cmdstring = "{\"cmd\":\"setusername\",\"count\":1,\"record\":[{\"enrollid\":1,\"name\":\"" + "ปิดเครื่อง!" + "\"}]}";
            //cmdstring = "{\"cmd\":\"setusername\",\"count\":1,\"record\":[{\"enrollid\":1,\"name\":\"" + "RFID ترمینال" + "\"}]}";
            session.Send(cmdstring);
        }
        public static void getnewlog(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getnewlog\",\"stn\":true}"; //get new log
            session.Send(cmdstring);
        }
        public static void getalllog(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getalllog\",\"stn\":true}"; //get all log

            session.Send(cmdstring);
        }
        public static void cleanlog(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"cleanlog\"}";
            session.Send(cmdstring);
        }
        public static void initsys(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"initsys\"}";
            session.Send(cmdstring);
        }
        public static void cleanadmin(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"cleanadmin\"}";
            session.Send(cmdstring);
        }
        public static void setdevinfo(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            int deviceid = 1;
            int language = 1;
            int volume = 6;
            int screensaver = 1;
            int verifymode = 0;
            int sleep = 0;
            int userfpnum = 3;
            int loghint = 1000;
            int reverifytime = 5;
            cmdstring = "{\"cmd\":\"setdevinfo\",\"deviceid\":" + deviceid + ",\"language\":" + language + ",\"volume\":" + volume + ",\"screensaver\":" + screensaver + ",\"verifymode\":" + verifymode + ",\"sleep\":" + sleep + ",\"userfpnum\":" + userfpnum + ",\"loghint\":" + loghint + ",\"reverifytime\":" + reverifytime + "}";
            session.Send(cmdstring);
        }
        public static void getdevinfo(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getdevinfo\"}";
            session.Send(cmdstring);
        }
        public static void opendoor(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"opendoor\"}";
            session.Send(cmdstring);
        }
        public static void setdevlock(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            int opendelay = 5;
            int doorsensor = 0;
            int alarmdelay = 0;
            int threat = 0;
            int InputAlarm = 0;
            int antpass = 0;
            int interlock = 0;
            int mutiopen = 0;
            int tryalarm = 0;
            int tamper = 0;
            int wgformat = 0;
            int wgoutput = 0;
            int cardoutput = 0;
            cmdstring = "{\"cmd\":\"setdevlock\",\"opendelay\":" + opendelay + ",\"doorsensor\":" + doorsensor + ",\"alarmdelay\":" + alarmdelay + ",\"threat\":" + threat + ",\"InputAlarm\":" + InputAlarm + ",\"antpass\":" + antpass + ",\"interlock\":" + interlock + ",\"mutiopen\":" + mutiopen + ",\"tryalarm\":" + tryalarm + ",\"tamper\":" + tamper + ",\"wgformat\":" + wgformat + ",\"wgoutput\":" + wgoutput + ",\"cardoutput\":" + cardoutput;
            int i, j;

            //////dayzone:
            int[,,] dayzone = new int[8, 5, 4];
            dayzone.Initialize();
            //"08:00~18:00"
            for (i = 0; i < 8; i++)
            {
                dayzone[i, 0, 0] = i + 1;
                dayzone[i, 0, 1] = 0;
            }
            cmdstring = cmdstring + ",\"dayzone\":[";
            for (i = 0; i < 8; i++)
            {
                cmdstring = cmdstring + "{\"day\":[";
                for (j = 0; j < 5; j++)
                {
                    string section = string.Format("{0:D2}:{1:D2}~{2:D2}:{3:D2}", dayzone[i, j, 0], dayzone[i, j, 1], dayzone[i, j, 2], dayzone[i, j, 3]);

                    cmdstring = cmdstring + "{\"section\":\"" + section + "\"}";
                    if (j < 4)
                        cmdstring = cmdstring + ","; //the enddiong item have no ","
                }
                cmdstring = cmdstring + "]}";
                if (i < 7)
                    cmdstring = cmdstring + ",";  //the enddiong item have no ","
            }
            cmdstring = cmdstring + "]";
            ///weekzone
            int[,] weekzone = new int[8, 7];
            weekzone.Initialize();
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 7; j++)
                    weekzone[i, j] = i * 10 + j;
            }
            cmdstring = cmdstring + ",\"weekzone\":[";
            for (i = 0; i < 8; i++)
            {
                cmdstring = cmdstring + "{\"week\":[";
                for (j = 0; j < 7; j++)
                {
                    cmdstring = cmdstring + "{\"day\":" + weekzone[i, j] + "}";
                    if (j < 6)
                        cmdstring = cmdstring + ","; //the enddiong item have no ","
                }
                cmdstring = cmdstring + "]}";
                if (i < 7)
                    cmdstring = cmdstring + ","; //the enddiong item have no ","
            }
            cmdstring = cmdstring + "]";
            ///lockgroup
            int[] lockgroup = new int[5];
            lockgroup.Initialize();
            for (i = 0; i < 5; i++)
                lockgroup[i] = i;

            cmdstring = cmdstring + ",\"lockgroup\":[";
            for (i = 0; i < 5; i++)
            {
                cmdstring = cmdstring + "{\"group\":" + lockgroup[i] + "}";
                if (i < 4)
                    cmdstring = cmdstring + ","; //the enddiong item have no ","
            }
            cmdstring = cmdstring + "]";

            ///normal open time 常开门时段
            int[] opentmezone = new int[7];
            opentmezone.Initialize();
            for (i = 0; i < 7; i++)
                opentmezone[i] = 0;

            cmdstring = cmdstring + ",\"nopentime\":[";
            for (i = 0; i < 7; i++)
            {
                cmdstring = cmdstring + "{\"day\":" + opentmezone[i] + "}";
                if (i < 6)
                    cmdstring = cmdstring + ","; //the enddiong item have no ","
            }
            cmdstring = cmdstring + "]";
            //all end
            cmdstring = cmdstring + "}";
            session.Send(cmdstring);
        }
        public static void getdevlock(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getdevlock\"}";
            session.Send(cmdstring);
        }
        public static void getuserlock(string sn, int enrollid)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"getuserlock\",\"enrollid\":" + enrollid + "}";
            session.Send(cmdstring);
        }
        public static void setuserlock(string sn) //max send cout <=50
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            int weekzone = 1;
            int group = 1;
            string starttime = "2016-03-25 00:00:00";
            string endtime = "2016-12-30 00:00:00";
            //this sample show id=1 set all ; id=2 jsut set weekzone; id=3 just set group;
            cmdstring = "{\"cmd\":\"setuserlock\",\"count\":3,\"record\":[{\"enrollid\":1,\"weekzone\":" + weekzone + "},{\"enrollid\":2,\"weekzone\":" + weekzone + ",\"group\":" + group + ",\"starttime\":\"" + starttime + "\",\"endtime\":\"" + endtime + "\"},{\"enrollid\":3,\"group\":" + group + "}]}";
            session.Send(cmdstring);
        }
        public static void deleteuserlock(string sn, int enrollid)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"deleteuserlock\",\"enrollid\":" + enrollid + "}";
            session.Send(cmdstring);
        }
        public static void cleanuserlock(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"cleanuserlock\"}";
            session.Send(cmdstring);
        }

        public static void reboot(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"reboot\"}";
            session.Send(cmdstring);
        }
        public static void settime(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            DateTime server = DateTime.Now;
            cmdstring = "{\"cmd\":\"settime\",\"cloudtime\":\"" + server.ToString() + "\"}";
            session.Send(cmdstring);
        }
        public static void disabledevice(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"disabledevice\"}";
            session.Send(cmdstring);
        }
        public static void enabledevice(string sn)
        {
            WebSocketSession session;
            session = GetSessionByID(sn);
            if (session == null)
                return;
            string cmdstring;
            cmdstring = "{\"cmd\":\"enabledevice\"}";
            session.Send(cmdstring);
        }

    }
}
