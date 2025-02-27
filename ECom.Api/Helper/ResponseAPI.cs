namespace ECom.Api.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(StatusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "The request has succeeded.",
                201 => "The request has succeeded and a new resource has been created.",
                204 => "The request has succeeded and the response body is empty.",
                400 => "The request could not be understood or was missing required parameters.",
                401 => "The request requires user authentication.",
                403 => "The server understood the request, but is refusing to fulfill it.",
                404 => "The requested resource could not be found.",
                405 => "The request method is not supported for the requested resource.",
                500 => "The server has encountered a situation it doesn't know how to handle.",
                _ => "An error occurred while processing the request."
            };
        }
    }

}
