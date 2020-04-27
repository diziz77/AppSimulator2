using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;

using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken.Model;

namespace AppSimulator2
{
    
    public partial class AppSimulatorUI : Form
    {
        AppSimulator AppSim = new AppSimulator();
        public AppSimulatorUI()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            AmazonCognitoIdentityProviderClient provider =
         new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast2);
            String AccessToken = "";

            CognitoUserPool userPool = new CognitoUserPool("us-east-2_5823vgrxD", "7dpaqpkf9b5pjp9gg94k3gndld", provider);
            CognitoUser user = new CognitoUser("AppUser2", "7dpaqpkf9b5pjp9gg94k3gndld", userPool, provider);
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = "XEl9G0&O_1"
            };

            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
            {
                String NewPassword;
                frmEnterNewPassword f = new frmEnterNewPassword();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    NewPassword = f.Controls["txtPassword"].Text;
                    authResponse = await user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
                    {
                        SessionID = authResponse.SessionID,
                        NewPassword = NewPassword
                    });
                    AccessToken = authResponse.AuthenticationResult.AccessToken;
                }


            }
            else
            {
                AccessToken = authResponse.AuthenticationResult.AccessToken;
            }


            CognitoAWSCredentials credentials =
    user.GetCognitoAWSCredentials("us-east-2:bd9037c3-26d6-432a-a6a9-9d66281f49ba", Amazon.RegionEndpoint.USEast2);


            //            CognitoAWSCredentials credentials = new CognitoAWSCredentials("us-east-2:bd9037c3-26d6-432a-a6a9-9d66281f49ba", Amazon.RegionEndpoint.USEast2);

            //   credentials.AddLogin("www.amazon.com", AccessToken);
            //credentials.

            //Cognitocr


            using (var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2))
            {
                //var response1 =
                //    await client.ListBucketsAsync(new ListBucketsRequest()).ConfigureAwait(false);
                //client.DeleteBucketAsync("Kalle");
                //client.ListVersionsAsync("ssdfsdf");
                ListBucketsResponse response =
                    await client.ListBucketsAsync(new ListBucketsRequest()).ConfigureAwait(false);

                foreach (S3Bucket bucket in response.Buckets)
                {
                    Console.WriteLine(bucket.BucketName);
                }
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AppSim.SignUpNewUser(txtSignUpEmail.Text, txtPassword.Text, txtFamilyName.Text, txtFirstName.Text, txtPhoneNumber.Text, txtDeviceID.Text);

        }

        private void btnSignUpSim_Click(object sender, EventArgs e)
        {
            AppSim.SignUpNewUser("joakim.bergstrom77777@gmail.com", "P@ssword_1", "Oskarsson", "Edwin", "0920992021", "FakeDeviceID_1111");
        }
    }
    }
