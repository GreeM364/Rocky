using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models.Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;

namespace Rocky.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(IPostRepository postRepository, UserManager<IdentityUser> userManager,
                              IWebHostEnvironment webHostEnvironment) 
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index(string searchTerm)
        {
            IEnumerable<Post> post = _postRepository.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                post = post.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                         p.Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            return View(post);
        }

        public IActionResult Create()
        {
            var postVM = new PostVM();

            return View(postVM);
        }

        [HttpPost]
        public IActionResult Create(PostVM postVM)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePathPosts;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            postVM.Post.Image = fileName + extension;
            postVM.Post.CreatedDate = DateTime.Now;
            postVM.Post.CreatedByUserId = _userManager.GetUserId(User)!;

            _postRepository.Add(postVM.Post);
            _postRepository.Save();


            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Like(int Id)
        {
            var post = _postRepository.Find(Id);

            post.Like++;

            _postRepository.Update(post);
            _postRepository.Save();

            return RedirectToAction("Index");
        }
    }
}
