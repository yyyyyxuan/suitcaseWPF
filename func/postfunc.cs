using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace suitcaseV2.func
{
    internal class postfunc
    {
        public void Post(int defect, int nodefect,int active,int notactive,int halt,int haltcount,TextBlock waBlock)//Update constant tags on webaccess
        {
            string jsonString = @"{
                ""Tags"":[{
                    ""Name"":""PassCount"",
                    ""Value"":" + nodefect + @"
                },{
                    ""Name"":""HaltCount"",
                    ""Value"":" + haltcount + @"
                },{
                    ""Name"":""DefectCount"",
                    ""Value"":" + defect + @"
                },{
                    ""Name"":""Active"",
                    ""Value"":" + active + @"
                },{
                    ""Name"":""NotActive"",
                    ""Value"":" + notactive + @"
                },{
                    ""Name"":""HaltTime"",
                    ""Value"":" + halt + @"
                }]}";

            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("admin" + ":" + "")));
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var responseTask = httpClient.PostAsync(Config.data.WebAPI, content);
                var response = responseTask.Result;
                httpClient.Dispose();
            }
            catch (Exception ex)
            {
                waBlock.Text = "Connection Failed";
                waBlock.Foreground = Brushes.Red;
            }
            waBlock.Text = "Connected";
            waBlock.Foreground = Brushes.Green;
        }
    }
}
