using NLog;

namespace TLWebAPI
{
    public static class NLogConfig
    {
        public static Logger logger = LogManager.GetCurrentClassLogger(); //suggested readonly
    }
}
