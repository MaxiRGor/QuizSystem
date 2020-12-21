using Airport.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Airport
{
    public static class DataGridHelper
    {
        public static object GetSelectedObject(object sender)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                var contextMenu = (ContextMenu)menuItem.Parent;
                var item = (DataGrid)contextMenu.PlacementTarget;
                if (item != null && item.SelectedCells != null && item.SelectedCells[0].Item != null)
                {
                    var obj = item.SelectedCells[0].Item;
                    if (obj is AirportService || obj is Category || obj is Employee
                        || obj is FinalTestResult || obj is FinalTestResultUserAnswer || obj is Job
                        || obj is ExcelModelTestResultsOfEmployeesList || obj is ExcelModeTestResultOfDistinctEmployee
                        || obj is Question || obj is TestResult || obj is Theme || obj is UserAnswer)
                    {

                        return obj;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }

        }
    }
}
