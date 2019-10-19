namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Protocoloes",
                c => new
                    {
                        CodProtocolo = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Numero = c.String(nullable: false),
                        Motivo = c.String(nullable: false),
                        Descricao = c.String(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                        Atendente = c.String(),
                        OutrasInfo = c.String(),
                        Resolvido = c.Boolean(nullable: false),
                        Login = c.Int(nullable: false),
                        Usuario_Login = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CodProtocolo)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Login)
                .Index(t => t.Usuario_Login);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Senha = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Login);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Protocoloes", "Usuario_Login", "dbo.Usuarios");
            DropIndex("dbo.Protocoloes", new[] { "Usuario_Login" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Protocoloes");
        }
    }
}
