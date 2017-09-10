﻿using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EmbeddedFiles;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    public class EmbeddedResourceViewFileProvider : EmbeddedResourceFileProvider
    {
        public EmbeddedResourceViewFileProvider(IObjectAccessor<IServiceProvider> serviceProvider) 
            : base(serviceProvider)
        {
        }

        protected override bool IsIgnoredFile(EmbeddedFileInfo resource)
        {
            return resource.FileExtension != "cshtml";
        }
    }
}