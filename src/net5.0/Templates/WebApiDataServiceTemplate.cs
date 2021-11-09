using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "WebApiDataService", version: "2021.11.9", uniqueTemplateIdGuid: "DA87D00B-525F-487A-934A-0925A3F99DB9",
        description: "Generates the WebApiDataService implementation.")]
    internal class WebApiDataServiceTemplate : BaseBlazorTemplate
    {
        public WebApiDataServiceTemplate()
        {
        }

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {

            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            return retVal;
        }
    }
}
