using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DesktopShop.API.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("upload-avatar")]
        public async Task<IHttpActionResult> UploadAvatar()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Không có file nào được chọn.");

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();
            if (file == null || file.Headers.ContentLength == 0)
                return BadRequest("Không có file nào được chọn.");

            // File size limit: 2MB
            if (file.Headers.ContentLength > 2 * 1024 * 1024)
                return BadRequest("Kích thước file không được vượt quá 2MB.");

            var fileName = file.Headers.ContentDisposition.FileName?.Trim('"') ?? "avatar.jpg";
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .webp).");

            var newFileName = $"avatar_{Guid.NewGuid()}{ext}";
            var uploadsFolder = HttpContext.Current.Server.MapPath("~/images/avatars");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, newFileName);
            var bytes = await file.ReadAsByteArrayAsync();
            File.WriteAllBytes(filePath, bytes);

            var avatarUrl = $"/images/avatars/{newFileName}";
            return Ok(new { avatarUrl });
        }
    }
}
