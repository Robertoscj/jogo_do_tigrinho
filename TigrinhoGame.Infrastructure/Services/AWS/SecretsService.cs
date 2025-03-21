using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace TigrinhoGame.Infrastructure.Services.AWS
{
    public interface ISecretsService
    {
        Task<string> GetSecretAsync(string secretName);
        Task<T> GetSecretAsync<T>(string secretName) where T : class;
        Task UpdateSecretAsync(string secretName, string secretValue);
    }

    public class SecretsService : ISecretsService
    {
        private readonly IAmazonSecretsManager _secretsManager;
        private readonly string _secretPrefix;

        public SecretsService(IAmazonSecretsManager secretsManager, IConfiguration configuration)
        {
            _secretsManager = secretsManager;
            _secretPrefix = configuration["AWS:SecretsManager:SecretName"] ?? "tigrinho/api/secrets";
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                var request = new GetSecretValueRequest
                {
                    SecretId = $"{_secretPrefix}/{secretName}"
                };

                var response = await _secretsManager.GetSecretValueAsync(request);
                return response.SecretString;
            }
            catch (ResourceNotFoundException)
            {
                throw new KeyNotFoundException($"Segredo '{secretName}' não encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao recuperar o segredo '{secretName}': {ex.Message}");
            }
        }

        public async Task<T> GetSecretAsync<T>(string secretName) where T : class
        {
            var secretString = await GetSecretAsync(secretName);
            return JsonSerializer.Deserialize<T>(secretString) 
                ?? throw new Exception($"Não foi possível deserializar o segredo '{secretName}' para o tipo {typeof(T).Name}");
        }

        public async Task UpdateSecretAsync(string secretName, string secretValue)
        {
            try
            {
                var request = new UpdateSecretRequest
                {
                    SecretId = $"{_secretPrefix}/{secretName}",
                    SecretString = secretValue
                };

                await _secretsManager.UpdateSecretAsync(request);
            }
            catch (ResourceNotFoundException)
            {
                var createRequest = new CreateSecretRequest
                {
                    Name = $"{_secretPrefix}/{secretName}",
                    SecretString = secretValue
                };

                await _secretsManager.CreateSecretAsync(createRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar o segredo '{secretName}': {ex.Message}");
            }
        }
    }
} 