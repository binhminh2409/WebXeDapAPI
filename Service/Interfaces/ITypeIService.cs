using WebXeDapAPI.Dto;
using Type = WebXeDapAPI.Models.Type;
namespace WebXeDapAPI.Service.Interfaces
{
    public interface ITypeIService
    {
        public Type Create(Type type);
        string Update(TypeDto typeDto);
        bool Delete(int typeId);
    }
}
