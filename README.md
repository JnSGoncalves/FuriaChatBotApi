# Furia ChatBot API

Esse projeto tem como objetivo a criação de um chat-bot relacionada aos time de E-Sports da FURIA.
Esse projeto foi realizada com base na criação de uma API Asp .NET Core.
A API de ChatBot permite enviar mensagens para um chatbot e obter respostas inteligentes. Ela oferece endpoints para criar uma sessão, enviar mensagens e obter sugestões com base nas respostas do chatbot.

## Instalação

1. Clone o repositório
2. Instale as dependências
3. Execute o projeto com o comando: `dotnet run`

## APIs externas utilizadas

Essa API utiliza as seguintes APIs externas:

- PandaScoreAPI - [Documentação](https://developers.pandascore.co/docs/introduction)

API utilizada para realizar as consultas sobre os times de E-Sports da FURIA

- WIT.AI - [Documentação](https://wit.ai/docs) - (foi utilizado para a criação do modelo NLP que essa API utiliza) 
> Para visualização ou teste local, será necessário criar uma conta META e criar um novo app importando o [Modelo](furia_chatbot-wit_ai_model.zip) desse repositório, conseguindo assim seu Token para utilização

Caso queira utilizar localmente, adicione seus Tokens de API seguindo os passos de [Localmente](Localmente)

## Fluxo da API
``` mermaid
flowchart TD
    A[Usuário faz pergunta no Frontend] --> B[Frontend envia pergunta para a API]
    B --> C[API recebe a pergunta]
    C --> D[API envia a pergunta para o Wit.ai - modelo NLP]
    D --> E[Wit.ai retorna intents e entities]
    E --> F[API processa as intents e entities]
    F --> G[API faz requisição à API do PandaScore]
    G --> H[PandaScore retorna os dados necessários]
    H --> I[API monta resposta para o usuário]
    I --> J[Frontend exibe a resposta ao usuário]
```

## ☕ Utilizando Furia ChatBot API

O projeto possui configurações específicas que separam a utilização localmente e em produção

### Em produção

Inicialmente essa API não possui ferramentas de autenticação para utilização.

A URL base para utilização dos endpoints atualmente em produção é:
```
https://furiachatbotapi-eqhme9g5g0d5c4ah.brazilsouth-01.azurewebsites.net/api/Chat
```

Caso queira testar o funcionamento a API, entre no site demo a seguir: [ChatBot](https://furiachatbotapi-eqhme9g5g0d5c4ah.brazilsouth-01.azurewebsites.net/)
O site acima apresenta uma landing page básica que demonstra sua utilização.

### Localmente

Passos para Testar Localmente

1. **Clone o repositório**

   Comece clonando o repositório para a sua máquina local. No terminal, execute:

   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd FuriaChatBotApi
   ```

2. **Crie um arquivo .env**

   Para a utilização local é necessário a criação do arquivo ``.env`` na pasta ``\FuriaChatBotApi``, para a configuração dos seus Tokens
   Exemplo do arquivo:
   ```
   WIT_AI_TOKEN=SEU_WIT_AI_TOKEN
   PANDA_SCORE_TOKEN=SEU_PANDA_SCORE_TOKEN
   ```

3. **Instale as dependências**

   Se você ainda não tem o .NET 8 instalado, você pode baixá-lo em [dotnet.microsoft.com](dotnet.microsoft.com).
   Para garantir que todas as dependências sejam instaladas corretamente, execute o seguinte comando:
   ```bash
   dotnet restore
  
5. **Inicie o projeto**
   Abra o projeto no Visual Studio 2022 e inicie o projeto ou execute via comando:
   ```bash
   dotnet run

O servidor estará rodando em http://localhost:7063 por padrão. Você pode acessar o site demo ou a API através deste endereço.

### Endpoints

Os atuais endpoints do projeto são:

- /getSession (GET) - Faz uma solicitação de criação de um Id de Sessão para armazenamento do contexto anterior das mensagens (a realização de perguntas e outras requisições devem ser feitas passando esse mesmo Id)
> A mensagem de retorno dessa solicitação é uma mensagem inicial que pode ser apresentada para o usuário.
  
- /ask (POST) - Realiza uma requisição através de uma pergunta para a API

As requisições POST devem ser feitas da seguinte maneira:

#### Headers obrigatórios

| Header         | Valor              | Descrição                                          |
| -------------- | ------------------ | -------------------------------------------------- |
| `Accept`       | `text/plain`       | Define o tipo de resposta esperada (`text/plain`). |
| `Content-Type` | `application/json` | Define o formato do corpo da requisição (JSON).    |

#### Exemplo de corpo a ser enviado na requisição (JSON)

```
{
  "sessionId": "(Id de Sessão obtido)",
  "message": "Pergunta ou requisição do usuário"
}
```


Ambas as requisições tem o mesmo padrão de retorno JSON: 

```
{
    "status": 0,
    "sessionId": "0b7d2acc-28b6-43fc-a2de-f439e5a45ca1",
    "reply": "Resposta formatada em string pela API",
    "sugestedOptions": [
        "opção 1",
        "opcão 2",
        "opção 2"
    ]
}
```

#### Status
Código de status da requisição enviada pela API:

| Código (int) | Nome           | Descrição                           |
| ------------ | -------------- | ----------------------------------- |
| 0            | Ok             | Operação bem-sucedida               |
| 1            | NotImplemented | Intenção não implementada           |
| 2            | Invalid        | Intenção inválida ou desconhecida   |
| 3            | InternalError  | Erro interno no servidor ou sistema |

## Tecnologias Utilizadas

- .NET 8
- Blazor
- Entity Framework Core

### ➕ Pacotes/Bibliotecas adicionais utilizados

Este projeto utiliza os seguintes pacotes NuGet:
- [DotNetEnv](https://github.com/tonerdo/dotnet-env)
- [RestSharp](https://restsharp.dev/)

## 📝 Licença

Esse projeto está sob licença MIT. Veja o arquivo [LICENÇA](LICENSE.md) para mais detalhes.
