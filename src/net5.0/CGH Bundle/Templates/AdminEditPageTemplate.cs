using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "AdminEditPage", version: "2021.12.3", uniqueTemplateIdGuid: "BF6A5C3B-7D19-4F08-83E6-C341BC350F81",
        description: "Generates a basic Razor page visible to Admin users that allows them to edit metadata entity.")]
    public class AdminEditPageTemplate : BaseBlazorTemplate
    {
        public AdminEditPageTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_AdminEditViewModelClassName_DEFAULT, description: Consts.PTG_AdminEditViewModelClassName_DESC)]
        public string AdminEditViewModelClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceNamespace_DEFAULT, description: Consts.PTG_WebApiDataServiceNamespace_DESC)]
        public string WebApiDataServiceNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.AdminEditPageOutputFilepath_DEFAULT, hiddenIndicator: true)]
        public string AdminEditPageOutputFilepath { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem("Microsoft.AspNetCore.Authorization"),
                    new NamespaceItem("Microsoft.AspNetCore.Components"),
                    new NamespaceItem("Microsoft.AspNetCore.Components.Forms"),
                    new NamespaceItem("Microsoft.JSInterop"),
                    new NamespaceItem($"{WebApiDataServiceNamespace}"),
                    new NamespaceItem($"{BaseNamespace}.App.Shared"),
                    new NamespaceItem($"{BaseNamespace}.Shared.Constants"),
                    new NamespaceItem($"{DtoNamespace}"),
                    new NamespaceItem("MudBlazor"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.IO"),
                    new NamespaceItem("System.Net.Http"),
                    new NamespaceItem("System.Net.Http.Headers"),
                    new NamespaceItem("System.Threading.Tasks")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);

                foreach (var entity in entities)
                {
                    string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                        fileName: Consts.OUT_AdminEditPageOutputFilepath_DEFAULT);
                    string filepath = TokenReplacements(outputFile, entity);

                    var generator = new AdminEditPageGenerator(inflector: Inflector);
                    var generatedCode = generator.Generate(entity, AdminEditViewModelClassName);

                    retVal.Files.Add(new OutputFile()
                    {
                        Content = generatedCode,
                        Name = filepath
                    });
                }
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
