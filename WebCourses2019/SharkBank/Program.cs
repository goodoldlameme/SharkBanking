using System;


// ReSharper disable StringLiteralTypo

namespace SharkBank
{
    class Program
    {
        private const string DataSource = @"den1.mssql7.gear.host";
        private const string DataBase = "kn301kopanova";
        private const string Username = "kn301kopanova";
        private const string Password = "n3n@vizhu";
        
        static void Main(string[] args)
        {
            var dbUtils = new DbUtils(DataSource, DataBase, Username, Password);
            var db = new Db(dbUtils);
            var server = new HttpServer(args[0], db);
            server.Run();
        }
    }
}