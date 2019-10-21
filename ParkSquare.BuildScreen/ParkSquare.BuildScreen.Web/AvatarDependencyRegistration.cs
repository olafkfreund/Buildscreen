﻿using Microsoft.Extensions.DependencyInjection;
using ParkSquare.BuildScreen.Core.Avatar;
using ParkSquare.BuildScreen.Core.Gravatar;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ParkSquare.BuildScreen.Web
{
    public static class AvatarDependencyRegistration
    {
        public static IServiceCollection AddAvatarDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAvatarService, AvatarService>();

            // Gravatar avatar provider
            services.AddSingleton<IAvatarProvider, GravatarProvider>();
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<ImageFormatManager, ImageFormatManager>(x => GetImageFormatManager());

            return services;
        }

        private static ImageFormatManager GetImageFormatManager()
        {
            var manager = new ImageFormatManager();
            manager.AddImageFormat(JpegFormat.Instance);
            manager.AddImageFormat(PngFormat.Instance);

            return manager;
        }
    }
}
