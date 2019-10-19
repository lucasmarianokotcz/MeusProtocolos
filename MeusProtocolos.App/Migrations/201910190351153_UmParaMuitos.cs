namespace MeusProtocolos.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UmParaMuitos : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Protocoloes", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Protocoloes", "Login", c => c.Int(nullable: false));
        }
    }
}
