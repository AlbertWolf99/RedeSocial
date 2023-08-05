

# Projeto RedeSocial

Para gerar gerar o banco de dados, é necessário rodar o seguinte comando

```powershell
dotnet ef database update
```

E depois para executar o projeto, utilize o comando

```powershell
dotnet run
```

Rodando o modo desenvolvimento, será aberto o Swagger como página inicial exibindo a documentação da API

### Registro de Novos Usuarios

Para realizar o registro de novos usuarios, deve-se utilizar a API /Register informando os dados cadastrais conforme documentação (Swagger).

Após o registro, seguirá para a autenticação conforme próximo tópico.

### Autenticação

Para realizá-la é necessário fazer o login de usuário com a API /User/Login, que retornará o token de autenticação para ser usado.

### Dados de Usuários de Exemplo

Para demonstrar o funcionamento da API, foi adicionado usuarios com posts publicados, com as seguintes credenciais de login:

```
Username: alice
Password: Alice##123
```

```
Username: bob.1998
Password: Bob##321
```

### Requisitos Mínimos

- SQL Server
- Dot Net
- Entity Frameworks Cli Tools

### Ambiente de Desenvolvimento

O projeto foi desenvolvido no seguinte ambiente

- Windows 10
- SQL Server 19.1
- Dot Net 7.0.306
- Entity Frameworks 7.0.9