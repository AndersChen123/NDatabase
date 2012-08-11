using NDatabase.Odb;
using NDatabase.Odb.Core.Query;
using NDatabase.Odb.Impl.Core.Query.Criteria;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.Odb.Test.List
{
	[TestFixture]
    public class TestFieldComparatorSort : ODBTest
	{
		[Test]
        public virtual void Test1()
		{
			string baseName = GetBaseName();
			IOdb odb = null;
			int k = 10;
            long t1 = OdbTime.GetCurrentTimeInMs();
			odb = Open(baseName);
			for (int i = 0; i < k; i++)
			{
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i + 1, new System.DateTime(t1 - i * 1000), i % 2 == 0));
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i, new System.DateTime(t1 - i * 1000), (i + 1) % 2 == 0));
			}
			odb.Close();
			odb = Open(baseName);
			IQuery q = new CriteriaQuery
				(typeof(User)).OrderByAsc("name,id");
			IObjects<User> users = odb.GetObjects<User>(q);
			odb.Close();
			if (k < 11)
			{
				//NDatabase.Tool.DisplayUtility.Display("test1", users);
			}
			User user = users.GetFirst();
			AssertTrue(user.GetName().StartsWith("john1"));
			AssertEquals(1, user.GetId());
		}

		[Test]
        public virtual void Test1_2()
		{
			string baseName = GetBaseName();
			IOdb odb = null;
			int k = 10;
            long t1 = OdbTime.GetCurrentTimeInMs();
			odb = Open(baseName);
			for (int i = 0; i < k; i++)
			{
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i + 1, new System.DateTime(t1 - i * 1000), i % 2 == 0));
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i, new System.DateTime(t1 - i * 1000), (i + 1) % 2 == 0));
			}
			odb.Close();
			odb = Open(baseName);
			IQuery q = new CriteriaQuery
				(typeof(User)).OrderByDesc("name,id");
			IObjects<User> users = odb.GetObjects<User>(q);
			odb.Close();
			if (k < 11)
			{
				//NDatabase.Tool.DisplayUtility.Display("test1", users);
			}
			User user = users.GetFirst();
			AssertTrue(user.GetName().StartsWith("john9"));
			AssertEquals(10, user.GetId());
		}

		[Test]
        public virtual void Test2()
		{
			string baseName = GetBaseName();
			IOdb odb = null;
			int k = 10;
            long t1 = OdbTime.GetCurrentTimeInMs();
			string[] fields = new string[] { "ok", "id", "name" };
			odb = Open(baseName);
			for (int i = 0; i < k; i++)
			{
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i + 1, new System.DateTime(t1 - i * 1000), i % 2 == 0));
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i, new System.DateTime(t1 - i * 1000), (i + 1) % 2 == 0));
			}
			odb.Close();
			odb = Open(baseName);
			IQuery q = new CriteriaQuery
				(typeof(User)).OrderByAsc("ok,id,name");
			IObjects<User> users = odb.GetObjects<User>(q);
			odb.Close();
			if (k < 11)
			{
				//NDatabase.Tool.DisplayUtility.Display("test1", users);
			}
			User user = users.GetFirst();
			AssertTrue(user.GetName().StartsWith("john1"));
			AssertEquals(2, user.GetId());
		}

		[Test]
        public virtual void Test2_2()
		{
			string baseName = GetBaseName();
			IOdb odb = null;
			int k = 10;
            long t1 = OdbTime.GetCurrentTimeInMs();
			string[] fields = new string[] { "ok", "id", "name" };
			odb = Open(baseName);
			for (int i = 0; i < k; i++)
			{
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i + 1, new System.DateTime(t1 - i * 1000), i % 2 == 0));
				odb.Store(new User("john" + (k - i), "john@ibm.com", "ny 875"
					, k - i, new System.DateTime(t1 - i * 1000), (i + 1) % 2 == 0));
			}
			odb.Close();
			odb = Open(baseName);
			IQuery q = new CriteriaQuery
				(typeof(User)).OrderByDesc("ok,id,name");
			IObjects<User> users = odb.GetObjects<User>(q);
			odb.Close();
			if (k < 11)
			{
				//NDatabase.Tool.DisplayUtility.Display("test1", users);
			}
			User user = users.GetFirst();
			AssertTrue(user.GetName().StartsWith("john10"));
			AssertEquals(11, user.GetId());
		}
	}
}
