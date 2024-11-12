using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Layout.Renderer;
using iText.IO.Source;
using ReportGeneratorApp.Classes;

namespace ReportGeneratorApp
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> templateFileNames;
        private Dictionary<string, List<TemplateField>> templateFields;
        private Dictionary<string, Dictionary<string, string>> subjectData;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
            LoadSubjects();
        }

        private void InitializeData()
        {
            templateFileNames = new Dictionary<string, string>
            {
                { "أبحاث", "Research.html" },
                { "شؤون الطلاب", "StudentAffairs.html" },
                { "اعادة قيد", "Template7.html" },
                { "ترقية", "Template1.html" }
            };

            templateFields = new Dictionary<string, List<TemplateField>>
            {
                { "أبحاث", new List<TemplateField> {
                    new TemplateField("drName", "اسم الدكتور", FieldType.ComboBox),
                    new TemplateField("date", "التاريخ", FieldType.DateTimePicker),
                    new TemplateField("details", "التفاصيل", FieldType.TextBox) }
                },
                { "شؤون الطلاب", new List<TemplateField>
                { new TemplateField("الاسم", "اسم الطالب", FieldType.TextBox),
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

            subjectData = new Dictionary<string, Dictionary<string, string>>();
        }

        private void LoadSubjects()
        {
            flowPanelSubjects.Controls.Clear();

            foreach (var subject in templateFileNames.Keys)
            {
                Button subjectButton = new Button
                {
                    Text = subject,
                    Width = 120,
                    Height = 40,
                    Tag = subject
                };

                subjectButton.Click += SubjectButton_Click;
                flowPanelSubjects.Controls.Add(subjectButton);
            }
        }

        private void SubjectButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string selectedSubject = button?.Tag.ToString();

            if (!string.IsNullOrEmpty(selectedSubject))
            {
                button.BackColor = button.BackColor == System.Drawing.Color.LightBlue
                    ? System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control)
                    : System.Drawing.Color.LightBlue;

                LoadDynamicFieldsForSubject(selectedSubject);
            }
        }

        private void LoadDynamicFieldsForSubject(string subject)
        {
            panelDynamicInputs.Controls.Clear();

            if (templateFields.TryGetValue(subject, out var fields))
            {
                int yPosition = 10;

                if (!subjectData.ContainsKey(subject))
                {
                    subjectData[subject] = new Dictionary<string, string>();
                }

                foreach (var field in fields)
                {
                    Label label = new Label { Text = field.Label, Location = new System.Drawing.Point(10, yPosition), Width = 100 };
                    Control inputControl = CreateInputControl(field, yPosition);

                    if (subjectData[subject].ContainsKey(field.Name))
                    {
                        inputControl.Text = subjectData[subject][field.Name];
                    }

                    inputControl.TextChanged += (s, args) =>
                    {
                        subjectData[subject][field.Name] = inputControl.Text;
                    };

                    panelDynamicInputs.Controls.Add(label);
                    panelDynamicInputs.Controls.Add(inputControl);
                    yPosition += 30; // Move the position down for the next set of controls
                }
            }
        }

        private Control CreateInputControl(TemplateField field, int yPosition)
        {
            Control inputControl;
            switch (field.Type)
            {
                case FieldType.TextBox:
                    inputControl = new TextBox
                    {
                        Name = $"txt{field.Name}",
                        Location = new System.Drawing.Point(120, yPosition),
                        Width = 200
                    };
                    break;

                case FieldType.DateTimePicker:
                    inputControl = new DateTimePicker
                    {
                        Name = $"dtp{field.Name}",
                        Location = new System.Drawing.Point(120, yPosition),
                        Width = 200
                    };

                    if (field.Name.Contains("Year"))
                    {
                        DateTimePicker datePicker = (DateTimePicker)inputControl;
                        // Custom setting for day-only date picker
                        datePicker.Format = DateTimePickerFormat.Custom;
                        datePicker.CustomFormat = "yyyy";
                        datePicker.ShowUpDown = true;
                    }
                    else
                    {
                        // Default DateTimePicker behavior for full date (Day, Month, Year)
                        DateTimePicker datePicker = (DateTimePicker)inputControl;
                        datePicker.Format = DateTimePickerFormat.Short;
                    }
                    break;

                case FieldType.CheckBox:
                    var checkBox = new CheckBox
                    {
                        Name = $"chk{field.Name}",
                        Location = new System.Drawing.Point(120, yPosition),
                        Width = 20
                    };

                    if (!subjectData.ContainsKey(field.Name))
                    {
                        subjectData[field.Name] = new Dictionary<string, string>();
                    }

                    if (subjectData[field.Name].ContainsKey("Checked"))
                    {
                        checkBox.Checked = bool.Parse(subjectData[field.Name]["Checked"]);
                    }
                    else
                    {
                        checkBox.Checked = false; 
                    }

                    checkBox.CheckedChanged += (s, e) =>
                    {
                        subjectData[field.Name]["Checked"] = checkBox.Checked.ToString(); 
                    };

                    inputControl = checkBox;
                    break;




                case FieldType.ComboBox:
                    var comboBox = new ComboBox
                    {
                        Name = $"cmb{field.Name}",
                        Location = new System.Drawing.Point(120, yPosition),
                        Width = 200
                    };

                    comboBox.Items.AddRange(new string[] { "Option 1", "Option 2", "Option 3" });

                    if (!subjectData.ContainsKey(field.Name))
                    {
                        subjectData[field.Name] = new Dictionary<string, string>();
                    }

                    comboBox.SelectedIndexChanged += (s, e) =>
                    {
                        subjectData[field.Name]["SelectedValue"] = comboBox.SelectedItem?.ToString();
                    };

                    inputControl = comboBox;
                    break;


                default:
                    throw new NotSupportedException($"Field type {field.Type} is not supported.");
            }
            return inputControl;
        }

        //private string AddDoctorTable(string htmlContent)
        //{
        //    // Example of dynamically adding doctor names to the table
        //    var doctors = new List<string> { "دكتور أحمد علي", "دكتور محمد مصطفى", "دكتور سامي يوسف" }; // Replace with actual doctor list

        //    string tableRows = "";
        //    foreach (var doctor in doctors)
        //    {
        //        tableRows += $"<tr><td>{doctor}</td><td><input type='checkbox' name='doctor' value='{doctor}' /></td></tr>";
        //    }

        //    // Insert the rows into the HTML table
        //    htmlContent = htmlContent.Replace("<!-- DYNAMIC_DOCTOR_ROWS -->", tableRows);

        //    return htmlContent;
        //}

        private void GenerateReportPdf(List<string> subjects)
        {
            if (subjects == null || subjects.Count == 0)
            {
                MessageBox.Show("يرجى اختيار موضوع واحد على الأقل.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFileDialog.DefaultExt = "pdf"; 
                saveFileDialog.FileName = $"Multi_Subject_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf"; // Default file name

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveFileDialog.FileName; 

                    try
                    {
                        using (PdfWriter writer = new PdfWriter(outputPath))
                        using (PdfDocument mainPdfDoc = new PdfDocument(writer))
                        using (Document document = new Document(mainPdfDoc))
                        {
                            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "ScheherazadeNew-Regular.ttf");
                            PdfFont arabicFont = PdfFontFactory.CreateFont(fontPath, "Identity-H");

                            document.SetFont(arabicFont)
                                    .SetTextAlignment(TextAlignment.RIGHT)
                                    .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);

                            for (int index = 0; index < subjects.Count; index++)
                            {
                                string subject = subjects[index];
                                if (!templateFileNames.ContainsKey(subject))
                                {
                                    MessageBox.Show($"موضوع غير موجود: {subject}");
                                    continue;
                                }

                                string htmlTemplatePath = Path.Combine(Application.StartupPath, "Templates", templateFileNames[subject]);
                                if (!File.Exists(htmlTemplatePath))
                                {
                                    MessageBox.Show($"Template file not found: {htmlTemplatePath}");
                                    continue;
                                }

                                string htmlContent = File.ReadAllText(htmlTemplatePath);
                                var data = subjectData.ContainsKey(subject) ? subjectData[subject] : new Dictionary<string, string>();

                                foreach (var placeholder in data)
                                {
                                    if (placeholder.Key.Contains("Checked"))
                                    {
                                        htmlContent = htmlContent.Replace("{" + placeholder.Key + "}", placeholder.Value == "True" ? "تم" : "لم يتم");
                                    }
                                    else
                                    {
                                        htmlContent = htmlContent.Replace("{" + placeholder.Key + "}", placeholder.Value);
                                    }
                                }

                                string rtlCss = "<style> body { direction: rtl; font-family: 'ScheherazadeNew'; margin: 0; padding: 0; } </style>";
                                htmlContent = rtlCss + htmlContent;

                                // Convert HTML to PDF and append to main document
                                using (var htmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent)))
                                using (var tempStream = new MemoryStream())
                                {
                                    PdfDocument tempPdfDoc = new PdfDocument(new PdfWriter(tempStream));
                                    ConverterProperties converterProperties = new ConverterProperties();
                                    DefaultFontProvider fontProvider = new DefaultFontProvider();
                                    fontProvider.AddFont(fontPath);
                                    converterProperties.SetFontProvider(fontProvider);
                                    converterProperties.SetCharset("UTF-8");

                                    HtmlConverter.ConvertToPdf(htmlStream, tempPdfDoc, converterProperties);
                                    tempPdfDoc.Close();

                                    PdfDocument tempForReading = new PdfDocument(new PdfReader(new MemoryStream(tempStream.ToArray())));
                                    tempForReading.CopyPagesTo(1, tempForReading.GetNumberOfPages(), mainPdfDoc);
                                    tempForReading.Close();
                                }
                            }

                            document.Close();
                        }

                        MessageBox.Show($"تم حفظ الملف بنجاح: {outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء إنشاء PDF: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            var selectedSubjects = flowPanelSubjects.Controls.OfType<Button>()
                .Where(btn => btn.BackColor == System.Drawing.Color.LightBlue)
                .Select(btn => btn.Tag.ToString())
                .ToList();

            GenerateReportPdf(selectedSubjects);
        }


    private void Form1_Load(object sender, EventArgs e)
        {

        }
    }


}





