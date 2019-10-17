using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolioSite.FolderForScenriosDownloaderTest
{
    public class Scenario
    {
        public string ScenarioName { get; set; }
        public string ScenarioDescription { get; set; }
    }

    public class ScenariosObject
    {
        public List<Scenario> Scenarios { get; set; }
    }

    public class ScenariosContext
    {
        public static object GetJSON (string Scenario)
        {
            try
            {
               object jsonObjectScenario = JsonConvert.DeserializeObject<List<ScenariosObject>>(Scenario);
               return jsonObjectScenario;
            }
            catch (Exception e)
            {
                string r = e.ToString();
                return new { ScenarioName = "Not Scenario Name", ScenariyDescription = "Not Scenario Description" };
            }

        }

    }
}