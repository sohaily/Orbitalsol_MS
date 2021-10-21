using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API.Models
{
    public enum SignUpStatusEnum
    {
        EmailConfirmationPending = 1, // New user registration and email sent for verification
        EmailVerified = 2,            // When a user confirm his/her email
        PasswordUpdated = 3,          // After email verification user prompt to set his/her pswd to continue and he/she updated it
        ProfileUpdated = 4,           // After pswd updation user needs to update his/her First or Last Name and he/she updated it
        Disabled = 5                  // To use block/restrict any user from signing in the app.
    }
}
