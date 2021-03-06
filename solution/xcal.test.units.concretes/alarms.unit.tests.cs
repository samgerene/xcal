﻿using FizzWare.NBuilder;
using reexjungle.infrastructure.operations.concretes;
using reexjungle.infrastructure.operations.contracts;
using reexjungle.xcal.domain.contracts;
using reexjungle.xcal.domain.models;
using reexjungle.xcal.test.units.contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace reexjungle.xcal.test.units.concretes
{
    public class AlarmUnitTests : IAlarmUnitTests
    {
        public IGuidKeyGenerator KeyGen { get; set; }

        public IPropertiesUnitTests PropertiesUnitTests { get; set; }

        public AlarmUnitTests()
        {
            this.KeyGen = new GuidKeyGenerator();
            this.PropertiesUnitTests = new PropertiesUnitTests();
        }

        public IEnumerable<AUDIO_ALARM> GenerateAudioAlarmsOfSize(int n)
        {
            var dgen = new SequentialGenerator<DateTime> { IncrementDateBy = IncrementDate.Day, Direction = GeneratorDirection.Ascending };
            dgen.StartingWith(new DateTime(2014, 06, 15));

            return Builder<AUDIO_ALARM>.CreateListOfSize(n)
                .All()
                .With(x => x.Id = this.KeyGen.GetNextKey())
                .And(x => x.Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)))
                .And(x => x.Repeat = new RandomGenerator().Next(0, 10))
                .And(x => x.Trigger = new TRIGGER
                {
                    Id = this.KeyGen.GetNextKey(),
                    DateTime = new DATE_TIME(dgen.Generate()),
                    Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)),
                    Related = Pick<RELATED>.RandomItemFrom(new List<RELATED> { RELATED.START, RELATED.END }),
                    ValueType = VALUE.DATE_TIME
                })
                .And(x => x.AttachmentUri = new ATTACH_URI
                {
                    Id = this.KeyGen.GetNextKey(),
                    Content = new URI("http://apes.jungle/music/hohoho.mp3"),
                    FormatType = new FMTTYPE("file", "audio")
                })
                .Build();
        }

        public IEnumerable<DISPLAY_ALARM> GenerateDisplayAlarmsOfSize(int n)
        {
            var dgen = new SequentialGenerator<DateTime> { IncrementDateBy = IncrementDate.Day, Direction = GeneratorDirection.Ascending };
            dgen.StartingWith(new DateTime(2014, 06, 15));

            return Builder<DISPLAY_ALARM>.CreateListOfSize(n)
                .All()
                .With(x => x.Id = this.KeyGen.GetNextKey())
                .And(x => x.Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)))
                .And(x => x.Repeat = new RandomGenerator().Next(0, 10))
                .And(x => x.Description = new DESCRIPTION(new RandomGenerator().Phrase(new RandomGenerator().Next(3, 15))))
                .And(x => x.Trigger = new TRIGGER
                {
                    Id = this.KeyGen.GetNextKey(),
                    DateTime = new DATE_TIME(dgen.Generate()),
                    Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)),
                    Related = Pick<RELATED>.RandomItemFrom(new List<RELATED> { RELATED.START, RELATED.END }),
                    ValueType = VALUE.DATE_TIME
                })
                .Build();
        }

        public IEnumerable<EMAIL_ALARM> GenerateEmailAlarmsOfSize(int n)
        {
            var dgen = new SequentialGenerator<DateTime> { IncrementDateBy = IncrementDate.Day, Direction = GeneratorDirection.Ascending };
            dgen.StartingWith(new DateTime(2014, 06, 15));

            return Builder<EMAIL_ALARM>.CreateListOfSize(n)
                .All()
                .With(x => x.Id = this.KeyGen.GetNextKey())
                .And(x => x.Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)))
                .And(x => x.Repeat = new RandomGenerator().Next(0, 10))
                .And(x => x.Summary = new SUMMARY(new RandomGenerator().Phrase(new RandomGenerator().Next(3, 15))))
                .And(x => x.Description = new DESCRIPTION(new RandomGenerator().Phrase(new RandomGenerator().Next(3, 100))))
                .And(x => x.Trigger = new TRIGGER
                {
                    Id = this.KeyGen.GetNextKey(),
                    DateTime = new DATE_TIME(dgen.Generate()),
                    Duration = new DURATION(0, 0, new RandomGenerator().Next(0, 23), new RandomGenerator().Next(0, 59), new RandomGenerator().Next(0, 59)),
                    Related = Pick<RELATED>.RandomItemFrom(new List<RELATED> { RELATED.START, RELATED.END }),
                    ValueType = VALUE.DATE_TIME
                })
                .And(x => x.AttachmentBinaries = new List<ATTACH_BINARY>
                {
                    new ATTACH_BINARY
                    {
                        Id = this.KeyGen.GetNextKey(),
                        Content = new BINARY(new RandomGenerator().Phrase(new RandomGenerator().Next(5, 20)), ENCODING.BIT8)
                    }
                })
                .And(x => x.Attendees = this.PropertiesUnitTests.GenerateAttendeesOfSize(3).ToList())
                .Build();
        }
    }
}