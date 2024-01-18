using Education.Application.Interface;
using Education.Core.Interface;
using Education.Core.Model;
using Education.Core.Model.Core;
using Education.Core.Model.DataModel;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Education.Application.Service
{
    public class CheckExamsService : ICheckExamsService
    {
        private IExamTestRepository _examTestRepository;
        private IStudentExamsRepository _studentExamsRepository;
        public CheckExamsService(IServiceProvider serviceProvider)
        {
            _examTestRepository = serviceProvider.GetRequiredService<IExamTestRepository>();
            _studentExamsRepository = serviceProvider.GetRequiredService<IStudentExamsRepository>();
        }

        public async Task<MemoryStream> ExportData(List<StudentDetects> resultStudent, string studentName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            var lstExamCodes = resultStudent.Select(x => x.ExamsCode).ToList();
            var resExamsDetail = await _examTestRepository.GetExamsdetailByExamCodes(lstExamCodes);
            BindDataToExcel(resultStudent, resExamsDetail, ref workSheet);
            var memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);
            return await Task.FromResult(memoryStream);
        }

        private void BindDataToExcel(List<StudentDetects> resultStudent, List<ExamsDetailDto> resExamsDetail, ref ExcelWorksheet workSheet)
        {
            BuildHeaderExcel(ref workSheet, resExamsDetail, resultStudent);
            BindDataStudent(ref workSheet, resExamsDetail, resultStudent);
        }

        private void BindDataStudent(ref ExcelWorksheet workSheet, List<ExamsDetailDto> resExamsDetail, List<StudentDetects> resultStudent)
        {
            var examsDetailDic = resExamsDetail.GroupBy(x => x.ExamTestCode).ToDictionary(k => k.Key, g => g.ToList());
            for (int i = 0; i < resultStudent.Count; i++)
            {
                var examsOfStudent = examsDetailDic[resultStudent[i].ExamsCode];
                workSheet.Cells[i + 3, 1].Value = resultStudent[i].StudentCode;
                workSheet.Cells[i + 3, 2].Value = resultStudent[i].ExamsCode;
                workSheet.Cells[i + 3, 3].Value = resultStudent[i].StudentName;
                workSheet.Cells[i + 3, 3].AutoFitColumns();
                workSheet.Cells[i + 3, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[i + 3, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[i + 3, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[i + 3, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                for (int j = 0; j < resExamsDetail.Count; j++)
                {
                    workSheet.Cells[i + 3, (j * 2) + 4].Value = "A";
                    workSheet.Cells[i + 3, (j * 2) + 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[i + 3, (j * 2) + 5].Value = examsOfStudent[i].Result == "A" ? "Đúng" : "Sai";
                    workSheet.Cells[i + 3, (j * 2) + 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }

        private void BuildHeaderExcel(ref ExcelWorksheet workSheet, List<ExamsDetailDto> resExamsDetail, List<StudentDetects> resultStudent)
        {
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            workSheet.Column(1).Width = 15;
            var studentCodeCell = workSheet.Cells[1, 1, 2, 1];
            studentCodeCell.Merge = true;
            studentCodeCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            studentCodeCell.Value = "Mã học sinh";
            var examCodeCell = workSheet.Cells[1, 2, 2, 2];
            examCodeCell.Merge = true;
            examCodeCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            examCodeCell.Value = "Mã đề";
            var studentNameCell = workSheet.Cells[1, 3, 2, 3];
            studentNameCell.Merge = true;
            studentNameCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            studentNameCell.Value = "Họ và tên";
            int startColumn = 4;
            for (int i = 0; i < resExamsDetail.Count; i++)
            {
                var header = workSheet.Cells[1, startColumn + (i * 2), 1, startColumn + (i * 2) + 1];
                workSheet.Cells[2, startColumn + (i * 2)].Value = "Đáp án";
                workSheet.Cells[2, startColumn + (i * 2)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                workSheet.Cells[2, startColumn + (i * 2) + 1].Value = "Kết quả";
                workSheet.Cells[2, startColumn + (i * 2) + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                header.Merge = true;
                header.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                header.Value = i + 1;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        public async Task ImportStudentsResultExams(IFormFile formFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;
                    var lstData = new List<StudentExamsExcelDto>();
                    for (int row = 3; row <= rowCount; row++)
                    {
                        var dataModel = new StudentExamsExcelDto();
                        dataModel.StudentCode = worksheet.Cells[row, 1].Value.ToString() ?? "";
                        dataModel.ExamTestCode = worksheet.Cells[row, 2].Value.ToString() ?? "";
                        dataModel.Name = worksheet.Cells[row, 3].Value.ToString() ?? "";
                        var detailQuestions = new List<DetailQuestion>();
                        for (int column = 1; column <= (columnCount - 3) / 2; column++)
                        {
                            var dataQuestion = new DetailQuestion();
                            dataQuestion.QuestionOrder = Convert.ToInt32(worksheet.Cells[1, column + 3].Value.ToString() ?? "0");
                            dataQuestion.Result = worksheet.Cells[row, column * 2 + 2].Value.ToString() ?? "";
                            detailQuestions.Add(dataQuestion);
                        }
                        dataModel.DetailQuestions = detailQuestions;
                        lstData.Add(dataModel);
                    }
                    var lstDataInsert = new List<StudenExams>();
                    foreach (var item in lstData)
                    {
                        lstDataInsert.Add(new StudenExams()
                        {
                            ExamTestCode = item.ExamTestCode,
                            StudentCode = item.StudentCode,
                            ResultJson = JsonConvert.SerializeObject(item.DetailQuestions)
                        }) ;
                    }
                    var dataCast = lstDataInsert.Cast<BaseModel>().ToList();
                    var resInserts = await _studentExamsRepository.MultiInsert(dataCast, false);
                }
            }
        }
    }
}
