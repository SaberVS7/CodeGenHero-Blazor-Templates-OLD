using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.ProjectTemplate.Blazor5.Wizard
{
    internal static class Consts
    {
        public static class DictionaryEntries
        {
            public const string ConnectionString = "$dbconnectionstring$";
            public const string IDPConnectionString = "$idpdbconnectionstring$";
            public const string ADMIN_USERNAME = "$adminUsername$";
            public const string ADMIN_EMAIL = "$adminEmail$";
            public const string DestinationDirectory = "$destinationdirectory$";
            public const string OriginalDestinationDirectory = "$origdestinationdirectory$";
            public const string ParentWizardName = "$parentwizardname$";
            public const string SafeProjectName = "$safeprojectname$";
            public const string SafeRootProjectName = "$saferootprojectname$";
            public const string SolutionDirectory = "$solutiondirectory$";
        }

        public static class ParentWizards
        {
            public const string RootWizard = "RootWizard";
        }

        public static class ProjectTemplates
        {
            public const string Root = "MSC.BlazorTemplate";
        }

        public static class ReadMeFiles
        {
            public const string Root = "ReadMe.md";
        }

    }
}
