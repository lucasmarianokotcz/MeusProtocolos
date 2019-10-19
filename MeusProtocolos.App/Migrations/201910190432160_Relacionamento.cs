namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacionamento : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Protocolo", name: "Usuario_Login", newName: "Login");
            RenameIndex(table: "dbo.Protocolo", name: "IX_Usuario_Login", newName: "IX_Login");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Protocolo", name: "IX_Login", newName: "IX_Usuario_Login");
            RenameColumn(table: "dbo.Protocolo", name: "Login", newName: "Usuario_Login");
        }
    }
}
