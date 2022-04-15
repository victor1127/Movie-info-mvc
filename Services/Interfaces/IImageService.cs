using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MovieProDemo.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> ConvertFileToByteArray(IFormFile poster);
        Task<byte[]> ConvertImagePathToByteArray(string poster);
        string ConvertByteArrayToFile(byte[] posterData, string type);
    }
}
