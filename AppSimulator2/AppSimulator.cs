using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken.Model;
using System.Configuration;
using System.Diagnostics;

namespace AppSimulator2
{
    class AppSimulator
    {

        public async void SignUpNewUser(string email, string password, string familyName, string firstName, string phoneNumber, string deviceId)
        {
            AnonymousAWSCredentials credentials = new AnonymousAWSCredentials();
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(credentials,Amazon.RegionEndpoint.USEast2);
            
            CognitoUserPool pool = new CognitoUserPool(ConfigurationManager.AppSettings["USERPOOL_ID"], ConfigurationManager.AppSettings["CLIENT_ID"], provider, "");

            // Based on latest user pool API
            // https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_CreateUserPool.html
            // https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_VerificationMessageTemplateType.html

            // https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/aws-resource-cfn-customresource.html

            Dictionary<string, string> userAttributes = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "email", email},
            { "family_name", familyName},
            { "given_name", firstName},
       };
            Dictionary<string, string> validationData = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "email", email}
        };

            await pool.SignUpAsync(email, password, userAttributes, validationData).ConfigureAwait(false);

            //Get the UsersVerificationCode programatically.
            Task<AdminConfirmSignUpResponse> myresponse = provider.AdminConfirmSignUpAsync(new AdminConfirmSignUpRequest{UserPoolId= ConfigurationManager.AppSettings["USERPOOL_ID"], Username = email});


            AdminUpdateUserAttributesRequest i = new AdminUpdateUserAttributesRequest();
            i.UserAttributes.Add(new AttributeType { Name = "email_verified", Value = "true" });
            i.UserPoolId = ConfigurationManager.AppSettings["USERPOOL_ID"];
            i.Username = email;
            
        
           AdminUpdateUserAttributesResponse T = await provider.AdminUpdateUserAttributesAsync(i);
            Debug.Print(T.ToString());
  //          client.adminUpdateUserAttributes({
  //          UserAttributes:
  //              [{
  //              Name: 'phone_number_verified',
  //    Value: 'true'
  //            }, {
  //              Name: 'email_verified',
  //    Value: 'true'
  //           }
  //  // other user attributes like phone_number or email themselves, etc
  //],
  //UserPoolId: 'COGNITO_USER_POOL_ID_HERE',
  //Username: 'USERNAME'


            //myresponse.res.
           // Debug.Print(myresponse.Result.ToString());
        }
        public async void LoginAsAdmin()
        {

        }


    }
}
