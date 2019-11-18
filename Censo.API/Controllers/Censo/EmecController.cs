using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Censo.API.Data;
using Censo.API.Model;
using Censo.API.Model.Censo;
using Censo.API.Parametros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Censo.API.Controllers.Censo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using System.ComponentModel;

namespace Censo.API.Controllers.Censo
{
    [AllowAnonymous]
    [Route("api/Censo/[controller]")]
    [ApiController]
    public class EmecController: ControllerBase
    {

        public EmecController()
        {
            
        }
        

        // download pdf from site
        [HttpGet]
        public FileStreamResult  Get(){

            FileStream fs = new FileStream(@"C:\Users\thiago.caldas\Desktop\Atividades_2019-11-05.xlsx", FileMode.Open, FileAccess.Read);
            
            // File fsr = File(fs ,"application/Excel", "Alunos.xlsx");

    
            return File(fs ,"application/Excel", "Alunos.xlsx");
            
        }

          // download pdf from site
        [HttpGet("Excel")]
        public FileStreamResult  ExportaExcel(){

            var Item = new ExcelSheetItemDto() {
                
                Name = "Thiago Caldas",
                Quantity = 34

            };
            
            var Item2 = new ExcelSheetItemDto() {
                
                Name = "Joyce Sena",
                Quantity = 27

            };

            List<ExcelSheetItemDto> sheetItems = new List<ExcelSheetItemDto>();

            sheetItems.Add(Item);
            sheetItems.Add(Item2);


            // IEnumerable<ExcelSheetItemDto> sheetItems;


            var stream = new MemoryStream();

             using (var package = new ExcelPackage(stream)) {                
                var workSheet = package.Workbook.Worksheets.Add("Dados");
                workSheet.Cells.LoadFromCollection(sheetItems, true);
                package.Save();            
            };  

           stream.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = "relatório.xlsx";
    
            return File(stream, contentType, fileName);
            
        }

        
    }

public class ExcelSheetItemDto {
  [DisplayName("First Name")]        
  public string Name { get; set; }
  [DisplayName("Quantity")]        
  public double Quantity { get; set; }   
}
}