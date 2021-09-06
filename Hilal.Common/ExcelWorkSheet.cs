using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Hilal.DataViewModel.Enum;
using Hilal.DataViewModel.Excel;
using Hilal.DataViewModel.Request.Admin.v1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Hilal.Common
{
    public static class ExcelWorkSheet
    {
        public static List<CreateCitiesRequest> ImportExcelGetCities(IFormFile file, Guid? countryId)
        {
            List<CityExcelViewmodel> data = new List<CityExcelViewmodel>();
            DataTable dt = new DataTable();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<CityExcel>();
                data = csv.GetRecords<CityExcelViewmodel>().ToList();
            }
            //Loop through the Worksheet rows.
            bool firstRow = true;
            List<CreateCitiesRequest> ItemList = new List<CreateCitiesRequest>();
            foreach (var item in data)
            {
                CreateCitiesRequest cityMaster = new CreateCitiesRequest();
                cityMaster.CitiesInformations = new List<GenericDetailRequest>();
                List<GenericDetailRequest> genericDetailList = new List<GenericDetailRequest>();
                genericDetailList.Add(new GenericDetailRequest
                {
                    Name = item.ECity,
                    LanguageId = (int) ELanguage.English
                });
                genericDetailList.Add(new GenericDetailRequest
                {
                    Name = item.UCity,
                    LanguageId = (int) ELanguage.Arabic
                });
                cityMaster.CitiesInformations = genericDetailList;
                cityMaster.FkCountry = countryId;
                ItemList.Add(cityMaster);
            }


            return ItemList;
        }
    }
}
