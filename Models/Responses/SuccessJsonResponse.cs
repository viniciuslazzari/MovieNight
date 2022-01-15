namespace CinemaApi.Models
{
    public class SuccessJsonResponse
    {
        public string Status { get; set; } = "success";
        public string Message { get; set; } = null;
        public object Data { get; set; } = null;

        public SuccessJsonResponse(string message = null, object data = null)
        {
            Message = message;
            Data = data;
        }

        public SuccessJsonResponse(object data)
        {
            Data = data;
        }

        public SuccessJsonResponse(string message)
        {
            Message = message;
        }
    }
}
