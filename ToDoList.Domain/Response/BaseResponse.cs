using ToDoList.Domain.Enum;
using ToDoList.Domain.Response.Interface;

namespace ToDoList.Domain.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public T Data { get; set; } = default(T);
        public StatusCode StatusCode { get; set; }
        public string Description { get; set; } = String.Empty;
    }
}
