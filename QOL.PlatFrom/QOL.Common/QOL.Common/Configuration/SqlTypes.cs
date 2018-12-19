namespace QOL.Common.Configuration
{
    public enum SqlTypes
    {
        /// <summary>
        /// 仅包含只读访问权限的连接串
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// 拥有读写权限的连接串
        /// </summary>
        ReadWrite = 2
    }
}
