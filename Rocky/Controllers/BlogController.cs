using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository;
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

        [Authorize(Roles = WC.AdminRole)]
        public IActionResult Create()
        {
            var postVM = new PostVM();

            return View(postVM);
        }

        [Authorize(Roles = WC.AdminRole)]
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

        public IActionResult Details(int Id)
        {
            var post = _postRepository.Find(Id);

            return View(post);
        }

        [Authorize(Roles = WC.AdminRole)]
        public IActionResult Edit(int Id)
        {
            PostVM postVM = new PostVM();

             postVM.Post = _postRepository.Find(Id);
             if (postVM == null)
                 return NotFound();

             return View(postVM);

        }

        [Authorize(Roles = WC.AdminRole)]
        [HttpPost]
        public IActionResult Edit(PostVM postVM)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            var objFromDb = _postRepository.FirstOrDefault(u => u.Id == postVM.Post.Id, isTracking: false);

            if (files.Count > 0)
            {
                string upload = webRootPath + WC.ImagePathPosts;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                var oldFile = Path.Combine(upload, objFromDb.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                postVM.Post.Image = fileName + extension;
            }
            else
            {
                postVM.Post.Image = objFromDb.Image;
            }

            postVM.Post.CreatedByUserId = objFromDb.CreatedByUserId;
            postVM.Post.CreatedDate = objFromDb.CreatedDate;
            postVM.Post.Like = objFromDb.Like;

            _postRepository.Update(postVM.Post);
            _postRepository.Save();

            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = WC.AdminRole)]
        public IActionResult Delete(int Id)
        {
            if (Id <= 0)
                return NotFound();

            var post = _postRepository.Find(Id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int Id)
        {
            var post = _postRepository.Find(Id);
            if (post == null)
            {
                NotFound();
            }

            _postRepository.Remove(post!);
            _postRepository.Save();

            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");
        }
    }
}
