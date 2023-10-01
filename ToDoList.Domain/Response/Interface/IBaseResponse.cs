using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Response.Interface
{
    public interface IBaseResponse<T>
    {
        public T Data { get; set; }
        public StatusCode StatusCode { get; set; }
        public string Description { get; set; }
    }
}
