using System;

namespace TimeManager.TimeBlockAdder
{
    public class Task
    {

        public Task (Int64 projectId, Int64? parentProjectId, string projectName)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ParentProjectId = parentProjectId;
        }

        public Int64 ProjectId;
        public Int64? ParentProjectId;
        public string ProjectName;
    }
}
