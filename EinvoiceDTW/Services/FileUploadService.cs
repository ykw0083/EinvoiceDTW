using EinvoiceDTW.Models;
using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EinvoiceDTW.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IBrowserFile file, string type);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IDataService _dataService;
        public FileUploadService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<string> UploadFileAsync(IBrowserFile file, string type)
        {
            if (file == null || file.Size == 0)
            {
                return null; // Or throw an exception
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            Directory.CreateDirectory(uploadsFolder);

            Guid guid = Guid.NewGuid();
            string myfilename = guid.ToString() + "_" + file.Name;
            var filePath = Path.Combine(uploadsFolder, myfilename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream().CopyToAsync(stream);
            }

            var jsonResult = ReadExcelToJson(filePath);

            IEnumerable<QueryResult> rtn = await _dataService.PushJsonToTable(jsonResult, type, myfilename);

            if (rtn.FirstOrDefault().QueryCode == 0)
                return "Upload done.";
            else
                return rtn.FirstOrDefault().QueryMsg;
        }
        private string ReadExcelToJson(string filePath)
        {
            var dataList = new List<Dictionary<string, object>>();

            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
