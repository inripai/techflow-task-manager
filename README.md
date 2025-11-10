# TechFlow Task Manager

## Visão geral
TechFlow Task Manager é um aplicativo web ASP.NET Core MVC que ajuda a equipe da TechFlow a organizar as atividades diárias, mantendo o contexto do usuário autenticado em sessão.

## Funcionalidades
- Autenticação via formulário com armazenamento em sessão, incluindo login, logout e carregamento automático de uma lista inicial de tarefas ao autenticar.
- Painel de tarefas com saudação personalizada, tabela responsiva, indicadores de status e ações de edição ou exclusão com confirmação.
- Formulários para criar e editar tarefas com validação de título obrigatório, descrição opcional e opção de marcar como concluída.
- Armazenamento das tarefas na sessão do usuário serializadas em JSON, preservando os dados durante a sessão ativa.

## Tecnologias
- .NET 9 e ASP.NET Core MVC.
- Bootstrap 5 e jQuery para a interface.
- Cache de sessão em memória do ASP.NET Core.

## Pré-requisitos
- .NET SDK 9.0 ou superior instalado.

## Como executar
1. Restaurar as dependências: `dotnet restore`
2. Executar a aplicação: `dotnet run --project techflow-task-manager/techflow-task-manager.csproj`
3. Acessar o endereço exibido no console (HTTPS habilitado por padrão) em um navegador compatível.
4. Informar qualquer combinação de usuário e senha para iniciar a sessão e carregar as tarefas padrão.

## Estrutura do projeto

techflow-task-manager/
├── Controllers/
├── Models/
├── Views/
├── Extensions/
├── Constants/
├── wwwroot/
└── Program.cs


## Próximos passos sugeridos
- Substituir as credenciais simuladas por um mecanismo real de autenticação.
- Persistir as tarefas em um repositório compartilhado (por exemplo, banco de dados).
- Acrescentar testes automatizados que cubram os fluxos de login e de CRUD de tarefas.

Referências do código que embasam o conteúdo proposto:

Configuração da aplicação ASP.NET Core, com MVC, cache distribuído em memória, sessões, HTTPS e rota padrão.

Projeto direcionado ao .NET 9 e namespace principal.

Fluxos de login, logout, definição do nome do usuário e carregamento das tarefas padrão na autenticação.

Operações de listagem, criação, edição, exclusão e persistência em sessão para as tarefas, além de validações do modelo.

Interface com saudação, tabela responsiva, formulários de criação/edição e uso dos componentes Bootstrap no layout principal.

Extensões para serializar objetos na sessão com JSON e estrutura de diretórios do projeto.
