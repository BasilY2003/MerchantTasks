using CommonLib.DTOs;
using SelectPdf;
using System.Text;

namespace CommonLib.Pdf
{
    public class PdfGenerator
    {
        public void GeneratePdf(MerchantGroupDto merchantGroup)
        {
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.PdfPageSize = PdfPageSize.A4;

            var sb = new StringBuilder();

            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append(@"<style>
                 table {
                     width: 100%;
                     border-collapse: collapse;
                 }
                 th, td {
                     border: 1px solid #ccc;
                     padding: 8px;
                     text-align: left;
                 }
                 th {
                     background-color: #f2f2f2;
                 }
             </style>");
            sb.Append("</head>");


            sb.Append($@"
               <body>
                   <h1>Group Name: {merchantGroup.Name}</h1>
                   <h3>Id: {merchantGroup.Id}</h3>
                   <p>Date Created: {merchantGroup.CreatedAt:yyyy-MM-dd}</p>
                   <p>Last Updated: {merchantGroup.UpdatedAt:yyyy-MM-dd}</p>
                   <h2>Items</h2>
                   <table>
                       <tr>
                           <th>Merchant Name</th>
                           <th>Id</th>
                           <th>Status</th>
                           <th>Manager Name</th>
                           <th>Created At</th>
                           <th>Business type number</th>
                       </tr>");

            foreach (var merchant in merchantGroup.Merchants)
            {
                sb.Append($@"
                <tr>
                    <td>{merchant.Name}</td>
                    <td>{merchant.Id}</td>
                    <td>{merchant.Status}</td>
                    <td>{merchant.ManagerName}</td>
                    <td>{merchant.CreatedAt}</td>
                    <td>{merchant.BusinessType}</td>
                </tr>");
            }

            sb.Append(@"
            </table>
        </body>
        </html>");

            PdfDocument doc = converter.ConvertHtmlString(sb.ToString());

            string filename = $"groupMerchants_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf";
            string folderPath = "C:\\Users\\HP\\source\\repos\\MerchantsManagement\\CommonLib\\Pdf\\PdfFiles\\";
            string fullPath = Path.Combine(folderPath, filename);

            doc.Save(fullPath);
            doc.Close();
        }
    }
}
