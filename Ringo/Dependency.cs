using System;

namespace Ringo
{
  public class Dependency : IDependency
  {
    public IPackage Dependent { get; private set; }
    public IPackage Parent { get; private set; }

    public Dependency(IPackage dependent, IPackage parent) {
      if (dependent == null) {
        throw new ArgumentNullException("dependent", "The dependent package " +
          "cannot be null.");
      }
      if (parent == null) {
        throw new ArgumentNullException("parent", "The parent package cannot " +
          "be null.");
      }
      if (dependent == parent) {
        throw new ArgumentException("A package cannot be dependent on itself.",
          "parent");
      }

      Dependent = dependent;
      Parent = parent;
    }
  }
}
