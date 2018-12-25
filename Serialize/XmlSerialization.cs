using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Serialize
{
    public class XmlSerialization<T> : ISerializable  where T : new()
    {

        public object Deserialize(string serStr)
        {
            var xDoc = new XmlDocument();

            xDoc.LoadXml(serStr);

            var inType = typeof(T);

            var ret = new T();

            foreach (XmlNode node in xDoc.DocumentElement)
            {
                var childNodes = node.ChildNodes;

                if (childNodes.Count > 1)
                {
                    var type = childNodes[0].Name;

                    dynamic list = null;

                    switch (type)
                    {
                        case "int":
                            list = new List<int>();
                            break;

                        case "decimal":
                            list = new List<decimal>();
                            break;

                        case "double":
                            list = new List<double>();
                            break;
                    }

                   
                    foreach (XmlNode cn in childNodes)
                    {
                        switch (type)
                        {
                            case "int":
                                list.Add(int.Parse(cn.InnerText));
                                break;

                            case "decimal":
                                list.Add(decimal.Parse(cn.InnerText.Replace('.',',')));
                                break;

                            case "double":
                                list.Add(double.Parse(cn.InnerText.Replace('.', ',')));
                                break;
                        }
                    }

                    foreach (var field in inType.GetFields())
                    {
                        if (field.Name == node.Name)
                            field.SetValue(ret, list.ToArray());
                    }

                }
                else
                {
                    foreach (var field in inType.GetFields())
                    {
                        if (field.Name == node.Name)
                            field.SetValue(ret, int.Parse(node.InnerText));
                    }

                }

            }

            return ret;
        }

        public string Serialize(object inputObj)
        {
            var xDoc = new XDocument();

            var type = inputObj.GetType();

            var xRoot = new XElement(type.Name);

            foreach (var field in type.GetFields())
            {
                dynamic fValue = field.GetValue(inputObj);

                if (fValue.ToString().Contains("[]"))
                {
                    var node = new XElement(field.Name);
                    
                    foreach (var v in fValue)
                    {
                        var vtype = v.GetType().ToString().Split('.')[1].ToLower();
                        node.Add(new XElement(vtype, v.ToString().Replace(',', '.')));
                    }
                    xRoot.Add(node);
                }
                else
                    xRoot.Add(new XElement(field.Name, fValue.ToString().Replace(',','.')));
            }

            xDoc.Add(xRoot);

            return xDoc.ToString().Replace(" ", "").Replace("\n", "").Replace("\r","");
        }
    }
}