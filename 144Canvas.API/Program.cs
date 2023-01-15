using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using _144Canvas.API.Models;

using _144Canvas.API.Data;

namespace _144Canvas.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;
                try{
                    var context = services.GetRequiredService<DataContext>();
                    var UserManager = services.GetRequiredService<UserManager<User>>();
                    var RolesManager = services.GetRequiredService<RoleManager<Role>>();
                    //var repo = services.GetRequiredService<IOFFCanvasRepository>();//Reenable only when importing old pictures
                    context.Database.Migrate();
                    Seed.SeedRoles(RolesManager);
                    Seed.SeedUsers(UserManager);//context);
                    Seed.SeedDefaultAdmin("144Admin",UserManager,RolesManager);
                    //Seed.UpdateOldPictures(repo);//Reenable only when importing old pictures
                } catch (Exception e){
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Migration error");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
