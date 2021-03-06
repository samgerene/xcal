﻿using reexjungle.foundation.essentials.concretes;
using reexjungle.foundation.essentials.contracts;
using reexjungle.xcal.domain.contracts;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace reexjungle.xcal.domain.models
{
    #region Descriptive Properties

    /// <summary>
    /// Specifies a contract for attaching an inline binary encooded content information.
    /// </summary>
    [DataContract]
    public class ATTACH_BINARY : IATTACH<BINARY>, IEquatable<ATTACH_BINARY>, IContainsKey<string>
    {
        /// <summary>
        /// ID of an Attachment for a particular Calendar component
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Binary encoded content of the attachment
        /// </summary>
        [DataMember]
        public BINARY Content { get; set; }

        /// <summary>
        /// Media Type of the resource in an Attachmant
        /// </summary>
        [DataMember]
        public FMTTYPE FormatType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ATTACH_BINARY()
        {
            this.FormatType = null;
            this.Content = null;
        }

        public ATTACH_BINARY(IATTACH<BINARY> attachment)
        {
            this.FormatType = attachment.FormatType;
            this.Content = attachment.Content;
        }

        public ATTACH_BINARY(BINARY content, FMTTYPE format = null)
        {
            this.Content = content;
            this.FormatType = format;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String of the format "ATTACH;FormatType;ENCODING=Encoding; VALUE=BINARY:Content"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (this.Content != null)
            {
                sb.AppendFormat("ATTACH");
                if (this.FormatType != null) sb.AppendFormat(";{0}", this.FormatType);
                sb.AppendFormat(";ENCODING={0}", this.Content.Encoding);
                sb.AppendFormat(";VALUE=BINARY:{0}", this.Content);
            }

            return sb.ToString();
        }

        public bool Equals(ATTACH_BINARY other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as ATTACH_BINARY);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(ATTACH_BINARY a, ATTACH_BINARY b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(ATTACH_BINARY a, ATTACH_BINARY b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Capability to associate a document or another resource given by it's URI with a calendar component
    /// </summary>
    [DataContract]
    public class ATTACH_URI : IATTACH<URI>, IEquatable<ATTACH_URI>, IComparable<ATTACH_URI>, IContainsKey<string>
    {
        private FMTTYPE format;

        /// <summary>
        /// ID of an Attachment for a particular Calendar component
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Media Type of the resource in an Attachmant
        /// </summary>
        [DataMember]
        public FMTTYPE FormatType
        {
            get { return this.format; }
            set { this.format = (FMTTYPE)value; }
        }

        /// <summary>
        /// Universal Resource Identifier of the attached Content
        /// </summary>
        [DataMember]
        public URI Content { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ATTACH_URI()
        {
        }

        public ATTACH_URI(IATTACH<URI> attachment)
        {
            this.Content = attachment.Content;
            this.FormatType = attachment.FormatType;
        }

        /// <summary>
        /// Constructor on the base of CONTENT of the Attachment and it's TYPE
        /// </summary>
        /// <param name="content">URI of the attached content</param>
        /// <param name="format">The format type of the attachmen</param>
        public ATTACH_URI(URI content, FMTTYPE format = null)
        {
            this.FormatType = format;
            this.Content = content;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String of the format "ATTACH;FormatType;ENCODING=Encoding; VALUE=BINARY:Content"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ATTACH");
            if (!this.FormatType.Equals(string.Empty)) sb.AppendFormat(";{0}", this.FormatType);
            sb.AppendFormat(":{0}", this.Content);
            return sb.ToString();
        }

        public bool Equals(ATTACH_URI other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as ATTACH_URI);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public int CompareTo(ATTACH_URI other)
        {
            return this.Content.CompareTo(other.Content);
        }

        public static bool operator <(ATTACH_URI a, ATTACH_URI b)
        {
            if (a == null || b == null) return false;
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(ATTACH_URI a, ATTACH_URI b)
        {
            if (a == null || b == null) return false;
            return a.CompareTo(b) > 0;
        }

        public static bool operator ==(ATTACH_URI a, ATTACH_URI b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(ATTACH_URI a, ATTACH_URI b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Defines the categories or subtypes for a calendar component
    /// </summary>
    [DataContract]
    public class CATEGORIES : ICATEGORIES, IEquatable<CATEGORIES>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the category
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Description of the category or the subtype
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }

        /// <summary>
        /// Language used for this category of calendar components
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        public CATEGORIES()
        {
            this.Values = new List<string>();
            this.Language = null;
        }

        public CATEGORIES(ICATEGORIES categories)
        {
            this.Language = categories.Language;
            this.Values = categories.Values;
        }

        /// <summary>
        /// Constructor based on the TEXT and LANGUAGE properties of the class
        /// </summary>
        /// <param name="text">Description of the calendar component category or subtype</param>
        /// <param name="language">Language, used in a given category of calendar components</param>
        public CATEGORIES(List<string> values, LANGUAGE language = null)
        {
            this.Values = values;
            this.Language = language;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String representation of a category property in the form of "CATEGORIES;Language;Text"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CATEGORIES");
            if (this.Language.Equals(string.Empty)) sb.AppendFormat(";{0}", this.Language);
            sb.Append(":");
            var last = this.Values.Last();
            foreach (var val in this.Values)
            {
                if (val != last) sb.AppendFormat("{0}, ", val);
                else sb.Append(val);
            }
            return sb.ToString();
        }

        public bool Equals(CATEGORIES other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as CATEGORIES);
        }

        public override int GetHashCode()
        {
            return this.Values.GetHashCode();
        }

        public static bool operator ==(CATEGORIES a, CATEGORIES b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(CATEGORIES a, CATEGORIES b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Specify non-processing information intended to provide a comment to the calendar user
    /// </summary>
    [DataContract]
    [KnownType(typeof(COMMENT))]
    [KnownType(typeof(CONTACT))]
    [KnownType(typeof(SUMMARY))]
    [KnownType(typeof(LOCATION))]
    [KnownType(typeof(DESCRIPTION))]
    public abstract class TEXTUAL : ITEXTUAL, IEquatable<TEXTUAL>, IComparable<TEXTUAL>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the Comment to the Calendar Component
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The TEXT content of the comment
        /// </summary>
        [DataMember]
        [StringLength(int.MaxValue)]
        public string Text { get; set; }

        /// <summary>
        /// Alternative Text of the Comment, can content more particular Description of a Calendar Component
        /// </summary>
        [DataMember]
        public URI AlternativeText { get; set; }

        /// <summary>
        /// Languge of the Comment
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TEXTUAL()
        {
            this.Text = string.Empty;
            this.AlternativeText = null;
        }

        public TEXTUAL(string text)
        {
            this.Text = text;
            this.AlternativeText = null;
        }

        public TEXTUAL(ITEXTUAL value)
        {
            this.Language = value.Language;
            this.AlternativeText = value.AlternativeText;
            this.Text = value.Text;
        }

        /// <summary>
        /// Constructor specifying the Text, Alternative Text and Language of the Comment
        /// </summary>
        /// <param name="text">Text content of the comment</param>
        /// <param name="alt">Alternative text content of the comment</param>
        /// <param name="language">Language of the comment</param>
        public TEXTUAL(string text, URI altrep, LANGUAGE language = null)
        {
            this.Text = text;
            this.AlternativeText = altrep;
            this.Language = language;
        }

        public bool Equals(TEXTUAL other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as TEXTUAL);
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode();
        }

        public int CompareTo(TEXTUAL other)
        {
            return this.Text.CompareTo(other.Text);
        }

        public static bool operator ==(TEXTUAL a, TEXTUAL b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(TEXTUAL a, TEXTUAL b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }

        public static bool operator <(TEXTUAL a, TEXTUAL b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(TEXTUAL a, TEXTUAL b)
        {
            return a.CompareTo(b) > 0;
        }
    }

    [DataContract]
    public class DESCRIPTION : TEXTUAL
    {
        public DESCRIPTION()
            : base()
        {
        }

        public DESCRIPTION(string text)
            : base(text)
        {
        }

        public DESCRIPTION(string text, URI altrep, LANGUAGE language = null)
            : base(text, altrep, language)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("DESCRIPTION");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Text.EscapeStrings());
            return sb.ToString();
        }
    }

    [DataContract]
    public class COMMENT : TEXTUAL
    {
        public COMMENT()
            : base()
        {
        }

        public COMMENT(string text)
            : base(text)
        {
        }

        public COMMENT(string text, URI altrep, LANGUAGE language = null)
            : base(text, altrep, language)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("COMMENT");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Text.EscapeStrings());
            return sb.ToString();
        }
    }

    [DataContract]
    public class CONTACT : TEXTUAL
    {
        public CONTACT()
            : base()
        {
        }

        public CONTACT(string text)
            : base(text)
        {
        }

        public CONTACT(string text, URI altrep, LANGUAGE language = null)
            : base(text, altrep, language)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("CONTACT");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Text.EscapeStrings());
            return sb.ToString();
        }
    }

    [DataContract]
    public class SUMMARY : TEXTUAL
    {
        public SUMMARY()
            : base()
        {
        }

        public SUMMARY(string text)
            : base(text)
        {
        }

        public SUMMARY(string text, URI altrep, LANGUAGE language = null)
            : base(text, altrep, language)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("SUMMARY");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Text.EscapeStrings());
            return sb.ToString();
        }
    }

    [DataContract]
    public class LOCATION : TEXTUAL
    {
        public LOCATION()
            : base()
        {
        }

        public LOCATION(string text)
            : base(text)
        {
        }

        public LOCATION(string text, URI altrep, LANGUAGE language = null)
            : base(text, altrep, language)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("LOCATION");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Text.EscapeStrings());
            return sb.ToString();
        }
    }

    /// <summary>
    /// Specifies the information related to the global position for the activity spiecified by a calendar component
    /// </summary>
    [DataContract]
    public struct GEO : IGEO, IEquatable<GEO>
    {
        private readonly float longitude;
        private readonly float latitude;

        /// <summary>
        /// Longitude of the Geographical Position
        /// </summary>
        public float Longitude
        {
            get { return this.longitude; }
        }

        /// <summary>
        /// Lattitude of the Geographical Position
        /// </summary>
        public float Latitude
        {
            get { return this.latitude; }
        }

        public GEO(IGEO geo)
        {
            this.latitude = geo.Latitude;
            this.longitude = geo.Longitude;
        }

        /// <summary>
        /// Constructor based on the Longitude and Lattitude of the specified location
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        public GEO(float lon, float lat)
        {
            this.longitude = lon;
            this.latitude = lat;
        }

        public GEO(string value)
        {
            this.longitude = default(float);
            this.latitude = default(float);

            var pattern = @"^(?<lon>[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?)(?<lat>[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?)$";
            if (Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture))
            {
                foreach (Match match in Regex.Matches(value, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture))
                {
                    if (match.Groups["lon"].Success) this.longitude = float.Parse(match.Groups["lon"].Value);
                    if (match.Groups["lat"].Success) this.latitude = float.Parse(match.Groups["lat"].Value);
                }
            }
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String representation of the GEO property in form of "GEO:Lattilude;Longitude" </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("GEO:{0};{1}", this.Latitude, this.Longitude);
            return sb.ToString();
        }

        public bool Equals(GEO other)
        {
            return this.Latitude == other.Latitude && this.Longitude == other.Longitude;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals((GEO)obj);
        }

        public override int GetHashCode()
        {
            return this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode();
        }

        public static bool operator ==(GEO a, GEO b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(GEO a, GEO b)
        {
            if ((object)a == null || (object)b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Defines the Equipment or resources anticipated for an activity specified by a calendar component
    /// </summary>
    [DataContract]
    public class RESOURCES : IRESOURCES, IEquatable<RESOURCES>, IContainsKey<string>
    {
        /// <summary>
        /// ID of a particular Resource
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Alternative Text for a specified Resource, can content the Description of this resource
        /// </summary>
        [DataMember]
        public URI AlternativeText { get; set; }

        /// <summary>
        /// Language needed for a particular Resource
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        /// <summary>
        /// Name and other parameters, descripting a particular Resource
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }

        /// <summary>
        /// Indicates, if the Resource Property is set to default
        /// </summary>
        public bool IsDefault()
        {
            return this.Values.Count() == 0 && this.AlternativeText.IsDefault();
        }

        public RESOURCES()
        {
            this.Values = null;
            this.AlternativeText = null;
            this.Language = null;
        }

        public RESOURCES(IRESOURCES resources)
        {
            this.AlternativeText = resources.AlternativeText;
            this.Language = resources.Language;
            this.Values = resources.Values;
        }

        /// <summary>
        /// Constructor specifying the Text, Alternative Text and Language for Resource Property
        /// </summary>
        /// <param name="text">Text with the name and other parameters, specifying the current Resource</param>
        /// <param name="alt">Alternative Text, can represent particular the description of the Resource</param>
        /// <param name="language">Language nescessary for the Resource</param>
        public RESOURCES(List<string> values, URI alt, LANGUAGE language)
        {
            this.Values = values;
            this.AlternativeText = alt;
            this.Language = language;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String representation of the Resource property in form of "RESOURCES;AlternativeText;Language:Text"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RESOURCES");
            if (this.AlternativeText != null) sb.AppendFormat(";ALTREP=\"{0}\"", this.AlternativeText);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.Append(":");
            var last = this.Values.Last();
            foreach (var val in this.Values)
            {
                if (val != last) sb.AppendFormat("{0}, ", val);
                else sb.Append(val);
            }
            return sb.ToString();
        }

        public bool Equals(RESOURCES other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as RESOURCES);
        }

        public override int GetHashCode()
        {
            return this.Values.GetHashCode();
        }

        public static bool operator ==(RESOURCES a, RESOURCES b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(RESOURCES a, RESOURCES b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    [DataContract]
    public struct PRIORITY : IPRIORITY, IEquatable<PRIORITY>, IComparable<PRIORITY>
    {
        private readonly int value;
        private readonly PRIORITYLEVEL level;
        private readonly PRIORITYSCHEMA schema;
        private readonly PriorityType format;

        private int LevelToValue(PRIORITYLEVEL level)
        {
            var val = 0;
            switch (level)
            {
                case PRIORITYLEVEL.UNKNOWN: val = 0; break;
                case PRIORITYLEVEL.LOW: val = 9; break;
                case PRIORITYLEVEL.MEDIUM: val = 5; break;
                case PRIORITYLEVEL.HIGH: val = 1; break;
            }
            return val;
        }

        private PRIORITYLEVEL ValueToLevel(int value)
        {
            if (value >= 1 && this.value <= 4) return PRIORITYLEVEL.HIGH;
            else if (value == 5) return PRIORITYLEVEL.MEDIUM;
            else if (value >= 6 && value <= 9) return PRIORITYLEVEL.LOW;
            return PRIORITYLEVEL.UNKNOWN;
        }

        private int SchemaToValue(PRIORITYSCHEMA schema)
        {
            var val = 0;
            switch (schema)
            {
                case PRIORITYSCHEMA.UNKNOWN: val = 0; break;
                case PRIORITYSCHEMA.C3: val = 9; break;
                case PRIORITYSCHEMA.C2: val = 8; break;
                case PRIORITYSCHEMA.C1: val = 7; break;
                case PRIORITYSCHEMA.B3: val = 6; break;
                case PRIORITYSCHEMA.B2: val = 5; break;
                case PRIORITYSCHEMA.B1: val = 4; break;
                case PRIORITYSCHEMA.A3: val = 3; break;
                case PRIORITYSCHEMA.A2: val = 2; break;
                case PRIORITYSCHEMA.A1: val = 1; break;
            }
            return val;
        }

        private PRIORITYSCHEMA ValueToSchema(int value)
        {
            var schema = PRIORITYSCHEMA.UNKNOWN;
            switch (value)
            {
                case 1: schema = PRIORITYSCHEMA.A1; break;
                case 2: schema = PRIORITYSCHEMA.A2; break;
                case 3: schema = PRIORITYSCHEMA.A3; break;
                case 4: schema = PRIORITYSCHEMA.B1; break;
                case 5: schema = PRIORITYSCHEMA.B2; break;
                case 6: schema = PRIORITYSCHEMA.B3; break;
                case 7: schema = PRIORITYSCHEMA.C1; break;
                case 8: schema = PRIORITYSCHEMA.C2; break;
                case 9: schema = PRIORITYSCHEMA.C3; break;
            }

            return schema;
        }

        private PRIORITYLEVEL SchemaToLevel(PRIORITYSCHEMA schema)
        {
            var val = PRIORITYLEVEL.UNKNOWN;
            switch (schema)
            {
                case PRIORITYSCHEMA.UNKNOWN: val = PRIORITYLEVEL.UNKNOWN; break;
                case PRIORITYSCHEMA.C3: val = PRIORITYLEVEL.LOW; break;
                case PRIORITYSCHEMA.C2: val = PRIORITYLEVEL.LOW; break;
                case PRIORITYSCHEMA.C1: val = PRIORITYLEVEL.LOW; break;
                case PRIORITYSCHEMA.B3: val = PRIORITYLEVEL.LOW; break;
                case PRIORITYSCHEMA.B2: val = PRIORITYLEVEL.MEDIUM; break;
                case PRIORITYSCHEMA.B1: val = PRIORITYLEVEL.HIGH; break;
                case PRIORITYSCHEMA.A3: val = PRIORITYLEVEL.HIGH; break;
                case PRIORITYSCHEMA.A2: val = PRIORITYLEVEL.HIGH; break;
                case PRIORITYSCHEMA.A1: val = PRIORITYLEVEL.HIGH; break;
            }
            return val;
        }

        private PRIORITYSCHEMA LevelToSchema(PRIORITYLEVEL schema)
        {
            var value = PRIORITYSCHEMA.UNKNOWN;
            switch (schema)
            {
                case PRIORITYLEVEL.LOW: value = PRIORITYSCHEMA.C3; break;
                case PRIORITYLEVEL.MEDIUM: value = PRIORITYSCHEMA.B2; break;
                case PRIORITYLEVEL.HIGH: value = PRIORITYSCHEMA.A1; break;
                case PRIORITYLEVEL.UNKNOWN: value = PRIORITYSCHEMA.UNKNOWN; break;
            }

            return value;
        }

        public PriorityType Format
        {
            get { return this.format; }
        }

        public int Value
        {
            get { return this.value; }
        }

        public PRIORITYLEVEL Level
        {
            get { return this.level; }
        }

        public PRIORITYSCHEMA Schema
        {
            get { return this.schema; }
        }

        public PRIORITY(IPRIORITY priority)
        {
            this.format = priority.Format;
            this.value = priority.Value;
            this.schema = priority.Schema;
            this.level = priority.Level;
        }

        public PRIORITY(int value)
        {
            this.format = PriorityType.Integral;
            this.value = value;
            this.schema = PRIORITYSCHEMA.UNKNOWN;
            this.level = PRIORITYLEVEL.UNKNOWN;
            this.schema = this.ValueToSchema(value);
            this.level = this.ValueToLevel(value);
        }

        public PRIORITY(PRIORITYLEVEL level)
        {
            this.format = PriorityType.Level;
            this.value = 0;
            this.schema = PRIORITYSCHEMA.UNKNOWN;
            this.level = level;
            this.value = this.LevelToValue(level);
            this.schema = this.LevelToSchema(level);
        }

        public PRIORITY(PRIORITYSCHEMA schema)
        {
            this.format = PriorityType.Schema;
            this.value = 0;
            this.schema = schema;
            this.level = PRIORITYLEVEL.UNKNOWN;
            this.value = this.SchemaToValue(schema);
            this.level = this.SchemaToLevel(schema);
        }

        public PRIORITY(string value)
        {
            this.format = PriorityType.Integral;
            this.value = 0;
            this.schema = PRIORITYSCHEMA.UNKNOWN;
            this.level = PRIORITYLEVEL.UNKNOWN;

            var pattern = @"^(?<priority>PRIORITY:)?(?<value>\d)$$";
            if (Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture))
            {
                foreach (Match match in Regex.Matches(value, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture))
                {
                    if (match.Groups["value"].Success) this.value = int.Parse(match.Groups["value"].Value);
                }
            }

            this.schema = this.ValueToSchema(this.value);
            this.level = this.ValueToLevel(this.value);
        }

        public override string ToString()
        {
            if (this.Format == PriorityType.Integral) return string.Format("PRIORITY:{0}", this.value);
            else if (this.Format == PriorityType.Level) return string.Format("PRIORITY:{0}", this.LevelToValue(this.level));
            else return string.Format("PRIORITY:{0}", this.SchemaToValue(this.schema));
        }

        public bool Equals(PRIORITY other)
        {
            return this.value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals((PRIORITY)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public int CompareTo(PRIORITY other)
        {
            return this.Value.CompareTo(other.Value);
        }

        public static bool operator ==(PRIORITY a, PRIORITY b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(PRIORITY a, PRIORITY b)
        {
            if ((object)a == null || (object)b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }

        public static bool operator <(PRIORITY a, PRIORITY b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(PRIORITY a, PRIORITY b)
        {
            return a.CompareTo(b) > 0;
        }
    }

    #endregion Descriptive Properties

    #region Date and Time Component Properties

    /// <summary>
    /// Defines one or more free or busy time intervals
    /// </summary>
    [DataContract]
    public class FREEBUSY_INFO : IFREEBUSY_INFO, IContainsKey<string>
    {
        /// <summary>
        /// ID of a free od busy time interval
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Defines, whether the time interval is FREE or BUSY
        /// </summary>
        [DataMember]
        public FBTYPE Type { get; set; }

        /// <summary>
        /// Period of time, taken by a calendar component
        /// </summary>
        [DataMember]
        [Ignore]
        public List<PERIOD> Periods { get; set; }

        public FREEBUSY_INFO()
        {
        }

        /// <summary>
        /// Constructor based on the time interval
        /// </summary>
        /// <param name="period"></param>
        public FREEBUSY_INFO(List<PERIOD> periods, FBTYPE type = FBTYPE.FREE)
        {
            this.Periods = periods;
            this.Type = type;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String representation of the FREEBUSY property in form of "FREEBUSY;Type:Period"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("FREEBUSY");
            if (this.Type != FBTYPE.UNKNOWN) sb.AppendFormat(";{0}", this.Type);
            sb.Append(":");

            if (!this.Periods.NullOrEmpty())
            {
                var last = this.Periods.Last();
                foreach (var period in this.Periods)
                {
                    if (period != last) sb.AppendFormat("{0}, ", period);
                    else sb.AppendFormat("{0}", period);
                }
            }
            return sb.ToString();
        }
    }

    #endregion Date and Time Component Properties

    #region Time-zone Properties

    /// <summary>
    /// Specifies the customary designation for a time zone description
    /// </summary>
    [DataContract]
    public class TZNAME : ITZNAME, IEquatable<TZNAME>, IComparable<TZNAME>
    {
        private LANGUAGE language;
        private string text;

        /// <summary>
        /// Language, inherent for this time zone
        /// </summary>
        [DataMember]
        public LANGUAGE Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        /// <summary>
        /// Text, containing the Name of the Time-Zone
        /// </summary>
        [DataMember]
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
            }
        }

        public TZNAME()
        {
        }

        /// <summary>
        /// Constructor, based on the Name of the Time Zone
        /// </summary>
        /// <param name="text"></param>
        public TZNAME(string text, LANGUAGE language = null)
        {
            this.text = text;
            this.language = language;
        }

        public TZNAME(ITZNAME tzname)
        {
            this.Language = tzname.Language;
            this.Text = tzname.Text;
        }

        public bool Equals(TZNAME other)
        {
            if (other == null) return false;
            return this.text == other.Text && this.language == other.Language;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as TZNAME);
        }

        public override int GetHashCode()
        {
            return (this.language == null) ?
                 this.text.GetHashCode() :
                 this.text.GetHashCode() ^ this.language.GetHashCode();
        }

        public int CompareTo(TZNAME other)
        {
            return this.text.CompareTo(other.Text);
        }

        public static bool operator ==(TZNAME a, TZNAME b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(TZNAME a, TZNAME b)
        {
            if ((object)a == null || (object)b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }

        public static bool operator <(TZNAME a, TZNAME b)
        {
            if ((object)a == null || (object)b == null) return false;
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(TZNAME a, TZNAME b)
        {
            if ((object)a == null || (object)b == null) return false;
            return a.CompareTo(b) > 0;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String Representation of the Time Zone Name property in form of "TZNAME;Language:Text"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TZNAME");
            if (this.language != null) sb.AppendFormat(";{0}", this.language);
            sb.AppendFormat(":{0}", this.text);
            return sb.ToString();
        }
    }

    #endregion Time-zone Properties

    #region Relationship Component Properties

    /// <summary>
    /// Defines an "Attendee" within a calendar component
    /// </summary>
    [DataContract]
    public class ATTENDEE : IATTENDEE, IEquatable<ATTENDEE>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the current Attendee
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Address of the Current Attendee
        /// </summary>
        [DataMember]
        public URI Address { get; set; }

        /// <summary>
        /// Calendar User Type of the Attendee
        /// </summary>
        [DataMember]
        public CUTYPE CalendarUserType { get; set; }

        /// <summary>
        /// Membership of the Attendee
        /// </summary>
        [DataMember]
        public MEMBER Member { get; set; }

        /// <summary>
        /// Participation Role of the Attendee
        /// </summary>
        [DataMember]
        public ROLE Role { get; set; }

        /// <summary>
        /// Participation Status of the Attendee
        /// </summary>
        [DataMember]
        public PARTSTAT Participation { get; set; }

        /// <summary>
        ///  Rapid Serial Visual Presentation expectation: will be or not
        /// </summary>
        [DataMember]
        public BOOLEAN Rsvp { get; set; }

        /// <summary>
        /// Specifies the Delegate for current Attendee
        /// </summary>
        [DataMember]
        public DELEGATE Delegatee { get; set; }

        /// <summary>
        /// Specifies the Delegator to the current Attendee
        /// </summary>
        [DataMember]
        public DELEGATE Delegator { get; set; }

        /// <summary>
        /// Specifyes the company or community that sends the Attendee
        /// </summary>
        [DataMember]
        public URI SentBy { get; set; }

        /// <summary>
        /// Common name of the Attendee
        /// </summary>
        [DataMember]
        public string CN { get; set; }

        /// <summary>
        /// Specifies a reference to a directory entry associated with the current Attendee
        /// </summary>
        [DataMember]
        public URI Directory { get; set; }

        /// <summary>
        /// Native Language of the current Attendee
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        public ATTENDEE()
        {
            this.Address = null;
            this.CalendarUserType = CUTYPE.UNKNOWN;
            this.Member = null;
            this.Role = ROLE.UNKNOWN;
            this.Participation = PARTSTAT.UNKNOWN;
            this.Rsvp = BOOLEAN.UNKNOWN;
            this.Delegatee = null;
            this.Delegator = null;
            this.SentBy = null;
            this.CN = null;
            this.Directory = null;
        }

        /// <summary>
        /// Constructor, defining all parameters defining the current Attendee for domestical Puposes (no language specification)
        /// </summary>
        /// <param name="address">Address of the Current Attendee</param>
        /// <param name="cutype">Calendat User Type of the Attendee</param>
        /// <param name="member">Membership type of the Attendee</param>
        /// <param name="role">Participation Role of the Attendee</param>
        /// <param name="partstat">Participation Status of the attendee</param>
        /// <param name="rsvp"> Rapid serial visual presentation expectation</param>
        /// <param name="delegatee">Delegatee for the current Attendee</param>
        /// <param name="delegator">Delegation of the current Attendee</param>
        /// <param name="sentby">Organisation sendin the Current Attendee</param>
        /// <param name="name">Name of the current Attendee</param>
        /// <param name="dir">Reference to a directory entry associated with the current Attendee</param>
        public ATTENDEE(URI address, CUTYPE cutype = CUTYPE.UNKNOWN, ROLE role = ROLE.UNKNOWN,
            PARTSTAT partstat = PARTSTAT.UNKNOWN, BOOLEAN rsvp = BOOLEAN.UNKNOWN, MEMBER member = null, DELEGATE delegatee = null, DELEGATE delegator = null,
            URI sentby = null, string cname = null, URI dir = null, LANGUAGE language = null)
        {
            this.Address = address;
            this.CalendarUserType = cutype;
            this.Member = member;
            this.Role = role;
            this.Participation = partstat;
            this.Rsvp = rsvp;
            this.Delegatee = delegatee;
            this.Delegator = delegator;
            this.SentBy = sentby;
            this.CN = cname;
            this.Directory = dir;
        }

        /// <summary>
        /// Overloaded To String Method
        /// </summary>
        /// <returns>String representation of the Attendee property in form of "ATTENDEE;UserType;RSVP:TRUE(or FALSE);Delegatee;Delegator;SentBy;CN;Directory;Language:Address"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ATTENDEE");
            if (this.CalendarUserType != CUTYPE.UNKNOWN) sb.AppendFormat(";CUTYPE={0}", this.CalendarUserType);
            if (this.Member != null) sb.AppendFormat(";{0}", this.Member);
            if (this.Role != default(ROLE)) sb.AppendFormat(";ROLE={0}", this.Role);
            if (this.Participation != default(PARTSTAT)) sb.AppendFormat(";PARTSTAT={0}", this.Participation);
            if (this.Rsvp != BOOLEAN.UNKNOWN) sb.AppendFormat(";RSVP={0}", this.Rsvp);
            if (this.Delegatee != null) sb.AppendFormat(";DELEGATED-TO={0}", this.Delegatee);
            if (this.Delegator != null) sb.AppendFormat(";DELEGATED-FROM={0}", this.Delegator);
            if (this.SentBy != null) sb.AppendFormat("SENT-BY=\"mailto:{0}\"", this.SentBy);
            if (!string.IsNullOrEmpty(this.CN)) sb.AppendFormat(";CN={0}", this.CN);
            if (this.Directory != null) sb.AppendFormat(";DIR={0}", this.Directory);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":mailto:{0}", this.Address);
            return sb.ToString();
        }

        public bool Equals(ATTENDEE other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as ATTENDEE);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(ATTENDEE a, ATTENDEE b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(ATTENDEE a, ATTENDEE b)
        {
            if ((object)a == null || (object)b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Defines the Organizer for Calendar Component
    /// </summary>
    [DataContract]
    public class ORGANIZER : IORGANIZER, IEquatable<ORGANIZER>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the Organizer
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Address of an Organizer
        /// </summary>
        [DataMember]
        public URI Address { get; set; }

        /// <summary>
        /// Common or Display Name associated with "Organizer"
        /// </summary>
        [DataMember]
        public string CN { get; set; }

        /// <summary>
        /// Directory information associated with "Organizer"
        /// </summary>
        [DataMember]
        public URI Directory { get; set; }

        /// <summary>
        /// Specifies another calendar user that is acting on behalf of the Organizer
        /// </summary>
        [DataMember]
        public URI SentBy { get; set; }

        /// <summary>
        /// Native Language of the Organizer
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        public ORGANIZER()
        {
            this.Address = null;
            this.SentBy = null;
            this.CN = string.Empty;
            this.Directory = null;
        }

        public ORGANIZER(IORGANIZER organizer, string id = null)
        {
            this.Id = id;
            this.Address = organizer.Address;
            this.CN = organizer.CN;
            this.Directory = organizer.Directory;
            this.SentBy = organizer.SentBy;
            this.Language = organizer.Language;
        }

        public ORGANIZER(URI address, URI sentby, string cname = null, URI dir = null, LANGUAGE language = null)
        {
            this.Address = address;
            this.SentBy = sentby;
            this.CN = cname;
            this.Directory = dir;
            this.Language = language;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String Representation of the Organizer oroperty in form of "ORGANIZER;CN;Directory;SentBy;Language:Address"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ORGANIZER");
            if (this.CN != null) sb.AppendFormat(";CN={0}", this.CN);
            if (this.Directory != null) sb.AppendFormat(";{0}", this.Directory);
            if (this.SentBy != null) sb.AppendFormat(";SENT-BY=\"mailto:{0}\"", this.SentBy);
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":mailto:{0}", this.Address);
            return sb.ToString();
        }

        public bool Equals(ORGANIZER other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as ORGANIZER);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(ORGANIZER a, ORGANIZER b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(ORGANIZER a, ORGANIZER b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Identifies a specific instance of a recurring "VEVENT", "VTODO", "VJOURNAL" calendar component
    /// </summary>
    [DataContract]
    public class RECURRENCE_ID : IRECURRENCE_ID, IEquatable<RECURRENCE_ID>, IContainsKey<string>
    {
        /// <summary>
        /// Original date time: settable only once
        /// </summary>
        private DATE_TIME value;

        /// <summary>
        /// ID of the Recurrence
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Time when the original recurrence instance would occur
        /// </summary>
        [DataMember]
        public DATE_TIME Value
        {
            get { return value; }
            set
            {
                this.value = value;
                this.TimeZoneId = value.TimeZoneId;
            }
        }

        /// <summary>
        /// ID of the  current Time Zone
        /// </summary>
        [DataMember]
        public TZID TimeZoneId { get; set; }

        /// <summary>
        /// Effective Range of te recurrentce instances from the instance specified by the "RECURRENCE_ID"
        /// </summary>
        [DataMember]
        public RANGE Range { get; set; }

        public RECURRENCE_ID()
        {
            this.Value = default(DATE_TIME);
            this.TimeZoneId = null;
            this.Range = RANGE.UNKNOWN;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"> Gets or sets the Time when the original recurrence instance would occur</param>
        /// <param name="tzid">ID of the  current Time Zone</param>
        public RECURRENCE_ID(DATE_TIME value, RANGE range = RANGE.THISANDFUTURE)
        {
            this.Value = value;
            this.Range = range;
        }

        /// <summary>
        /// Overloaded ToStringMethod
        /// </summary>
        /// <returns>String representation of the Recurrence property in form of "RECURRENCE-ID;TimeZoneId;Range:DateTime"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RECURRENCE-ID");
            if (this.TimeZoneId != null) sb.AppendFormat(";{0}", this.TimeZoneId);
            if (this.Range != RANGE.UNKNOWN) sb.AppendFormat(";RANGE={0}", this.Range);
            sb.AppendFormat(":{0}", this.Value);
            return sb.ToString();
        }

        public bool Equals(RECURRENCE_ID other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as RECURRENCE_ID);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(RECURRENCE_ID a, RECURRENCE_ID b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(RECURRENCE_ID a, RECURRENCE_ID b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Represents the relationship or reference between one calendar component and another
    /// </summary>
    [DataContract]
    public class RELATEDTO : IRELATEDTO, IEquatable<RELATEDTO>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the Relationship or Reference between one calendar component and another
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Description of the relationship or reference to another Calendar component
        /// </summary>
        [DataMember]
        public string Reference { get; set; }

        /// <summary>
        /// Type of the relationship: dafault - PARENT, can be: CHILD and SIBLING
        /// </summary>
        [DataMember]
        public RELTYPE RelationshipType { get; set; }

        public RELATEDTO()
        {
            this.Reference = null;
            this.RelationshipType = RELTYPE.UNKNOWN;
        }

        /// <summary>
        /// Constructor specifying the Text and Type properties of the Related_To property
        /// </summary>
        /// <param name="text">Description of the relationship or reference to another Calendar component</param>
        /// <param name="type"> Type of the relationship: dafault - PARENT, can be: CHILD and SIBLING</param>
        public RELATEDTO(string text, RELTYPE type = RELTYPE.UNKNOWN)
        {
            this.Reference = text;
            this.RelationshipType = type;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String Representation of the Related_To property in form of "RELATED-TO;Relationship:Text"</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Reference)) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("RELATED-TO");
            if (this.RelationshipType != RELTYPE.UNKNOWN) sb.AppendFormat(";{0}", this.RelationshipType);
            sb.AppendFormat(":{0}", this.Reference);
            return sb.ToString();
        }

        public bool Equals(RELATEDTO other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as RELATEDTO);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(RELATEDTO a, RELATEDTO b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(RELATEDTO a, RELATEDTO b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    #endregion Relationship Component Properties

    #region Recurrence Component properties

    /// <summary>
    /// Defines the list of DATE-TIME exceptions for recurring events, to-dos, entries, or time zone definitions
    /// </summary>
    [DataContract]
    public class EXDATE : IEXDATE, IEquatable<EXDATE>, IContainsKey<string>
    {
        private TZID tzid;

        /// <summary>
        ///  ID of the Date-Time Exception
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Set of Dates of exceptions in a Recurrence
        /// </summary>
        [DataMember]
        public List<DATE_TIME> DateTimes { get; set; }

        public TZID TimeZoneId
        {
            get { return this.tzid; }
            set { this.tzid = value; }
        }

        /// <summary>
        /// Format of the Exception Date
        /// </summary>
        [DataMember]
        public VALUE ValueType { get; set; }

        public EXDATE()
        {
            this.DateTimes = new List<DATE_TIME>();
            this.TimeZoneId = null;
            this.ValueType = VALUE.UNKNOWN;
        }

        /// <summary>
        /// Constructor based on the set of Dates of Recurrence Exceptions and ID of the Time Zone
        /// </summary>
        /// <param name="values">Set of Dates of Recurrence Exceptions</param>
        /// <param name="tzid"></param>
        public EXDATE(List<DATE_TIME> values, TZID tzid, VALUE value_type = VALUE.UNKNOWN)
        {
            this.DateTimes = values;
            this.TimeZoneId = tzid;
            this.ValueType = value_type;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String representation of the EXDATE property in form of "EXDATE;TimeZone:comma splited dates of exctptions"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("EXDATE");
            if (this.ValueType != VALUE.UNKNOWN) sb.AppendFormat(";VALUE={1}", this.ValueType);
            else if (this.tzid != null) sb.AppendFormat(";{0}", this.tzid);
            sb.Append(":"); var last = this.DateTimes.Last();
            foreach (var datetime in this.DateTimes)
            {
                if (datetime != last) sb.AppendFormat("{0}, ", datetime);
                else sb.AppendFormat("{0}", datetime);
            }
            return sb.ToString();
        }

        public bool Equals(EXDATE other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as EXDATE);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(EXDATE a, EXDATE b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(EXDATE a, EXDATE b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// Defines the list of DATE-TIME values for recurring events, to-dos, journal entries or time-zone definitions
    /// </summary>
    [DataContract]
    public class RDATE : IRDATE, IEquatable<RDATE>, IContainsKey<string>
    {
        private TZID tzid;
        private List<DATE_TIME> datetimes;
        private List<PERIOD> periods;

        /// <summary>
        /// ID of the list of DATE-TIME values for recurring events, to-dos, journal entries or time-zone definitions
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// List of DATE-TIME values for recurring events, to-dos, journal entries or time-zone definitions
        /// </summary>
        [DataMember]
        public List<DATE_TIME> DateTimes
        {
            get { return this.datetimes; }
            set
            {
                if (!this.periods.NullOrEmpty() && !value.NullOrEmpty()) throw new ArgumentException("Date times and periods are mutually exclusive");
                this.datetimes = value;
            }
        }

        /// <summary>
        /// List of Periods between recurring events, to-dos, journal entries or time-zone definitions
        /// </summary>
        [DataMember]
        public List<PERIOD> Periods
        {
            get { return this.periods; }
            set
            {
                //if (!this.datetimes.NullOrEmpty() && !value.NullOrEmpty()) throw new ArgumentException("Date times and periods are mutually exclusive");
                this.periods = value;
            }
        }

        /// <summary>
        /// ID of the current time zone
        /// </summary>
        [DataMember]
        public TZID TimeZoneId
        {
            get { return this.tzid; }
            set { this.tzid = (TZID)value; }
        }

        /// <summary>
        /// Mode of the recurrence: events, to-dos, journal entries or time-zone definitions
        /// </summary>
        [DataMember]
        public VALUE ValueType { get; set; }

        public RDATE()
        {
            this.DateTimes = new List<DATE_TIME>();
            this.TimeZoneId = null;
            this.Periods = new List<PERIOD>();
            this.ValueType = VALUE.UNKNOWN;
        }

        /// <summary>
        /// Constructor based on the list of DATE-TIME values and the ID of the current Time Zone
        /// </summary>
        /// <param name="values">List of DATE-TIME values for recurring events, to-dos, journal entries or time-zone definitions</param>
        /// <param name="tzid">ID of the current Time Zone</param>
        public RDATE(List<DATE_TIME> values, TZID tzid = null, VALUE value_type = VALUE.UNKNOWN)
        {
            this.DateTimes = values;
            this.TimeZoneId = tzid;
            this.Periods = new List<PERIOD>();
            this.ValueType = value_type;
        }

        /// <summary>
        /// Constructor based on the list of periods and the ID of currtnt time zone
        /// </summary>
        /// <param name="periods">List of Periods between recurring events, to-dos, journal entries or time-zone definitions</param>
        /// <param name="tzid">ID of the current Time Zone</param>
        public RDATE(List<PERIOD> periods, TZID tzid = null, VALUE value_type = VALUE.UNKNOWN)
        {
            this.DateTimes = new List<DATE_TIME>();
            this.Periods = periods;
            this.TimeZoneId = tzid;
            this.ValueType = value_type;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String representation of the RDATE property in form of "RDATE;VALUE=Mode;TimeZoneId:coma separated list of DATE-TIME values(or Periods)"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("RDATE");
            if (this.ValueType != VALUE.UNKNOWN) sb.AppendFormat(";VALUE={0}", this.ValueType);
            else if (this.TimeZoneId != null) sb.AppendFormat(";{0}", this.TimeZoneId);
            sb.AppendFormat(":");
            if (!this.datetimes.NullOrEmpty())
            {
                var last = this.datetimes.Last();
                foreach (var datetime in this.datetimes)
                {
                    if (datetime != last) sb.AppendFormat("{0}, ", datetime);
                    else sb.AppendFormat("{0}", datetime);
                }
            }
            else if (!this.periods.NullOrEmpty())
            {
                var last = this.periods.Last();
                foreach (var period in this.Periods)
                {
                    if (period != last) sb.AppendFormat("{0}, ", period);
                    else sb.AppendFormat("{0}", period);
                }
            }
            return sb.ToString();
        }

        public bool Equals(RDATE other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as RDATE);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(RDATE a, RDATE b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(RDATE a, RDATE b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    #endregion Recurrence Component properties

    #region Alarm Component Properties

    /// <summary>
    /// Specifies, when the alarm will trigger
    /// </summary>
    [DataContract]
    public class TRIGGER : ITRIGGER, IEquatable<TRIGGER>, IContainsKey<string>
    {
        private TriggerType type;

        /// <summary>
        /// ID of the trigger
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Duration between two alarm actions
        /// </summary>
        [DataMember]
        public DURATION Duration { get; set; }

        /// <summary>
        /// Date-Time, when the Alarm action must be invoked
        /// </summary>
        [DataMember]
        public DATE_TIME DateTime { get; set; }

        /// <summary>
        /// Action value mode, for example "AUDIO", "DISPLAY", "EMAIL"
        /// </summary>
        [DataMember]
        public VALUE ValueType { get; set; }

        [DataMember]
        public RELATED Related { get; set; }

        public TRIGGER()
        {
        }

        public TRIGGER(DURATION duration, RELATED related = RELATED.UNKNOWN, VALUE value_type = VALUE.UNKNOWN)
        {
            this.Duration = duration;
            this.Related = related;
            this.ValueType = value_type;
            this.type = TriggerType.Related;
        }

        public TRIGGER(DATE_TIME datetime, VALUE vformat = VALUE.UNKNOWN)
        {
            this.ValueType = vformat;
            this.DateTime = default(DATE_TIME);
            this.type = TriggerType.Absolute;
        }

        /// <summary>
        /// Overloaded ToString Method
        /// </summary>
        /// <returns>String Representation of the Trigger property in Form of "TRIGGER;VALUE=Mode:Dutarion(or DateTime)"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("TRIGGER");
            if (this.type == TriggerType.Related)
            {
                if (this.ValueType == VALUE.DURATION) sb.AppendFormat(";VALUE={0}", this.ValueType);
                else if (this.Related != default(RELATED)) sb.AppendFormat(";RELATED={0}", this.Related);
                sb.AppendFormat(":{0}", this.Duration);
            }
            else
            {
                if (this.ValueType == VALUE.DATE_TIME) sb.AppendFormat(";VALUE={0}", this.ValueType);
                sb.AppendFormat(":{0}", this.DateTime);
            }
            return sb.ToString();
        }

        public bool Equals(TRIGGER other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as TRIGGER);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(TRIGGER a, TRIGGER b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(TRIGGER a, TRIGGER b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    #endregion Alarm Component Properties

    #region Miscellaneous Component Properties

    /// <summary>
    /// Internet Assigned Numbers Authority properties
    /// </summary>
    /// <typeparam name="TValue">Specific Value Type</typeparam>
    [DataContract]
    [KnownType(typeof(IANA_PROPERTY))]
    [KnownType(typeof(X_PROPERTY))]
    [KnownType(typeof(TZID))]
    [KnownType(typeof(FMTTYPE))]
    [KnownType(typeof(LANGUAGE))]
    [KnownType(typeof(DELEGATE))]
    [KnownType(typeof(MEMBER))]
    public abstract class MISC_PROPERTY<TValue> : IMISC_PROPERTY<TValue>
    {
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The value assigned to the IANA-Property
        /// </summary>
        [DataMember]
        public TValue Value { get; set; }

        /// <summary>
        /// List of any nescessary parameters
        /// </summary>
        [DataMember]
        [Ignore]
        public List<IPARAMETER> Parameters { get; set; }

        [DataMember]
        public VALUE ValueType { get; set; }

        public MISC_PROPERTY()
        {
        }

        /// <summary>
        /// Constructor based on the value of the IANA-Property
        /// </summary>
        /// <param name="value"> The value assigned to the IANA-Property</param>
        public MISC_PROPERTY(string name, TValue value, List<IPARAMETER> parameters)
        {
            this.Name = name;
            this.Value = value;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String Representation of the IANA-Property in form of "Key;semicolon separated Parameters:value"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Name);
            foreach (var parameter in this.Parameters) sb.AppendFormat(";{0}", parameter);
            if (this.ValueType == VALUE.UNKNOWN) sb.AppendFormat(":{0}", this.Value);
            else sb.AppendFormat("VALUE={0}:{1}", this.ValueType, this.Value);
            return sb.ToString();
        }
    }

    [DataContract]
    public class IANA_PROPERTY : MISC_PROPERTY<object>
    {
        public IANA_PROPERTY()
            : base()
        {
        }

        public IANA_PROPERTY(string name, object value, List<IPARAMETER> parameters)
            : base(name, value, parameters)
        {
        }
    }

    [DataContract]
    public class X_PROPERTY : MISC_PROPERTY<object>
    {
        public X_PROPERTY()
            : base()
        {
        }

        public X_PROPERTY(string name, object value, List<IPARAMETER> parameters)
            : base(name, value, parameters)
        {
        }
    }

    [DataContract]
    public struct STATCODE : ISTATCODE, IEquatable<STATCODE>, IComparable<STATCODE>
    {
        private uint l1, l2, l3;

        [DataMember]
        public uint L1
        {
            get
            {
                return l1;
            }
            set
            {
                l1 = value;
            }
        }

        [DataMember]
        public uint L2
        {
            get
            {
                return l2;
            }
            set
            {
                l2 = value;
            }
        }

        [DataMember]
        public uint L3
        {
            get
            {
                return l3;
            }
            set
            {
                l3 = value;
            }
        }

        public STATCODE(uint l1, uint l2 = 0, uint l3 = 0)
        {
            this.l1 = l1;
            this.l2 = l2;
            this.l3 = l3;
        }

        public bool Equals(STATCODE other)
        {
            return this.l1 == other.L2 && this.l2 == other.L2 && this.l3 == other.L3;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals((STATCODE)obj);
        }

        public override int GetHashCode()
        {
            return this.l1.GetHashCode() ^ this.l2.GetHashCode() ^ this.l3.GetHashCode();
        }

        public int CompareTo(STATCODE other)
        {
            return this.l1.CompareTo(other.L1) +
                this.l2.CompareTo(other.L2) +
                this.l3.CompareTo(other.L3);
        }

        public override string ToString()
        {
            return (this.l3 != 0) ?
                string.Format("{0:D1}.{1:D1}.{2:D1}", this.l1, this.l2, this.l3) :
                string.Format("{0:D1}.{1:D1}", this.l1, this.l2);
        }

        public static bool operator ==(STATCODE a, STATCODE b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(STATCODE a, STATCODE b)
        {
            //if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }

        public static bool operator <(STATCODE a, STATCODE b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(STATCODE a, STATCODE b)
        {
            return a.CompareTo(b) > 0;
        }
    }

    /// <summary>
    /// Defines the status code returned for a scheduling request
    /// </summary>
    [DataContract]
    public class REQUEST_STATUS : IREQUEST_STATUS, IEquatable<REQUEST_STATUS>, IContainsKey<string>
    {
        /// <summary>
        /// ID of the status code
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Status code returned for a scheduling request
        /// </summary>
        [DataMember]
        public ISTATCODE Code { get; set; }

        /// <summary>
        /// Description of the status (what does the code mean)
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Exception data, received with current request
        /// </summary>
        [DataMember]
        public string ExceptionData { get; set; }

        /// <summary>
        /// Language for current request
        /// </summary>
        [DataMember]
        public LANGUAGE Language { get; set; }

        public REQUEST_STATUS()
        {
            this.Code = null;
            this.Description = null;
            this.ExceptionData = null;
            this.Language = null;
        }

        /// <summary>
        /// Constructor, specifying the status code, description, exception data and the language
        /// </summary>
        /// <param name="code">Status code returned for a scheduling request</param>
        /// <param name="description">Description of the status (what does the code mean)</param>
        /// <param name="exception">Exception data, received with current request</param>
        /// <param name="language">Language used for the request</param>
        public REQUEST_STATUS(STATCODE code, string description, string exception = null, LANGUAGE language = null)
        {
            this.Code = code;
            this.Description = description;
            this.ExceptionData = exception;
            this.Language = language;
        }

        /// <summary>
        /// Overloaded ToString method
        /// </summary>
        /// <returns>String representation of the REQUEST_STATUS property in form of "REQUEST-STQTUS;Lanquage:Code;Description;ExceptionData"</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("REQUEST-STATUS");
            if (this.Language != null) sb.AppendFormat(";{0}", this.Language);
            sb.AppendFormat(":{0}", this.Code);
            sb.AppendFormat(";{0}", this.Description.EscapeStrings());
            if (!string.IsNullOrEmpty(this.ExceptionData)) sb.AppendFormat(";{0}", this.ExceptionData);
            return sb.ToString();
        }

        public bool Equals(REQUEST_STATUS other)
        {
            if (other == null) return false;
            return this.Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.Equals(obj as REQUEST_STATUS);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(REQUEST_STATUS a, REQUEST_STATUS b)
        {
            if ((object)a == null || (object)b == null) return object.Equals(a, b);
            return a.Equals(b);
        }

        public static bool operator !=(REQUEST_STATUS a, REQUEST_STATUS b)
        {
            if (a == null || b == null) return !object.Equals(a, b);
            return !a.Equals(b);
        }
    }

    #endregion Miscellaneous Component Properties
}