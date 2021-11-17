using CodeGenHero.Core.Metadata.Interfaces;
using CodeGenHero.Inflector;
using CodeGenHero.Template.Helpers;
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
            string interfaceName,
            string apiUrl)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GenerateHeader(usings, classNamespace));

            sb.AppendLine($"\tpublic partial class {className} : WebApiDataServiceBase, {interfaceName}");
            sb.AppendLine("\t{");
            sb.AppendLine(string.Empty);

            sb.Append(GenerateConstructor(className, apiUrl, namespacePostfix));
            sb.Append(GenerateGetAllPages(entities));
            sb.Append(GenerateGetOnePage(entities, apiUrl));
            sb.Append(GenerateGetByPK(entities, apiUrl));
            sb.Append(GenerateCreate(entities, apiUrl));
            sb.Append(GenerateUpdate(entities, apiUrl));
            sb.Append(GenerateDelete(entities, apiUrl));

            sb.Append(GenerateFooter());
            return sb.ToString();
        }

        private string GenerateConstructor(string className, string apiUrl, string namespacePostfix)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine($"public {className}(ILogger<{className}> logger,");
            sb.AppendLine("\tIHttpClientFactory httpClientFactory,");
            sb.AppendLine("\tISerializationHelper serializationHelper)");
            sb.AppendLine("\t: base(logger, serializationHelper, httpClientFactory,");
            sb.AppendLine($"\t\tisServiceOnlineRelativeUrl: \"{apiUrl}/{namespacePostfix}ApiStatus\")");
            sb.AppendLine("{");
            sb.AppendLine("}");

            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        private string GenerateGetAllPages(IList<IEntityType> entities)
        {
            string signatureThirdLine = "\tEnums.RelatedEntitiesType relatedEntitiesType)";
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region GetAllPages");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string pluralEntityName = Inflector.Pluralize(entityName);

                // Start of first method overload
                sb.AppendLine($"public async Task<IList<xDTO.{entityName}>> GetAllPages{pluralEntityName}Async(");
                sb.AppendLine("\tbool? isActive, string sort,");
                sb.AppendLine(signatureThirdLine);
                sb.AppendLine("{");

                sb.AppendLine("\tList<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();");
                sb.AppendLine(string.Empty);

                sb.AppendLine("\tif (isActive.HasValue)");
                sb.AppendLine("\t{");

                sb.AppendLine("\t\tIFilterCriterion filterCriterion = new FilterCriterion(");
                sb.AppendLine($"\t\t\tfieldName: nameof(xDTO.{entityName}.IsActive),");
                sb.AppendLine("\t\t\tcondition: Enums.CriterionCondition.IsEqualTo,");
                sb.AppendLine("\t\t\tvalue: isActive");
                sb.AppendLine(string.Empty);

                sb.AppendLine("\t\tfilterCriteria.Add(filterCriterion);");

                sb.AppendLine("\t}");

                sb.AppendLine($"\tvar retVal = await GetAllPages{pluralEntityName}Async(filterCriteria, sort, relatedEntitiesType);");
                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
                // End of first method overload

                // Start of second method overload
                sb.AppendLine($"public async Task<IList<xDTO.{entityName}>> GetAllPages{pluralEntityName}Async(");
                sb.AppendLine("\tList<IFilterCriterion> filterCriteria, string sort,");
                sb.AppendLine(signatureThirdLine);
                sb.AppendLine("{");

                sb.AppendLine("\tIPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: 1, pageSize: 100, relatedEntitiesType: relatedEntitiesType);");
                sb.AppendLine(string.Empty);
                
                sb.AppendLine($"\tvar retVal = await GetAllPageDataResultsAsync(pageDataRequest, Get{pluralEntityName}Async);");
                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
                // End of second method overload
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGetOnePage(IList<IEntityType> entities, string apiUrl)
        {
            string signatureThirdLine = "\tEnums.RelatedEntitiesType relatedEntitiesType)";
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region GetOnePage");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string pluralEntityName = Inflector.Pluralize(entityName);
                string signatureFirstLine = $"public async Task<IHttpCallResultCGHT<IPageDataT<IList<xDTO.{entityName}>>>> Get{pluralEntityName}Async(";

                // First Method Overload
                sb.AppendLine($"{signatureFirstLine}IPageDataRequest pageDataRequest)");
                sb.AppendLine("{");

                sb.AppendLine("\tList<string> filter = BuildFilter(pageDataRequest.FilterCriteria);");
                sb.AppendLine(string.Empty);

                sb.AppendLine($"\tstring requestUrl = $\"{apiUrl}/{entityName}/{{(int)pageDataRequest.RelatedEntitiesType}}/\";");
                sb.AppendLine($"\tvar retVal = await SerializationHelper.SerializeCallResultsGet<IList<xDTO.{entityName}>>(");
                sb.AppendLine("\t\tLog, HttpClient, requestUrl,");
                sb.AppendLine("\t\tfilter: filter,");
                sb.AppendLine("\t\tsort: pageDataRequest.Sort,");
                sb.AppendLine("\t\tpage: pageDataRequest.Page,");
                sb.AppendLine("\t\tpageSize: pageDataRequest.PageSize);");
                sb.AppendLine(string.Empty);

                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);

                // Second Method Overload
                sb.AppendLine(signatureFirstLine);
                sb.AppendLine($"\tList<IFilterCriterion> filterCriteria, string sort, int page, int pageSize,");
                sb.AppendLine(signatureThirdLine);
                sb.AppendLine("{");

                sb.AppendLine("\tIPageDataRequest pageDataRequest = new PageDataRequest(filterCriteria: filterCriteria, sort: sort, page: page, pageSize: pageSize, relatedEntitiesType: relatedEntitiesType);");
                sb.AppendLine(string.Empty);

                sb.AppendLine($"\tvar retVal = await Get{pluralEntityName}Async(pageDataRequest);");
                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);

                // Third Method Overload
                sb.AppendLine(signatureFirstLine);
                sb.AppendLine("\tbool? isActive, string sort, int page, int pageSize,");
                sb.AppendLine(signatureThirdLine);
                sb.AppendLine("{");

                sb.AppendLine("\tList<IFilterCriterion> filterCriteria = new List<IFilterCriterion>();");
                sb.AppendLine("\tif (isActive.HasValue)");
                sb.AppendLine("\t{");

                sb.AppendLine("\t\tIFilterCriterion filterCriterion = new FilterCriterion(");
                sb.AppendLine($"\t\t\tfieldName: nameof(xDTO.{entityName}.IsActive),");
                sb.AppendLine("\t\t\tcondition: Enums.CriterionCondition.IsEqualTo,");
                sb.AppendLine("\t\t\tvalue: isActive);");
                sb.AppendLine("\t\tfilterCriteria.Add(filterCriterion);");

                sb.AppendLine("\t}");
                sb.AppendLine(string.Empty);

                sb.AppendLine($"\tvar retVal = await Get{pluralEntityName}Async(filterCriteria, sort, page, pageSize, relatedEntitiesType);");
                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateGetByPK(IList<IEntityType> entities, string apiUrl)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Get By PK");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                var pk = entity.FindPrimaryKey();
                string fieldSignature = GetSignatureWithFieldTypes(string.Empty, pk);
                string signature = GetSignatureWithoutFieldTypes(string.Empty, pk);

                sb.AppendLine($"public async Task<IHttpCallResultCGHT<xDTO.{entityName}>> Get{entityName}Async(");
                sb.AppendLine($"\t{fieldSignature}, Enums.RelatedEntitiesType relatedEntitiesType)");
                sb.AppendLine("{");

                sb.AppendLine($"\tvar retVal = await SerializationHelper.SerializeCallResultsGet<xDTO.{entityName}>(Log, HttpClient,");
                sb.AppendLine($"\t\t$\"{apiUrl}/{entityName}/ById/{signature}/{{(int)relatedEntitiesType}}\");");
                sb.AppendLine("\treturn retVal");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateCreate(IList<IEntityType> entities, string apiUrl)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Create");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;

                sb.AppendLine($"public async Task<IHttpCallResultCGHT<xDTO.{entityName}>> Create{entityName}Async(xDTO.{entityName} item);");
                sb.AppendLine("{");

                sb.AppendLine($"\tvar retVal = await SerializationHelper.SerializeCallResultsPost<xDTO.{entityName}>(");
                sb.AppendLine("\t\tLog, HttpClient,");
                sb.AppendLine($"\t\t$\"{apiUrl}/{entityName}/\", item);");
                sb.AppendLine("\treturn retVal");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateUpdate(IList<IEntityType> entities, string apiUrl)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Update");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                var primaryKeys = GetPrimaryKeys(entity);

                sb.AppendLine($"public async Task<IHttpCallResultCGHT<xDTO.{entityName}>> Update{entityName}Async(xDTO.{entityName} item)");
                sb.AppendLine("{");
                
                sb.AppendLine($"\tvar retVal = await SerializationHelper.SerializeCallResultsPut<xDTO.{entityName}>(");
                sb.AppendLine("\t\tLog, HttpClient,");
                sb.Append($"$\"{apiUrl}/{entityName}");
                foreach (var key in primaryKeys)
                {
                    sb.Append($"/{{item.{key}}}");
                }
                sb.AppendLine("\", item);");
                sb.AppendLine("\treturn retVal;");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
            }

            sb.Append("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }

        private string GenerateDelete(IList<IEntityType> entities, string apiUrl)
        {
            IndentingStringBuilder sb = new IndentingStringBuilder(2);

            sb.AppendLine("#region Delete");
            sb.AppendLine(string.Empty);

            foreach (var entity in entities)
            {
                string entityName = entity.ClrType.Name;
                string signature = GetSignatureWithFieldTypes(string.Empty, entity.FindPrimaryKey());
                var primaryKeys = GetPrimaryKeys(entity);

                sb.AppendLine($"public async Task<IHttpCallResultCGHT<xDTO.{entityName}>> Delete{entityName}Async({signature})");
                sb.AppendLine("{");

                sb.AppendLine($"\tvar retVal = await SerializationHelper.SerializeCallResultsDelete<xDTO.{entityName}>(");
                sb.AppendLine("\t\tLog, HttpClient,");
                sb.Append($"\t\t$\"{apiUrl}/{entityName}");
                foreach(var key in primaryKeys)
                {
                    var camelKey = Inflector.Camelize(key);
                    sb.Append($"/{{{camelKey}}}");
                }
                sb.AppendLine(");");
                sb.AppendLine(string.Empty);

                sb.AppendLine("\treturn retVal");

                sb.AppendLine("}");
                sb.AppendLine(string.Empty);
            }

            sb.AppendLine("#endregion");
            sb.AppendLine(string.Empty);

            return sb.ToString();
        }
    }
}
