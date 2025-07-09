namespace ProductionAndStockERP.Helpers
{
    public class ResponseHelper<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }


        public static ResponseHelper<T> Ok(T data, string message = "İşlem başarılı.", int statusCode = 200)
        {
            return new ResponseHelper<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ResponseHelper<T> Fail(string message, int statusCode = 400)
        {
            return new ResponseHelper<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }
    }
}
