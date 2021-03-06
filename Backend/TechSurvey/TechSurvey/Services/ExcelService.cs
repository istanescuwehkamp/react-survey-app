﻿using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using TechSurvey.Models;

namespace TechSurvey.Services
{
    public class ExcelService
    {
        private readonly string excelFilePath =
            @"C:\OneDrive\OneDrive - Levi9 IT Services\TechSurveyAnswers.xlsx";

        public void UpdateExcelUsingClosedXml(SurveyData surveyData)
        {
            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheets.Worksheet(1);

                var firstEmptyRow = worksheet.Worksheet.LastRowUsed().RowNumber() + 1;

                var columnIndex = 1;

                //set user id in first cell
                worksheet.Cell(firstEmptyRow, 1).Value = surveyData.UserId + firstEmptyRow;

                //set answers in rest of cells
                foreach (var answer in surveyData.Questions)
                {
                    columnIndex++;
                    worksheet.Cell(firstEmptyRow, columnIndex).Value = string.Join(", ", answer.SelectedAnswers.ToArray());
                }

                workbook.CalculateMode = XLCalculateMode.Auto;

                workbook.Save();
            }
        }
        public void UpdateExcel(SurveyData surveyData)
        {
            //using Interop.Excel
            var excelApp = new Application();
            var workbooks = excelApp.Workbooks;
            var openedWorkbook = workbooks.Open(excelFilePath, 0, false, 5, "", "", true, XlPlatform.xlWindows, "\t",
                true, false, 0, true, 1, 0);
            var allWorksheets = openedWorkbook.Worksheets;
            var firstWorksheet = (Worksheet)allWorksheets.get_Item(1);
            var worksheetCells = firstWorksheet.Cells;

            var rowIdx = 0;
            var notEmpty = 1;
            while (notEmpty > 0)
            {
                var aCellAddress = "A" + (++rowIdx);
                var row = excelApp.get_Range(aCellAddress, aCellAddress).EntireRow;
                notEmpty = (int)excelApp.WorksheetFunction.CountA(row);
            }

            //update actual data
            var columnIndex = 1;
            var cellGuid = (Range)worksheetCells.Item[rowIdx, columnIndex];
            cellGuid.Value = surveyData.UserId;

            foreach (var answer in surveyData.Questions)
            {
                columnIndex++;
                var cellToUpdate = (Range)worksheetCells.Item[rowIdx, columnIndex];
                cellToUpdate.Value = string.Join(", ", answer.SelectedAnswers.ToArray()); ;
            }

            //force refresh of all formulas in document
            var worksheetsCount = allWorksheets.Count;
            for (var iWorksheet = 1; iWorksheet <= worksheetsCount; iWorksheet++)
            {
                var ws = (Worksheet)allWorksheets.get_Item(iWorksheet);
                var usedRange = ws.UsedRange;
                usedRange.Formula = usedRange.Formula;
                ws.Calculate();
            }

            openedWorkbook.Save();
            workbooks.Close();
            excelApp.Quit();
        }

    }
}