﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AcceptanceTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Interfaces")]
    public partial class InterfacesFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "Interfaces.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Interfaces", "\tInterfaces can be used as a placeholder for types, only describing (parts of) th" +
                    "e available member variables and functions of the type.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
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
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("An interface can be defined")]
        public virtual void AnInterfaceCanBeDefined()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("An interface can be defined", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 5
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
  testRunner.Given("the following interface is defined:", "interface HasToString { }", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 10
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 11
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 12
  testRunner.Then("there are no errors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("An interface can be implemented")]
        public virtual void AnInterfaceCanBeImplemented()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("An interface can be implemented", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 14
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 15
  testRunner.Given("the following interface is defined:", "interface HasToString { }", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 19
  testRunner.And("the following class is defined:", "class Foo : HasToString { }", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 23
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 24
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 25
  testRunner.Then("there are no errors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Implementing an interface and not overriding all members causes an error")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void ImplementingAnInterfaceAndNotOverridingAllMembersCausesAnError()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Implementing an interface and not overriding all members causes an error", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 28
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 29
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 35
  testRunner.And("the following class is defined:", "class Foo : HasToString { }", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 39
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 40
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 41
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 42
  testRunner.And("the error contains \"Missing override\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Overriding a function with the wrong return type causes an error")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void OverridingAFunctionWithTheWrongReturnTypeCausesAnError()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Overriding a function with the wrong return type causes an error", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 45
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 46
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 52
  testRunner.And("the following class is defined:", "class Foo : HasToString {\r\n\tpublic int foo() {}\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 59
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 61
  testRunner.And("the error contains \"Override return type mismatch\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Overriding a member with the wrong member type causes an error")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void OverridingAMemberWithTheWrongMemberTypeCausesAnError()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Overriding a member with the wrong member type causes an error", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 64
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 65
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 71
  testRunner.And("the following class is defined:", "class Foo : HasToString {\r\n\tpublic int foo;\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 77
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 78
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 79
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 80
  testRunner.And("the error contains \"Override member type mismatch\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Overriding a function with different parameter types causes an error")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void OverridingAFunctionWithDifferentParameterTypesCausesAnError()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Overriding a function with different parameter types causes an error", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 83
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 84
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 90
  testRunner.And("the following class is defined:", "class Foo : HasToString {\r\n\tpublic void foo(string bar) {}\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 96
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 97
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 98
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 99
  testRunner.And("the error contains \"Override function signature mismatch\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Overriding a function correctly works")]
        public virtual void OverridingAFunctionCorrectlyWorks()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Overriding a function correctly works", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 101
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 102
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 108
  testRunner.And("the following class is defined:", "class Foo : HasToString {\r\n\tpublic void foo() {}\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 114
  testRunner.And("the main function is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 115
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 116
  testRunner.Then("there are no errors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Instantiating an interface causes an error")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void InstantiatingAnInterfaceCausesAnError()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Instantiating an interface causes an error", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 119
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 120
  testRunner.Given("the following interface is defined:", "interface HasToString {\r\n\tpublic void foo();\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 126
  testRunner.And("the main function contains the following code:", "new HasToString();", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 130
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 131
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 132
  testRunner.And("the error contains \"Invalid constructor call\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Overriding more specific parameter and return types works")]
        public virtual void OverridingMoreSpecificParameterAndReturnTypesWorks()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Overriding more specific parameter and return types works", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 134
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 135
  testRunner.Given("the following interface is defined:", "interface IPlusOp {\r\n\tpublic IPlusOp operator_plus(IPlusOp other);\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 141
  testRunner.And("the following class is defined:", "class Foo {\r\n\tpublic int x;\r\n\r\n\tpublic Foo(this.x) {}\r\n\r\n\tpublic Foo operator_plu" +
                        "s(Foo other) {\r\n\t\treturn new Foo(x + other.x);\r\n\t}\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 153
  testRunner.And("the following function is defined:", "IPlusOp add(IPlusOp a, IPlusOp b) {\r\n\treturn a + b;\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 159
  testRunner.And("the main function contains the following code:", "Foo f1 = new Foo(10);\r\nFoo f2 = new Foo(25);\r\nIPlusOp sum = add(f1, f2);", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 165
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 166
  testRunner.Then("there are no errors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fail on using less specific type as function parameter")]
        [NUnit.Framework.CategoryAttribute("Error")]
        public virtual void FailOnUsingLessSpecificTypeAsFunctionParameter()
        {
            string[] tagsOfScenario = new string[] {
                    "Error"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fail on using less specific type as function parameter", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 169
 this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 170
  testRunner.Given("the following interface is defined:", "interface IPlusOp {\r\n\tpublic IPlusOp operator_plus(IPlusOp other);\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 176
  testRunner.And("the following class is defined:", "class Foo : IPlusOp {\r\n\tpublic int x;\r\n\r\n\tpublic Foo(this.x) {}\r\n\r\n\tpublic Foo op" +
                        "erator_plus(Foo other) {\r\n\t\treturn new Foo(x + other.x);\r\n\t}\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 188
  testRunner.And("the following function is defined:", "Foo add(Foo a, Foo b) {\r\n\treturn a + b;\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 194
  testRunner.And("the main function contains the following code:", "Foo f1 = new Foo(10);\r\nFoo f2 = new Foo(25);\r\nIPlusOp op1 = new Foo(234);\r\nIPlusO" +
                        "p sum = add(f1, op1);", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 201
  testRunner.When("the code is compiled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 202
  testRunner.Then("an error is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 203
  testRunner.And("the error contains \"No matching overload\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
