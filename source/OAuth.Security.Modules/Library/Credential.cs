using System;

namespace OAuth.Security.Modules.Library
{
  public class Credential
  {
    public Guid? Id { get; set; }
    public Guid User_Id { get; set; }
    public Guid CredentialType_Id { get; set; }
    public string Identifier { get; set; }
    public string Secret { get; set; }
    public string Extra { get; set; }

    public virtual User User { get; set; }
    public virtual CredentialType CredentialType { get; set; }
  }
}