using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalkToAPI.Database;

namespace TalkToAPI
{
    public class Startup
    {
        public IConfiguration Conf { get; set; }

        public Startup(IConfiguration conf)
        {
            Conf = conf;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TalkToContext>(cfg => {
                cfg.UseSqlite(Conf.GetConnectionString("TalkToContext"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
