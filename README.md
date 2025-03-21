# Jogo do Tigrinho - API

API para o jogo de slot machine Tigrinho.

## 🚀 Tecnologias

- .NET 9
- SQL Server
- Dapper
- JWT Authentication 
- Swagger/OpenAPI
- FluentValidation
- Serilog
- AWS SDK (.NET)

## 🌥️ AWS

### Serviços Implementados

#### 1. Amazon S3 e CloudFront
- Gerenciamento de arquivos no S3
- Upload, download e deleção de arquivos
- Integração com CloudFront para CDN

#### 2. AWS Secrets Manager
- Gerenciamento seguro de credenciais
- Armazenamento de chaves JWT
- Configurações sensíveis do sistema

#### 3. Game Assets Service
- Gerenciamento de assets do jogo
- Organização em pastas no S3
- URLs otimizadas via CloudFront

## 🎮 Configurações do Jogo

- RTP (Return to Player): 96%
- Aposta Mínima: R$ 1,00
- Aposta Máxima: R$ 1.000,00
- Saldo Inicial: R$ 1.000,00

## 🛡️ Rate Limiting

- 100 requisições por 10 segundos por usuário/IP
- Implementado usando System.Threading.RateLimiting

## 📝 Logs

- Console logging
- Arquivo de log diário em `logs/tigrinho-.txt`
- Retenção de logs por 7 dias

## 🎮 Funcionalidades

- Sistema de apostas com validação
- Gerenciamento de jogadores
- Histórico de jogadas
- Cálculo de RTP
- Linhas de pagamento configuráveis
- Sistema de bônus
- Monitoramento e segurança 