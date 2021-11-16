using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "AdminListPage", version: "2021.11.12", uniqueTemplateIdGuid: "414C3369-6F09-4341-B755-A133EAB5E775",
        description: "Generates a basic Razor page visible to Admin users that lists all of a certain metadata entity.")]
    class AdminListPageTemplate : BaseBlazorTemplate
    {
        public AdminListPageTemplate()
        {
        }

        #region TemplateVariables

        #endregion

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

            AddTemplateVariablesManagerErrorsToRetVal(ref retVal, Enums.LogLevel.Error);
            return retVal;
        }
    }
}
