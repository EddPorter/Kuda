using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Ringo
{
  public class PackageManager
  {
    public void Add(IPackage package) {
      if (package == null) {
        throw new ArgumentNullException("package", "The package must be not " +
          "be null.");
      }

      packages_.Add(package);
    }

    private List<IPackage> packages_ = new List<IPackage>();
    public ReadOnlyCollection<IPackage> Items {
      get {
        return packages_.AsReadOnly();
      }
    }

    private List<IDependency> dependencies_ = new List<IDependency>();

    public void SetDependency(string dependent_package, string parent_package) {
      var dependent = packages_.Find(p => p.Name == dependent_package);
      if (dependent == null) {
        throw new ArgumentException(string.Format("No package matching the " +
          "dependent package ({0}) was found.", dependent_package),
          "dependent_package");
      }

      var parent = packages_.Find(p => p.Name == parent_package);
      if (parent == null) {
        throw new ArgumentException(string.Format("No package matching the " +
          "parent package ({0}) was found.", parent_package), "parent_package");
      }

      Depencency d = new Depencency(dependent, parent);
      dependencies_.Add(d);
    }

    public List<IPackage> Flatten() {
      List<IPackage> output = new List<IPackage>();

      List<IPackage> packages_clone = new List<IPackage>(packages_);
      List<IDependency> dependencies_clone =
                                           new List<IDependency>(dependencies_);
      while (dependencies_clone.Count > 0) {
        IDependency d = dependencies_clone[0];
        while (true) {
          var parent = d.Parent;
          // See if anything depends on this
          var d2 = dependencies_clone.FirstOrDefault(dep => dep.Dependent == parent);
          if (d2 == null) { break; }
          d = d2;
        }
        // The dependency d is the top of its chain. Add the parent package if
        // it hasn't already been added.
        if (packages_clone.Contains(d.Parent)) {
        output.Add(d.Parent);
          packages_clone.Remove(d.Parent);
          dependencies_clone.Remove(d);
        }
      }

      // Add the remaining packages - these have no dependencies.
      foreach (IPackage package in packages_clone) {
        output.Add(package);
      }
      return output;
    }
  }
}