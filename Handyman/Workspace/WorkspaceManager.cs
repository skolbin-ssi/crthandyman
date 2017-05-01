﻿using System.Linq;
using Handyman.Generators;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CommerceRuntimeHandyman
{
    public class WorkspaceManager
    {
        private Workspace workspace;

        public WorkspaceManager(Workspace workspace)
        {
            this.workspace = workspace;
        }

        public void CreateOrUpdateRequestHandlerDefinition(RequestHandlerDefinition requestHandler)
        {
            var generator = new MemberedTypeGenerator();

            string requestCode = generator.GenerateSyntax(requestHandler.RequestType);
            this.CreateOrUpdateDocument(requestHandler.RequestType.Name, requestCode);

            if (!requestHandler.ResponseType.IsVoidResponse)
            {
                string responseCode = generator.GenerateSyntax(requestHandler.ResponseType);
                this.CreateOrUpdateDocument(requestHandler.ResponseType.Name, responseCode);
            }
        }

        private void CreateOrUpdateDocument(string name, string documentContent)
        {
            name = name + ".cs";

            var project = this.workspace.CurrentSolution.Projects.First();
            var document = project.Documents.FirstOrDefault(d => d.Name == name);
            var text = SourceText.From(documentContent);

            if (document == null)
            {
                document = project.AddDocument(name, text);                
            }            
            else
            {
                document = document.WithText(text);
            }

            workspace.TryApplyChanges(document.Project.Solution);
        }

        ////    //this.dte = (DTE2)ServiceProvider.GetService(typeof(DTE2));
        ////    private EnvDTE.Project ActiveProject
        ////    {
        ////        get
        ////        {
        ////            EnvDTE.Project activeProject = null;

        ////            Array activeSolutionProjects = this.dte.ActiveSolutionProjects as Array;
        ////            if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
        ////            {
        ////                activeProject = activeSolutionProjects.GetValue(0) as EnvDTE.Project;
        ////            }

        ////            return activeProject;
        ////        }
        ////    }

        ////    /*
        ////     public static IList<Project> Projects()
        ////{
        ////    Projects projects = GetActiveIDE().Solution.Projects;
        ////    List<Project> list = new List<Project>();
        ////    var item = projects.GetEnumerator();
        ////    while (item.MoveNext())
        ////    {
        ////        var project = item.Current as Project;
        ////        if (project == null)
        ////        {
        ////            continue;
        ////        }

        ////        if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
        ////        {
        ////            list.AddRange(GetSolutionFolderProjects(project));
        ////        }
        ////        else
        ////        {
        ////            list.Add(project);
        ////        }
        ////    }

        ////    return list;
        ////}

        ////private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        ////{
        ////    List<Project> list = new List<Project>();
        ////    for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
        ////    {
        ////        var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
        ////        if (subProject == null)
        ////        {
        ////            continue;
        ////        }

        ////        // If this is another solution folder, do a recursive call, otherwise add
        ////        if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
        ////        {
        ////           list.AddRange(GetSolutionFolderProjects(subProject));
        ////        }
        ////       else
        ////       {
        ////           list.Add(subProject);
        ////       }
        ////    }
        ////    return list;
        ////}
        ////     */
    }
}