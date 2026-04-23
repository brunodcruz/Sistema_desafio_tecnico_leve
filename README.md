# Leve Task System

Sistema web em ASP.NET Core MVC com Razor, arquitetura em camadas com DDD, autenticacao por e-mail/senha, cadastro de usuarios e agendamento de tarefas.

## Tecnologias

- ASP.NET Core MVC com Razor
- C#
- Entity Framework Core
- SQL Server
- UIkit

## Arquitetura

- `src/LeveTaskSystem.Domain`: entidades, enums, contratos de repositorio e contratos de servicos de dominio
- `src/LeveTaskSystem.Application`: DTOs, UseCases e Services de aplicacao
- `src/LeveTaskSystem.Infrastructure`: DbContext, repositorios EF Core, hash de senha, notificador de e-mail SMTP e injecao de dependencia
- `src/LeveTaskSystem.Web`: controllers, views Razor, autenticacao por cookie e composicao da aplicacao

## Usuario inicial

- E-mail: `ti@leveinvestimentos.com.br`
- Senha: `teste123`
- Perfil: gestor

## Banco de dados

### Scripts (modelagem e populacao)

| Arquivo | Finalidade |
|---------|--------------|
| `database/01_create_database.sql` | Cria o banco `LeveTaskSystemDb` (se nao existir), tabelas, restricoes e insere o **usuario gestor inicial** (`ti@leveinvestimentos.com.br`). |

Execute o script no **SQL Server Management Studio**, **Azure Data Studio**, **DBeaver** ou `sqlcmd`, conectado na instancia correta.

### Modelo logico (resumo)

- **Users**: cadastro de pessoas do sistema. Campos principais: identificador (`Id`), nome completo, data de nascimento, telefones, e-mail (unico), endereco, **foto** (`PhotoPath` com URL relativa apos upload em `wwwroot/uploads/users/`), hash da senha, perfil (`Role`), gestor opcional (`ManagerId` para subordinados). Chave estrangeira de `ManagerId` para `Users(Id)`.
- **Tasks**: tarefas atribuidas por um gestor a um usuario. Campos principais: mensagem, data limite, status (pendente ou concluida), responsavel (`AssignedToUserId`), gestor criador (`CreatedByManagerId`), datas de criacao e conclusao. Chaves estrangeiras para `Users`.
- **Role** (coluna inteira em `Users`): `0` = subordinado, `1` = gestor.

## Passo a passo para executar em outra maquina

1. Instale:
   - [.NET SDK 10](https://dotnet.microsoft.com/download)
   - SQL Server (Express ou outra edicao) com o servico do motor em execucao
2. Clone este repositorio na maquina destino.
3. Configure a string de conexao em `src/LeveTaskSystem.Web/appsettings.json`, chave `ConnectionStrings:DefaultConnection`.
   - Instancia padrao (exemplo): `Server=localhost;Database=LeveTaskSystemDb;Trusted_Connection=True;TrustServerCertificate=True;`
   - SQL Server Express nomeada (exemplo no `appsettings.json`, note o `\\` por ser JSON): `"Server=localhost\\SQLEXPRESS;Database=LeveTaskSystemDb;Trusted_Connection=True;TrustServerCertificate=True;"`
   - Ajuste `Server` se o host ou o nome da instancia for diferente.
4. E-mail (SendGrid): crie uma conta em [SendGrid](https://sendgrid.com/), gere uma **API Key** com permissao de **Mail Send** e verifique um **Single Sender** (ou dominio). No projeto, `Host` e `Username` ja vao preenchidos para SMTP SendGrid (`smtp.sendgrid.net` e `apikey`). Preencha `SenderEmail` com o endereco verificado no SendGrid. Guarde a API key fora do Git com User Secrets:
   - `cd src/LeveTaskSystem.Web`
   - `dotnet user-secrets set "Email:Password" "SUA_API_KEY_SENDGRID"` (somente a chave `SG...`, nunca no campo de remetente)
   - `dotnet user-secrets set "Email:SenderEmail" "seu-remetente-verificado@dominio.com"` (o mesmo e-mail aprovado no Single Sender do SendGrid)
   - Conta gratuita pode exigir **sair do sandbox** ou autorizar destinatarios de teste no painel ate verificar dominio. Mensagens transacionais podem cair em **Spam** ate o dominio/remetente estiver bem configurado.
5. Banco de dados: execute `database/01_create_database.sql` na instancia configurada no passo 3.
6. Na raiz do repositorio (pasta que contem `LeveTaskSystem.slnx`), execute:
   - `dotnet restore`
   - `dotnet build LeveTaskSystem.slnx`
   - `dotnet run --project src/LeveTaskSystem.Web/LeveTaskSystem.Web.csproj`
   Se o `build` acusar arquivo em uso, encerre o processo `LeveTaskSystem.Web` ou o terminal onde o site ainda estiver rodando e tente de novo.
7. Abra no navegador a URL exibida no terminal (ex.: `http://localhost:5208`).
8. Entre com o usuario inicial (`ti@leveinvestimentos.com.br` / `teste123`) e cadastre os demais usuarios conforme o fluxo do sistema.

## Regras implementadas

- Apenas usuarios gestores podem cadastrar novos usuarios.
- Apenas usuarios gestores podem cadastrar tarefas.
- Gestor acompanha tarefas da equipe no dashboard.
- Subordinado visualiza tarefas atribuidas e pode concluir tarefas.
- E-mail para subordinado no cadastro da tarefa.
- E-mail para gestor na conclusao da tarefa.

## Observacao sobre envio de e-mails

O envio usa **SMTP SendGrid** quando `Email:Host` e `Email:Password` estao configurados (recomendado via User Secrets).

 Importante: em contas novas do SendGrid (principalmente usando **Single Sender** e sem **Domain Authentication** com SPF/DKIM/DMARC), os e-mails podem cair na caixa de **Spam/Lixo Eletronico**. Para melhor entregabilidade, autentique o dominio e acompanhe o status no painel **Email Activity** do SendGrid.
