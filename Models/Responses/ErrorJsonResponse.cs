namespace CinemaApi.Models.Responses
{
    public class ErrorJsonResponse
    {
        public string Status { get; set; } = "error";
        public string Message { get; set; }

        public ErrorJsonResponse(string message)
        {
            Message = message;
        }
    }
}
