using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace TigrinhoGame.Infrastructure.Services.AWS
{
    public interface IGameAssetsService
    {
        Task<string> UploadSymbolImageAsync(string symbolCode, Stream imageStream);
        Task<Dictionary<string, string>> GetAllSymbolUrlsAsync();
        Task<bool> UpdateSymbolImageAsync(string symbolCode, Stream newImageStream);
    }

    public class GameAssetsService : IGameAssetsService
    {
        private readonly IAwsStorageService _storageService;
        private readonly string _symbolsFolder = "symbols";

        public GameAssetsService(IAwsStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<string> UploadSymbolImageAsync(string symbolCode, Stream imageStream)
        {
            var fileName = $"{_symbolsFolder}/{symbolCode.ToLower()}.png";
            return await _storageService.UploadFileAsync(fileName, imageStream);
        }

        public async Task<Dictionary<string, string>> GetAllSymbolUrlsAsync()
        {
            // Aqui você implementaria a lógica para listar todos os símbolos
            // usando ListObjectsV2Request do S3
            // Por enquanto, retornamos um dicionário vazio
            return new Dictionary<string, string>();
        }

        public async Task<bool> UpdateSymbolImageAsync(string symbolCode, Stream newImageStream)
        {
            try
            {
                var fileName = $"{_symbolsFolder}/{symbolCode.ToLower()}.png";
                await _storageService.DeleteFileAsync(fileName);
                await _storageService.UploadFileAsync(fileName, newImageStream);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 