using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
using CodeGenHero.Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenHero.Template.Blazor5.Generators
{
    class AdminListPageViewModelGenerator : BaseBlazorGenerator
    {
        public AdminListPageViewModelGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }

        public string Generate(
            List<NamespaceItem> usings,
            string classNamespace,
            string namespacePostfix,
            IEntityType entity,
            string className,
            string webApiDataServiceClassName)
        {
            var entityName = $"{entity.ClrType.Name}";
            var pluralizedEntityName = Inflector.Pluralize(entityName);
            var pk = entity.FindPrimaryKey();
            var methodSignature = GetSignatureWithFieldTypes(string.Empty, pk);
            var methodSignatureUntyped = GetSignatureWithoutFieldTypes(string.Empty, pk, lowercasePkNameFirstChar: true);

            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine("\t[Authorize(Roles = Consts.ROLE_ADMIN_OR_USER)]");
            sb.AppendLine($"\tpublic class {className} : AdminPageBase");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {className}()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t}");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateProperties(entityName, pluralizedEntityName));
            sb.Append(GenerateDeleteMethods(entityName, methodSignature, methodSignatureUntyped, webApiDataServiceClassName));

            sb.Append(GenerateFooter());

            return sb.ToString();
        }

        private string GenerateProperties(string entityName, string pluralizedEntityName)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public IList<{entityName}> {pluralizedEntityName} {{ get; set; }} = new List<{entityName}>();");
            sb.AppendLine(string.Empty);

            sb.AppendLine("protected bool Bordered { get; set; } = false;");
            sb.AppendLine("protected bool Dense { get; set; } = false;");
            sb.AppendLine("protected bool Hover { get; set; } = true;");
            sb.AppendLine("protected bool Striped { get; set; } = true;");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine("protected ILocalHttpClientService LocalHttpClientService { get; set; }");
            sb.AppendLine(string.Empty);

            sb.AppendLine("protected string SearchString1 { get; set; } = \"\";");
            sb.AppendLine(string.Empty);

            sb.AppendLine($"protected {entityName} Selected{entityName} {{ get; set; }}");
            sb.AppendLine(string.Empty);

            sb.AppendLine("[Inject]");
            sb.AppendLine("private IDialogService DialogService { get; set; }");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDeleteMethods(string entityName, string methodSignature, string methodSignatureUntyped, string webApiDataServiceClassName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateConfirmDelete(entityName, methodSignatureUntyped));
            sb.AppendLine(GenerateDeleteEntity(entityName, methodSignature, methodSignatureUntyped, webApiDataServiceClassName));

            return sb.ToString();
        }

        #region Delete Method Generators

        private string GenerateConfirmDelete(string entityName, string methodSignatureUntyped)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected async Task ConfirmDeleteAsync({entityName} item)");
            sb.AppendLine("{");

            sb.AppendLine("\tvar parameters = new DialogParameters();");
            sb.AppendLine("\tparameters.Add(\"ContentText\", $\"Are you sure you want to delete {item.Name}?\");"); // Hmm, might be a bold assumption that entities have a "Name" column?
            sb.AppendLine("\tparameters.Add(\"ButtonText\", \"Yes\");");
            sb.AppendLine("\tparameters.Add(\"Color\", Color.Success);");
            sb.AppendLine(string.Empty);

            sb.AppendLine("\tvar result = await DialogService.Show<ConfirmationDialog>(\"Confirm\", parameters).Result;");
            sb.AppendLine("\tif (!result.Cancelled)");
            sb.AppendLine("\t{");

            sb.AppendLine("\t}");
            sb.AppendLine($"\t\tawait Delete{entityName}Async(item.{methodSignatureUntyped});");
            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDeleteEntity(string entityName, string methodSignature, string methodSignatureUntyped, string webApiDataServiceClassName)
        {
            string successCodeLiteral = 
                    @"StatusClass = ""alert-success"";
                    Message = ""Deleted successfully"";
                    await SetSavedAsync(true);";

            string failCodeLiteral =
                    @"StatusClass = ""alert-danger"";
                    Message = ""Something went wrong deleting the item. Please try again."";
                    await SetSavedAsync(false);";

            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"protected async Task Delete{entityName}Async({methodSignature})");
            sb.AppendLine("{");

            sb.AppendLine($"\tvar result = await {webApiDataServiceClassName}.Delete{entityName}Async({methodSignatureUntyped});");
            sb.AppendLine($"\tif (result.IsSuccessStatusCode)");    // Remember to include a Commented Out reference to the WebApiDataService in the MPT's Component-Base for the end-dev to hand-populate.
            sb.AppendLine("\t{");

            sb.AppendLine(successCodeLiteral);

            sb.AppendLine("\t}");
            sb.AppendLine("\telse");
            sb.AppendLine("\t{");

            sb.AppendLine(failCodeLiteral);

            sb.AppendLine("\t}");

            sb.AppendLine("}");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        #endregion
    }
}
