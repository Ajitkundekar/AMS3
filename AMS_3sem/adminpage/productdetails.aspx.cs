using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS_3sem.adminpage
{
    public partial class productdetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {

            // Load the report
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(Server.MapPath("~/adminpage/productdetail1.rpt"));

            // Set any report parameters here if needed
            // reportDocument.SetParameterValue("paramName", paramValue);

            // Export the report to a byte array
            byte[] reportBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                reportDocument.ExportToStream(ExportFormatType.PortableDocFormat).CopyTo(memoryStream);
                reportBytes = memoryStream.ToArray();
            }

            // Send the report to the browser
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=productdetails.pdf");
            Response.BinaryWrite(reportBytes);
            Response.End();

        }
    }
}