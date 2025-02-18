namespace OrderManagementAPI.Dtos
{
    public class ApiResponse<T>
    {
        public Status Status { get; set; } 
        public string ResultMessage { get; set; } 
        public int? ErrorCode { get; set; } 
        public T Data { get; set; } 
    }

    public enum Status
    {
        Success,
        Failed
    }

}
