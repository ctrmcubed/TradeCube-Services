using System;

namespace TradeCube_Services.DataObjects
{
    public class PeriodCounter
    {
        private int count;

        public int Count(DateTime dt)
        {
            if (dt.Hour == 0 && dt.Minute == 0)
            {
                count = 0;
            }
            else
            {
                count++;
            }

            return count + 1;
        }
    }
}