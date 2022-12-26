using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ESCS_PORTAL.Common
{
    public class CExcelMLFiller
    {
        private const string repeatAttribute = "SMLRepeat";
        private DataSet dsData;
        private string templateContent;
        private XmlDocument xmlTemplateDoc;
        private XmlNamespaceManager nsmgr = new XmlNamespaceManager((XmlNameTable)new NameTable());
        private ArrayList errorList = new ArrayList();
        private bool bOperationFailed = false;
        private static string excelmlPrefix = "urn:schemas-microsoft-com:office:excel";
        private static string spreadsheetPrefix = "urn:schemas-microsoft-com:office:spreadsheet";
        private XmlNodeList templateRowsColl;
        private XmlNode tableNode;
        private XmlNode workbookNode;
        private XmlNodeList worksheetTemplates;
        private string languageName;
        private CultureInfo ci;
        private string dateTimeFormat;
        private NumberFormatInfo numberFormat;
        private string numberDecimalSeparator;
        private string numberGroupSeparator;
        private int numberDecimalDigits;
        public CExcelMLFiller(DataSet dsData, string templateContent)
        {
            this.dsData = dsData;
            this.templateContent = templateContent;
            this.LoadTemplate();
        }
        public CExcelMLFiller(string templateContent)
        {
            this.templateContent = templateContent;
            this.dsData = new DataSet();
            this.LoadTemplate();
        }
        public ArrayList ErrorList => this.errorList;
        public bool OperationFailed => this.bOperationFailed;
        public XmlDocument ExcelMLDocument => this.xmlTemplateDoc;
        private void LoadTemplate()
        {
            this.errorList = new ArrayList();
            this.bOperationFailed = false;
            try
            {
                this.xmlTemplateDoc = new XmlDocument();
                this.xmlTemplateDoc.LoadXml(this.templateContent);
                this.nsmgr.AddNamespace("x", CExcelMLFiller.excelmlPrefix);
                this.nsmgr.AddNamespace("ss", CExcelMLFiller.spreadsheetPrefix);
                this.workbookNode = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook", this.nsmgr);
                this.worksheetTemplates = this.workbookNode.SelectNodes("ss:Worksheet", this.nsmgr);
                XmlNode xmlNode1 = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Names/ss:NamedRange[@ss:Name='LanguageName']/@ss:RefersTo", this.nsmgr);
                if (xmlNode1 != null)
                {
                    this.languageName = xmlNode1.Value;
                    this.languageName = this.languageName.Replace("=", string.Empty);
                    this.languageName = this.languageName.Replace("\"", string.Empty);
                    try
                    {
                        this.ci = new CultureInfo(this.languageName, false);
                        this.numberFormat = this.ci.NumberFormat;
                    }
                    catch
                    {
                        this.languageName = (string)null;
                        this.numberFormat = (NumberFormatInfo)null;
                    }
                }
                else
                    this.languageName = (string)null;
                XmlNode xmlNode2 = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Names/ss:NamedRange[@ss:Name='DateTimeFormat']/@ss:RefersTo", this.nsmgr);
                if (xmlNode2 != null)
                {
                    this.dateTimeFormat = xmlNode2.Value;
                    this.dateTimeFormat = this.dateTimeFormat.Replace("=", string.Empty);
                    this.dateTimeFormat = this.dateTimeFormat.Replace("\"", string.Empty);
                }
                else
                    this.dateTimeFormat = (string)null;
                XmlNode xmlNode3 = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Names/ss:NamedRange[@ss:Name='NumberDecimalSeparator']/@ss:RefersTo", this.nsmgr);
                if (xmlNode3 != null)
                {
                    this.numberDecimalSeparator = xmlNode3.Value;
                    this.numberDecimalSeparator = this.numberDecimalSeparator.Replace("=", string.Empty);
                    this.numberDecimalSeparator = this.numberDecimalSeparator.Replace("\"", string.Empty);
                    if (this.numberFormat == null)
                        this.numberFormat = new NumberFormatInfo();
                    this.numberFormat.NumberDecimalSeparator = this.numberDecimalSeparator.Trim() == string.Empty ? "." : this.numberDecimalSeparator;
                }
                else
                    this.numberDecimalSeparator = (string)null;
                XmlNode xmlNode4 = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Names/ss:NamedRange[@ss:Name='NumberGroupSeparator']/@ss:RefersTo", this.nsmgr);
                if (xmlNode4 != null)
                {
                    this.numberGroupSeparator = xmlNode4.Value;
                    this.numberGroupSeparator = this.numberGroupSeparator.Replace("=", string.Empty);
                    this.numberGroupSeparator = this.numberGroupSeparator.Replace("\"", string.Empty);
                    if (this.numberFormat == null)
                        this.numberFormat = new NumberFormatInfo();
                    this.numberFormat.NumberGroupSeparator = this.numberGroupSeparator.Trim() == string.Empty ? "," : this.numberGroupSeparator;
                }
                else
                    this.numberGroupSeparator = (string)null;
                XmlNode xmlNode5 = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Names/ss:NamedRange[@ss:Name='NumberDecimalDigits']/@ss:RefersTo", this.nsmgr);
                if (xmlNode5 != null)
                {
                    string s = xmlNode5.Value.Replace("=", string.Empty).Replace("\"", string.Empty);
                    try
                    {
                        this.numberDecimalDigits = int.Parse(s);
                    }
                    catch
                    {
                        this.numberDecimalDigits = -1;
                    }
                    if (this.numberFormat == null)
                        this.numberFormat = new NumberFormatInfo();
                    this.numberFormat.NumberDecimalDigits = this.numberDecimalDigits == -1 ? 2 : this.numberDecimalDigits;
                }
                else
                    this.numberDecimalDigits = -1;
            }
            catch (Exception ex)
            {
                for (Exception exception = ex; exception != null; exception = exception.InnerException)
                    this.errorList.Add((object)exception.Message);
                this.bOperationFailed = true;
            }
        }
        public void Transform()
        {
            try
            {
                this.tableNode = this.xmlTemplateDoc.SelectSingleNode("/ss:Workbook/ss:Worksheet/ss:Table", this.nsmgr);
                this.templateRowsColl = this.xmlTemplateDoc.SelectNodes("/ss:Workbook/ss:Worksheet/ss:Table/ss:Row", this.nsmgr);
                foreach (DataTable table in (InternalDataCollectionBase)this.dsData.Tables)
                {
                    foreach (DataColumn column in (InternalDataCollectionBase)table.Columns)
                        column.ColumnName = column.ColumnName.ToUpper();
                    this.TransformTemplateRows(table);
                    this.TransformTemplateGroup(table);
                }
                for (int i = this.templateRowsColl.Count - 1; i >= 0; --i)
                {
                    if (this.templateRowsColl[i].SelectSingleNode("ss:Cell[contains(@ss:Formula, 'SMLRepeat')]", this.nsmgr) != null)
                        this.tableNode.RemoveChild(this.templateRowsColl[i]);
                }
              ((XmlElement)this.tableNode).RemoveAttribute("ss:ExpandedRowCount");
            }
            catch (Exception ex)
            {
                for (Exception exception = ex; exception != null; exception = exception.InnerException)
                    this.errorList.Add((object)exception.Message);
                this.bOperationFailed = true;
            }
        }
        public void Transform(DataSet ds, string sheetName) => this.Transform(0, ds, sheetName);
        public void Transform(int worksheetTemplateNumber, DataSet ds, string sheetName)
        {
            if (worksheetTemplateNumber > this.worksheetTemplates.Count - 1)
                return;
            try
            {
                XmlNode newChild = this.worksheetTemplates[worksheetTemplateNumber].Clone();
                (newChild as XmlElement).RemoveAttribute("ss:ExpandedRowCount");
                this.tableNode = newChild.SelectSingleNode("ss:Table", this.nsmgr);
                this.templateRowsColl = newChild.SelectNodes("ss:Table/ss:Row", this.nsmgr);
                foreach (DataTable table in (InternalDataCollectionBase)ds.Tables)
                {
                    this.TransformTemplateRows(table);
                    this.TransformTemplateGroup(table);
                }
                for (int i = this.templateRowsColl.Count - 1; i >= 0; --i)
                {
                    if (this.templateRowsColl[i].SelectSingleNode("ss:Cell[contains(@ss:Formula, 'SMLRepeat')]", this.nsmgr) != null)
                        this.tableNode.RemoveChild(this.templateRowsColl[i]);
                }
              ((XmlElement)this.tableNode).RemoveAttribute("ss:ExpandedRowCount");
                (newChild as XmlElement).SetAttribute("ss:Name", sheetName);
                this.workbookNode.AppendChild(newChild);
            }
            catch (Exception ex)
            {
                for (Exception exception = ex; exception != null; exception = exception.InnerException)
                    this.errorList.Add((object)exception.Message);
                this.bOperationFailed = true;
            }
        }
        private void TransformTemplateGroup(DataTable dt)
        {
            foreach (XmlNode baseNode in this.templateRowsColl)
            {
                foreach (DataColumn column in (InternalDataCollectionBase)dt.Columns)
                {
                    string fieldName = dt.TableName + column.ColumnName + "_sum";
                    if (baseNode.SelectSingleNode("ss:Cell[@ss:Formula='=" + fieldName + "']", this.nsmgr) != null)
                        this.ReplaceFieldData(baseNode, fieldName, dt.Compute("Sum([" + column.ColumnName + "])", "").ToString(), column.DataType);
                }
            }
        }
        private void TransformTemplateRows(DataTable dt)
        {
            foreach (XmlNode xmlNode1 in this.templateRowsColl)
            {
                if (xmlNode1.SelectSingleNode("ss:Cell[contains(@ss:Formula, '=" + dt.TableName + "SMLRepeat')]", this.nsmgr) != null)
                {
                    this.tableNode = xmlNode1.ParentNode;
                    foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    {
                        XmlNode xmlNode2 = xmlNode1.Clone();
                        ((XmlElement)xmlNode2).RemoveAttribute("ss:Index");
                        XmlNode xmlNode3 = xmlNode2.SelectSingleNode("ss:Cell[contains(@ss:Formula, '=" + dt.TableName + "SMLRepeat')]", this.nsmgr);
                        ((XmlElement)xmlNode3).RemoveAttribute("ss:Formula");
                        xmlNode3.FirstChild.InnerText = string.Empty;
                        this.tableNode.InsertBefore(xmlNode2, xmlNode1);
                        for (int index = 0; index < row.ItemArray.Length; ++index)
                        {
                            string fieldName = dt.TableName + dt.Columns[index].ColumnName;
                            this.ReplaceFieldData(xmlNode2, fieldName, row[index].ToString(), dt.Columns[index].DataType);
                        }
                    }
                }
                else if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    for (int index = 0; index < row.ItemArray.Length; ++index)
                    {
                        string fieldName = dt.TableName + dt.Columns[index].ColumnName;
                        this.ReplaceFieldData(xmlNode1, fieldName, row[index].ToString(), dt.Columns[index].DataType);
                    }
                }
            }
        }
        private void ReplaceFieldData(XmlNode baseNode, string fieldName, string data, Type colType)
        {
            this.errorList = new ArrayList();
            this.bOperationFailed = false;
            foreach (XmlNode selectNode in baseNode.SelectNodes("ss:Cell[@ss:Formula='=" + fieldName + "']", this.nsmgr))
            {
                XmlNode xmlNode = selectNode.SelectSingleNode("ss:Data", this.nsmgr);
                if (xmlNode == null)
                {
                    this.errorList.Add((object)"The field data is selected from the fields definition data source or merge document is corrupted!");
                    this.bOperationFailed = true;
                    break;
                }
              ((XmlElement)selectNode).RemoveAttribute("ss:Formula");
                if (colType == typeof(DateTime))
                {
                    if (this.dateTimeFormat != null)
                    {
                        DateTime dateTime = DateTime.Parse(data);
                        xmlNode.InnerText = dateTime.ToString(this.dateTimeFormat);
                    }
                    else
                        xmlNode.InnerText = data;
                }
                else if (colType == typeof(int) || colType == typeof(short) || colType == typeof(long))
                {
                    if (this.numberFormat != null)
                    {
                        int num = int.Parse(data);
                        xmlNode.InnerText = num.ToString((IFormatProvider)this.numberFormat);
                    }
                    else
                        xmlNode.InnerText = data == "" ? "0" : data;
                    ((XmlElement)xmlNode).SetAttribute("ss:Type", "Number");
                }
                else if (colType == typeof(Decimal) || colType == typeof(float) || colType == typeof(double))
                {
                    if (this.numberFormat != null)
                    {
                        Decimal num = Decimal.Parse(data);
                        xmlNode.InnerText = num.ToString("N", (IFormatProvider)this.numberFormat);
                    }
                    else
                        xmlNode.InnerText = data == "" ? "0" : data;
                    ((XmlElement)xmlNode).SetAttribute("ss:Type", "Number");
                }
                else
                {
                    xmlNode.InnerText = data;
                    ((XmlElement)xmlNode).SetAttribute("ss:Type", "String");
                }
            }
        }
        public void RemoveWorksheetTemplate()
        {
            foreach (XmlNode worksheetTemplate in this.worksheetTemplates)
                this.workbookNode.RemoveChild(worksheetTemplate);
        }
    }
}
