using Microsoft.Extensions.DependencyInjection;
using ScraiBox.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ScraiBox.Core
{
    public static class ScraiBoxPluginExtensions
    {
        public static IServiceCollection AddScraiBoxPlugins(this IServiceCollection services, string pluginsPath)
        {
            if (!Directory.Exists(pluginsPath)) return services;

            // Najde všechna DLL, která začínají na ScraiBox.Plugin
            var pluginAssemblies = Directory.GetFiles(pluginsPath, "ScraiBox.Plugin.*.dll");

            foreach (var assemblyPath in pluginAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(IUseCasePlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in pluginTypes)
                {
                    // Zaregistrujeme pod rozhraním IUseCasePlugin
                    services.AddSingleton(typeof(IUseCasePlugin), type);
                }
            }

            return services;
        }
    }
}
