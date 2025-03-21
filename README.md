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

## 📐 Arquitetura

O projeto segue a Clean Architecture com as seguintes camadas:

### Domain
- Entidades: Player, Symbol, PayLine, Spin, WinningLine
- Interfaces de repositório
- Regras de negócio do jogo
- Value Objects

### Application
- DTOs para requests/responses
- Interfaces de serviço
- Implementação dos casos de uso
- Validadores com FluentValidation
- Mapeamentos

### Infrastructure
- Repositórios com Dapper
- Migrations SQL
- Serviços AWS
- Configurações
- Logging com Serilog

### API
- Controllers REST
- Middlewares
- Autenticação JWT
- Rate Limiting
- Swagger/OpenAPI

## 🌥️ AWS

### Serviços Implementados

#### 1. Amazon S3 e CloudFront
- Gerenciamento de arquivos no S3
- Upload, download e deleção de arquivos
- Integração com CloudFront para CDN
- Tipos MIME automáticos
- Versionamento de assets

#### 2. AWS Secrets Manager
- Gerenciamento seguro de credenciais
- Armazenamento de chaves JWT
- Configurações sensíveis do sistema
- Rotação automática de secrets

#### 3. Game Assets Service
- Gerenciamento de assets do jogo
- Organização em pastas no S3
- URLs otimizadas via CloudFront
- Cache de assets

### Implementações AWS Pendentes

#### 1. Amazon CloudWatch
- Integração com Serilog para logs centralizados
- Métricas personalizadas do jogo
- Dashboards de monitoramento
- Alertas automáticos
- Monitoramento de RTP em tempo real

#### 2. Amazon ElastiCache (Redis)
- Cache distribuído para sessões
- Cache de resultados de jogadas
- Cache de dados frequentes
- Otimização de performance

#### 3. Amazon DynamoDB
- Armazenamento de sessões
- Histórico de jogadas em tempo real
- Leaderboards
- Estatísticas em tempo real

#### 4. AWS WAF & Shield
- Proteção contra DDoS
- Regras de segurança personalizadas
- Proteção contra bots
- Firewall de aplicação web

#### 5. Amazon EventBridge
- Eventos assíncronos do jogo
- Processamento de jackpots
- Notificações em tempo real
- Integrações com outros serviços

#### 6. AWS Lambda
- Processamento de eventos assíncronos
- Cálculos de estatísticas
- Geração de relatórios
- Funções auxiliares

## 🎮 Sistema do Jogo

### Configurações
- RTP (Return to Player): 96%
- Aposta Mínima: R$ 1,00
- Aposta Máxima: R$ 1.000,00
- Saldo Inicial: R$ 1.000,00

### Matriz do Jogo
- 5 colunas x 3 linhas
- 10 linhas de pagamento
- Símbolos especiais (Wild/Scatter)
- Multiplicadores

### Símbolos
- 8 símbolos regulares
- 1 símbolo Wild
- 1 símbolo Scatter
- Multiplicadores: 2x até 500x

### Linhas de Pagamento
- 10 padrões diferentes
- Combinações de 3-5 símbolos
- Pagamento da esquerda para direita
- Multiplicadores por linha

## 🔒 Autenticação

### JWT
- Tokens JWT para autenticação
- Refresh tokens
- Claims personalizadas
- Expiração configurável

### Segurança
- Senha com hash
- Proteção contra força bruta
- Validação de IP
- HTTPS

## 🛡️ Rate Limiting

- 100 requisições por 10 segundos por usuário/IP
- Implementado usando System.Threading.RateLimiting
- Headers de controle
- Blacklist de IPs

## 📝 Logs

- Console logging
- Arquivo de log diário em `logs/tigrinho-.txt`
- Retenção de logs por 7 dias
- Níveis configuráveis
- Contexto enriquecido
- Preparado para CloudWatch

## 🎮 Funcionalidades Implementadas

### Jogador
- Registro de conta
- Login/Logout
- Gerenciamento de saldo
- Histórico de jogadas
- Estatísticas pessoais
- Perfil configurável

### Apostas
- Validação de valores
- Verificação de saldo
- Processamento de vitórias
- Cálculo de linhas vencedoras
- Sistema anti-fraude
- Histórico detalhado

### Jogo
- Geração de rodadas
- Cálculo de RTP
- Linhas de pagamento
- Símbolos especiais
- Sistema de bônus
- Free spins

### Monitoramento
- Performance metrics
- Logging detalhado
- Detecção de anomalias
- Alertas automáticos
- Rate limiting
- Proteção contra abusos

## 🔌 API Endpoints

### Autenticação
- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/refresh-token
- POST /api/auth/logout

### Jogador
- GET /api/player/profile
- GET /api/player/balance
- GET /api/player/history
- PUT /api/player/profile

### Jogo
- POST /api/game/spin
- GET /api/game/config
- GET /api/game/symbols
- GET /api/game/paylines

### Transações
- GET /api/transactions/history
- GET /api/transactions/statistics
- POST /api/transactions/deposit
- POST /api/transactions/withdraw 