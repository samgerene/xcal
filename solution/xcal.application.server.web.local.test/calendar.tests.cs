﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.ServiceClient.Web;
using reexmonkey.xcal.domain.contracts;
using reexmonkey.xcal.domain.models;
using reexmonkey.xcal.domain.operations;
using reexmonkey.infrastructure.operations.concretes;
using System.Collections.Generic;

namespace reexmonkey.xcal.application.server.web.dev.test
{
    [TestClass]
    public class CalendarServiceUnitTests
    {
        private JsonServiceClient client;
        private GuidKeyGenerator guidkeygen;
        private FPIKeyGenerator<string> fpikeygen;

        public CalendarServiceUnitTests()
        {
            client = new JsonServiceClient(Properties.Settings.Default.test_server);
            guidkeygen = new GuidKeyGenerator();
            fpikeygen = new FPIKeyGenerator<string>
            {
                Owner = Properties.Settings.Default.fpiOwner,
                Authority = Properties.Settings.Default.fpiAuthority,
                Description = Properties.Settings.Default.fpiDescription,
                LanguageId = Properties.Settings.Default.fpiLanguageId
            };
        }

        private void Teardown()
        {
            client.Post(new FlushDatabase { Force = false });
        }

        [TestMethod]
        public void MaintainSingleCalendar()
        {
            this.Teardown();
            var calendar = new VCALENDAR
            {
                Id = this.guidkeygen.GetNextKey(),
                ProdId = this.fpikeygen.GetNextKey(),
                Method = METHOD.PUBLISH
            };

            this.client.Post(new AddCalendar  { Calendar = calendar });

            var retrieved = this.client.Get(new FindCalendar { CalendarId = calendar.Id });
            Assert.AreEqual(retrieved.Calscale, CALSCALE.GREGORIAN);
            Assert.AreEqual(retrieved.ProdId, calendar.ProdId);
            Assert.AreEqual(retrieved, calendar);

            calendar.Method = METHOD.REQUEST;
            calendar.Version = "3.0";
            calendar.Calscale = CALSCALE.HEBREW;
            
            this.client.Put(new UpdateCalendar { Calendar = calendar });
            var updated = this.client.Get(new FindCalendar { CalendarId = calendar.Id });
            Assert.AreEqual(updated.Calscale, CALSCALE.HEBREW);
            Assert.AreEqual(updated.Version, "3.0");
            Assert.AreEqual(updated.Method, METHOD.REQUEST);
            Assert.AreEqual(updated, calendar);

            this.client.Patch(new PatchCalendar { Scale = CALSCALE.JULIAN, CalendarId = calendar.Id });
            var patched = this.client.Get(new FindCalendar { CalendarId = calendar.Id });
            Assert.AreEqual(patched.Calscale, CALSCALE.JULIAN);

            this.client.Delete(new DeleteCalendar { CalendarId = calendar.Id });
            var deleted = this.client.Get(new FindCalendar { CalendarId = calendar.Id });
            Assert.AreEqual(deleted, null);

        }

        [TestMethod]
        public void MaintainMultipleCalendars()
        {
            this.Teardown();
            var max = 5;
            var calendars = new VCALENDAR[max];
            for (int i = 0; i < max; i++)
            {
                calendars[i] = new VCALENDAR 
                {
                    Id = this.guidkeygen.GetNextKey(),
                    ProdId = this.fpikeygen.GetNextKey()
                };
            }

            //customize calendars
            calendars[0].Method = METHOD.PUBLISH;
            calendars[0].Version = "1.0";
            calendars[1].Method = METHOD.REQUEST;
            calendars[1].Version = "1.0";
            calendars[2].Method = METHOD.REFRESH;
            calendars[2].Version = "3.0";
            calendars[3].Method = METHOD.ADD;
            calendars[2].Version = "3.0";
            calendars[4].Method = METHOD.CANCEL;
            calendars[4].Version = "1.0";

            this.client.Post(new AddCalendars { Calendars = calendars.ToList() });
            var keys = calendars.Select(x => x.Id).ToList();

            var retrieved = this.client.Post(new FindCalendars { CalendarIds = keys});
            Assert.AreEqual(retrieved.Count, max);
            Assert.AreEqual(retrieved.Where(x => x.Calscale == CALSCALE.GREGORIAN).Count(), max);
            Assert.AreEqual(retrieved.Where(x => x.ProdId == calendars[0].ProdId).Count(), max);
            Assert.AreEqual(retrieved.Where(x => x.Version == "1.0").Count(), 3);
            Assert.AreEqual(retrieved.Where(x => x.Version == "3.0").FirstOrDefault().Method, METHOD.REFRESH);

            calendars[3].Calscale = CALSCALE.ISLAMIC;
            calendars[4].Version = "2.0";
            this.client.Put(new UpdateCalendars { Calendars = calendars.ToList() });
            retrieved = this.client.Post(new FindCalendars { CalendarIds = keys });
            Assert.AreEqual(retrieved.Where(x => x.Id == keys[3]).FirstOrDefault().Calscale, CALSCALE.ISLAMIC);
            Assert.AreEqual(retrieved.Where(x => x.Id == keys[4]).FirstOrDefault().Version, "2.0");
            Assert.AreEqual(retrieved.Where(x => x.Calscale == CALSCALE.GREGORIAN).Count(), max - 1);
            Assert.AreEqual(retrieved.Where(x => x.Version == "2.0").Count(), max - 3);


            this.client.Patch(new PatchCalendars 
            { 
                Scale = CALSCALE.JULIAN, 
                CalendarIds = new List<string> {keys[0], keys[1], keys[2] } 
            });
            
            retrieved = this.client.Post(new FindCalendars  {CalendarIds = keys });
            Assert.AreEqual(retrieved.Where(x => x.Calscale == CALSCALE.JULIAN).Count(), 3);
            Assert.AreEqual(retrieved.Where(x => x.Id == keys[4]).FirstOrDefault().Calscale, CALSCALE.GREGORIAN);

            retrieved = this.client.Get(new GetCalendars { Page = 1, Size = 2 });
            Assert.AreEqual(retrieved.Count(), 2);

            retrieved = this.client.Get(new GetCalendars { Page = 1, Size = 10 });
            Assert.AreEqual(retrieved.Count(), max >= 10 ? 10 : max);

            retrieved = this.client.Get(new GetCalendars { Page = 2, Size = 5 });
            Assert.AreEqual(retrieved.Count(), max >= 5 ? max % 5: 0);

            retrieved = this.client.Get(new GetCalendars { Page = max + 1, Size = 50 });
            Assert.AreEqual(retrieved.Count(), 0);
        }
    }
}