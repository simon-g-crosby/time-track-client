
using System;

namespace TimeManager
{


	public class Project
	{
		public string ProjectName;
		public Int64 ProjectId;
		public Int64 CategoryId;

		public Project (string projectName, Int64 projectId, Int64 catId)
		{
			ProjectName = projectName;
			ProjectId = projectId;
			CategoryId = catId;
		}
	}
}
