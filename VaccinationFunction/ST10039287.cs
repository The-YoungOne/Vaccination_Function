using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

namespace VaccinationFunction
{
    public static class ST10039287
    {
        //main function
        [FunctionName("ST10039287")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //object of the vaccineList class that stores vaccine info
            List<vaccineList> dummyData = new List<vaccineList>();

            //dummy values saved to List of <T>
            dummyData.AddRange(new vaccineList[] {

                new vaccineList(6085007902,"Steve Jobs",87,false,null),
                new vaccineList(7453804830,"Lucian Young",19,true,"J&J"),
                new vaccineList(5043707093,"Andre Adkins",23,true,"Pfizer")
            });

            //log function that notifies whena request is processed
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.id;

            //fetching 
            long idInt;
            if(long.TryParse(id, out idInt))
            {
                int personIndex = dummyData.FindIndex(personIndex => personIndex.id == idInt);
                String responseMessage = "";
                if (personIndex != -1)
                {
                    responseMessage = $"The ID entered in the URL does not exist in th database! Please try again with a different ID.";
                }
                else
                {
                    responseMessage = $"ID: {dummyData[personIndex].id}\nFull Name: {dummyData[personIndex].fullName}" +
                        $"\nAge: {dummyData[personIndex].age}\nVaccinated: {dummyData[personIndex].vaccinated}\nVaccine Name: {dummyData[personIndex].vaccineName}";

                }

                return new OkObjectResult(responseMessage);
            }
            else
            {
                string responseMessage = "Welcome to the COVID-19 vaccination site! To check the vaccination status of an individual, please enter their id at the end of the URL --> e.g: ?id=7453804830";
                return new BadRequestObjectResult(responseMessage);
            }
        }//end of function

        //checks the id entered in the URL
    }

    //class that stores all details of the vaccination clinic
    class vaccineList
    {
        public long id { get; set; }
        public string fullName { get; set; }
        public int age { get; set; }
        public bool vaccinated { get; set; }
        public string vaccineName { get; set; }

        public vaccineList(long id, string fullName, int age, bool vaccinated, string vaccineName)
        {
            this.id = id;
            this.fullName = fullName;
            this.age = age;
            this.vaccinated = vaccinated;
            this.vaccineName = vaccineName;
        }
    }
}
