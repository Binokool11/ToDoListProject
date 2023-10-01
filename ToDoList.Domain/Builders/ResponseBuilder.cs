using ToDoList.Domain.Enum;
using ToDoList.Domain.Response;

namespace ToDoList.Domain.Builders
{
    public class ResponseBuilder<T>
    {
        private BaseResponse<T> response { get; set; } = new BaseResponse<T>();

        public void Reset()
        {
            response = new BaseResponse<T>();
        }

        public BaseResponse<T> GetResponse()
        {
            return response;
        }

        public ResponseBuilder<T> SetStatusCode(StatusCode status)
        {
            response.StatusCode = status;
            return this;
        }

        public ResponseBuilder<T> SetMessage(string message)
        {
            response.Description = message;
            return this;
        }

        public ResponseBuilder<T> SetData(T data)
        {
            response.Data = data;
            return this;
        }
    }
}
