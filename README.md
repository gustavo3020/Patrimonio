# Patrimonio

Sistema pessoal para gerenciamento de patrimônio e finanças.

## Estrutura

```text
src/
├── Core/
│   ├── Patrimonio.Api/
│   └── Patrimonio.Wpf/
│
└── ...
```

## Tecnologias

- .NET 10
- ASP.NET Core
- Entity Framework Core
- OpenAPI
- PostgreSQL
- Scalar
- Serilog
- WPF

## Arquitetura

A solução será organizada por módulos de negócio.

Cada módulo poderá possuir:

- Api
- Application
- Contracts
- Domain
- Infrastructure
- WPF

O core da aplicação contém os projetos de entrada e componentes
compartilhados pelo sistema.

## Desenvolvimento

### API

Executar o projeto `Patrimonio.Api`.

### WPF

Executar o projeto `Patrimonio.WPF`.