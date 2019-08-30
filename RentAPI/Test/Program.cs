using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;

using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            test();
        }

        async static void test()
        {
            //string sFileName = @"demo.xlsx";
            //FileInfo file = new FileInfo(Path.Combine("./", sFileName));
            //var memory = new MemoryStream();
            //using (var fs = new FileStream(Path.Combine("./", sFileName), FileMode.Create, FileAccess.Write))
            //{
            //    IWorkbook workbook;
            //    workbook = new XSSFWorkbook(Path.Combine("./", @"report.xlsx"));
            //    ISheet excelSheet = workbook.GetSheet("DATA");
            //    IRow row = excelSheet.CreateRow(0);

            //    excelSheet.GetRow(5).GetCell(6).SetCellValue("HEY :D");

            //    /*
            //    row.CreateCell(10).SetCellValue("ID");
            //    row.CreateCell(1).SetCellValue("Name");
            //    row.CreateCell(2).SetCellValue("Age");

            //    row = excelSheet.CreateRow(1);
            //    row.CreateCell(0).SetCellValue(1);
            //    row.CreateCell(1).SetCellValue("Kane Williamson");
            //    row.CreateCell(2).SetCellValue(29);

            //    row = excelSheet.CreateRow(2);
            //    row.CreateCell(0).SetCellValue(2);
            //    row.CreateCell(1).SetCellValue("Martin Guptil");
            //    row.CreateCell(2).SetCellValue(33);

            //    row = excelSheet.CreateRow(3);
            //    row.CreateCell(0).SetCellValue(3);
            //    row.CreateCell(1).SetCellValue("Colin Munro");
            //    row.CreateCell(2).SetCellValue(23);
            //    */

            //    workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine("./", sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}
