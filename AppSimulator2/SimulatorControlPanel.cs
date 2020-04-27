using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
using Newtonsoft.Json;


namespace AppSimulator2
{
    class SimulatorControlPanel
    {

        int Userscreated = 0;

        /// <summary>
        /// Get the Example Data with simulated names and Usa Cities
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Geo.RootObject GetExampleData () {
            //  Read all 5000 Random Names
            string fullpath = System.IO.Directory.GetCurrentDirectory() + "\\ExampleData\\5000Names.txt";
            System.IO.StreamReader sr = new System.IO.StreamReader(fullpath);

            // Read All Cities
            string citiespath = System.IO.Directory.GetCurrentDirectory() + "\\ExampleData\\USACities.json";
            string cities = System.IO.File.ReadAllText(citiespath);
            var serializer = new JsonSerializer();
            Geo.RootObject p;

            using (var sw = new System.IO.StreamReader(fullpath))
            {
                using (var reader = new JsonTextReader(sw))
                {
                    p = serializer.Deserialize<Geo.RootObject>(reader);
                }
            }
            return p;
        }


        void GenerateUsers(ushort numUsers)
        {

                    


           Geo.RootObject ExampleData = GetExampleData()

            for (int i = 0; i < numUsers; i++)
            {

                // Generate Random Users
                grpcPlayDateService.UserEntity User = new grpcPlayDateService.UserEntity();
                User.ID = Guid.NewGuid().ToString();
                User.FullName = sr.ReadLine();
                Random rand = new Random();

                User.SearchRadiusKM = rand.Next(100, 701);

                int Pos = rand.Next(1, 1000);
                User.Longitude = ExampleData.features[Pos].geometry.coordinates[0];
                User.Latitude = ExampleData.features[Pos].geometry.coordinates[1];
                User.Gender = rand.Next(1, 3);
                User.Genderpreference = rand.Next(1, 4);
                User.CreatedDate = DateTime.UtcNow;

                //Generate random birthday from 1940 to 2000.
                DateTime birth;
                birth = new DateTime(rand.Next(1940, 2001), rand.Next(1, 12), rand.Next(1, 28), System.Globalization.Calendar.CurrentEra, 0, 0, DateTimeKind.Utc);
                User.BirthDay = birth;

                //Generate Random Agepreference by using the users current age
                TimeSpan ts = DateTime.UtcNow - birth.AddYears(rand.Next(1, 8));
                User.AgePreferenceMin = Convert.ToInt32(ts.TotalDays / 365.25);

                ts = DateTime.UtcNow - birth.AddYears(-rand.Next(1, 8));
                User.AgePreferenceMax = Convert.ToInt32(ts.TotalDays / 365.25);

                YoloDatingClient.SaveUser(User);
                Userscreated++;
                StateHasChanged();

            }




        }

    }
}
