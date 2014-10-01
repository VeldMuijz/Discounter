using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discounter
{
    class HashCreator
    {

        public static string getHash(string text){
            string hash = "";
            if (!text.Equals(""))
            {
                var dateString = DateTime.Now.ToLongDateString() + DateTime.Now.Millisecond;
                text = text + dateString;
                hash = String.Format("{0:X}", text.GetHashCode());
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " - Hashcreator: Text: " + text + ", hash: "+ hash+"\n");
            }
            else {
                hash = "##Nohash##";
            }

            return hash;            

        }
    }
}
