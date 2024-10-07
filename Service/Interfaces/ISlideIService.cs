using WebXeDapAPI.Dto;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface ISlideIService
    {
        Task<SlideDto> Create(SlideDto slideDto);
        string Update(UpdateSlideDto updateSlideDto);
        bool Delete(int Id);
        byte[] GetSileBytesImage(string imagePath);
        byte[] GetSlideBytesImageid4(string imagePath);
        byte[] GetSlideBytesImageid5(string imagePath);
    }
}
