using api1.Model;
using System.Xml.Serialization;
using weatherForm.DTO;

namespace api1.XMLHelper
{
    public class XmlHelper
    {
        public void saveToXml(Weateher weateher)
        {

            xmlmodel xmlmodel;
            var serializer = new XmlSerializer(typeof(xmlmodel));


            using (Stream mystream = System.IO.File.OpenRead("D:\\Practicies\\api1\\caching\\lastcountery.xml"))
            {
                    
               xmlmodel = (xmlmodel)serializer.Deserialize(mystream);
             
            }

            var flag = xmlmodel?.Weatehers.SingleOrDefault(x => x.Location.name == weateher.Location.name);
            if (flag is null)
            {
                xmlmodel?.Weatehers.Add(weateher);
            }


            using (Stream stream = System.IO.File.Open("D:\\Practicies\\api1\\caching\\lastcountery.xml", FileMode.Create))
            {
                serializer.Serialize(stream, xmlmodel);
            }
        }

    }
}
