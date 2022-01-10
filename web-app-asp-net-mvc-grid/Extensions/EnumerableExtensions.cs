using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using web_app_asp_net_mvc_grid.Models.Xlsx;

namespace web_app_asp_net_mvc_grid.Extensions
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
        {
            foreach (var item in items)
            {
                var value = valueSelector(item);

                yield return new SelectListItem
                {
                    Text = nameSelector(item),
                    Value = value.ToString(),
                    Selected = selectedValueSelector(item)
                };
            }
        }

        public static MemoryStream ToXlsx<TItem>(this IEnumerable<TItem> items)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Data");
            var table = new DataTable();

            var columns = new List<XlsxColumn>();
            var data = new XlsxRender()
            {
                Items = items.Select(x => (object)x).ToList(),
                Columns = typeof(TItem).GetProperties().Where(XlsxRender.ValidTypePredicate).Select((x, i) => new XlsxColumn()
                {
                    ColumnType = x.PropertyType,
                    Order = i,
                    DisplayName = x.Name
                }).ToList()
            };

            foreach (var column in data.Columns)
                table.Columns.Add(column.DisplayName, column.ColumnType.IsEnum || column.ColumnType == typeof(bool) || (column.ColumnType.IsGenericType && column.ColumnType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? typeof(string) : (column.ColumnType ?? typeof(string)));

            foreach (var item in data.Items)
                table.Rows.Add(item.GetType().GetProperties()
                    .Where(XlsxRender.ValidTypePredicate)
                    .Select(column =>
                    column.PropertyType.IsEnum ? ((Enum)column.GetValue(item)).GetDisplayValue() :
                    column.PropertyType == typeof(bool) ? (((bool)column.GetValue(item)) ? "Да" : "Нет") :
                    (column.PropertyType.IsGenericType && column.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? (column.GetValue(item) != null ? column.GetValue(item).ToString() : "-") :
                    column.GetValue(item)).ToArray());

            var xlTable = ws.Cell(1, 1).InsertTable(table.AsEnumerable());
            ws.Columns().AdjustToContents();

            var ms = new MemoryStream();
            wb.SaveAs(ms);

            return ms;
        }

    }
}