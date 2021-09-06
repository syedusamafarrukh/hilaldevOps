using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hilal.DataViewModel.Excel
{
    public class CityExcel : ClassMap<CityExcelViewmodel>
    {
        public CityExcel()
        {
            Map(m => m.ECity).Name("ECity");
            Map(m => m.UCity).Name("UCity");
        }
    }

    public class CityExcelViewmodel
    {
        [Name("ECity")]
        public string ECity { get; set; }
        [Name("UCity")]
        public string UCity { get; set; }
    }
}
