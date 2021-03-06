﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.17379
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Ringo.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Dependencies")]
    public partial class DependenciesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Dependencies.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Dependencies", "In order to ensure that items are executed in the correct order\r\nAs the flattener" +
                    " process\r\nI want items to be output so that dependencies come before their depen" +
                    "dents.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Make one package reliant on another")]
        public virtual void MakeOnePackageReliantOnAnother()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Make one package reliant on another", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("I have a package \"PackageA\"");
#line 8
 testRunner.And("I have a package \"PackageB\"");
#line 9
 testRunner.And("\"PackageB\" is dependent on \"PackageA\"");
#line 10
 testRunner.When("I output the flat list");
#line 11
 testRunner.Then("\"PackageA\" should appear before \"PackageB\"");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Make one package reliant two others")]
        public virtual void MakeOnePackageReliantTwoOthers()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Make one package reliant two others", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("I have a package \"PackageA\"");
#line 15
 testRunner.And("I have a package \"PackageB\"");
#line 16
 testRunner.And("I have a package \"PackageC\"");
#line 17
 testRunner.And("\"PackageA\" is dependent on \"PackageB\"");
#line 18
 testRunner.And("\"PackageA\" is dependent on \"PackageC\"");
#line 19
 testRunner.When("I output the flat list");
#line 20
 testRunner.Then("\"PackageB\" should appear before \"PackageA\"");
#line 21
 testRunner.And("\"PackageC\" should appear before \"PackageA\"");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Dependency chain")]
        public virtual void DependencyChain()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Dependency chain", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 24
 testRunner.Given("I have a package \"PackageA\"");
#line 25
 testRunner.And("I have a package \"PackageB\"");
#line 26
 testRunner.And("I have a package \"PackageC\"");
#line 27
 testRunner.And("\"PackageA\" is dependent on \"PackageB\"");
#line 28
 testRunner.And("\"PackageB\" is dependent on \"PackageC\"");
#line 29
 testRunner.When("I output the flat list");
#line 30
 testRunner.Then("\"PackageC\" should appear before \"PackageB\"");
#line 31
 testRunner.And("\"PackageB\" should appear before \"PackageA\"");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
