using Ringo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;

namespace Ringo.Tests
{
  [TestClass()]
  public class DepencencyTests
  {
    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DepencencyConstructor_NullDependent_ThrowsException() {
      // Arrange
      IPackage dependent = null;
      var parent = new Mock<IPackage>();

      // Act
      new Depencency(dependent, parent.Object);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DepencencyConstructor_NullParent_ThrowsException() {
      // Arrange
      var dependent = new Mock<IPackage>();
      IPackage parent = null;

      // Act
      new Depencency(dependent.Object, parent);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void DepencencyConstructor_SamePackages_ThrowsException() {
      // Arrange
      var dependent = new Mock<IPackage>();

      // Act
      new Depencency(dependent.Object, dependent.Object);
    }

    [TestMethod()]
    public void DepencencyConstructor_ValidPackages_StoresDependent() {
      // Arrange
      var dependent = new Mock<IPackage>();
      var parent = new Mock<IPackage>();

      // Act
      var dependency= new Depencency(dependent.Object, parent.Object);

      // Assert
      Assert.AreEqual(dependent.Object, dependency.Dependent);
    }

    [TestMethod()]
    public void DepencencyConstructor_ValidPackages_StoresParent() {
      // Arrange
      var dependent = new Mock<IPackage>();
      var parent = new Mock<IPackage>();

      // Act
      var dependency = new Depencency(dependent.Object, parent.Object);

      // Assert
      Assert.AreEqual(parent.Object, dependency.Parent);
    }
  }
}
