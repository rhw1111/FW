using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace MSLibrary.Security
{
    public static class RsaExtention
    {
        public static void FromXmlString(this RSA rsa, string xmlString)

        {

            RSAParameters parameters = new RSAParameters();

            XDocument xmlDoc = XDocument.Parse(xmlString);


            if (xmlDoc.Root.Name.Equals("RSAKeyValue"))

            {

                foreach (var node in xmlDoc.Root.Elements())

                {

                    switch (node.Name.LocalName)

                    {

                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.Value); break;

                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.Value); break;

                        case "P": parameters.P = Convert.FromBase64String(node.Value); break;

                        case "Q": parameters.Q = Convert.FromBase64String(node.Value); break;

                        case "DP": parameters.DP = Convert.FromBase64String(node.Value); break;

                        case "DQ": parameters.DQ = Convert.FromBase64String(node.Value); break;

                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.Value); break;

                        case "D": parameters.D = Convert.FromBase64String(node.Value); break;

                    }

                }

            }

            else

            {

                throw new Exception("Invalid XML RSA key.");

            }

            rsa.ImportParameters(parameters);

        }



        public static string ToXmlString(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            if (includePrivateParameters)
            {
                return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",

                    Convert.ToBase64String(parameters.Modulus),

                    Convert.ToBase64String(parameters.Exponent),

                    Convert.ToBase64String(parameters.P),

                    Convert.ToBase64String(parameters.Q),

                    Convert.ToBase64String(parameters.DP),

                    Convert.ToBase64String(parameters.DQ),

                    Convert.ToBase64String(parameters.InverseQ),

                    Convert.ToBase64String(parameters.D));

            }

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",

                    Convert.ToBase64String(parameters.Modulus),

                    Convert.ToBase64String(parameters.Exponent));

        }



    }
}
