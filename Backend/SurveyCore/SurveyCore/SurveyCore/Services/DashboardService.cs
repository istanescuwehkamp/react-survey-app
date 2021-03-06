﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SurveyCore.Models;

namespace SurveyCore.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IAggregationService aggregationService;
        public DashboardService(IAggregationService aggregationService)
        {
            this.aggregationService = aggregationService;
        }
        public async Task UpdateDashboard(SurveyData surveyData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://survey-dashboards.azurewebsites.net");
                var content = new StringContent(JsonConvert.SerializeObject(aggregationService.GetChartData(surveyData.FormName),new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()}), Encoding.UTF8, "application/json");
                var result = await client.PostAsync("/questionnaire/", content);
                string resultContent = await result.Content.ReadAsStringAsync();
            }
        }
    }
}
