using NDatabase2.Odb.Core.Query.Criteria;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.Newbie.VO;

namespace Test.NDatabase.Odb.Test.Newbie
{
    /// <summary>
    ///   It is just a simple test to help the newbies
    /// </summary>
    public class InsertTest : ODBTest
    {
        protected static readonly string NewbieOdb = "newbie.neodatis";

        /// <summary>
        ///   Insert different objects on database
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        /// <exception cref="System.IO.IOException">System.IO.IOException</exception>
        [Test]
        public virtual void TestInsert()
        {
            DeleteBase(NewbieOdb);

            // open is called on NewbieTest
            using (var odb = Open(NewbieOdb))
            {
                var marcelo = new Driver("marcelo");
                var car = new Car("car1", 4, "ranger", marcelo);
                var car1 = new Car("car2", 2, "porche");
                var car2 = new Car("car3", 2, "fusca");
                var car3 = new Car("car4", 4, "opala");
                var car4 = new Car("car5", 4, "vectra", marcelo);

                // insert 5 car's
                odb.Store(car);
                odb.Store(car1);
                odb.Store(car2);
                odb.Store(car3);
                odb.Store(car4);

                // find for all car objects
                var cars = odb.Query<Car>();
                AssertEquals("The objects weren't added correctly", 5, cars.Count);

                // find for a specific car object
                var query = new CriteriaQuery<Car>();
                query.Equal("Name", "car1");
                cars = odb.Query<Car>(query);
                AssertEquals("The objects couldn't be found correctly", 1, cars.Count);

                // find for a specific composition
                query = new CriteriaQuery<Car>();
                query.Equal("Driver.Name", "marcelo");
                cars = odb.Query<Car>(query);
                AssertEquals("The objects couldn't be found correctly", 2, cars.Count);
            }

            DeleteBase(NewbieOdb);
        }
    }
}
