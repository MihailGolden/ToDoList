namespace ToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationChangeTablesNAmes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.JobTasks", newName: "tasks");
            RenameTable(name: "dbo.Projects", newName: "projects");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tasks", newName: "JobTasks");
            RenameTable(name: "dbo.Projects", newName: "projects");
        }
    }
}
