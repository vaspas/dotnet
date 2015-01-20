using System.Collections.Generic;
using Sigflow.Dataflow;
using Sigflow.Module;
using Sigflow.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SigFlowTests
{
    
    
    /// <summary>
    ///This is a test class for ChangeModuleTest and is intended
    ///to contain all ChangeModuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChangeModulePropertiesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        class TestModuleA:IModule
        {
            public ISignalReader<int> In { get; set; }
            public ISignalReader<int> In2 { get; set; }
            public ISignalReader<int> Out { get; set; }

            public ISignalReader[] Ins { get; set; }
            public IList<ISignalWriter> Outs { get; set; }
        }
        class TestModuleB : IModule
        {
            public ISignalReader<int> In { get; set; }
            public ISignalReader<int> In2 { get; set; }
            public ISignalReader<int> Out { get; set; }

            public IList<ISignalReader> Ins { get; set; }
            public ISignalWriter[] Outs { get; set; }
        }

        /// <summary>
        ///A test for ChangeModule Constructor
        ///</summary>
        [TestMethod()]
        public void ChangeModuleTest()
        {
            var m1 = new TestModuleA
                         {
                             In = new Block<int>(),
                             Out = new Block<int>(),
                             Ins = new Block<float>[2] { new Block<float>(), new Block<float>() },
                             Outs = new List<ISignalWriter>{new Block<float>(), new Block<float>()}
                         };
            var m2 = new TestModuleB
                         {
                             Ins=new List<ISignalReader>(),
                             Outs = new ISignalWriter[2]
                         };
            

            Sigflow.Module.ChangeModulePropertiesHelper.Change(m1, m2);
            
            Assert.IsTrue(m1.In==m2.In);
            Assert.IsTrue(m1.In2 == m2.In2);
            Assert.IsTrue(m1.Out == m2.Out);

            Assert.IsTrue(m2.Ins.Count == m1.Ins.Length);
            for(int i=0;i<m1.Ins.Length;i++)
                Assert.IsTrue(m2.Ins[i] == m1.Ins[i]);

            Assert.IsTrue(m2.Outs.Length == m1.Outs.Count);
            for (int i = 0; i < m1.Outs.Count; i++)
                Assert.IsTrue(m2.Outs[i] == m1.Outs[i]);
        }
    }
}
