using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface ISlideInterface
    {
        ICollection<Slide> GetSlides();
        Slide GetSlideImage(int slideId);
        Slide GetSlideImageid4(int slideId = 4);
        Slide GetSlideImageid5(int slideId = 5);
    }
}
