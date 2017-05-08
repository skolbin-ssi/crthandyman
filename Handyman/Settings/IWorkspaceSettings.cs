namespace Handyman.Settings
{
    /// <summary>
    /// Workspace-specific settings.
    /// </summary>
    public interface IWorkspaceSettings
    {
        /// <summary>
        /// The name of the project that request and responses are to be created at.
        /// </summary>
        string DefaultRequestProjectName { get; }
    }
}