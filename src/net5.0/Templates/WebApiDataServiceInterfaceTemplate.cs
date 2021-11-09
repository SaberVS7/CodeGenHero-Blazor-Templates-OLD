using CodeGenHero.Core;
using CodeGenHero.Template.Blazor5.Generators;
using CodeGenHero.Template.Models;
using System;
using System.Collections.Generic;

namespace CodeGenHero.Template.Blazor5.Templates
{
    [Template(name: "WebApiDataServiceInterface", version: "2021.11.9", uniqueTemplateIdGuid: "FB79D688-1931-4893-82F6-B7AD98BC5754",
        description: "Generates the Interface for the WebApiDataService to implement.")]
    internal class WebApiDataServiceInterfaceTemplate : BaseBlazorTemplate
    {
        public WebApiDataServiceInterfaceTemplate()
        {
        }

        #region TemplateVariables

        [TemplateVariable(defaultValue: Consts.PTG_DtoNamespace_DEFAULT, description: Consts.PTG_DtoNamespace_DESC)]
        public string DtoNamespace { get; set; }

        [TemplateVariable(defaultValue: Consts.PTG_WebApiDataServiceInterfaceName_DEFAULT, description: Consts.PTG_WebApiDataServiceInterfaceName_DESC)]
        public string WebApiDataServiceInterfaceClassName { get; set; }

        [TemplateVariable(defaultValue: Consts.WebApiDataServiceInterfaceOuputFilepath_DEFAULT, hiddenIndicator: true)]
        public string WebApiDataServiceInterfaceOuputFilepath { get; set; }

        #endregion

        public override TemplateOutput Generate()
        {
            TemplateOutput retVal = new TemplateOutput();

            try
            {
                string outputFile = TemplateVariablesManager.GetOutputFile(templateIdentity: ProcessModel.TemplateIdentity,
                    fileName: Consts.OUT_WebApiDataServiceInterfaceOuputFilepath_DEFAULT);
                string filepath = outputFile;

                var usings = new List<NamespaceItem>
                {
                    new NamespaceItem($"{BaseNamespace}.Shared.DataService"),
                    new NamespaceItem("System"),
                    new NamespaceItem("System.Collections.Generic"),
                    new NamespaceItem("System.Threading.Tasks"),
                    new NamespaceItem($"Enums = {BaseNamespace}.Shared.Constants.Enums"),
                    new NamespaceItem($"xDTO = {DtoNamespace}")
                };

                var entities = ProcessModel.MetadataSourceModel.GetEntityTypesByRegEx(RegexExclude, RegexInclude);


            }
            catch (Exception ex)
            {
                base.AddError(ref retVal, ex, Enums.LogLevel.Error);
            }

            return retVal;
        }
    }
}
