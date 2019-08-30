using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using System.Drawing;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Rent.Helpers;
using Rent.Repositories;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        IStorage _storage;

        public ImageController(IStorage storage)
        {
            _storage = storage;
        }

        [HttpGet("{imageLocation}")]
        public async Task<IActionResult> Get(string imageLocation) {
            byte[] bitmap = await _storage.GetImage(imageLocation);
            return Ok(bitmap);
        }

        [HttpGet("{imageLocation}")]
        public async Task<IActionResult> GetPng(string imageLocation)
        {
            byte[] bitmap = await _storage.GetImage(imageLocation);
            return File(bitmap, "image/jpeg");

        }

        [HttpPost("AddContainer/{containerName}")]
        public async Task<IActionResult> AddContainer([FromRoute] string containerName)
        {
            var result = await _storage.CreateContainer(containerName);
            if (!result)
            {
                return BadRequest("Could not create container");
            }
            return NoContent();
        }

        const int quality = 75;


        [HttpGet("resize/{imageLocation}/{size}")]
        public async Task<IActionResult> Test([FromRoute] string imageLocation, [FromRoute] int size)
        {
            byte[] byteArray = await _storage.GetImage(imageLocation);

            Stream stream = new MemoryStream(byteArray);
            int width, height;


            using (var image = new Bitmap(System.Drawing.Image.FromStream(stream)))
            {
                if (image.Width > image.Height)
                {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                }
                else
                {
                    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                    height = size;
                }
                var resized = new Bitmap(width, height);

                //byteArray = GetBytesOfImage(resized);


                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);

                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    var codec = ImageCodecInfo.GetImageDecoders()
                        .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    byteArray = GetBytesOfImage(resized);
                }

            }

            return File(byteArray, "image/jpeg");
        }

        public static byte[] GetBytesOfImage(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
