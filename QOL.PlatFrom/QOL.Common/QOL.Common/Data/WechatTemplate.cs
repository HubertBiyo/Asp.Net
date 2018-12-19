namespace QOL.Common.Data
{
    public class WechatTemplate
    {

        public string AlarmLocation { get; set; }
        public string AlarmType { get; set; }
        public string AlarmDescription { get; set; }
        public string AlarmDateTime { get; set; }

        public string OpenIds { get; set; }
        
        /// <summary>
        /// 所在园所
        /// </summary>
        public string AccountName { get; set; }

    }
}
