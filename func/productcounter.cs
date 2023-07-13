using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suitcaseV2.func
{
    internal class productcounter
    {
        public long endProduct = 0;
        Random random = new Random();

        public string productcounterfunc(string input,int setInterval)
        {
            int timeinterval=10;
            if(setInterval!=0) { 
                timeinterval = setInterval;
            }
            long StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            double RandomNumber = random.NextDouble();
            if (input == "start")
            {
                if(StartTime-endProduct >=timeinterval*1000){
                    endProduct = StartTime;
                    

                    if (RandomNumber >= 0.5)
                    {
                        return "defect";
                    }
                    else
                    {
                        return "nodefect";
                    }
                }
            }

            return null;
        }


    }
}
