using Microsoft.AspNetCore.Http;
using MovieProDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class ImageService : IImageService
    {
        public string ConvertByteArrayToFile(byte[] posterData, string type)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ConvertFileToByteArray(IFormFile poster)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ConvertImagePathToByteArray(string poster)
        {
            throw new NotImplementedException();
        }
    }
}
