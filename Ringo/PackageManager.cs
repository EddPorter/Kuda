using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ringo
{
  public class PackageManager
  {

    private List<IPackage> packages_ = new List<IPackage>();
    public ReadOnlyCollection<IPackage> Items {
      get {
        return packages_.AsReadOnly();
      }
    }

    private List<IDependency> dependencies_ = new List<IDependency>();
    public ReadOnlyCollection<IDependency> Dependencies {
      get {
        return dependencies_.AsReadOnly();
      }
    }

    public void Add(IPackage package) {
      if (package == null) {
        throw new ArgumentNullException("package", "The package must be not " +
          "be null.");
      }

      packages_.Add(package);
    }

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

      if (dependencies_.Any(
                         d => d.Parent == parent && d.Dependent == dependent)) {
        throw new ArgumentException(string.Format("Package {0} is already " +
          "dependent on package {1}.", dependent_package, parent_package),
          "dependent_package");
      }

      List<IPackage> to_explore = new List<IPackage>();
      List<IPackage> explored = new List<IPackage>();
      to_explore.Add(dependent);
      while (to_explore.Count > 0) {
        var exploring = to_explore[0];
        if (exploring == parent) {
          throw new ArgumentException("Adding this dependency would create a " +
            "dependency cycle.", "dependent_package");
        }
        to_explore.Remove(exploring);
        if (!explored.Contains(exploring)) {
          explored.Add(exploring);
          var existing_dependencies = dependencies_.
                                       Where(d => d.Parent == exploring).
                                       Select(d => d.Dependent);
          to_explore.AddRange(existing_dependencies);
        }
      }

      dependencies_.Add(new Depencency(dependent, parent));
    }

    public List<IPackage> Flatten() {
      // This method assumes that the dependency list is acyclic.

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