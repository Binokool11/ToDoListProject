using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ToDoList.Domain.Extenstions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum valueEnum)
        {
            return valueEnum.GetType()
                            .GetMember(valueEnum.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? "Неопределенный";

        }
    }
}
