namespace BashSoft.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using NUnit.Framework;

    [TestFixture]
    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [SetUp]
        public void TestInit()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestEmptyCtor()
        {
            this.names = new SimpleSortedList<string>();

            Assert.AreEqual(16, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(20);

            Assert.AreEqual(20, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithAllParams()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);

            Assert.AreEqual(30, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithInitialComparer()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(16, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestAddIncreaseSize()
        {
            this.names.Add("Nasko");

            Assert.AreEqual(1, this.names.Size);
        }

        [Test]
        public void TestAddNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.Add(null));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            var someNames = new[] { "Rosen", "Georgi", "Balkan" };
            this.names.AddAll(someNames);
            Array.Sort(someNames);

            int i = 0;
            foreach (var name in this.names)
            {
                Assert.AreEqual(someNames[i], name);
                i++;
            }
        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            for (int i = 0; i < 17; i++)
            {
                this.names.Add(i.ToString());
            }

            Assert.AreEqual(17, this.names.Size);
            Assert.AreNotEqual(16, this.names.Capacity);
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            var someNames = new List<string>() {"Pesho", "Gosho"};

            this.names.AddAll(someNames);

            Assert.AreEqual(2, this.names.Size);
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.AddAll(null));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            var someNames = new List<string>() { "Pesho", "Gosho" };

            this.names.AddAll(someNames);
            someNames.Sort();

            Assert.AreEqual(someNames, this.names);
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            this.names.Add("Pesho");
            this.names.Remove("Pesho");

            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            this.names.AddAll(new[] {"Ivan", "Nasko"});

            this.names.Remove("Ivan");

            Assert.AreEqual(false, this.names.Contains("Ivan"));
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.names.Remove(null));
        }

        [Test]
        public void TestJoinWithNull()
        {
            this.names.AddAll(new[] { "Ivan", "Nasko" });

            Assert.Throws<ArgumentNullException>(() => this.names.JoinWith(null));
        }

        [Test]
        public void TestJoinWorksFine()
        {
            this.names.AddAll(new[] { "Ivan", "Nasko" });

            Assert.AreEqual("Ivan, Nasko", this.names.JoinWith(", "));
        }

    }
}