using Microsoft.AspNetCore.Http;
using MovieProDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class ImageService : IImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public string ConvertByteArrayToFile(byte[] posterData, string extension)
        {
            if (posterData is null) return string.Empty;

            string imageBase64Data = Convert.ToBase64String(posterData);
            return $"data:{extension};base64,{imageBase64Data}";
        }

        public async Task<byte[]> ConvertFileToByteArray(IFormFile poster)
        {
            using MemoryStream memoryStream = new();
            await poster.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        public async Task<byte[]> ConvertImagePathToByteArray(string posterName)
        {
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/img/{posterName}";
            return await File.ReadAllBytesAsync(file);
        }
        public async Task<byte[]> EncodeImageUrlAsync(string imageUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode) return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            return memory.ToArray();
        }
    }
}
