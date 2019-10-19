namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposConta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Protocoloes", "Usuario_Login", "dbo.Usuarios");
            DropIndex("dbo.Protocoloes", new[] { "Usuario_Login" });
            DropPrimaryKey("dbo.Usuarios");
            AlterColumn("dbo.Protocoloes", "Usuario_Login", c => c.String(maxLength: 20));
            AlterColumn("dbo.Usuarios", "Login", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Usuarios", "Senha", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.Usuarios", "Login");
            CreateIndex("dbo.Protocoloes", "Usuario_Login");
            AddForeignKey("dbo.Protocoloes", "Usuario_Login", "dbo.Usuarios", "Login");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Protocoloes", "Usuario_Login", "dbo.Usuarios");
            DropIndex("dbo.Protocoloes", new[] { "Usuario_Login" });
            DropPrimaryKey("dbo.Usuarios");
            AlterColumn("dbo.Usuarios", "Senha", c => c.String(nullable: false));
            AlterColumn("dbo.Usuarios", "Login", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Protocoloes", "Usuario_Login", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Usuarios", "Login");
            CreateIndex("dbo.Protocoloes", "Usuario_Login");
            AddForeignKey("dbo.Protocoloes", "Usuario_Login", "dbo.Usuarios", "Login");
        }
    }
}
