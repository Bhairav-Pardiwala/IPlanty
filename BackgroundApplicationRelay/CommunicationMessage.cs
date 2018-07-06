using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundApplicationRelay
{
    public sealed class CommunicationMessage
    {
         string _id = "";
         string _msg = "";
         string _response = "";

        public String id {
            get
            {
                return _id;
            }
                set
            {
                _id = value;
            }
                }
        public String msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
            }
        }

        public String response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
            }
        }
           
    }
}
