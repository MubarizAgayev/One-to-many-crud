using EntityFramework_Slider.Models;

namespace EntityFramework_Slider.Services.interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAll();
        Task<BlogHeader> GetBlogHeader();
    }
}
