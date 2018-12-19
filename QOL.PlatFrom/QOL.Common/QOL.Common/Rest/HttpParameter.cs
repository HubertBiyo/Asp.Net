using System;

namespace QOL.Common.Rest
{
    public class HttpParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool EncodeName { set; get; }

        public bool EncodeValue { set; get; }
    }
    public class HttpParameterWx:HttpParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool EncodeName { set; get; }

        public bool EncodeValue { set; get; }
    }
}
