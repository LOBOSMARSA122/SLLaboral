using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Sigesoft.Common;
using Sigesoft.Server.WebClientAdmin.BLL;
using System.Data;
using System.IO;
using System.Diagnostics;
using NetPdf;
using Sigesoft.Server.WebClientAdmin.BE;

namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class FRM031B : System.Web.UI.Page
    {
        private string _tempSourcePath;
        private MergeExPDF _mergeExPDF = new MergeExPDF();

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> _filesNameToMerge = new List<string>();

            _tempSourcePath = Path.Combine(Server.MapPath("/TempMerge"));
            List<MyListWeb> ListaServicios = (List<MyListWeb>)Session["objLista"];


            foreach (var item in ListaServicios)
            {

                ReportDocument rp = new ReportDocument();

                OperationResult objOperationResult = new OperationResult();

                var aptitudeCertificate = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, item.IdServicio).ToList();

                DataSet ds = new DataSet();
                DataTable dt = Sigesoft.Server.WebClientAdmin.UI.Utils.ConvertToDatatable(aptitudeCertificate);
                dt.TableName = "AptitudeCertificate";
                ds.Tables.Add(dt);

                if (aptitudeCertificate[0].i_EsoTypeId == ((int)TypeESO.Retiro).ToString())
                {
                    rp.Load(Server.MapPath("crOccupationalMedicalAptitudeCertificateRetiros.rpt"));
                }
                else
                {
                    if (aptitudeCertificate[0].i_AptitudeStatusId == (int)AptitudeStatus.AptoObs)
                    {
                        rp.Load(Server.MapPath("cr.CertficadoObservado.rpt"));
                    }
                    else
                    {
                        rp.Load(Server.MapPath("crOccupationalMedicalAptitudeCertificate.rpt"));
                        
                    }
                }



                //rp.Load(Server.MapPath("crOccupationalMedicalAptitudeCertificate.rpt"));
                rp.SetDataSource(ds);

                rp.SetDataSource(ds);
                var ruta = Server.MapPath("files/CM" + item.IdServicio.ToString() + ".pdf");


                _filesNameToMerge.Add(ruta);

                rp.ExportToDisk(ExportFormatType.PortableDocFormat, ruta);
            }
            _mergeExPDF.FilesName = _filesNameToMerge;
            string Dif = Guid.NewGuid().ToString();
            string NewPath = Server.MapPath("files/" + Dif + ".pdf");
            _mergeExPDF.DestinationFile = NewPath;
            _mergeExPDF.Execute();
            ShowPdf1.FilePath = "files/" + Dif + ".pdf";
        }
    }
}