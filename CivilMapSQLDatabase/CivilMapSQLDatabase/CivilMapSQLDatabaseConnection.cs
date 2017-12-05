using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace CivilMapSQLDatabase
{
    public partial class CivilMapSQLDatabaseConnection : IDisposable
    {
        static void Main(string[] args)
        {
            CrimeAndArrestDataRetrieve crimeAndArrestDataRetrieve = new CrimeAndArrestDataRetrieve();
            AddressPurification addressPurification = new AddressPurification();
            VisualizationDataAggregation visualizationDataAggregation = new VisualizationDataAggregation();

            //List<AddressPolygonModel> list = addressPurification.SelectArrestAddressPolygon();
            //addressPurification.UpdateCivilMapPurifiedAddressOnPolygons(list);
            //list = addressPurification.SelectCrimeAddressPolygon();
            //addressPurification.UpdateCivilMapPurifiedAddressOnPolygons(list);

            //var address = new PurifiedAddressModel();
            //address.AddressModel.Street = "Waldron Street";
            //address.AddressModel.StreetNumber = "417";
            //address.AddressModel.Zipcode = "47906";
            //addressPurification.ValidateAddress(address);


            //var lon = -87.743611;
            //var lat = 41.116365;
            //var rad = 2;
            //visualizationDataAggregation.SelectCivilMapPurifiedAddress(lon, lat, rad);


            //crimeAndArrestDataRetrieve.GetCivilMapCrime();
            //crimeAndArrestDataRetrieve.GetCivilMapArrest();

            //addressPurification.DeleteTable();
            //addressPurification.AddKristinTest(PurifiedAddressModelPrepare());

            //addressPurification.AddCivilMapPurifiedAddress(PurifiedAddressModelPrepare());
            //addressPurification.AddCivilMapPoints(PointsModelPrepare());
            //addressPurification.AddCivilMapNonPurifiedAddress(NonPurifiedAddressModelPrepare());

            //addressPurification.GetCivilMapPurifiedAddress();
            //addressPurification.GetCivilMapNonPurifiedAddress();
            //addressPurification.GetCivilMapPoints();


            //////////////////////////////////////////////////////////////////
            //Debug.WriteLine("Before");
            //addressPurification.Validate100Addresses();

            //Debug.WriteLine("AFter");
            //////////////////////////////////////////////////////////////////


            //addressPurification.SelectCivilMapPurifiedAddress("cf4ac28b-d847-45f5-849c-15a7e6820227", "hah", Convert.ToDecimal(125.36), Convert.ToDecimal(468.25));
            //addressPurification.SelectCivilMapNonPurifiedAddress(125568, "din", "cf4ac28b-d847-45f5-849c-15a7e6820227");
            //addressPurification.SelectCivilMapPoints(56896, "f8592e81-2dff-48c0-b005-0068df163e0e");

            //addressPurification.UpdateCivilMapNonPurifiedAddress(100, "Lawson computer science building", "ca173871-2ef4-4eff-a3c2-241cb1500d4b");
        }

        void IDisposable.Dispose()
        {
           
        }

        private static void ReadExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\zhan1803\Desktop\Polygons\xls\cpd_beats.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            List<string> objectId = new List<string>();
            List<string> district = new List<string>();
            List<string> sector = new List<string>();
            List<string> beat = new List<string>();
            List<string> beat_num = new List<string>();

            for (int i = 2; i <= rowCount; i++)
            {
                objectId.Add(xlRange.Cells[i, 1].Value2.ToString());
                district.Add(xlRange.Cells[i, 2].Value2.ToString());
                sector.Add(xlRange.Cells[i, 3].Value2.ToString());
                beat.Add(xlRange.Cells[i, 4].Value2.ToString());
                beat_num.Add(xlRange.Cells[i, 5].Value2.ToString());
            }

            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
        }

        private static void WriteExcel(List<PurifiedAddressModel> model)
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

            for (int i = 0; i < model.Count(); i++)
            {
                xlWorkSheet.Cells[i + 1, 3] = model[i].Longitude;
                xlWorkSheet.Cells[i + 1, 4] = model[i].Latitude;
            }

            xlWorkBook.SaveAs(@"C:\Users\zhan1803\Desktop\newFakeData.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }

        private static void InsertTestArrest()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\zhan1803\Desktop\ArrestFakeData.xls");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            List<string> id = new List<string>();
            List<DateTime> date = new List<DateTime>();
            List<string> longitude = new List<string>();
            List<string> latitude = new List<string>();
            List<string> beat = new List<string>();

            for (int i = 1; i <= 704; i++)
            {
                id.Add(xlRange.Cells[i, 1].Value2.ToString()); 
                date.Add(DateTime.FromOADate(xlRange.Cells[i, 2].Value2));
                longitude.Add(xlRange.Cells[i, 3].Value2.ToString());
                latitude.Add(xlRange.Cells[i, 4].Value2.ToString());
                beat.Add(xlRange.Cells[i, 5].Value2.ToString());
            }
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    for (int i = 0; i < id.Count(); i++)
                    {
                        string commandText = "Insert into TestCrime (Id, Date, Longitude, Latitude, Beat) values " +
                                 "(@Id, @Date, @Longitude, @Latitude, @Beat)";
                        SqlCommand command = new SqlCommand(commandText, connection);

                        command.Parameters.AddWithValue("@Id", id[i]);
                        command.Parameters.AddWithValue("@Date", date[i]);
                        command.Parameters.AddWithValue("@Longitude", longitude[i]);
                        command.Parameters.AddWithValue("@Latitude", latitude[i]);
                        command.Parameters.AddWithValue("@Beat", beat[i]);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    Debug.WriteLine("finished!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
