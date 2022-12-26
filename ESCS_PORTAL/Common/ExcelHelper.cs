using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ESCS_PORTAL.Common
{
    public class ExcelHelper
    {
        public static void XUAT_EXCEL(DataSet ds, string b_ten, string report_template_url, ref string report_export_url)
        {
            if (!Directory.Exists(report_export_url))
            {
                Directory.CreateDirectory(report_export_url);
            }
            string filename1 = report_template_url + b_ten + ".xml";
            report_export_url = report_export_url + b_ten + DateTime.Now.ToString("yyyyMMdd_hhmmss_ffftt") + ".xls";
            string filename2 = report_export_url;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename1);
            CExcelMLFiller cexcelMlFiller = new CExcelMLFiller(ds, xmlDocument.OuterXml);
            string current;
            if (!cexcelMlFiller.OperationFailed)
            {
                cexcelMlFiller.Transform();
                if (cexcelMlFiller.OperationFailed)
                {
                    IEnumerator enumerator = cexcelMlFiller.ErrorList.GetEnumerator();
                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            current = (string)enumerator.Current;
                            return;
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable disposable)
                            disposable.Dispose();
                    }
                }
            }
            else
            {
                IEnumerator enumerator = cexcelMlFiller.ErrorList.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext())
                    {
                        current = (string)enumerator.Current;
                        return;
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                        disposable.Dispose();
                }
            }
            cexcelMlFiller.ExcelMLDocument.Save(filename2);
        }
    }
}
