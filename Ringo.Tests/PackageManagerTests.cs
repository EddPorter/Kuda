using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ringo.Tests
{
  [TestClass()]
  public class PackageManagerTests
  {
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
      var package = new Mock<IPackage>();
      package.SetupGet(p => p.Name).Returns("Package Name");

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

      var parent_mock = new Mock<IPackage>();
      parent_mock.SetupGet(p => p.Name).Returns(parent_package);
      target.Add(parent_mock.Object);

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

      var dependent_mock = new Mock<IPackage>();
      dependent_mock.SetupGet(p => p.Name).Returns(dependent_package);
      target.Add(dependent_mock.Object);

      // Act
      target.SetDependency(dependent_package, parent_package);
    }

    [TestMethod()]
    public void Flatten_NoPackages_EmptyList() {
      // Arrange
      PackageManager target = new PackageManager();
      List<IPackage> actual;

      // Act
      actual = target.Flatten();

      // Assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(0, actual.Count);
    }

    [TestMethod()]
    public void Flatten_NoDependencies_AllPackagesAreOutput() {
      // Arrange
      PackageManager target = new PackageManager();
      List<IPackage> packages = new List<IPackage>();
      for (int n = 0; n < 10; ++n) {
        var package = new Mock<IPackage>();
        package.SetupGet(p => p.Name).Returns("Package " + n);
        packages.Add(package.Object);
        target.Add(package.Object);
      }
      List<IPackage> actual;

      // Act
      actual = target.Flatten();

      // Assert
      Assert.IsNotNull(actual);
      Assert.AreEqual(10, actual.Count);
      CollectionAssert.AreEquivalent(packages, actual);
    }

    [TestMethod()]
    public void Flatten_Dependencies_OutputInOrder() {
      // Arrange
            var package_a = new Mock<IPackage>();
      package_a.SetupGet(p => p.Name).Returns("Package A");
      var package_b = new Mock<IPackage>();
      package_b.SetupGet(p => p.Name).Returns("Package B");

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
      var package_a = new Mock<IPackage>();
      package_a.SetupGet(p => p.Name).Returns("Package A");
      var package_b = new Mock<IPackage>();
      package_b.SetupGet(p => p.Name).Returns("Package B");

      PackageManager target = new PackageManager();
      target.Add(package_b.Object);
      target.Add(package_a.Object);
      target.SetDependency(package_b.Object.Name, package_a.Object.Name);

      // Act
      var actual = target.Flatten();

      // Assert
      Assert.IsTrue(actual.IndexOf(package_a.Object) <
                     actual.IndexOf(package_b.Object));
    }
  }
}
