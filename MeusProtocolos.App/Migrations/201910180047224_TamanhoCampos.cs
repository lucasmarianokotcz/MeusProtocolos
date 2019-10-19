namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TamanhoCampos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Protocoloes", "Titulo", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Protocoloes", "Numero", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Protocoloes", "Motivo", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Protocoloes", "Descricao", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Protocoloes", "Atendente", c => c.String(maxLength: 30));
            AlterColumn("dbo.Protocoloes", "OutrasInfo", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Protocoloes", "OutrasInfo", c => c.String());
            AlterColumn("dbo.Protocoloes", "Atendente", c => c.String());
            AlterColumn("dbo.Protocoloes", "Descricao", c => c.String(nullable: false));
            AlterColumn("dbo.Protocoloes", "Motivo", c => c.String(nullable: false));
            AlterColumn("dbo.Protocoloes", "Numero", c => c.String(nullable: false));
            AlterColumn("dbo.Protocoloes", "Titulo", c => c.String(nullable: false));
        }
    }
}
