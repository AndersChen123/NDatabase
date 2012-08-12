﻿using System;
using NDatabase.Odb;
using NDatabase.Odb.Core.Trigger;
using NDatabase.Tool.Wrappers.IO;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.CodeSnippets
{
    public class Documentation_triggers
    {
        public class MyTrigger : InsertTrigger
        {
            public override bool BeforeInsert(object @object)
            {
                Console.WriteLine("before inserting {0}", @object);
                ((Mage) @object).Attack += 1.1;
                Console.WriteLine("before inserting (after change) {0}", @object);

                BeforeInvoked = true;

                return true;
            }

            public override void AfterInsert(object @object, OID oid)
            {
                Console.WriteLine("after insert object with id {0}({1})", oid, @object.GetType().Name);
                AfterInvoked = true;
            }

            public bool BeforeInvoked { get; set; }
            public bool AfterInvoked { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            OdbFile.DeleteFile("inserting_trigger.db");
        }

        [Test] 
        public void Using_insert_trigger()
        {
            var mage = new Mage("Merlin", 3.3, 3.4);
            var myTrigger = new MyTrigger();

            using (var odb = OdbFactory.Open("inserting_trigger.db"))
            {
                odb.AddInsertTrigger(typeof(Mage), myTrigger);
                odb.Store(mage);
            }

            Assert.That(myTrigger.BeforeInvoked, Is.True);
            Assert.That(myTrigger.AfterInvoked, Is.True);

            using (var odb = OdbFactory.Open("inserting_trigger.db"))
            {
                var merlin = odb.GetObjects<Mage>().GetFirst();
                Assert.That(merlin.Attack, Is.EqualTo(3.3 + 1.1));
            }
        }
    }
}