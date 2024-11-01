using WebXeDapAPI.Data;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Repository
{
    public class SlideRepository : ISlideInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public SlideRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Slide> GetById(int id)
        {
            var slides = _dbContext.Slides
                .Where(x => x.Status == StatusSlide.Active && x.Id == id)
                .OrderBy(x => x.Sort)
                .ToList();

            return slides;
        }

        public Slide GetSlideImage(int slideId)
        {
            return _dbContext.Slides.FirstOrDefault(x => x.Id == slideId);
        }

        public Slide GetSlideImageid4(int slideId = 4)
        {
            return _dbContext.Slides.FirstOrDefault(x => x.Id == slideId);
        }

        public Slide GetSlideImageid5(int slideId = 5)
        {
            return _dbContext.Slides.FirstOrDefault(x => x.Id == slideId);
        }

        public ICollection<Slide> GetSlides()
        {
            var slides = _dbContext.Slides.Where(x => x.Status == StatusSlide.Active).OrderBy(x => x.Sort).ToList();
            return slides;
        }
    }
}
