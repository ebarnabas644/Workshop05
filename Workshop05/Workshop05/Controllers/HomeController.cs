using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Workshop05.Data;
using Workshop05.Models;

namespace Workshop05.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;

        BlobServiceClient serviceClient;
        BlobContainerClient containerClient;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _ctx = ctx;
            _userManager = userManager;
            serviceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=workshop05;AccountKey=viH/S0xgcMW+0YUdYk03yvyvnJD+cWbL6M5HpsJcgcNyx1OBzfhTVE33ZDZiMd+2SAdjZBYhWwc++AStj+IZAQ==;EndpointSuffix=core.windows.net");
            containerClient = serviceClient.GetBlobContainerClient("workshop05");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddPhoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromForm] Advertisement adv, [FromForm] IFormFile photoUpload)
        {
            adv.UserId = _userManager.GetUserId(this.User);

            BlobClient blobClient = containerClient.GetBlobClient(adv.UserId + "_" + adv.CarModel.Replace(" ", "").ToLower());
            using (var uploadFileStream = photoUpload.OpenReadStream())
            {
                await blobClient.UploadAsync(uploadFileStream, true);
            }
            blobClient.SetAccessTier(AccessTier.Cool);
            adv.PhotoUrl = blobClient.Uri.AbsoluteUri;

            _ctx.Advertisements.Add(adv);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(ListPhoto));
        }

        public IActionResult ListPhoto()
        {
            return View(_ctx.Advertisements);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}