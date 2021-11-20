using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.ProjectTemplate.Blazor5.Wizard
{
    public class BlazorTemplateWizard : IWizard
    {
        private static MainWindow _mainWindow;
        private static string _templateName; // Hmm, in the VSTNT code example the IDE warning for this got disabled, for a reason I'm not entirely read in
                                             // - Does this get used by the VS Templating system somewhere this library will not be aware of?

        public static void GetConnectionStringUserInput(string templateName)
        {
            _templateName = templateName;

            _mainWindow = new MainWindow();
            _mainWindow.TemplateParametersSet += TemplateParametersSet;
            _mainWindow.ShowDialog();
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            // Obtain connection string value from RootDictionary and place it in the replacementsDictionary.
            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.ConnectionString))
            {
                replacementsDictionary[Consts.DictionaryEntries.ConnectionString] = RootWizard.RootDictionary[Consts.DictionaryEntries.ConnectionString];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.IDPConnectionString))
            {
                replacementsDictionary[Consts.DictionaryEntries.IDPConnectionString] = RootWizard.RootDictionary[Consts.DictionaryEntries.IDPConnectionString];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.ADMIN_USERNAME))
            {
                replacementsDictionary[Consts.DictionaryEntries.ADMIN_USERNAME] = RootWizard.RootDictionary[Consts.DictionaryEntries.ADMIN_USERNAME];
            }

            if (RootWizard.RootDictionary.ContainsKey(Consts.DictionaryEntries.ADMIN_EMAIL))
            {
                replacementsDictionary[Consts.DictionaryEntries.ADMIN_EMAIL] = RootWizard.RootDictionary[Consts.DictionaryEntries.ADMIN_EMAIL];
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// Event Subscribers
        private static void TemplateParametersSet(object sender, MainWindow.TemplateParametersSetEventArgs e)
        {
            // Place connection string input by user into the root dictionary
            RootWizard.RootDictionary[Consts.DictionaryEntries.ConnectionString] = e.ConnectionString;

            RootWizard.RootDictionary[Consts.DictionaryEntries.IDPConnectionString] = e.IDPConnectionString;
            if (!String.IsNullOrWhiteSpace(e.IDPConnectionString))
            {
                RootWizard.IncludeIDPProject = true;
            }

            RootWizard.RootDictionary[Consts.DictionaryEntries.ADMIN_USERNAME] = e.AdminUsername;

            RootWizard.RootDictionary[Consts.DictionaryEntries.ADMIN_EMAIL] = e.AdminEmail;

            _mainWindow.Close();
        }
    }
}
