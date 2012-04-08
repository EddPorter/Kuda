using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ringo.Tests
{
  [TestClass()]
  public class PackageTests
  {
    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void PackageConstructor_NullName_ThrowsException() {
      // Arrange
      string package_name = null;

      // Act
      new Package(package_name);

      // Assert      
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void PackageConstructor_EmptyName_ThrowsException() {
      // Arrange
      string package_name = string.Empty;

      // Act
      new Package(package_name);

      // Assert      
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void PackageConstructor_WhiteSpaceName_ThrowsException() {
      // Arrange
      string package_name = "  \t\t \n \r ";

      // Act
      new Package(package_name);

      // Assert      
    }

    [TestMethod()]
    public void PackageConstructor_ValidName_PackageKeepsName() {
      // Arrange
      string package_name = "Package Name";

      // Act
      var package = new Package(package_name);

      // Assert 
      Assert.AreEqual(package_name, package.Name);
    }

    [TestMethod()]
    public void PackageConstructor_NameSurroundedByWhiteSpace_PackageKeepsTrimmedName() {
      // Arrange
      string whitespace = "  \r\t \n ";
      string package_name = "Package Name";

      // Act
      var package = new Package(whitespace + package_name + whitespace);

      // Assert 
      Assert.AreEqual(package_name, package.Name);
    }
  }
}
