using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    public class WebApiDataServiceGenerator : BaseBlazorGenerator
    {
        public WebApiDataServiceGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IList<IEntityType> entities,
            string className,
            string interfaceName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : WebApiDataServiceBase, {interfaceName}");
            sb.AppendLine("\t{");
            sb.AppendLine(string.Empty);



            sb.Append(GenerateFooter());
            return sb.ToString();
        }
    }
}
