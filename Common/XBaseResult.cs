using System.Net;

namespace WebXeDapAPI.Common
{
    [Serializable]
    public class XBaseResult<T> where T : class
    {
        public bool success { get; set; }

        public int httpStatusCode { get; set; }

        public string message { get; set; }

        public T data { get; set; }

        public int totalCount { get; set; }
        public bool isBirthday { get; set; }
        public XBaseResult()
        {
            success = true;
            httpStatusCode = (int)HttpStatusCode.OK;
        }

        public XBaseResult(XBaseResult<T> obj)
        {
            success = obj.success;
            httpStatusCode = obj.httpStatusCode;
            message = obj.message;
            data = obj.data;
            totalCount = obj.totalCount;
            isBirthday = obj.isBirthday;
        }
    }

    [Serializable]
    public class XBaseResult : XBaseResult<dynamic>
    {
    }

}
