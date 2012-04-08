using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Ringo.Tests
{
  [Binding]
  public class DependencySteps
  {
    private PackageManager packages_ = new PackageManager();
    private List<IPackage> flat_list_;

    [Given(@"I have a package ""(.*)""")]
    public void GivenIHaveAPackage(string package_name) {
      packages_.Add(new Package(package_name));
    }
    [Given(@"""(.*)"" is dependent on ""(.*)""")]
    public void GivenPackageIsDependentOnAnotherPackage(string dependent_package, string parent_package) {
      packages_.SetDependency(dependent_package, parent_package);
    }
    [When(@"I output the flat list")]
    public void WhenIOutputTheFlatList() {
      flat_list_ = packages_.Flatten();
    }
    [Then(@"""(.*)"" should appear before ""(.*)""")]
    public void ThenFirstPackageShouldAppearBeforeSecondPackage(string first_package, string second_package) {
      int first = flat_list_.FindIndex(p => p.Name == first_package);
      int second = flat_list_.FindIndex(p => p.Name == second_package);
      Assert.IsTrue(first < second);
    }
  }
}
