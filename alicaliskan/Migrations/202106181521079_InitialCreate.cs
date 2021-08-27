namespace alicaliskan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KrediKartlariHarcamas",
                c => new
                    {
                        HarcamaID = c.Int(nullable: false, identity: true),
                        IslemTarihi = c.DateTime(nullable: false),
                        CalisilanFirma = c.String(),
                        IslemTutari = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaksitSaysi = c.Int(nullable: false),
                        SubeAdi = c.String(),
                    })
                .PrimaryKey(t => t.HarcamaID);
            
            CreateTable(
                "dbo.KrediKartlaris",
                c => new
                    {
                        KartID = c.Int(nullable: false, identity: true),
                        BankaAdi = c.String(),
                    })
                .PrimaryKey(t => t.KartID);
            
            CreateTable(
                "dbo.SabitGiderlers",
                c => new
                    {
                        Sabit_Gider_ID = c.Int(nullable: false, identity: true),
                        Sabit_Gider_Tutari = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sabit_Gider_Kalemi = c.String(),
                        Gider_Sube_Adi = c.String(),
                        Sabit_Gider_Odeme_Durumu = c.Boolean(nullable: false),
                        Sabit_Gider_Aciklama = c.String(),
                    })
                .PrimaryKey(t => t.Sabit_Gider_ID);
            
            CreateTable(
                "dbo.Subelers",
                c => new
                    {
                        SubeID = c.Int(nullable: false, identity: true),
                        SubeAdi = c.String(),
                    })
                .PrimaryKey(t => t.SubeID);
            
            CreateTable(
                "dbo.TumGiderlers",
                c => new
                    {
                        Gider_ID = c.Int(nullable: false, identity: true),
                        Gider_Tarihi = c.DateTime(nullable: false),
                        Gider_Tutari = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gider_Kalemi = c.String(),
                        Gider_Taksit_Saysi = c.Int(nullable: false),
                        Gider_Sube_Adi = c.String(),
                        Gider_Odeme_Durumu = c.Boolean(nullable: false),
                        Gider_Aciklama = c.String(),
                        SabitGider_Sabit_Gider_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Gider_ID)
                .ForeignKey("dbo.SabitGiderlers", t => t.SabitGider_Sabit_Gider_ID)
                .Index(t => t.SabitGider_Sabit_Gider_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TumGiderlers", "SabitGider_Sabit_Gider_ID", "dbo.SabitGiderlers");
            DropIndex("dbo.TumGiderlers", new[] { "SabitGider_Sabit_Gider_ID" });
            DropTable("dbo.TumGiderlers");
            DropTable("dbo.Subelers");
            DropTable("dbo.SabitGiderlers");
            DropTable("dbo.KrediKartlaris");
            DropTable("dbo.KrediKartlariHarcamas");
        }
    }
}
