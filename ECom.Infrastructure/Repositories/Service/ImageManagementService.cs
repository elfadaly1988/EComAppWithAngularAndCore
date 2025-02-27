using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ECom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace ECom.Infrastructure.Repositories.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc = new List<string>();
            var imageDirectory = Path.Combine("wwwroot", "images", src);
            if (Directory.Exists(imageDirectory) is not true)
            {
                Directory.CreateDirectory(imageDirectory);
            }
            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    var imageName = item.FileName;
                    var imageSource = $"images/{src}/{imageName}";
                    var root = Path.Combine(imageDirectory, item.FileName);
                    using (var fileStream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(fileStream);
                    }
                    saveImageSrc.Add(imageSource);

                }
            }
            return saveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}

