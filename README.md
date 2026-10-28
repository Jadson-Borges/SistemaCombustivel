# ⛽ SistemaCombustivel

**Aplicação web para gerenciamento de combustível, desenvolvida com C# e .NET, utilizando Blazor e os princípios da Clean Architecture.**

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-orange)
![C%23](https://img.shields.io/badge/C%23-.NET-blueviolet)
![Blazor](https://img.shields.io/badge/UI-Blazor-purple)
![SQL Server](https://img.shields.io/badge/database-SQL%20Server-red)
![Architecture](https://img.shields.io/badge/architecture-Clean%20Architecture-blue)

> 🚧 **Projeto em desenvolvimento**
>
> A aplicação está em construção. As funcionalidades, regras de negócio e estruturas internas podem sofrer alterações conforme o avanço da implementação.

## 📌 Sobre o projeto

O **SistemaCombustivel** é um projeto de desenvolvimento de uma aplicação web voltada ao gerenciamento de operações relacionadas ao controle de combustível.

A aplicação está sendo construída utilizando C# e o ecossistema .NET, com **Blazor** na camada de apresentação, permitindo a criação de interfaces interativas por meio de componentes Razor.

A solução segue a proposta da **Clean Architecture**, buscando separar as regras de negócio, os casos de uso, a infraestrutura e a interface do usuário.

O desenvolvimento também contempla conceitos como:

- Separação de responsabilidades entre camadas.
- Utilização de DTOs para transferência de dados.
- Persistência de dados com Entity Framework Core.
- Integração com SQL Server.
- Autenticação e gerenciamento de usuários.
- Validação de dados e tratamento de erros.
- Testes automatizados e evolução contínua da aplicação.

O objetivo é construir uma aplicação organizada, segura, testável e preparada para receber novas funcionalidades.

## 🎯 Objetivos do projeto

- Desenvolver uma aplicação web utilizando Blazor e ASP.NET Core.
- Aplicar os princípios da Clean Architecture.
- Organizar o código em camadas com responsabilidades bem definidas.
- Manter as regras de negócio independentes dos detalhes de infraestrutura.
- Utilizar DTOs nos limites apropriados da aplicação.
- Implementar persistência de dados com Entity Framework Core.
- Trabalhar com autenticação e autorização.
- Desenvolver testes para aumentar a confiabilidade do sistema.
- Aplicar boas práticas de segurança e manutenção de código.

## 🛠️ Tecnologias e ferramentas

| Tecnologia | Utilização |
|---|---|
| C# | Linguagem principal |
| .NET | Plataforma de desenvolvimento |
| ASP.NET Core | Infraestrutura da aplicação web |
| Blazor | Construção da interface interativa |
| Razor Components | Organização da interface em componentes |
| Entity Framework Core | Acesso e mapeamento de dados |
| SQL Server | Banco de dados relacional |
| ASP.NET Core Identity | Autenticação e gerenciamento de usuários, conforme implementado |
| DTOs | Transferência estruturada de dados |
| Clean Architecture | Organização e separação de responsabilidades |
| Git | Controle de versão |
| GitHub | Hospedagem do código-fonte |

## 🏗️ Arquitetura da solução

A solução está organizada em projetos dentro da pasta `src`, com uma pasta `tests` destinada aos testes automatizados.

A arquitetura segue a proposta de Clean Architecture, com o objetivo de separar as regras de negócio dos detalhes técnicos da aplicação.

### Domain

Responsável pelos conceitos centrais do negócio, entidades e regras que devem permanecer independentes dos detalhes de apresentação e persistência.

### Application

Responsável pelos casos de uso, contratos, DTOs e pela coordenação das operações da aplicação.

### Infrastructure

Responsável pelas implementações técnicas, incluindo acesso ao banco de dados, persistência com Entity Framework Core e integrações necessárias.

### Web

Responsável pela camada de apresentação, pela interação com o usuário e pela integração com os serviços da aplicação, utilizando Blazor e ASP.NET Core.

### Tests

Destinada aos testes automatizados, permitindo validar regras de negócio, comportamentos e integrações à medida que o projeto evolui.

> A divisão acima descreve as responsabilidades arquiteturais pretendidas. Os detalhes de cada projeto devem acompanhar a implementação atual do código-fonte.

## ⚡ Por que Blazor?

O Blazor permite desenvolver interfaces web interativas utilizando C# e componentes Razor, aproveitando ferramentas e conhecimentos do ecossistema .NET.

Entre os benefícios dessa abordagem estão:

- **Componentização:** criação de componentes reutilizáveis.
- **Interatividade:** resposta às ações do usuário na interface.
- **Integração com .NET:** utilização de serviços e injeção de dependência.
- **Organização:** divisão da interface em componentes com responsabilidades específicas.
- **Manutenibilidade:** facilidade para evoluir a apresentação conforme os requisitos mudam.

O modelo de renderização e interatividade utilizado será documentado de acordo com a configuração efetiva do projeto.

## ⚙️ Pré-requisitos

Para executar o projeto localmente, você precisará de:

- [.NET SDK](https://dotnet.microsoft.com/download), compatível com a versão configurada na solução.
- [SQL Server](https://www.microsoft.com/sql-server), instalado ou acessível.
- [Visual Studio](https://visualstudio.microsoft.com/) ou editor compatível com .NET.
- [Git](https://git-scm.com/).
- Entity Framework Core CLI (`dotnet ef`), para operações com migrations.

## 🚀 Como executar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/Jadson-Borges/SistemaCombustivel.git
```

### 2. Acessar o diretório

```bash
cd SistemaCombustivel
```

### 3. Restaurar as dependências

```bash
dotnet restore SistemaCombustivel.slnx
```

### 4. Configurar o ambiente de desenvolvimento

O projeto utiliza um arquivo de exemplo para documentar as configurações necessárias ao ambiente local.

Localize o arquivo `appsettings.Development.example.json` no projeto Web e utilize-o como modelo para criar o arquivo `appsettings.Development.json` no mesmo diretório.

No PowerShell, caso os arquivos estejam na pasta `src/SistemaCombustivel.Web`, você pode executar:

```powershell
Copy-Item `
  "src/SistemaCombustivel.Web/appsettings.Development.example.json" `
  "src/SistemaCombustivel.Web/appsettings.Development.json"
```

Confira se o arquivo de destino está ignorado pelo Git antes de inserir credenciais reais.

### 5. Configurar o SQL Server

Edite o arquivo local `appsettings.Development.json` e ajuste a connection string `ConnectionStrings:DefaultConnection`.

Exemplo de configuração:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=nome-do-computador\\instancia;Database=Nome-do-banco;User Id=user-sql;Password=SUA-SENHA-AQUI;Encrypt=True;TrustServerCertificate=True"
  },
  "Seed": {
    "Admin": {
      "Nome": "Administrador",
      "Email": "",
      "Senha": ""
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore.Database.Command": "Information"
    }
  }
}
```

Substitua os valores de exemplo pelas informações do seu ambiente.

- `Server`: nome do computador e instância do SQL Server.
- `Database`: nome do banco de dados.
- `User Id`: usuário de conexão com o SQL Server.
- `Password`: senha correspondente ao usuário.
- `Seed:Admin`: configurações do administrador inicial, caso o mecanismo de inicialização de usuários esteja implementado.

**Segurança:** nunca publique senhas reais, tokens ou outras credenciais no repositório. O arquivo de exemplo deve conter apenas valores fictícios.

> O arquivo `appsettings.Development.example.json` é um modelo. O ASP.NET Core não o carrega automaticamente pelo nome; a aplicação utiliza normalmente o `appsettings.Development.json` quando executada no ambiente `Development`, conforme a configuração padrão de carregamento.

### 6. Aplicar as migrations

Com a conexão configurada e o SQL Server acessível, aplique as migrations existentes:

```bash
Update-Database 
```

Esse comando deve ser executado no Package Manager Console do projetos usando o Default project: src\SistemaCombustivel.Infrastructure.

Caso a ferramenta não esteja instalada, consulte a [documentação oficial do Entity Framework Core](https://learn.microsoft.com/ef/core/cli/dotnet) para instalar uma versão compatível com os pacotes do projeto.

### 7. Executar a aplicação

```bash
dotnet run --project src/SistemaCombustivel.Web --environment Development
```

Após a inicialização, acesse no navegador o endereço local informado no terminal.

### 8. Executar os testes

Para executar os testes da solução:

```bash
dotnet test SistemaCombustivel.slnx
```

Os testes disponíveis e os resultados dependem do estado atual dos projetos de teste.

## 🔐 Segurança e configuração

Durante o desenvolvimento, algumas práticas importantes devem ser observadas:

- Não versionar arquivos com credenciais reais.
- Manter configurações específicas para cada ambiente.
- Validar os dados recebidos pela aplicação.
- Implementar autorização no servidor para operações protegidas.
- Não depender apenas da ocultação de links ou botões na interface.
- Garantir consistência nas operações de persistência.
- Revisar as configurações antes de disponibilizar a aplicação em produção.

## 📈 Estado do desenvolvimento

O projeto continua em evolução. As próximas etapas incluem o aprimoramento da arquitetura, a implementação e revisão dos casos de uso, a evolução da interface e o fortalecimento dos testes.

O planejamento deve ser atualizado conforme as funcionalidades forem efetivamente concluídas.

## 👨‍💻 Autor

**Jadson Borges**

Desenvolvedor com foco em C#, .NET, desenvolvimento web e arquitetura de software.

- **GitHub:** [Jadson-Borges](https://github.com/Jadson-Borges)
- **Repositório:** [SistemaCombustivel](https://github.com/Jadson-Borges/SistemaCombustivel)

---

⭐ Projeto em desenvolvimento, com foco na aplicação prática de C#, Blazor, Clean Architecture e boas práticas de engenharia de software.
