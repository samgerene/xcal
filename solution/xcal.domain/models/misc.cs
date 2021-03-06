﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using reexjungle.xcal.domain.contracts;



namespace reexjungle.xcal.domain.models
{
    [DataContract]
    public class IANA_COMPONENT: IMISC_COMPONENT
    {
        [DataMember]
        public string TokenName { get; set; }

        [DataMember]
        [Ignore]
        List<IANA_PROPERTY> ContentLines { get; set; }

        public IANA_COMPONENT()
        {
            this.ContentLines = new List<IANA_PROPERTY>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("BEGIN:{0}", this.TokenName).AppendLine();
            foreach (var line in this.ContentLines) sb.Append(line);
            sb.AppendFormat("END:{0}", this.TokenName);
            return sb.ToString();
        }

    }

    [DataContract]
    public class XCOMPONENT : IMISC_COMPONENT
    {
        [DataMember]
        public string TokenName { get; set; }

        [DataMember]
        [Ignore]
        List<X_PROPERTY> ContentLines { get; set; }

        public XCOMPONENT()
        {
            this.ContentLines = new List<X_PROPERTY>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("BEGIN:{0}", this.TokenName).AppendLine();
            foreach (var line in this.ContentLines) sb.Append(line);
            sb.AppendFormat("END:{0}", this.TokenName);
            return sb.ToString();
        }

    }
}
