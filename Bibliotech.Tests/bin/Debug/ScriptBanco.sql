
    if exists (select * from dbo.sysobjects where id = object_id(N'[Livro]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Livro]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Usuario]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Usuario]

    create table [Livro] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Titulo NVARCHAR(200) not null,
       Isbn NVARCHAR(15) not null,
       AnoPublicao INT null,
       Edicao INT null,
       NumeroPaginas INT null,
       Assunto NVARCHAR(200) null,
       NomeArquivo NVARCHAR(300) null,
       primary key (Id)
    )

    create table [Usuario] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Nome NVARCHAR(200) not null,
       Telefone NVARCHAR(15) null,
       DataNascimento DATETIME null,
       Login NVARCHAR(200) not null,
       Email NVARCHAR(200) not null,
       Senha NVARCHAR(255) not null,
       Perfil INT null,
       DataAlteracaoSenha DATETIME null,
       NomeFoto NVARCHAR(255) null,
       primary key (Id)
    )
