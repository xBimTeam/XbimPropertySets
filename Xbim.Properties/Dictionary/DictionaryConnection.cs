using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using No.Catenda.Peregrine.Model.Objects;

namespace Xbim.Properties.Dictionary
{
    public class DictionaryConnection
    {
        private string baseUrl = "http://bsdd.buildingsmart.org";
        private IfdAPISession _session;

        public IfdAPISession LogIn(string email, string password)
        {
            //read a resource from a REST url 
            Uri uri = new Uri(baseUrl + "/api/4.0/session/login"); 
            
            //Create the request object 
            WebRequest req = WebRequest.Create(uri);
            var msg = String.Format("email={0}&password={1}", email, password);
            //msg = HttpUtility.UrlEncode(msg);
            var data = Encoding.UTF8.GetBytes(msg);

            req.Method = "POST";
            req.ContentLength = data.Length;
            req.ContentType = "application/x-www-form-urlencoded";
            using (var outStream = req.GetRequestStream())
            {
                outStream.Write(data, 0, data.Length);

                try
                {
                    using (var resp = req.GetResponse())
                    {
                        Stream stream = resp.GetResponseStream();
                        _session = Deserialize<IfdAPISession>(stream);
                        return _session;
                    }
                }
                catch (Exception)
                {
                    return null ;
                }
            }
        }

        public IfdProperty GetIfdProperty( string propertyGuid)
        {
            var uriString = String.Format("{0}/IfdProperty/forConcept/{1}", baseUrl, propertyGuid);
            var uri = new Uri(uriString);
            WebRequest req = WebRequest.Create(uri);
            req.Method = "GET";
            try
            {
                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        return Deserialize<IfdProperty>(stream);
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public IfdConcepts SearchConcept(string name)
        {
            var uriString = String.Format("{0}/api/4.0/IfdConcept/search/{1}", baseUrl, name);
            var uri = new Uri(uriString);
            WebRequest req = WebRequest.Create(uri);
            req.Method = "GET";
            try
            {
                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        return Deserialize<IfdConcepts>(stream);
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        private T Deserialize<T>(Stream stream)
        {
            XmlSerializer s = new XmlSerializer(typeof(T)); 
            TextReader r = new StreamReader( stream );
            //var data = r.ReadToEnd();
            //File.WriteAllText("Output.txt", data);

            return (T)s.Deserialize(r);
        }

        //based on the code from here: http://stackoverflow.com/questions/11164275/how-to-add-cookies-to-webrequest
        private bool SetSessionCookie(WebRequest webRequest, IfdAPISession session)
        {
            Cookie cookie = new Cookie("peregrineapisessionid", session.Guid);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            httpRequest.CookieContainer.Add(cookie);
            return true;
        }
    }

}
