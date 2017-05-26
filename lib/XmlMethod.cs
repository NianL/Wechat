using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using System.Text;

namespace WeiXinPublic.lib
{
    public class XmlMethod
    {
        //通过传入的特定XML字符串，通过 ReadXml函数读取到DataSet中。
        public static DataSet GetDataSetByXml(string xmlData)
        {
            DataSet ds = new DataSet();
            try
            {
                using (StringReader xmlSR = new StringReader(xmlData))
                {
                    //ds.ReadXml(xmlSR, XmlReadMode.InferTypedSchema); //忽视任何内联架构，从数据推断出强类型架构并加载数据.如果无法推断，则解释成字符串数据
                    ds.ReadXml(xmlSR);
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                    else//可能是根节点下面直接有值
                    {
                        /*string value = "";
                        System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                        xml.LoadXml(xmlData);
                        ds.ReadXml(xml);

                        System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xml);
                        /*XmlElement xe = (XmlElement)xml.SelectNodes([0];
                        if (xe != null)
                        {
                            value = xe.InnerText;
                        }
                        return value;*/
                    }
                }
                return null;
            }
            catch (Exception)
            {
                StringBuilder info = new StringBuilder();
                ds = new DataSet();
                foreach (char cc in xmlData)
                {
                    int ss = (int)cc;
                    if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)) || (ss == 38))
                    {
                        if (ss == 38)//&符号
                        {
                            info.Append("&amp;");
                        }
                        else
                        {
                            info.AppendFormat(" ", ss);//&#x{0:X};
                        }
                    }
                    else info.Append(cc);
                }
                using (StringReader xmlSR = new StringReader(info.ToString()))
                {
                    //ds.ReadXml(xmlSR, XmlReadMode.IgnoreSchema); //忽视任何内联架构，从数据推断出强类型架构并加载数据.如果无法推断，则解释成字符串数据
                    ds.ReadXml(xmlSR);
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
                return null;
            }
        }
    }
}