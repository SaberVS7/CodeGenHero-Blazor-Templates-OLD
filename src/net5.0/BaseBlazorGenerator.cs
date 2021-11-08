using CodeGenHero.Inflector;
using CodeGenHero.Template.Models;

namespace CodeGenHero.Template.Blazor5
{
    public abstract class BaseBlazorGenerator : BaseGenerator
    {
        public BaseBlazorGenerator(ICodeGenHeroInflector inflector) : base(inflector)
        {
        }
    }
}