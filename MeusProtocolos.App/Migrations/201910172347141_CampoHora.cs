namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoHora : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Protocoloes", "Dia", c => c.DateTime(nullable: false));
            AddColumn("dbo.Protocoloes", "Hora", c => c.DateTime(nullable: false));
            DropColumn("dbo.Protocoloes", "DataHora");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Protocoloes", "DataHora", c => c.DateTime(nullable: false));
            DropColumn("dbo.Protocoloes", "Hora");
            DropColumn("dbo.Protocoloes", "Dia");
        }
    }
}
