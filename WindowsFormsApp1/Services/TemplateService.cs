using ReportGeneratorApp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ReportGeneratorApp.Services
{
    public class TemplateService
    {
        public Dictionary<string, string> TemplateFileNames { get; private set; }
        public Dictionary<string, List<TemplateField>> TemplateFields { get; private set; }

        public TemplateService()
        {
            InitializeTemplates();
        }

        private void InitializeTemplates()
        {
            // Using Application.StartupPath to ensure paths are correct
            TemplateFileNames = new Dictionary<string, string>
            {
                { "أبحاث", Path.Combine(Application.StartupPath, "Templates", "Research.html") },
                { "شؤون الطلاب", Path.Combine(Application.StartupPath, "Templates", "StudentAffairs.html") },
                { "اعادة قيد", Path.Combine(Application.StartupPath, "Templates", "Template7.html") },
                { "ترقية", Path.Combine(Application.StartupPath, "Templates", "Template1.html") }
            };

            TemplateFields = new Dictionary<string, List<TemplateField>>
            {
                { "أبحاث", new List<TemplateField> {
                    new TemplateField("drName", "اسم الدكتور", FieldType.TextBox),
                    new TemplateField("date", "التاريخ", FieldType.DateTimePicker),
                    new TemplateField("details", "التفاصيل", FieldType.TextBox) }
                },
                { "شؤون الطلاب", new List<TemplateField> {
                    new TemplateField("الاسم", "اسم الطالب", FieldType.TextBox),
                    new TemplateField("الرقم", "الرقم", FieldType.TextBox),
                    new TemplateField("توقيع الموافقة", "توقيع الموافقة", FieldType.CheckBox) }
                },
                { "اعادة قيد", new List<TemplateField> {
                    new TemplateField("اسم الطالب", "اسم الطالب", FieldType.TextBox),
                    new TemplateField("الرقم الاكاديمى", "الرقم الأكاديمي", FieldType.TextBox),
                    new TemplateField("الفصل الدراسى", "الفصل الدراسي", FieldType.TextBox),
                    new TemplateField("العام", "السنة", FieldType.TextBox) }
                },
                { "ترقية", new List<TemplateField> {
                    new TemplateField("رقم الموضوع", "رقم الموضوع", FieldType.TextBox),
                    new TemplateField("القرار", "القرار", FieldType.TextBox),
                    new TemplateField("اسم الدكتور", "اسم الدكتور", FieldType.TextBox) }
                }
            };
        }

        public string GetTemplatePath(string subject)
        {
            if (TemplateFileNames.TryGetValue(subject, out var templatePath))
                return templatePath;

            MessageBox.Show($"Template file not found for subject: {subject}");
            return null;
        }
    }
}
