using System;
using System.Collections.Generic;
using System.Text;

namespace IProduct.Models.Security
{
    public enum SignUpResultError
    {
        CredentialTypeNotFound
    }

    public enum ValidateResultError
    {
        CredentialTypeNotFound,
        CredentialNotFound,
        SecretNotValid
    }

    public enum ChangeSecretResultError
    {
        CredentialTypeNotFound,
        CredentialNotFound
    }


}
