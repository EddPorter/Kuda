using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo
{
  public class Package : IPackage
  {
    public string Name { get; private set; }

    public Package(string package_name) {
      if (string.IsNullOrWhiteSpace(package_name)) {
        throw new ArgumentNullException("package_name", "A package name must " +
          "be specified that is not null nor comprised of solely white space.");
      }

      Name = package_name.Trim();
    }
  }
}
