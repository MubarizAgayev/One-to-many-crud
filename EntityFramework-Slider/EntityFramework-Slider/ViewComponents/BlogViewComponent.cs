using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.interfaces;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.ViewComponents
{
    public class BlogViewComponent:ViewComponent
    {
        private readonly IBlogService _blogService;

        public BlogViewComponent(IBlogService blogService)
        {
            _blogService= blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogService.GetAll();
            var blogHeader = await _blogService.GetBlogHeader();
            var model = new BlogVM
            {
                Blogs = blogs,
                BlogHeader = blogHeader,
            };
            return await Task.FromResult(View(model));
        }
    }
}
