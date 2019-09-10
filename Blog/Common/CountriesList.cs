using System;
using System.Collections.Generic;
using System.Globalization;

namespace Blog.Common
{
    public class CountriesList
    {
        public static List<string> Countries()
        {
            //get list of countries 
            List<string> countryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo r = new RegionInfo(CInfo.LCID);
                if (!(countryList.Contains(r.EnglishName)))
                {
                    countryList.Add(r.EnglishName);
                }
            }

            countryList.Sort();

            return countryList;
        }
    }
}