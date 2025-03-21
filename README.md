# Jogo do Tigrinho (Slot Machine API)

Uma API de slot machine desenvolvida em .NET 9 usando Clean Architecture e princípios SOLID.

## 🚀 Tecnologias

- .NET 9
- SQL Server
- Dapper
- AutoMapper
- JWT Authentication
- Swagger/OpenAPI
- FluentValidation
- Serilog
- Health Checks

## 📐 Arquitetura

O projeto segue a Clean Architecture com as seguintes camadas:

### Domain
- Entidades de domínio
- Interfaces de repositório
- Objetos de valor
- Regras de negócio

### Application
- DTOs (Data Transfer Objects)
- Interfaces de serviço
- Implementações de serviço
- Validadores
- Mapeamentos AutoMapper

### Infrastructure
- Implementações de repositório
- Contexto do Dapper
- Serviço RNG (Random Number Generator)
- Configurações de banco de dados

### API
- Controllers
- Middlewares
- Configurações
- Health Checks
- Documentação Swagger

## 🎮 Funcionalidades Principais

### Autenticação
- Registro de jogador
- Login com JWT
- Validação de senha segura
- Proteção contra ataques de força bruta

### Gerenciamento de Jogador
- Atualização de perfil
- Consulta de saldo
- Histórico de jogadas
- Cálculo de RTP individual

### Sistema de Apostas
- Validação de valores mínimos e máximos
- Verificação de saldo suficiente
- Processamento de vitórias
- Cálculo de linhas vencedoras

### Linhas de Pagamento
- Configuração flexível
- Múltiplas linhas ativas
- Diferentes padrões de vitória
- Multiplicadores de prêmio

### Sistema de Bônus
- Free spins
- Símbolos especiais (Wild/Scatter)
- Rodadas bônus
- Multiplicadores especiais

### Monitoramento e Segurança
- Logging detalhado com Serilog
- Monitoramento de performance
- Detecção de atividades suspeitas
- Validação de IP
- Rate limiting
- Health checks

## 🔌 Endpoints da API

### Autenticação
- POST /api/v1/auth/register - Registro de novo jogador
- POST /api/v1/auth/login - Login do jogador

### Jogador
- GET /api/v1/player/balance - Consulta saldo
- GET /api/v1/player/history - Histórico de jogadas
- GET /api/v1/player/rtp - RTP do jogador
- PUT /api/v1/player - Atualização de dados

### Jogo
- POST /api/v1/game/spin - Realizar jogada
- GET /api/v1/game/rtp - RTP atual do jogo
- GET /api/v1/game/limits - Limites de apostas

## 🛠️ Configuração do Banco de Dados

1. Instale o SQL Server
2. Atualize a connection string em `appsettings.json`
3. Execute os scripts de criação do banco:
   - `DadosIniciais.sql`
   - `TigrinhoGame.sql`

## 🚦 Health Checks

A API possui endpoints de health check para monitoramento:

- /health - Status geral da API
- /health-ui - Interface visual do health check

Monitora:
- Conexão com banco de dados
- Disponibilidade do jogo
- RTP dentro dos limites (90% - 97%)

## 🔒 Segurança

### Rate Limiting
- 100 requisições por minuto por usuário
- Bloqueio temporário após exceder limite
- Headers de controle de rate limit

### Validação de IP
- Blacklist de IPs
- Whitelist opcional
- Logging de tentativas bloqueadas

### Monitoramento de Transações
- Detecção de padrões suspeitos
- Monitoramento de win rate
- Análise de RTP individual
- Alertas automáticos

## 📊 Logging

Implementado com Serilog:
- Logs em JSON
- Rotação diária de arquivos
- Retenção por 7 dias
- Níveis configuráveis
- Enriquecimento de contexto

## 🎲 Regras do Jogo

### Matriz do Jogo
- 5 colunas x 3 linhas
- Múltiplas linhas de pagamento
- Símbolos especiais (Wild/Scatter)

### RTP (Return to Player)
- RTP padrão: 95%
- Monitoramento constante
- Ajuste automático

### Limites de Apostas
- Mínimo: Configurável
- Máximo: Configurável
- Precisão: 2 casas decimais

### Símbolos Especiais
- Wild: Substitui outros símbolos
- Scatter: Ativa free spins
- Multiplicadores especiais

## 🤝 Contribuição
- Faz o pix 

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes. 