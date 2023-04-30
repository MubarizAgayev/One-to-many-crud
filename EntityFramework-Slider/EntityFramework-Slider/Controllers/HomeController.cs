using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.interfaces;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly ICategoryService _categoryService;
        public HomeController(AppDbContext context,
                             IProductService productService,
                             IBasketService basketService,
                             ICategoryService categoryService)
        {
            _context = context;
            _basketService = basketService;
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _context.Sliders.Where(m=>!m.SoftDelete).ToListAsync();

            SliderInfo sliderInfo = await _context.SliderInfos.OrderByDescending(m=>m.Id).FirstOrDefaultAsync();

            IEnumerable<Category> categories = await _categoryService.GetAll();

            IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).ToListAsync();

            IEnumerable<Expert> experts = await _context.Experts.ToListAsync();

            


            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Categories = categories,
                Products = products,
                Experts = experts
                
            };

            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            Product? dbProduct = await _productService.GetById((int)id);

            if (dbProduct == null) return NotFound();

            List<BasketVM> basket = _basketService.GetBasketDatas();

            

            BasketVM? existProduct = basket.FirstOrDefault(m=>m.Id == dbProduct.Id);


            _basketService.AddProductToBasket(existProduct, dbProduct, basket);

            

            int basketCount = basket.Sum(m => m.Count);

            return Ok(basketCount);
        }




        

    }


    
}