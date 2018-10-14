using System;
using System.Collections.Generic;

namespace OAuth.Security.Modules.Library
{
  public class User
  {
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }

    public List<UserRole> UserRoles { get; set; }

  }
}