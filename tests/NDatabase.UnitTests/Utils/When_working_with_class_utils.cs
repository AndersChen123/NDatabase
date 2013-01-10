﻿using NDatabase.Tool.Wrappers;
using NUnit.Framework;

public class WithoutNamespace
{

}

namespace NDatabase.UnitTests.Utils
{
    public class Person
    {
        public string Name { get; set; }
    }

    public class When_working_with_class_utils
    {
        [Test] 
        public void It_should_return_short_class_name_for_any_custom_class()
        {
            var className = OdbClassUtil.GetClassName(typeof (Person).FullName);
            Assert.That(className, Is.EqualTo("Person"));
        }

        [Test]
        public void It_should_return_short_class_name_for_any_base_type()
        {
            var stringClassName = OdbClassUtil.GetClassName(typeof (string).FullName);
            Assert.That(stringClassName, Is.EqualTo("String"));

            var intStructName = OdbClassUtil.GetClassName(typeof(int).FullName);
            Assert.That(intStructName, Is.EqualTo("Int32"));
        }

        [Test]
        public void It_should_return_namespace_for_any_custom_class()
        {
            var @namespace = OdbClassUtil.GetNamespace(typeof(Person).FullName);
            Assert.That(@namespace, Is.EqualTo("NDatabase.UnitTests.Utils"));
        }

        [Test]
        public void It_should_return_System_as_the_namespace_for_basic_types()
        {
            var @namespace = OdbClassUtil.GetNamespace(typeof(int).FullName);
            Assert.That(@namespace, Is.EqualTo("System"));
        }

        [Test]
        public void It_should_handle_properly_class_without_namespace()
        {
            var @namespace = OdbClassUtil.GetNamespace(typeof(WithoutNamespace).FullName);
            Assert.That(@namespace, Is.Empty);

            var className = OdbClassUtil.GetClassName(typeof(WithoutNamespace).FullName);
            Assert.That(className, Is.EqualTo("WithoutNamespace"));
        }

        [Test]
        public void It_should_return_proper_full_name_for_custom_class()
        {
            var fullName = OdbClassUtil.GetFullName(typeof (Person));
            Assert.That(fullName, Is.StringStarting("NDatabase.UnitTests.Utils.Person"));
        }

        [Test]
        public void It_should_return_proper_full_name_for_basic_type()
        {
            var fullName = OdbClassUtil.GetFullName(typeof(int));
            Assert.That(fullName, Is.StringStarting("System.Int32"));
        }
    }
}