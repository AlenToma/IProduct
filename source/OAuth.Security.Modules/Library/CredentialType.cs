using System;
using System.Collections.Generic;

namespace OAuth.Security.Modules.Library
{
  public class CredentialType
  {
    public Guid? Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? Position { get; set; }

    public virtual List<Credential> Credentials { get; set; }
  }
}