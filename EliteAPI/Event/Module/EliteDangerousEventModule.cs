﻿using System;
using EliteAPI.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EliteAPI.Event.Module
{
    public abstract class EliteDangerousEventModule
    {
        protected EliteDangerousEventModule(IServiceProvider services)
        {
            EliteAPI = services.GetRequiredService<IEliteDangerousAPI>();
        }
            
        protected readonly IEliteDangerousAPI EliteAPI;
    }
}