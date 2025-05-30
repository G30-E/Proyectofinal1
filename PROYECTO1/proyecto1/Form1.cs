// Importaci�n de librer�as necesarias
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml;
using System.IO;

namespace proyecto1
{
    public partial class Form1 : Form
    {
        // Constantes para conectarse con la API de Groq
        private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";
        private const string GroqApiKey = "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";
        private const string Model = "llama3-70b-8192";

        public Form1()
        {
            InitializeComponent();
        }

        // Evento que se ejecuta cuando se presiona el bot�n "Consultar"
        private async void buttonConsultar_Click(object sender, EventArgs e)
        {
            string consulta = textBoxConsultaIA.Text.Trim();

            if (string.IsNullOrEmpty(consulta))
            {
                MessageBox.Show("Por favor, ingresa tu consulta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBoxResultadoAI.Text = "Cargando...";

            try
            {
                // Se consulta a la IA y se muestra la respuesta
                string respuesta = await ObtenerRespuestaGroq(consulta);
                textBoxResultadoAI.Text = respuesta;

                // Se guarda autom�ticamente en la base de datos y se generan archivos Word y PowerPoint
                GuardarEnBaseDeDatos(consulta, respuesta);
                GenerarArchivos(consulta, respuesta);
            }
            catch (Exception ex)
            {
                textBoxResultadoAI.Text = $"Error: {ex.Message}";
            }
        }

        // Funci�n que realiza la consulta a la API de Groq y devuelve la respuesta generada
        private async Task<string> ObtenerRespuestaGroq(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GroqApiKey);

                var requestData = new
                {
                    model = Model,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                int maxRetries = 3;
                int delay = 2000; // milisegundos

                // Se intenta enviar la solicitud hasta 3 veces si se recibe error 429 (demasiadas solicitudes)
                for (int i = 0; i < maxRetries; i++)
                {
                    var response = await client.PostAsync(GroqEndpoint, content);

                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(delay);
                        delay *= 2; // Espera exponencial
                        continue;
                    }

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic resultado = JsonConvert.DeserializeObject(responseBody);
                    return resultado.choices[0].message.content.ToString().Trim();
                }

                throw new Exception("Se super� el l�mite de reintentos por demasiadas solicitudes (429).");
            }
        }

        // Guarda la consulta y la respuesta en la base de datos SQL Server
        private void GuardarEnBaseDeDatos(string consulta, string respuesta)
        {
            string connectionString = "Server=USER-PC\\SQLEXPRESS01;Database=ConsultasGroq;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO ConsultasGroq (Fecha, Consulta, Respuesta) VALUES (@Fecha, @Consulta, @Respuesta)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                        command.Parameters.AddWithValue("@Consulta", consulta);
                        command.Parameters.AddWithValue("@Respuesta", respuesta);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Datos guardados con �xito en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar en la base de datos: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Genera los archivos Word y PowerPoint con la consulta y respuesta
        private void GenerarArchivos(string consulta, string respuesta)
        {
            string carpetaPrincipal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ConsultasGroq");

            if (!Directory.Exists(carpetaPrincipal))
                Directory.CreateDirectory(carpetaPrincipal);

            // Crea una subcarpeta para cada consulta con nombre basado en la consulta
            string nombreSubCarpeta = string.Concat(consulta.Split(Path.GetInvalidFileNameChars()));
            string carpetaConsulta = Path.Combine(carpetaPrincipal, nombreSubCarpeta);

            if (!Directory.Exists(carpetaConsulta))
                Directory.CreateDirectory(carpetaConsulta);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string nombreBase = $"Consulta_{timestamp}";

            string rutaWord = Path.Combine(carpetaConsulta, nombreBase + ".docx");
            string rutaPptx = Path.Combine(carpetaConsulta, nombreBase + ".pptx");

            CrearDocumentoWordOpenXml(rutaWord, consulta, respuesta);
            CrearPresentacionPowerPointOpenXml(rutaPptx, consulta, respuesta);
        }

        // Crea un documento Word con la consulta y la respuesta usando Open XML
        private void CrearDocumentoWordOpenXml(string ruta, string consulta, string respuesta)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(ruta, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();
                body.Append(
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Consulta:")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(consulta))),
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Respuesta:")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(respuesta)))
                );

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }
        }

        // Crea una presentaci�n PowerPoint con una diapositiva para la consulta y otra para la respuesta
        private void CrearPresentacionPowerPointOpenXml(string ruta, string consulta, string respuesta)
        {
            using (PresentationDocument presentationDocument = PresentationDocument.Create(ruta, PresentationDocumentType.Presentation))
            {
                PresentationPart presentationPart = presentationDocument.AddPresentationPart();
                presentationPart.Presentation = new P.Presentation();

                // Crear dos diapositivas: una para la consulta y otra para la respuesta
                SlidePart slidePart1 = AddSlide(presentationPart, "Consulta", consulta);
                SlidePart slidePart2 = AddSlide(presentationPart, "Respuesta", respuesta);

                presentationPart.Presentation.SlideIdList = new SlideIdList(
                    new SlideId() { Id = 256U, RelationshipId = presentationPart.GetIdOfPart(slidePart1) },
                    new SlideId() { Id = 257U, RelationshipId = presentationPart.GetIdOfPart(slidePart2) }
                );

                presentationPart.Presentation.Save();
            }
        }

        // Agrega una nueva diapositiva al documento de PowerPoint con t�tulo y contenido
        private SlidePart AddSlide(PresentationPart presentationPart, string titulo, string contenido)
        {
            SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.Slide = new P.Slide(
                new P.CommonSlideData(
                    new P.ShapeTree(
                        new P.NonVisualGroupShapeProperties(
                            new P.NonVisualDrawingProperties() { Id = 1, Name = "Title" },
                            new P.NonVisualGroupShapeDrawingProperties(),
                            new ApplicationNonVisualDrawingProperties()
                        ),
                        new P.GroupShapeProperties(new A.TransformGroup()),
                        CreateTextShape(2, titulo, 0, 0, 7200000, 1000000),
                        CreateTextShape(3, contenido, 0, 1000000, 7200000, 4000000)
                    )
                )
            );
            return slidePart;
        }

        // Crea una forma de texto (cuadro de texto) con posici�n y tama�o para una diapositiva de PowerPoint
        private P.Shape CreateTextShape(uint id, string text, int x, int y, int cx, int cy)
        {
            return new P.Shape(
                new P.NonVisualShapeProperties(
                    new P.NonVisualDrawingProperties() { Id = id, Name = "TextBox " + id },
                    new P.NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                    new ApplicationNonVisualDrawingProperties()
                ),
                new P.ShapeProperties(new A.Transform2D(
                    new A.Offset() { X = x, Y = y },
                    new A.Extents() { Cx = cx, Cy = cy })),
                new P.TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle(),
                    new A.Paragraph(new A.Run(new A.Text(text)))
                )
            );
        }
    }
}
