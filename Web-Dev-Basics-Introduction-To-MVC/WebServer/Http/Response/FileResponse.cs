namespace WebServer.Http.Response
{
    using global::WebServer.Common;
    using global::WebServer.Enums;
    using global::WebServer.Exceptions;
    using System;

    public class FileResponse : HttpResponse
    {
        private const string Attachment = "attachment";

        public FileResponse(HttpStatusCode statusCode, byte[] fileData)
        {
            CoreValidator.ThrowIfNull(fileData, nameof(fileData));
            this.ValidateStatusCode(statusCode);

            this.FileData = fileData;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentLength, this.FileData.Length.ToString());
            this.Headers.Add(HttpHeader.ContentDisposition, Attachment);
        }

        public byte[] FileData { get; private set; }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            int statusCodeNumber = (int)statusCode;

            if (statusCodeNumber < 300 || 
                statusCodeNumber >= 400)
            {
                throw new InvalidResponseException("File responses need a status code above 300 and below 400.");
            } 
        }
    }
}
