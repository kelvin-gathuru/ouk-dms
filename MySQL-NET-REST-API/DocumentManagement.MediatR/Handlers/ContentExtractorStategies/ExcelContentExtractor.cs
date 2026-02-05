using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;


namespace DocumentManagement.MediatR.Handlers
{
    public class ExcelContentExtractor : IContentExtractor
    {
        public string ExtractContentByBytes(byte[] documentBytes, string tessdataPath,string tessLang)
        {
            using var memoryStream = new MemoryStream(documentBytes);
            IWorkbook workbook = new XSSFWorkbook(memoryStream); // Use HSSFWorkbook for .xls files
            ISheet sheet = workbook.GetSheetAt(0);
            string str = string.Empty;
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                IRow excelRow = sheet.GetRow(row);
                if (excelRow != null)
                {
                    for (int col = 0; col < excelRow.LastCellNum; col++)
                    {
                        ICell cell = excelRow.GetCell(col);
                        if (cell != null)
                        {
                            str += cell.ToString() + " ";
                        }
                    }
                }
            }
            if (str.Length > 0)
            {
                return str;
            }
            else
            {
                return "";
            }
        }

        public string ExtractContentByFile(IFormFile file, string tessdataPath, string tessLang)
        {
            if (file == null || file.Length == 0)
            {
                return "No file uploaded or file is empty.";
            }

            var stringBuilder = new StringBuilder();

            using (var stream = file.OpenReadStream())
            {
                // Load the Excel file into XSSFWorkbook
                XSSFWorkbook workbook = new XSSFWorkbook(stream);

                // Loop through all sheets in the workbook
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    stringBuilder.Append(sheet.SheetName);

                    // Loop through all rows in the sheet
                    for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        var row = sheet.GetRow(rowIndex);
                        if (row != null)
                        {
                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {
                                var cell = row.GetCell(cellIndex);
                                if (cell != null)
                                {
                                    // Get the value of the cell
                                    stringBuilder.Append(cell.ToString());
                                }
                            }
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
