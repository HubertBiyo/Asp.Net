namespace QOL.Common.Data
{
    public class Result
    {
        private int _code;
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public Result() { }
        public Result(int code, string msg)
        {
            this.Code = code;
            this.Message = msg;
        }
    }
    public class ResultData : Result
    {
        public object Data { get; set; }

        public ResultData() { }
        /// <summary>
        /// 返回带数据的结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        public ResultData(int code, string msg, object data)
        {
            this.Code = code;
            this.Message = msg;
            this.Data = data;
        }
    }
    public class PagingResult : ResultData
    {
        public int Total { get; set; }

        public PagingResult(int code, string msg, object data, int total)
        {
            this.Code = code;
            this.Message = msg;
            this.Data = data;
            this.Total = total;
        }
    }
}
