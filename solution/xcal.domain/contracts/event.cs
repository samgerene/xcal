﻿using System;
using System.Collections.Generic;
using reexjungle.xcal.domain.models;

namespace reexjungle.xcal.domain.contracts
{
    /// <summary>
    /// Specifes a contract for the VEVENT component of the iCalendar Core Object
    /// </summary>
    public interface IEVENT : ICOMPONENT
    {
        string Uid {get; set;}

        DATE_TIME Datestamp { get; set; }

        DATE_TIME Start{get; set;}

        CLASS Classification { get; set; }

        DATE_TIME Created { get; set; }

        DESCRIPTION Description { get; set; }

        GEO Position { get; set; }

        DATE_TIME LastModified { get; set; }

        LOCATION Location { get; set; }

        ORGANIZER Organizer { get; set; }

        PRIORITY Priority { get; set; }

        int Sequence { get; set; }

        STATUS Status { get; set; }

        SUMMARY Summary { get; set; }

        TRANSP Transparency { get; set; }

        URI Url { get; set; }

        RECURRENCE_ID RecurrenceId { get; set; }

        RECUR RecurrenceRule { get; set; }

        DATE_TIME End {get; set;}

        DURATION Duration {get; set;}

        List<ATTACH_BINARY> AttachmentBinaries { get; set; }

        List<ATTACH_URI> AttachmentUris { get; set; }

        List<ATTENDEE> Attendees { get; set; }

        CATEGORIES Categories { get; set; }

        List<COMMENT> Comments { get; set; }

        List<CONTACT> Contacts { get; set; }

        List<EXDATE> ExceptionDates { get; set; }

        List<REQUEST_STATUS> RequestStatuses { get; set; }

        List<RESOURCES> Resources { get; set; }

        List<RELATEDTO> RelatedTos { get; set; }

        List<RDATE> RecurrenceDates { get; set; }

        List<AUDIO_ALARM> AudioAlarms { get; set; }

        List<DISPLAY_ALARM> DisplayAlarms { get; set; }

        List<EMAIL_ALARM> EmailAlarms { get; set; }

        Dictionary<string, IANA_PROPERTY> IANAProperties { get; set; }

        Dictionary<string, X_PROPERTY> XProperties { get; set; }

        List<TEVENT> GenerateRecurrences<TEVENT>() where TEVENT : class, IEVENT, new();
    }



}
