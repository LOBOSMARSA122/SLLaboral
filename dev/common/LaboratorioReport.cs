using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Sigesoft.Node.WinClient.BE;


namespace NetPdf
{
    public class LaboratorioReport
    {
        #region Report de Laboratorio

        public static void CreateLaboratorioReport(PacientList filiationData, List<ServiceComponentList> serviceComponent, organizationDto infoEmpresaPropietaria, string filePDF)
        {
            //
            // step 1: creation of a document-object
            Document document = new Document();
            //Document document = new Document(new Rectangle(500f, 300f), 10, 10, 10, 10);
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            //Document document = new Document(PageSize.A4, 0, 0, 20, 20);
            //
            try
            {
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));
                //
                //create an instance of your PDFpage class. This is the class we generated above.
                pdfPage page = new pdfPage();
                //set the PageEvent of the pdfWriter instance to the instance of our PDFPage class
                writer.PageEvent = page;

                // step 3: we open the document
                document.Open();
                // step 4: we Add content to the document
                // we define some fonts

                #region Fonts

                Font fontTitle1 = FontFactory.GetFont(FontFactory.HELVETICA, 18, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTable = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontTitleTableNegro = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontSubTitle = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontSubTitleNegroNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

                Font fontColumnValue = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
               

                #endregion

                #region Logo

                Image logo = HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image, 20F);
                PdfPTable headerTbl = new PdfPTable(1);
                headerTbl.TotalWidth = writer.PageSize.Width;
                PdfPCell cellLogo = new PdfPCell(logo);

                cellLogo.VerticalAlignment = Element.ALIGN_TOP;
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;

                cellLogo.Border = PdfPCell.NO_BORDER;
                headerTbl.AddCell(cellLogo);
                document.Add(headerTbl);

                #endregion

                #region Title

                Paragraph cTitle = new Paragraph("LABORATORIO CLÍNICO", fontTitle2);
                //Paragraph cTitle2 = new Paragraph(customerOrganizationName, fontTitle2);
                cTitle.Alignment = Element.ALIGN_CENTER;
                //cTitle2.Alignment = Element.ALIGN_CENTER;

                document.Add(cTitle);
                //document.Add(cTitle2);

                #endregion

                #region Declaration Tables
                var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.White);
                string include = string.Empty;
                List<PdfPCell> cells = null;
                float[] columnWidths = null;
                string[] columnValues = null;
                string[] columnHeaders = null;

                //PdfPTable header1 = new PdfPTable(2);
                //header1.HorizontalAlignment = Element.ALIGN_CENTER;
                //header1.WidthPercentage = 100;
                ////header1.TotalWidth = 500;
                ////header1.LockedWidth = true;    // Esto funciona con TotalWidth
                //float[] widths = new float[] { 150f, 200f};
                //header1.SetWidths(widths);


                //Rectangle rec = document.PageSize;
                PdfPTable header2 = new PdfPTable(6);
                header2.HorizontalAlignment = Element.ALIGN_CENTER;
                header2.WidthPercentage = 100;
                //header1.TotalWidth = 500;
                //header1.LockedWidth = true;    // Esto funciona con TotalWidth
                float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                header2.SetWidths(widths1);
                //header2.SetWidthPercentage(widths1, rec);

                PdfPTable companyData = new PdfPTable(6);
                companyData.HorizontalAlignment = Element.ALIGN_CENTER;
                companyData.WidthPercentage = 100;
                //header1.TotalWidth = 500;
                //header1.LockedWidth = true;    // Esto funciona con TotalWidth
                float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                companyData.SetWidths(widthscolumnsCompanyData);

                PdfPTable filiationWorker = new PdfPTable(4);

                PdfPTable table = null;

                PdfPCell cell = null;

                #endregion

                // Salto de linea
                document.Add(new Paragraph("\r\n"));

                #region Datos personales del trabajador

                cells = new List<PdfPCell>()
                {
                    new PdfPCell(new Phrase("Paciente: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_FirstName + " " + filiationData.v_FirstLastName + " " + filiationData.v_SecondLastName, fontColumnValue)),                   
                    new PdfPCell(new Phrase("Empresa: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_FullWorkingOrganizationName, fontColumnValue)),     
                    new PdfPCell(new Phrase("Puesto: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_CurrentOccupation, fontColumnValue)),     
                    new PdfPCell(new Phrase("Fecha Atención: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.d_ServiceDate.Value.ToShortDateString(), fontColumnValue)),                                    
                };

                columnWidths = new float[] { 20f, 70f };

                filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, "I. DATOS PERSONALES", fontTitleTableNegro, null);

                document.Add(filiationWorker);

                #endregion

                #region BIOQUIMICA

                cells = new List<PdfPCell>();

                // Subtitulo  ******************
                cell = new PdfPCell(new Phrase("INMUNOLÓGICO", fontSubTitleNegroNegrita))
                {
                    Colspan = 4,
                    BackgroundColor = subTitleBackGroundColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                };

                cells.Add(cell);
                //*****************************************

                var xAntigenoProstatico = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ID);
                if (xAntigenoProstatico != null)
                {
                    var antigenoprostatico = xAntigenoProstatico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ANTIGENO_PROSTATICO_VALOR);
                    var antigenoprostaticoValord = xAntigenoProstatico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_VALOR_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("ANTÍGENO PROSTÁTICO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(antigenoprostatico == null ? string.Empty : antigenoprostatico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(antigenoprostaticoValord == null ? string.Empty : antigenoprostaticoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }
                else
                {
                    cells.Add(new PdfPCell(new Phrase("SIN EXAMEN", fontColumnValue)) { Colspan=4});
                }


                // Subtitulo  ******************
                cell = new PdfPCell(new Phrase("BIOQUIMICA", fontSubTitleNegroNegrita))
                {
                    Colspan = 4,
                    BackgroundColor = subTitleBackGroundColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                };

                cells.Add(cell);
                //*****************************************

                // titulo
                cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                var xGlucosa = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.GLUCOSA_ID);
                var xColesterol = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_ID);
                var xTrigliceridos = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TRIGLICERIDOS_ID);
                var xColesterolHdl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_HDL_ID);
                var xColesterolLdl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_LDL_ID);
                var xColesterolVldl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_VLDL_ID);
                var xUrea = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.UREA_ID);
                var xCreatinina = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.CREATININA_ID);
                var xTgo = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TGO_ID);
                var xTgp = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TGP_ID);
                var xAcidoUrico = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.ACIDO_URICO_ID);
              
                var xPlomoSangre = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.PLOMO_SANGRE_ID);


                var xBioquimico01 = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.BIOQUIMICA01_ID);
                var xBioquimico02 = serviceComponent.Find(p => p.v_ComponentId == "");
                //var xBioquimico02 = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.BIOQUIMICA02_ID);
                //var xBioquimico03 = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.BIOQUIMICA03_ID);


                if (xGlucosa != null)
                {                   
                    var glucosa = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GLUCOSA_GLUCOSA_ID);
                    var glucosaValord = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GLUCOSA_GLUCOSA_VALOR_DESEABLE_ID);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("GLUCOSA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(glucosa == null ? string.Empty : glucosa.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(glucosaValord == null ? string.Empty : glucosaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                   
                }

                if (xColesterol != null)
                {
                    var colesterol = xColesterol.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_COLESTEROL_TOTAL_ID);
                    var colesterolValord = xColesterol.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_COLESTEROL_TOTAL_DESEABLE_ID);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("COLESTEROL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterol == null ? string.Empty : colesterol.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolValord == null ? string.Empty : colesterolValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }


                if (xTrigliceridos != null)
                {
                    var Triglicerido = xColesterol.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS);
                    var TrigliceridoValord = xColesterol.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TRIGLICÉRIDOS", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(Triglicerido == null ? string.Empty : Triglicerido.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(TrigliceridoValord == null ? string.Empty : TrigliceridoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolHdl != null)
                {
                    var colesterolHdl = xColesterolHdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_HDL);
                    var colesterolHdlValord = xColesterolHdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_HDL_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("COLESTEROL HDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolHdl == null ? string.Empty : colesterolHdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolHdlValord == null ? string.Empty : colesterolHdlValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolHdl != null)
                {
                    var colesteroTotallHdl = xColesterolHdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_TOTAL_HDL);
                    var colesterolTotalHdlValord = xColesterolHdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_TOTAL_HDL_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("COLESTEROL TOTAL / HDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesteroTotallHdl == null ? string.Empty : colesteroTotallHdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolTotalHdlValord == null ? string.Empty : colesterolTotalHdlValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }
                
                if (xColesterolLdl != null)
                {
                    var colesterolLdl = xColesterolLdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL);
                    var colesterolLdlValord = xColesterolLdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("LDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolLdl == null ? string.Empty : colesterolLdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolLdlValord == null ? string.Empty : colesterolLdlValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolLdl != null)
                {
                    var colesterolVLdl = xColesterolLdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL_HDL);
                    var colesterolVLdlValord = xColesterolLdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL_HDL_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("LDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolVLdl == null ? string.Empty : colesterolVLdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolVLdlValord == null ? string.Empty : colesterolVLdlValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolVldl != null)
                {
                    var colesterolVldl = xColesterolVldl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_VLDL_BIOQUIMICA_COLESTEROL_VLDL);
                    var colesterolVldlValord = xColesterolVldl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_VLDL_BIOQUIMICA_COLESTEROL_VLDL_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("VLDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolVldl == null ? string.Empty : colesterolVldl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolVldlValord == null ? string.Empty : colesterolVldlValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xUrea != null)
                {
                    var urea = xUrea.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.UREA_BIOQUIMICA_UREA);
                    var ureaValord = xUrea.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.UREA_BIOQUIMICA_UREA_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("UREA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(urea == null ? string.Empty : urea.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(ureaValord == null ? string.Empty : ureaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xCreatinina != null)
                {
                    var creatinina = xCreatinina.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.CREATININA_BIOQUIMICA_CREATININA);
                    var creatininaValord = xCreatinina.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.CREATININA_BIOQUIMICA_CREATININA_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("CREATININA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(creatinina == null ? string.Empty : creatinina.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(creatininaValord == null ? string.Empty : creatininaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }
                if (xTgo != null)
                {
                    var tgo = xTgo.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGO_BIOQUIMICA_TGO);
                    var tgoValord = xTgo.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGO_BIOQUIMICA_TGO_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TGO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tgo == null ? string.Empty : tgo.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(tgoValord == null ? string.Empty : tgoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xTgp != null)
                {
                    var tgp = xTgp.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGP_BIOQUIMICA_TGP);
                    var tgpValord = xTgp.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGP_BIOQUIMICA_TGP_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TGP", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tgp == null ? string.Empty : tgp.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(tgpValord == null ? string.Empty : tgpValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xAcidoUrico != null)
                {
                    var acidourico = xAcidoUrico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ACIDO_URICO_BIOQUIMICA_ACIDO_URICO);
                    var acidouricoValord = xAcidoUrico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ACIDO_URICO_BIOQUIMICA_ACIDO_URICO_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("ÁCIDO ÚRICO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(acidourico == null ? string.Empty : acidourico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(acidouricoValord == null ? string.Empty : acidouricoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                //if (xAntigenoProstatico != null)
                //{
                //    var antigenoprostatico = xAntigenoProstatico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ANTIGENO_PROSTATICO_VALOR);
                //    var antigenoprostaticoValord = xAntigenoProstatico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_VALOR_DESEABLE);

                //    // 1era fila
                //    cells.Add(new PdfPCell(new Phrase("ANTÍGENO PROSTÁTICO", fontColumnValue)));
                //    cells.Add(new PdfPCell(new Phrase(antigenoprostatico == null ? string.Empty : antigenoprostatico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                //    cells.Add(new PdfPCell(new Phrase(antigenoprostaticoValord == null ? string.Empty : antigenoprostaticoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                //    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                //}

                if (xPlomoSangre != null)
                {
                    var plomoensangre = xPlomoSangre.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PLOMO_SANGRE_BIOQUIMICA_PLOMO_SANGRE);
                    var plomoensangreValord = xPlomoSangre.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PLOMO_SANGRE_BIOQUIMICA_PLOMO_SANGRE_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("PLOMO EN SANGRE", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(plomoensangre == null ? string.Empty : plomoensangre.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(plomoensangreValord == null ? string.Empty : plomoensangreValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xBioquimico01 != null)
                {
                    var Bioquimico01 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA01_VALOR);
                    var BioquimicoValord01 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA01_VALOR_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("BIOQUÍMICA 01", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(Bioquimico01 == null ? string.Empty : Bioquimico01.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(BioquimicoValord01 == null ? string.Empty : BioquimicoValord01.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }


                if (xBioquimico02 != null)
                {
                    var Bioquimico02 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA02_VALOR);
                    var BioquimicoValord02 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA02_VALOR_DESEABLE);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("BIOQUÍMICA 02", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(Bioquimico02 == null ? string.Empty : Bioquimico02.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(BioquimicoValord02 == null ? string.Empty : BioquimicoValord02.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                //if (xBioquimico03 != null)
                //{
                //    var Bioquimico03 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA03_VALOR);
                //    var BioquimicoValord03 = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BIOQUIMICA03_VALOR_DESEABLE);

                //    // 1era fila
                //    cells.Add(new PdfPCell(new Phrase("BIOQUÍMICA 02", fontColumnValue)));
                //    cells.Add(new PdfPCell(new Phrase(Bioquimico03 == null ? string.Empty : Bioquimico03.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                //    cells.Add(new PdfPCell(new Phrase(BioquimicoValord03 == null ? string.Empty : BioquimicoValord03.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                //    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                //}


                columnWidths = new float[] { 25f, 25f, 25f, 25f };

                table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);
                document.Add(table);

                #endregion
          
                #region Imprimir todos los examenes de laboratorio

                string[] orderPrint = new string[]
                { 
                    Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_ID, 
                    Sigesoft.Common.Constants.EXAMEN_COMPLETO_DE_ORINA_ID,                  
                    Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID,
                    Sigesoft.Common.Constants.GLUCOSA_ID,                
                    Sigesoft.Common.Constants.COLESTEROL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_HDL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_LDL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_VLDL_ID,
                    Sigesoft.Common.Constants.CREATININA_ID,
                    Sigesoft.Common.Constants.ACIDO_URICO_ID,
                    Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ID,
                    Sigesoft.Common.Constants.PLOMO_SANGRE_ID,
                    Sigesoft.Common.Constants.TGO_ID,
                    Sigesoft.Common.Constants.TGP_ID,
                    Sigesoft.Common.Constants.UREA_ID,
                    Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_ID,
                    Sigesoft.Common.Constants.EXAMEN_ELISA_ID,
                    Sigesoft.Common.Constants.HEPATITIS_A_ID,
                    Sigesoft.Common.Constants.HEPATITIS_C_ID,
                    Sigesoft.Common.Constants.VDRL_ID,
                    Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_ID,
                    Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_ID,
                    Sigesoft.Common.Constants.BK_DIRECTO_ID,
                    Sigesoft.Common.Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,
                    Sigesoft.Common.Constants.BIOQUIMICA01_ID,
                    //Sigesoft.Common.Constants.BIOQUIMICA02_ID,
                    //Sigesoft.Common.Constants.BIOQUIMICA03_ID,

                };
           
                #endregion

                //Capturar la firma del medico que grabo en laboratorio
                var lab = serviceComponent.Find(p => p.i_CategoryId == (int)Sigesoft.Common.Consultorio.Laboratorio);

                ReportBuilderReportForLaboratorioReport(serviceComponent, orderPrint, fontTitleTable, fontSubTitleNegroNegrita, fontColumnValue, subTitleBackGroundColor, document);

                // Salto de linea
                document.Add(new Paragraph("\r\n"));

                #region Firma y sello Médico
              
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.WidthPercentage = 40;

                columnWidths = new float[] { 15f, 25f };
                table.SetWidths(columnWidths);

                PdfPCell cellFirma = null;
                        
                if (lab != null)
                {
                    if (lab.FirmaMedico != null)
                        cellFirma = new PdfPCell(HandlingItextSharp.GetImage(lab.FirmaMedico, 25, 25));
                    else
                        cellFirma = new PdfPCell(new Phrase("Sin Foto", fontColumnValue));
                }
                else
                {
                    cellFirma = new PdfPCell();
                }
                
                cellFirma.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFirma.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellFirma.FixedHeight = 70F;

                cell = new PdfPCell(new Phrase("Firma y Sello Médico", fontColumnValue));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                table.AddCell(cell);
                table.AddCell(cellFirma);

                document.Add(table);

                #endregion
    
                // step 5: we close the document
                document.Close();
                writer.Close();
                writer.Dispose();
                //RunFile(filePDF);

            }
            catch (DocumentException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }

        }

        private static void ReportBuilderReportForLaboratorioReport(List<ServiceComponentList> serviceComponent, string[] order, Font fontTitle, Font fontSubTitle, Font fontColumnValue, BaseColor SubtitleBackgroundColor, Document document)
        {
            if (order != null)
            {
                var sortEntity = GetSortEntity(order, serviceComponent);

                if (sortEntity != null)
                {
                    foreach (var ent in sortEntity)
                    {
                        var table = TableBuilderReportForLaboratorioReport(ent, fontTitle, fontSubTitle, fontColumnValue, SubtitleBackgroundColor);

                        if (table != null)
                        {
                            if (ent.v_ComponentId == Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID)
                            {
                                document.NewPage();
                            }

                            document.Add(table);
                        }
                    }
                }
            }
        }

        private static PdfPTable TableBuilderReportForLaboratorioReport(ServiceComponentList serviceComponent, Font fontTitle, Font fontSubTitle, Font fontColumnValue, BaseColor SubtitleBackgroundColor)
        {
            PdfPTable table = null;
            List<PdfPCell> cells = null;
            PdfPCell cell = null;
            float[] columnWidths = null;
            Font fontColumnValueNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));


            switch (serviceComponent.v_ComponentId)
            {
                case Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_ID:

                    #region HEMOGRAMA_COMPLETO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var hematocritos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO);
                        var hematocritosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMATOCRITO_DESEABLE);
                        var hemoglobina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA);
                        var hemoglobinaValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA_DESEABLE);
                        var globulosRojos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMATIES);
                        var globulosRojosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_HEMATIES_DESEABLE);
                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS);
                        var leucocitosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_LEUCOCITOS);
                        var plaquetas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_PLAQUETAS);
                        var plaquetasValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_PLAQUETAS_DESEABLE);

                        var abastonados = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS);
                        var abastonadosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_ABASTONADOS_DESEABLE);
                        var segmentados = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS);
                        var segmentadosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_SEGMENTADOS_DESEABLE);
                        var eosinofilos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS);
                        var eosinofilosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_EOSINOFILOS_DESEABLE);
                        var basofilos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS);
                        var basofilosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_BASOFILOS_DESEABLE);
                        var monocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS);
                        var monocitosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_MONOCITOS_DESEABLE);
                        var linfocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS);
                        var linfocitosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_LINFOCITOS_DESEABLE);
                        var observaciones = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_FORMULA_LEUCOCITARIA_OBSERVACIONES);
                        
                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("HEMATOCRITO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematocritos == null ? string.Empty : hematocritos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hematocritosValord == null ? string.Empty : hematocritosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("HEMOGLOBINA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hemoglobina == null ? string.Empty : hemoglobina.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hemoglobinaValord == null ? string.Empty : hemoglobinaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("g/di", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("GLÓBULOS ROJOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(globulosRojos == null ? string.Empty : globulosRojos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(globulosRojosValord == null ? string.Empty : globulosRojosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("x106/mm3", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(leucocitosValord == null ? string.Empty : leucocitosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("x103/mm3", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("RECUENTO PLAQUETAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(plaquetas == null ? string.Empty : plaquetas.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(plaquetasValord == null ? string.Empty : plaquetasValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("x103/mm3", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("FORMULA LEUCOCITARIA", fontColumnValueNegrita)) { Colspan = 4 });
                      
                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("ABASTONADOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(abastonados == null ? string.Empty : abastonados.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(abastonadosValord == null ? string.Empty : abastonadosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("SEGMENTADOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(segmentados == null ? string.Empty : segmentados.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(segmentadosValord == null ? string.Empty : segmentadosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("EOSINÓFILOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(eosinofilos == null ? string.Empty : eosinofilos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(eosinofilosValord == null ? string.Empty : eosinofilosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("BASÓFILOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(basofilos == null ? string.Empty : basofilos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(basofilosValord == null ? string.Empty : basofilosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("MONOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(monocitos == null ? string.Empty : monocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(monocitosValord == null ? string.Empty : monocitosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("LINFOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(linfocitos == null ? string.Empty : linfocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(linfocitosValord == null ? string.Empty : linfocitosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("%", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                 
                        // 12va fila
                        cells.Add(new PdfPCell(new Phrase("OBSERVACIONES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(observaciones == null ? string.Empty : observaciones.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)));

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.EXAMEN_COMPLETO_DE_ORINA_ID:

                    #region EXAMEN_COMPLETO_DE_ORINA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var color = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_COLOR);
                        var colorValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_COLOR_DESEABLE);

                        var aspecto = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_ASPECTO);
                        var aspectoValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_ASPECTO_DESEABLE);

                        var densidad = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_DENSIDAD);
                        var densidadValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_DENSIDAD_DESEABLE);

                        var ph = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_PH);
                        var phValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_PH_DESEABLE);

                        var celulasEpiteleales = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES);
                        var celulasEpitelealesValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES_DESEABLE);

                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_LEUCOCITOS);
                        var leucocitosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_LEUCOCITOS_DESEABLE);

                        var hematies = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_HEMATIES);
                        var hematiesValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_HEMATIES_DESEABLE);

                        var germenes = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_GERMENES);
                        var germenesValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_GERMENES_DESEABLE);

                        var cilindros = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CILINDROS);
                        var cilindrosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CILINDROS_DESEABLE);

                        var cristales = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CRISTALES);
                        var cristalesValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CRISTALES_DESEABLE);

                        var filamentoMucoide = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_FILAMENTO_MUCOIDE);
                        var filamentoMucoideValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_FILAMENTO_MUCOIDE_DESEABLE);

                        var nitrittos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_NITRITOS);
                        var nitrittosValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_NITRITOS_DESEABLE);

                        var proteinas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_PROTEINAS);
                        var proteinasValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_PROTEINAS_DESEABLE);

                        var glucosa = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_GLUCOSA);
                        var glucosaValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_GLUCOSA_DESEABLE);

                        var cetonas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_CETONAS);
                        var cetonasValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_CETONAS_DESEABLE);

                        var urobilinogeno = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_UROBILINOGENO);
                        var urobilinogenoValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_UROBILINOGENO_DESEABLE);

                        var bilirrubinas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_BILIRRUBINA);
                        var bilirrubinasValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_BILIRRUBINA_DESEABLE);

                        var sangre = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_SANGRE);
                        var sangreValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_SANGRE_DESEABLE);

                        var hemoglobina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_HEMOGLOBINA);
                        var hemoglobinaValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_HEMOGLOBINA_DESEABLE);
              

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("EXAMEN FISICO", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(color == null ? string.Empty : color.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(colorValord == null ? string.Empty : colorValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("ASPECTO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(aspecto == null ? string.Empty : aspecto.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(aspectoValord == null ? string.Empty : aspectoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("DENSIDAD", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(densidad == null ? string.Empty : densidad.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(densidadValord == null ? string.Empty : densidadValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("PH", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(ph == null ? string.Empty : ph.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(phValord == null ? string.Empty : phValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("SEDIMENTO URINARIO", fontColumnValueNegrita)) { Colspan = 4 });
                        
                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("CELULAS EPITELEALES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(celulasEpiteleales == null ? string.Empty : celulasEpiteleales.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(celulasEpitelealesValord == null ? string.Empty : celulasEpitelealesValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                       
                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(leucocitosValord == null ? string.Empty : leucocitosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematies == null ? string.Empty : hematies.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hematiesValord == null ? string.Empty : hematiesValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("GERMENES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(germenes == null ? string.Empty : germenes.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(germenesValord == null ? string.Empty : germenesValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("CILINDROS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(cilindros == null ? string.Empty : cilindros.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(cilindrosValord == null ? string.Empty : cilindrosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("CRISTALES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(cristales == null ? string.Empty : cristales.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(cristalesValord == null ? string.Empty : cristalesValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("FILAMENTO MUCOIDE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(filamentoMucoide == null ? string.Empty : filamentoMucoide.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(filamentoMucoideValord == null ? string.Empty : filamentoMucoideValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("EXAMEN BIOQUIMICO", fontColumnValueNegrita)) { Colspan = 4 });
                        
                        // 12va fila
                        cells.Add(new PdfPCell(new Phrase("NITRITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(nitrittos == null ? string.Empty : nitrittos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(nitrittosValord == null ? string.Empty : nitrittosValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("PROTEINAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(proteinas == null ? string.Empty : proteinas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(proteinasValord == null ? string.Empty : proteinasValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("GLUCOSA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(glucosa == null ? string.Empty : glucosa.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(glucosaValord == null ? string.Empty : glucosaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("CETONAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(cetonas == null ? string.Empty : cetonas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(cetonasValord == null ? string.Empty : cetonasValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("UROBILINOGENO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(urobilinogeno == null ? string.Empty : urobilinogeno.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(urobilinogenoValord == null ? string.Empty : urobilinogenoValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("BILIRRUBINAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(bilirrubinas == null ? string.Empty : bilirrubinas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(bilirrubinasValord == null ? string.Empty : bilirrubinasValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(sangre == null ? string.Empty : sangre.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(sangreValord == null ? string.Empty : sangreValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("HEMOGLOBINA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hemoglobina == null ? string.Empty : hemoglobina.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hemoglobinaValord == null ? string.Empty : hemoglobinaValord.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID:

                    #region GRUPO_Y_FACTOR_SANGUINEO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var grupoSanguineo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GRUPO_SANGUINEO_ID);                  
                        var factorSanguineo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.FACTOR_SANGUINEO_ID);
                                          
                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("GRUPO SANGUINEO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(grupoSanguineo == null ? string.Empty : grupoSanguineo.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("FACTOR SANGUINEO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(factorSanguineo == null ? string.Empty : factorSanguineo.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    
                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_ID:

                    #region PARASITOLOGICO_SIMPLE

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var color = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_COLOR);
                        var consistencia = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_CONSISTENCIA);
                        var restosAlimenticios = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESTOS_ALIMENTICIOS);
                        var sangre = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_SANGRE);
                        var moco = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_MOCO);
                        var quistes = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_QUISTES);
                        var huevos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HUEVOS);
                        var trofozoitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_TROFOZOITOS);
                        var hematies = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_HEMATIES);
                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_LEUCOCITOS);
                        var resultado = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(color == null ? string.Empty : color.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CONSISTENCIA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(consistencia == null ? string.Empty : consistencia.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("RESTOS ALIMENTICIOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(restosAlimenticios == null ? string.Empty : restosAlimenticios.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(sangre == null ? string.Empty : sangre.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MOCO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(moco == null ? string.Empty : moco.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //cells.Add(new PdfPCell(new Phrase("FORMULA LEUCOCITARIA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("QUISTES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(quistes == null ? string.Empty : quistes.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HUEVOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(huevos == null ? string.Empty : huevos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("TROFOZOITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(trofozoitos == null ? string.Empty : trofozoitos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematies == null ? string.Empty : hematies.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("RESULTADO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultado == null ? string.Empty : resultado.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });                      

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_ID:

                    #region PARASITOLOGICO_SERIADO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        #region PRIMERA MUESTRA
                        
                      
                        var color = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_COLOR);
                        var consistencia = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_CONSISTENCIA);
                        var restosAlimenticios = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_RESTOS_ALIMENTICIOS);
                        var sangre = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_SANGRE);
                        var moco = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_MOCO);
                        var quistes = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_QUISTES);
                        var huevos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HUEVOS);
                        var trofozoitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_TROFOZOITOS);
                        var hematies = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_HEMATIES);
                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_LEUCOCITOS);
                      

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("PRIMERA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(color == null ? string.Empty : color.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CONSISTENCIA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(consistencia == null ? string.Empty : consistencia.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("RESTOS ALIMENTICIOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(restosAlimenticios == null ? string.Empty : restosAlimenticios.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(sangre == null ? string.Empty : sangre.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MOCO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(moco == null ? string.Empty : moco.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                      
                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("QUISTES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(quistes == null ? string.Empty : quistes.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HUEVOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(huevos == null ? string.Empty : huevos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("TROFOZOITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(trofozoitos == null ? string.Empty : trofozoitos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematies == null ? string.Empty : hematies.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        #endregion

                        #region SEGUNDA MUESTRA

                        // SEGUNDA MUESTRA                    
                        var colorSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_COLOR);
                        var consistenciaSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_CONSISTENCIA);
                        var restosAlimenticiosSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_RESTOS_ALIMENTICIOS);
                        var sangreSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_SANGRE);
                        var mocoSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_MOCO);
                        var quistesSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_QUISTES);
                        var huevosSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HUEVOS);
                        var trofozoitosSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_TROFOZOITOS);
                        var hematiesSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_HEMATIES);
                        var leucocitosSegundaMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_LEUCOCITOS);

                        cells.Add(new PdfPCell(new Phrase("SEGUNDA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(colorSegundaMuestra == null ? string.Empty : colorSegundaMuestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CONSISTENCIA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(consistenciaSegundaMuestra == null ? string.Empty : consistenciaSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("RESTOS ALIMENTICIOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(restosAlimenticiosSegundaMuestra == null ? string.Empty : restosAlimenticiosSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(sangreSegundaMuestra == null ? string.Empty : sangreSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MOCO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(mocoSegundaMuestra == null ? string.Empty : mocoSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("QUISTES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(quistesSegundaMuestra == null ? string.Empty : quistesSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HUEVOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(huevosSegundaMuestra == null ? string.Empty : huevosSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("TROFOZOITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(trofozoitosSegundaMuestra == null ? string.Empty : trofozoitosSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematiesSegundaMuestra == null ? string.Empty : hematiesSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitosSegundaMuestra == null ? string.Empty : leucocitosSegundaMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        #endregion

                        #region TERCERA MUESTRA


                        // TERCERA MUESTRA                    
                        var colorTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_COLOR);
                        var consistenciaTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_CONSISTENCIA);
                        var restosAlimenticiosTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_RESTOS_ALIMENTICIOS);
                        var sangreTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_SANGRE);
                        var mocoTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_MOCO);
                        var quistesTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_QUISTES);
                        var huevosTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HUEVOS);
                        var trofozoitosTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_TROFOZOITOS);
                        var hematiesTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_HEMATIES);
                        var leucocitosTerceraMuestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_LEUCOCITOS);

                        cells.Add(new PdfPCell(new Phrase("TERCERA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(colorTerceraMuestra == null ? string.Empty : colorTerceraMuestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CONSISTENCIA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(consistenciaTerceraMuestra == null ? string.Empty : consistenciaTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("RESTOS ALIMENTICIOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(restosAlimenticiosTerceraMuestra == null ? string.Empty : restosAlimenticiosTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(sangreTerceraMuestra == null ? string.Empty : sangreTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MOCO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(mocoTerceraMuestra == null ? string.Empty : mocoTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("QUISTES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(quistesTerceraMuestra == null ? string.Empty : quistesTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HUEVOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(huevosTerceraMuestra == null ? string.Empty : huevosTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("TROFOZOITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(trofozoitosTerceraMuestra == null ? string.Empty : trofozoitosTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 9na fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematiesTerceraMuestra == null ? string.Empty : hematiesTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitosTerceraMuestra == null ? string.Empty : leucocitosTerceraMuestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        #endregion

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.BK_DIRECTO_ID:

                    #region BK_DIRECTO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_MUESTRA);
                        var colaboracion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_COLORACION);
                        var resultados = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MUESTRA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("COLABORACION", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(colaboracion == null ? string.Empty : colaboracion.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("RESULTADOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultados == null ? string.Empty : resultados.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID:

                    #region TOXICOLOGICO_COCAINA_MARIHUANA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MUESTRA);
                        var metodo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_METODO);
                        var resultadosCocaina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_COCAINA);
                        var resultadosMarihuana = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MARIHUANA);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MUESTRA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("METODO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(metodo == null ? string.Empty : metodo.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("RESULTADOS COCAINA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultadosCocaina == null ? string.Empty : resultadosCocaina.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("RESULTADOS MARIHUANA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultadosMarihuana == null ? string.Empty : resultadosMarihuana.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    //table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
          
                default:            

                    break;
            }

            return table;

        }

        #endregion

        #region Utils

        private static List<ServiceComponentList> GetSortEntity(string[] order, List<ServiceComponentList> serviceComponent)
        {
            List<ServiceComponentList> orderEntities = new List<ServiceComponentList>();

            foreach (var op in order)
            {
                var find = serviceComponent.Find(P => P.v_ComponentId == op);

                if (find != null)
                {
                    orderEntities.Add(find);

                    // Eliminar 
                    serviceComponent.Remove(find);
                }
            }

            // Unir la entidades

            orderEntities.AddRange(serviceComponent);

            return orderEntities;
        }

        private static void RunFile(string filePDF)
        {
            Process proceso = Process.Start(filePDF);
            proceso.WaitForExit();
            proceso.Close();

        }

        #endregion   
    }
}
