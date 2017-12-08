using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace CivilMapSQLDatabase
{
    public class DataExport : CivilMapSQLDatabaseConnection
    {
        public void ExportCivilMapPurifiedAddress(string directry, List<PurifiedAddressModel> model)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Debug.WriteLine("Excel is not properly installed");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "PurifiedAddressId";
            xlWorkSheet.Cells[1, 2] = "StreetNumber";
            xlWorkSheet.Cells[1, 3] = "Direction";
            xlWorkSheet.Cells[1, 4] = "Street";
            xlWorkSheet.Cells[1, 5] = "City";
            xlWorkSheet.Cells[1, 6] = "Zipcode";
            xlWorkSheet.Cells[1, 7] = "StreetType";
            xlWorkSheet.Cells[1, 8] = "AddressType";
            xlWorkSheet.Cells[1, 9] = "Longitude";
            xlWorkSheet.Cells[1, 10] = "Latitude";
            xlWorkSheet.Cells[1, 11] = "Polygon";

            for (int i = 0; i < model.Count; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = model[i].PurifiedAddressId.ToString();
                xlWorkSheet.Cells[i + 2, 2] = model[i].AddressModel.StreetNumber;
                xlWorkSheet.Cells[i + 2, 3] = model[i].AddressModel.Direction;
                xlWorkSheet.Cells[i + 2, 4] = model[i].AddressModel.Street;
                xlWorkSheet.Cells[i + 2, 5] = model[i].AddressModel.City;
                xlWorkSheet.Cells[i + 2, 6] = model[i].AddressModel.Zipcode;
                xlWorkSheet.Cells[i + 2, 7] = model[i].StreetType;
                xlWorkSheet.Cells[i + 2, 8] = model[i].AddressType;
                xlWorkSheet.Cells[i + 2, 9] = model[i].Longitude;
                xlWorkSheet.Cells[i + 2, 10] = model[i].Latitude;
                xlWorkSheet.Cells[i + 2, 11] = model[i].Polygon;
            }

            xlWorkBook.SaveAs(directry, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
