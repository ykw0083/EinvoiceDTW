using EinvoiceDTW.Models;
using EinvoiceDTW.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EinvoiceDTW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IDataService _dataService;
        public UploadController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string type)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var jsonResult = ReadExcelToJson(path);

            IEnumerable<QueryResult> rtn = await _dataService.PushJsonToTable(jsonResult, type);

            return Ok(rtn.FirstOrDefault());
        }
        private string ReadExcelToJson(string filePath)
        {
            var dataList = new List<Dictionary<string, object>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                var headers = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Text);
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var rowData = new Dictionary<string, object>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                    }
                    dataList.Add(rowData);
                }
            }

            return JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
