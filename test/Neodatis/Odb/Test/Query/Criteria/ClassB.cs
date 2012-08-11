using Test.Odb.Test.VO.Login;

namespace Test.Odb.Test.Query.Criteria
{
	/// <author>olivier</author>
	public class ClassB
	{
		private string name;

		private System.Collections.Generic.IList<Profile> profiles;

		public ClassB(string name, System.Collections.Generic.IList<Profile
			> profiles) : base()
		{
			this.name = name;
			this.profiles = profiles;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual System.Collections.Generic.IList<Profile
			> GetProfiles()
		{
			return profiles;
		}

		public virtual void SetProfiles(System.Collections.Generic.IList<Profile
			> profiles)
		{
			this.profiles = profiles;
		}
	}
}
