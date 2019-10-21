﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkSquare.BuildScreen.Web.Avatar;
using ParkSquare.BuildScreen.Web.AzureDevOps;
using ParkSquare.BuildScreen.Web.Build;
using ParkSquare.BuildScreen.Web.Gravatar;
using Serilog;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ParkSquare.BuildScreen.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<ImageFormatManager, ImageFormatManager>(x => GetImageFormatManager());

            // Avatar providers
            services.AddSingleton<IAvatarService, AvatarService>();
            services.AddSingleton<IAvatarProvider, GravatarProvider>();

            // Build providers
            services.AddSingleton<IBuildService, BuildService>();
            services.AddSingleton<IBuildProvider, AzureDevOpsBuildProvider>();

            // Azure DevOps build provider 
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IBuildDtoConverter, BuildDtoConverter>();
            services.AddSingleton<IBuildFilter, LatestBuildsFilter>();
            services.AddSingleton<IBuildFilter, CompletedPullRequestFilter>();
            services.AddSingleton<ITestResultsProvider, TestResultsProvider>();
            services.AddSingleton<IConfig, Config>();
            services.AddSingleton<IBranchNameConverter, BranchNameConverter>();
            services.AddSingleton<IDisplayTransformer, DisplayTransformer>();
        }

        private static ImageFormatManager GetImageFormatManager()
        {
            var manager = new ImageFormatManager();
            manager.AddImageFormat(JpegFormat.Instance);
            manager.AddImageFormat(PngFormat.Instance);

            return manager;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSerilogRequestLogging(); // replaces noisy built-in version
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
