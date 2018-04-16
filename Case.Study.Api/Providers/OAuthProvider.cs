using System;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;
using Case.Study.Api.Models;
using Case.Study.Api.Utilities;
using System.Threading;

namespace Case.Study.Api.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        #region[GrantResourceOwnerCredentials]
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                var userName = context.UserName;
                var password = context.Password;

                using (UserRepository userRepository_ = new UserRepository(new MyTestDBEntities()))
                {
                    var user_ = userRepository_.GetUserByEmailOrUserName(userName, userName);
                    if (user_ != null)
                    { 
                        // password hash disabled. 
                        //var passwordHash_ = user_.User_PasswordHash;
                        //var encodingPasswordString_ = Helper.EncodePassword(password, passwordHash_);
                        if (user_.User_Password == password)
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(user_.User_ID)));
                            claims.Add(new Claim(ClaimTypes.Name, user_.User_UserName));
                            claims.Add(new Claim(ClaimTypes.Email, user_.User_Email));

                            switch (user_.User_RoleID)
                            {
                                case (int)AuthorizationTypes.User:
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.User.ToString()));
                                    break;
                                case (int)AuthorizationTypes.Moderator:
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.User.ToString()));
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.Moderator.ToString()));
                                    break;
                                case (int)AuthorizationTypes.Administrator:
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.User.ToString()));
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.Moderator.ToString()));
                                    claims.Add(new Claim(ClaimTypes.Role, AuthorizationTypes.Administrator.ToString()));
                                    break;
                                default:
                                    break;
                            }
                            ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                                        Startup.OAuthOptions.AuthenticationType); 
                            var properties = CreateProperties(user_.User_UserName);
                            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                            context.Validated(ticket); 
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Invalid User or Pass");
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Registration status QUE.");
                    }
                }
            });
        }
        #endregion

        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[CreateProperties]
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
        #endregion
    }
}