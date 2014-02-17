using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chess.Testbed
{
	class Serializer
	{
		public static string SerializeToXml(object data)
		{
			var serializer = new XmlSerializer(data.GetType());
			var sw = new StringWriter();
			serializer.Serialize(sw, data);
			return sw.ToString();
		}

		public static T DeserializeFromXml<T>(string root)
		{
			var serializer = new XmlSerializer(typeof(T));
			var sr = new StringReader(root);
			var rootDir = serializer.Deserialize(sr);
			return (T)rootDir;
		}
	}
}
