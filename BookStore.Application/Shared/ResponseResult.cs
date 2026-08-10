namespace BookStore.Application.Shared
{
    public enum Result
    {
        Failed = 0,
        Success = 1,
        Exist = 2,
        NoDataFound = 3,
        NotExsit = 4,
        Unauthorized = 5,
    }
    public class ResponseResult
    {
        public Result Result { get; set; }
        public int Code { get; set; }
        public object? DataCount { get; set; }
        public object? Data { get; set; }
        public int TotalCount { get; set; }
        public string MessageAr { get; set; } = string.Empty;
        public string MessageEn { get; set; } = string.Empty;
    }
}
