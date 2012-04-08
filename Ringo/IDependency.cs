using System;
namespace Ringo
{
  public interface IDependency
  {
    IPackage Dependent { get; }
    IPackage Parent { get; }
  }
}
