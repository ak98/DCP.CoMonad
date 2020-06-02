using System;
using System.Collections.Generic;
using System.Text;

namespace CoMonad
{
    internal class RezEmbelishedError : RezErrBase
    {
        private List<string> list = new List<string>();
        public RezEmbelishedError(RezErrBase err, string extendedInfo) : base(err)
        {
           
            list.Add(extendedInfo);
            if (err is RezEmbelishedError emb)
            {
                list.AddRange(emb.list);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            foreach (var emebelishment in list)
            {
                if (!string.IsNullOrEmpty(emebelishment))
                {
                    sb.AppendLine();
                    sb.Append(emebelishment);
                }
            }

            return sb.ToString();
        }

    }
}
