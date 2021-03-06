﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using reexjungle.xcal.domain.models;

namespace reexjungle.xcal.domain.contracts
{

    public interface IOBSERVANCE: ICOMPONENT
    { 
        DATE_TIME Start { get; set; }
        UTC_OFFSET TimeZoneOffsetFrom { get; set; }
        UTC_OFFSET TimeZoneOffsetTo { get; set; }
        RECUR RecurrenceRule { get; set; }
        List<COMMENT> Comments { get; set; }
        List<RDATE> RecurrenceDates { get; set; }
        List<TZNAME> TimeZoneNames { get; set; }    
    }

    public interface ITIMEZONE : ICOMPONENT
    {
        TZID TimeZoneId { get; set; }
        URI Url { get; set; }
        DATE_TIME LastModified { get; set; }
        List<STANDARD> StandardTimes { get; set; }
        List<DAYLIGHT> DaylightTimes { get; set; }
    }




}
