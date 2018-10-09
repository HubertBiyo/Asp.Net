using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Hubert.DefendService.SendEmail;
using System.Configuration;
using Hubert.DefendService.Log;

namespace Hubert.DefendService
{
    public partial class Service : ServiceBase
    {
        private static string currentExePath = string.Empty;
        public Service()
        {
            InitializeComponent();
            currentExePath = AppDomain.CurrentDomain.BaseDirectory;
        }
        protected override void OnStart(string[] args)
        {
            //服务启动时开启定时器
            _timer.Interval = _trimerInterval;
            _timer.Enabled = true;
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
            Logger.Info(currentExePath + "监听服务的守护进程开启", GetType());
        }

        protected override void OnStop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                Logger.Info(currentExePath + "监听服务的守护进程停止", GetType());
            }
        }
        private static object _lock = new object();
        /// <summary>
        /// 时间间隔
        /// </summary>
        private const int _trimerInterval = 20000;
        /// <summary>
        /// windows服务名称
        /// </summary>
        private static string myServiceName = ConfigurationManager.AppSettings["servicesName"];
        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer _timer = new Timer();

        private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            string[] Services = myServiceName.Split(',');

            //循环遍历所监控的服务
            foreach (string serviceName in Services)
            {
                Logger.Info("判断所监听的服务是否停掉" + serviceName, typeof(Service));
                if (!CheckService(serviceName))
                {
                    StartService(serviceName);
                }
            }

        }
        private static bool CheckService(string serviceName)
        {
            bool result = true;
            try
            {
                lock (_lock)
                {
                    //获取本机所有的服务
                    ServiceController[] services = ServiceController.GetServices();
                    foreach (ServiceController service in services)
                    {
                        if (service.ServiceName.Trim() == serviceName.Trim())
                        {
                            Logger.Info("监听的服务：" + serviceName.Trim() + "   服务状态：" + service.Status + "以及判断条件：" + ServiceControllerStatus.Stopped + "" + ServiceControllerStatus.StopPending);
                            if ((service.Status == ServiceControllerStatus.Stopped) || (service.Status == ServiceControllerStatus.StopPending))
                            {
                                Logger.Info("服务已停止：" + service.ServiceName + "状态：" + service.Status, typeof(Service));

                                Logger.Info("服务停止,发送邮件", typeof(Service));
                                //服务已停止，发送邮件给通知
                                SendEmail(serviceName);
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Logger.Info("监听服务是否停止的结果：" + result);
            return result;
        }
        /// <summary>
        /// 开启指定服务
        /// </summary>
        /// <param name="serviceName">检测的服务名称</param>
        private static void StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
                        //开启服务
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        Logger.Info(currentExePath + string.Format("启动服务:{0}", serviceName), typeof(Service));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("StartService is error", typeof(Service), ex);
            }
        }
        private void StopService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
                        service.Stop();
                        //直到服务停止
                        service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                        Logger.Info(currentExePath + string.Format("停止服务:{0}", serviceName), typeof(Service));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("StopService is error", typeof(Service), ex);
            }
        }

        public static void SendEmail(string serviceName)
        {
            string senderServerIp = ConfigurationManager.AppSettings["senderServerIp"];   //使用163代理邮箱服务器（也可是使用qq的代理邮箱服务器，但需要与具体邮箱对相应）
            string toMailAddress = ConfigurationManager.AppSettings["toMailAddress"];            //
            string fromMailAddress = ConfigurationManager.AppSettings["fromMailAddress"];//你的邮箱
            string subjectInfo = ConfigurationManager.AppSettings["subjectInfo"];             //主题
            string bodyInfo = "<p>" + "您好，" + ConfigurationManager.AppSettings["PlatIP"] + "   " + serviceName + "服务已经挂掉，并将服务重启，请知晓!" + "</p>";//以Html格式发送的邮件
            string mailUsername = ConfigurationManager.AppSettings["mailUsername"];          //登录邮箱的用户名
            string mailPassword = ConfigurationManager.AppSettings["mailPassword"]; //对应的登录邮箱的第三方密码（你的邮箱不论是163还是qq邮箱，都需要自行开通stmp服务）
            string mailPort = ConfigurationManager.AppSettings["mailPort"];                    //发送邮箱的端口号
            //创建发送邮箱的对象
            Email myEmail = new Email(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
            Logger.Info("发送的邮件:" + toMailAddress + "服务停止后邮件发送状态" + myEmail.Send(), typeof(Service));
        }

    }
}
