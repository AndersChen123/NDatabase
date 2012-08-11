using NDatabase.Odb;

namespace Test.Odb.Test.Oid
{
	public class ClassWithOid
	{
		private string name;

		private OID oid;

		public ClassWithOid(string name, OID oid) : base()
		{
			this.name = name;
			this.oid = oid;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual OID GetOid()
		{
			return oid;
		}

		public virtual void SetOid(OID oid)
		{
			this.oid = oid;
		}
	}
}
