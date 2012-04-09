using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ringo.Tests
{
  [TestClass()]
  public class PackageManagerTests
  {
    private static Mock<IPackage> CreateMockPackage(string package_name) {
      var mock_package = new Mock<IPackage>();
      mock_package.SetupGet(p => p.Name).Returns(package_name);
      return mock_package;
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Add_NullPackage_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();
      Package package = null;

      // Act
      target.Add(package);
    }

    [TestMethod()]
    public void Add_ValidPackage_PackageIsStored() {
      // Arrange
      PackageManager target = new PackageManager();
      var package = CreateMockPackage("Package Name");

      // Act
      target.Add(package.Object);

      // Assert
      CollectionAssert.Contains(target.Items, package.Object);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void SetDependency_FirstPackageDoesntExist_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();
      string dependent_package = "Non-existant";
      string parent_package = "Parent package";

      target.Add(CreateMockPackage(parent_package).Object);

      // Act
      target.SetDependency(dependent_package, parent_package);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void SetDependency_SecondPackageDoesntExist_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();
      string dependent_package = "Dependent package";
      string parent_package = "Non-existant";

      target.Add(CreateMockPackage(dependent_package).Object);

      // Act
      target.SetDependency(dependent_package, parent_package);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetDependency_DependencyAlreadyExists_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();
      string dependent_package = "Dependent package";
      string parent_package = "Parent Package";

      target.Add(CreateMockPackage(dependent_package).Object);
      target.Add(CreateMockPackage(parent_package).Object);

      target.SetDependency(dependent_package, parent_package);

      // Act
      target.SetDependency(dependent_package, parent_package);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetDependency_CreatesDependencyCycle_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();
      string dependent_package = "Dependent package";
      string parent_package = "Parent Package";

      target.Add(CreateMockPackage(dependent_package).Object);
      target.Add(CreateMockPackage(parent_package).Object);

      target.SetDependency(dependent_package, parent_package);

      // Act
      target.SetDependency(parent_package, dependent_package);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetDependency_CreatesDependencyCycle2_ThrowsException() {
      // Arrange
      PackageManager target = new PackageManager();

      target.Add(CreateMockPackage("Package 1").Object);
      target.Add(CreateMockPackage("Package 2").Object);
      target.Add(CreateMockPackage("Package 3").Object);
      target.Add(CreateMockPackage("Package 4").Object);

      target.SetDependency("Package 2", "Package 1");
      target.SetDependency("Package 3", "Package 1");
      target.SetDependency("Package 4", "Package 2");

      // Act
      target.SetDependency("Package 1", "Package 4");
    }

    [TestMethod]
    public void SetDependency_ExistingPackages_StoresDependency() {
      // Arrange
      PackageManager target = new PackageManager();
      string dependent_package = "Dependent package";
      string parent_package = "Parent Package";

      var dependent_mock = CreateMockPackage(dependent_package);
      target.Add(dependent_mock.Object);

      var parent_mock = CreateMockPackage(parent_package);
      target.Add(parent_mock.Object);

      // Act
      target.SetDependency(dependent_package, parent_package);

      // Assert
      Assert.IsTrue(target.Dependencies.Any(d => d.Parent == parent_mock.Object
                                      && d.Dependent == dependent_mock.Object));
    }

    [TestMethod]
    public void SetDependency_BridgeDependency_StoresDependency() {
      // Arrange
      PackageManager target = new PackageManager();
      target.Add(CreateMockPackage("Package 1").Object);
      target.Add(CreateMockPackage("Package 2").Object);
      target.Add(CreateMockPackage("Package 3").Object);

      target.SetDependency("Package 2", "Package 1");
      target.SetDependency("Package 3", "Package 2");

      // Act
      target.SetDependency("Package 3", "Package 1");

      // Assert
      Assert.IsTrue(target.Dependencies.Any(d => d.Parent.Name == "Package 1"
                                      && d.Dependent.Name == "Package 3"));
    }

    [TestMethod()]
    public void Flatten_NoPackages_EmptyList() {
      // Arrange
      PackageManager target = new PackageManager();
      IPackage[] actual;

      // Act
      actual = target.Flatten();

      // Assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(0, actual.Length);
    }

    [TestMethod()]
    public void Flatten_NoDependencies_AllPackagesAreOutput() {
      // Arrange
      PackageManager target = new PackageManager();
      List<IPackage> packages = new List<IPackage>();
      for (int n = 0; n < 10; ++n) {
        var package = CreateMockPackage("Package " + n);
        packages.Add(package.Object);
        target.Add(package.Object);
      }
      IPackage[] actual;

      // Act
      actual = target.Flatten();

      // Assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(10, actual.Length);
      CollectionAssert.AreEquivalent(packages, actual);
    }

    [TestMethod()]
    public void Flatten_Dependencies_OutputInOrder() {
      // Arrange
      var package_a = CreateMockPackage("Package A");
      var package_b = CreateMockPackage("Package B");

      PackageManager target = new PackageManager();
      target.Add(package_a.Object);
      target.Add(package_b.Object);
      target.SetDependency(package_b.Object.Name, package_a.Object.Name);

      // Act
      var actual = target.Flatten();

      // Assert
      Assert.IsTrue(target.Items.IndexOf(package_a.Object) <
                     target.Items.IndexOf(package_b.Object));
    }

    [TestMethod()]
    public void Flatten_Dependencies2_OutputInOrder() {
      // Arrange
      var package_a = CreateMockPackage("Package A");
      var package_b = CreateMockPackage("Package B");

      PackageManager target = new PackageManager();
      target.Add(package_b.Object);
      target.Add(package_a.Object);
      target.SetDependency(package_b.Object.Name, package_a.Object.Name);

      // Act
      var actual = new List<IPackage>(target.Flatten());

      // Assert
      Assert.IsTrue(actual.IndexOf(package_a.Object) <
                     actual.IndexOf(package_b.Object));
    }

    [TestMethod]
    public void Flatten_Dependencies3_OutputInOrder() {
      // Arrange
      var package_a = CreateMockPackage("Package A");
      var package_b = CreateMockPackage("Package B");
      var package_c = CreateMockPackage("Package C");

      PackageManager target = new PackageManager();
      target.Add(package_c.Object);
      target.Add(package_b.Object);
      target.Add(package_a.Object);
      target.SetDependency(package_c.Object.Name, package_b.Object.Name);
      target.SetDependency(package_c.Object.Name, package_a.Object.Name);
      target.SetDependency(package_b.Object.Name, package_a.Object.Name);

      // Act
      var actual = new List<IPackage>(target.Flatten());

      // Assert
      Assert.IsTrue(actual.IndexOf(package_a.Object) <
                     actual.IndexOf(package_b.Object));
      Assert.IsTrue(actual.IndexOf(package_b.Object) <
                     actual.IndexOf(package_c.Object));
    }

    [TestMethod()]
    public void Flatten_NotAllDependent_OutputInOrder() {
      // Arrange
      var package_a = CreateMockPackage("Package A");
      var package_b = CreateMockPackage("Package B");
      var package_c = CreateMockPackage("Package C");

      PackageManager target = new PackageManager();
      target.Add(package_b.Object);
      target.Add(package_a.Object);
      target.Add(package_c.Object);
      target.SetDependency(package_b.Object.Name, package_a.Object.Name);

      // Act
      var actual = new List<IPackage>(target.Flatten());

      // Assert
      Assert.IsTrue(actual.Count == 3);
      Assert.IsTrue(actual.IndexOf(package_a.Object) <
                     actual.IndexOf(package_b.Object));
      CollectionAssert.Contains(actual, package_c.Object);
    }

    
  }
}
