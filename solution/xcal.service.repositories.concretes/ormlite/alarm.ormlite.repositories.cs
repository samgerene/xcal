﻿using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using ServiceStack.OrmLite;
using reexmonkey.technical.data.contracts;
using reexmonkey.technical.data.concretes.extensions.ormlite;
using reexmonkey.foundation.essentials.contracts;
using reexmonkey.foundation.essentials.concretes;
using reexmonkey.crosscut.operations.concretes;
using reexmonkey.infrastructure.io.concretes;
using reexmonkey.xcal.domain.models;
using reexmonkey.xcal.service.repositories.contracts;
using reexmonkey.xcal.service.repositories.concretes.relations;
using reexmonkey.infrastructure.operations.concretes;
using reexmonkey.infrastructure.operations.contracts;

namespace reexmonkey.xcal.service.repositories.concretes.ormlite
{
    public class AudioAlarmOrmLiteRepository : IAudioAlarmOrmLiteRepository
    {

        private IDbConnection conn;
        private IDbConnectionFactory factory = null;
        private IKeyGenerator<string> keygen;
        private IDbConnection db
        {
            get { return (this.conn) ?? factory.OpenDbConnection(); }
        }

        public IDbConnectionFactory DbConnectionFactory
        {
            get { return this.factory; }
            set
            {
                if (value == null) throw new ArgumentNullException("Null factory");
                this.factory = value;
            }
        }
        public IKeyGenerator<string> KeyGenerator
        {
            get { return this.keygen; }
            set
            {
                if (value == null) throw new ArgumentNullException("KeyGenerator");
                this.keygen = value;
            }
        }
        public AudioAlarmOrmLiteRepository() { }
        public AudioAlarmOrmLiteRepository(IDbConnectionFactory factory)
        {
            this.DbConnectionFactory = factory;
        }
        public AudioAlarmOrmLiteRepository(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            this.conn = connection;
        }

        //cleanup
        public virtual void Dispose()
        {
            if (this.conn != null) this.conn.Dispose();
        }

        public AUDIO_ALARM Find(string key)
        {
            try
            {
                var dry = db.Select<AUDIO_ALARM>(q => q.Id == key).FirstOrDefault();
                return dry != null? this.Hydrate(dry): dry;
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<AUDIO_ALARM> FindAll(IEnumerable<string> keys,  int? skip = null, int? take = null)
        {
            try
            {
                var dry = db.Select<AUDIO_ALARM>(q => Sql.In(q.Id, keys.ToArray()), skip, take);
                return !dry.NullOrEmpty() ? this.Hydrate(dry) : dry;

            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<AUDIO_ALARM> Get(int? skip = null, int? take = null)
        {
            try
            {
                var dry = db.Select<AUDIO_ALARM>(skip, take);
                return !dry.NullOrEmpty() ? this.Hydrate(dry) : dry;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void Save(AUDIO_ALARM entity)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    //Save dry event entity i.a. without related details
                    db.Save(entity, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    try { transaction.Rollback();}
                    catch (Exception) {throw;}
                } 
            }
        }

        public void Erase(string key)
        {
            try
            {
                db.Delete<AUDIO_ALARM>(q => q.Id == key);
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void SaveAll(IEnumerable<AUDIO_ALARM> entities)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    db.SaveAll(entities, transaction);
                    transaction.Commit();
                }
                catch (Exception) 
                {
                    try { transaction.Rollback();}
                    catch (Exception) {throw;}
                } 
            }
        }

        public void Patch(AUDIO_ALARM source, Expression<Func<AUDIO_ALARM, object>> fields, IEnumerable<string> keys = null)
        {
            //1. Get fields slected for patching
            var selection = fields.GetMemberNames();

            //2.Get list of all non-related event details (primitives)
            Expression<Func<AUDIO_ALARM, object>> primitives = x => new
            {
                x.Id,
                x.Action,
                x.Trigger,
                x.Duration,
                Attachment = x.AttachmentBinary
            };

            //3. Get list of selected primitives
            var sprimitives = primitives.GetMemberNames().Intersect(selection, StringComparer.OrdinalIgnoreCase).Distinct(StringComparer.OrdinalIgnoreCase);

            //4. Update matching event primitives
            if (!sprimitives.NullOrEmpty())
            {
                var patchstr = string.Format("f => new {{ {0} }}", string.Join(", ", sprimitives.Select(x => string.Format("f.{0}", x))));
                var patchexpr = patchstr.CompileToExpressionFunc<AUDIO_ALARM, object>(CodeDomLanguage.csharp, Utilities.GetReferencedAssemblyNamesFromEntryAssembly());
                db.UpdateOnly<AUDIO_ALARM, object>(source, patchexpr, q => Sql.In(q.Id, keys.ToArray()));
            }
        }

        public void EraseAll(IEnumerable<string> keys = null)
        {
            try
            {
                if (!keys.NullOrEmpty()) db.Delete<AUDIO_ALARM>(q => Sql.In(q.Id, keys.ToArray()));
                else db.DeleteAll<AUDIO_ALARM>();
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public bool ContainsKey(string key)
        {
            try
            {
                return db.Count<AUDIO_ALARM>(q => q.Id == key) != 0;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public bool ContainsKeys(IEnumerable<string> keys, ExpectationMode mode = ExpectationMode.optimistic)
        {
            try
            {
                var dkeys = keys.Distinct().ToArray();
                if (mode == ExpectationMode.pessimistic || mode == ExpectationMode.unknown)
                    return db.Count<AUDIO_ALARM>(q => Sql.In(q.Id, dkeys)) == dkeys.Count();
                else return db.Count<AUDIO_ALARM>(q => Sql.In(q.Id, dkeys)) != 0;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public AUDIO_ALARM Hydrate(AUDIO_ALARM dry)
        {
            var full = dry;
            try
            {
                var okey = db.SelectParam<AUDIO_ALARM, string>(q => q.Id, p => p.Id == dry.Id).FirstOrDefault();
                if (!string.IsNullOrEmpty(okey))
                {
                    var attachbins = db.Select<ATTACH_BINARY, AUDIO_ALARM, REL_AALARMS_ATTACHBINS>(
                        r => r.AttachmentId,
                        r => r.AlarmId,
                        a => a.Id == okey);
                    if (!attachbins.NullOrEmpty()) full.AttachmentBinary = attachbins.FirstOrDefault();

                    var attachuris = db.Select<ATTACH_URI, AUDIO_ALARM, REL_AALARMS_ATTACHURIS>(
                        r => r.AttachmentId,
                        r => r.AlarmId,
                        a => a.Id == okey);
                    if (!attachuris.NullOrEmpty()) full.AttachmentUri = attachuris.FirstOrDefault();

                }
            }
            catch (InvalidOperationException) { throw; }
            catch (ApplicationException) { throw; }
            catch (Exception) { throw; }

            return full ?? dry;
        }

        public IEnumerable<AUDIO_ALARM> Hydrate(IEnumerable<AUDIO_ALARM> dry)
        {
            List<AUDIO_ALARM> full = null;

            try
            {
                full = dry.ToList();
                var keys = full.Select(q => q.Id).ToArray();
                var okeys = db.SelectParam<AUDIO_ALARM, string>(q => q.Id, p => Sql.In(p.Id, keys));

                if (!okeys.NullOrEmpty())
                {
                    #region 1. retrieve relationships

                    var rattachbins = this.db.Select<REL_AALARMS_ATTACHBINS>(q => Sql.In(q.AlarmId, okeys));
                    var rattachuris = this.db.Select<REL_AALARMS_ATTACHURIS>(q => Sql.In(q.AlarmId, okeys));

                    #endregion

                    #region 2. retrieve secondary entities

                    var attachbins = (!rattachbins.Empty()) ? db.Select<ATTACH_BINARY>(q => Sql.In(q.Id, rattachbins.Select(r => r.AttachmentId).ToList())) : null;
                    var attachuris = (!rattachuris.Empty()) ? db.Select<ATTACH_URI>(q => Sql.In(q.Id, rattachuris.Select(r => r.AttachmentId).ToList())) : null;

                    #endregion

                    #region 3. Use Linq to stitch secondary entities to primary entities

                    full.ForEach(x =>
                    {

                        if (!attachbins.NullOrEmpty())
                        {
                            var xattachbins = from y in attachbins
                                              join r in rattachbins on y.Id equals r.AttachmentId
                                              join a in full on r.AlarmId equals a.Id
                                              where a.Id == x.Id
                                              select y;
                            if (!xattachbins.NullOrEmpty()) x.AttachmentBinary = xattachbins.FirstOrDefault();

                        }

                        if (!attachuris.NullOrEmpty())
                        {
                            var xattachuris = from y in attachuris
                                              join r in rattachuris on y.Id equals r.AttachmentId
                                              join a in full on r.AlarmId equals a.Id
                                              where a.Id == x.Id
                                              select y;
                            if (!xattachuris.NullOrEmpty()) x.AttachmentUri = xattachuris.FirstOrDefault();
                        }

                    });

                    #endregion
                }

            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (ApplicationException) { throw; }
            catch (Exception) { throw; }

            return full ?? dry;
        }

        public AUDIO_ALARM Dehydrate(AUDIO_ALARM full)
        {
            try
            {
                full.AttachmentBinary = null;
            }
            catch (ArgumentNullException) { throw; }
            return full;        
        }

        public IEnumerable<AUDIO_ALARM> Dehydrate(IEnumerable<AUDIO_ALARM> full)
        {
            try
            {
                return full.Select(x => { return this.Dehydrate(x); });

            }
            catch (ArgumentNullException) { throw; }    
        }
    }

    public class DisplayAlarmOrmLiteRepository : IDisplayAlarmOrmLiteRepository
    {

        private IDbConnection conn;
        private IDbConnectionFactory factory = null;
        private int? take = null;
        public IKeyGenerator<string> KeyGenerator { get; set; }

        private IDbConnection db
        {
            get { return (this.conn) ?? factory.OpenDbConnection(); }
        }
        public IDbConnectionFactory DbConnectionFactory
        {
            get { return this.factory; }
            set
            {
                if (value == null) throw new ArgumentNullException("DbConnectionFactory");
                this.factory = value;
            }
        }
        public int? Take
        {
            get { return this.take; }
            set
            {
                if (value == null) throw new ArgumentNullException("Take");
                this.take = value;
            }
        }

        public DisplayAlarmOrmLiteRepository() { }
        public DisplayAlarmOrmLiteRepository(IDbConnectionFactory factory, int? take)
        {
            this.DbConnectionFactory = factory;
            this.Take = take;
            this.conn = this.factory.OpenDbConnection();
        }
        public DisplayAlarmOrmLiteRepository(IDbConnection connection, int? take)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            this.conn = connection;
            this.Take = take;
        }

        //cleanup
        public virtual void Dispose()
        {
            if (this.conn != null) this.conn.Dispose();
        }

        public DISPLAY_ALARM Find(string key)
        {
            try
            {
                return db.Select<DISPLAY_ALARM>(q => q.Id == key).FirstOrDefault();
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<DISPLAY_ALARM> FindAll(IEnumerable<string> keys, int? skip = null, int? take = null)
        {
            try
            {
                return db.Select<DISPLAY_ALARM>(q => Sql.In(q.Id, keys.ToArray()), skip, Take);
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<DISPLAY_ALARM> Get(int? skip = null, int? take = null)
        {
            IEnumerable<DISPLAY_ALARM> dry = null;
            try
            {
                dry = db.Select<DISPLAY_ALARM>(skip, take);
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }

            return dry;
        }

        public void Save(DISPLAY_ALARM entity)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    //Save dry event entity i.a. without related details
                    db.Save(entity, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    try { transaction.Rollback(); }
                    catch (Exception) { throw; }
                }
            }
        }

        public void SaveAll(IEnumerable<DISPLAY_ALARM> entities)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    db.SaveAll(entities, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    try { transaction.Rollback(); }
                    catch (Exception) { throw; }
                }
            }
        }

        public void Patch(DISPLAY_ALARM source, Expression<Func<DISPLAY_ALARM, object>> fields, IEnumerable<string> keys = null)
        {
            //1. Get fields slected for patching
            var selection = fields.GetMemberNames();

            //2.Get list of all non-related event details (primitives)
            Expression<Func<DISPLAY_ALARM, object>> primitives = x => new
            {
                x.Id,
                x.Action,
                x.Trigger,
                x.Duration,
                x.Description
            };

            //3. Get list of selected primitives
            var sprimitives = primitives.GetMemberNames().Intersect(selection, StringComparer.OrdinalIgnoreCase).Distinct(StringComparer.OrdinalIgnoreCase);

            //4. Update matching event primitives
            if (!sprimitives.NullOrEmpty())
            {
                var patchstr = string.Format("f => new {{ {0} }}", string.Join(", ", sprimitives.Select(x => string.Format("f.{0}", x))));
                var patchexpr = patchstr.CompileToExpressionFunc<DISPLAY_ALARM, object>(CodeDomLanguage.csharp, Utilities.GetReferencedAssemblyNamesFromEntryAssembly());
                db.UpdateOnly<DISPLAY_ALARM, object>(source, patchexpr, q => Sql.In(q.Id, keys.ToArray()));
            }
        }

        public void Erase(string key)
        {
            try
            {
                db.Delete<DISPLAY_ALARM>(q => q.Id == key);
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void EraseAll(IEnumerable<string> keys = null)
        {
            try
            {
                if (!keys.NullOrEmpty()) db.Delete<DISPLAY_ALARM>(q => Sql.In(q.Id, keys.ToArray()));
                else db.DeleteAll<DISPLAY_ALARM>();
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public bool ContainsKey(string key)
        {
            try
            {
                return db.Count<DISPLAY_ALARM>(q => q.Id == key) != 0;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public bool ContainsKeys(IEnumerable<string> keys, ExpectationMode mode = ExpectationMode.optimistic)
        {
            try
            {
                var dkeys = keys.Distinct().ToArray();
                if (mode == ExpectationMode.pessimistic || mode == ExpectationMode.unknown)
                    return db.Count<DISPLAY_ALARM>(q => Sql.In(q.Id, dkeys)) == dkeys.Count();
                else return db.Count<DISPLAY_ALARM>(q => Sql.In(q.Id, dkeys)) != 0;

            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }

        }

    }

    public class EmailAlarmOrmLiteRepository: IEmailAlarmOrmLiteRepository
    {
               
        private IDbConnection conn;
        private IDbConnectionFactory factory = null;
        private IDbConnection db
        {
            get { return (this.conn) ?? factory.OpenDbConnection(); }
        }
        public IDbConnectionFactory DbConnectionFactory
        {
            get { return this.factory; }
            set 
            {
                if (value == null) throw new ArgumentNullException("DbConnectionFactory");
                this.factory = value; 
            }
        }
        public IKeyGenerator<string> KeyGenerator { get; set; }
        public EmailAlarmOrmLiteRepository() { }
        public EmailAlarmOrmLiteRepository(IDbConnectionFactory factory)
        {
            this.DbConnectionFactory = factory;
            this.conn = this.factory.OpenDbConnection();
        }
        public EmailAlarmOrmLiteRepository(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            this.conn = connection; 
        }

        //cleanup
        public virtual void Dispose()
        {
            if (this.conn != null) this.conn.Dispose();
        }

        public EMAIL_ALARM Hydrate(EMAIL_ALARM dry)
        {
            EMAIL_ALARM full = null;
            try
            {
                full = db.Select<EMAIL_ALARM>(q => q.Id == dry.Id).FirstOrDefault();
                if (full != null)
                {
                    var attendees = db.Select<ATTENDEE, EMAIL_ALARM, REL_EALARMS_ATTENDEES>(
                        r => r.AttendeeId,
                        r => r.AlarmId,
                        a => a.Id == dry.Id);
                    if (!attendees.NullOrEmpty()) full.Attendees.AddRangeComplement(attendees);

                    var attachbins = db.Select<ATTACH_BINARY, EMAIL_ALARM, REL_EALARMS_ATTACHBINS>(
                        r => r.AttachmentId,
                        r => r.AlarmId,
                        a => a.Id == dry.Id);
                    if (!attachbins.NullOrEmpty()) full.AttachmentBinaries.AddRangeComplement(attachbins);

                    var attachuris = db.Select<ATTACH_URI, EMAIL_ALARM, REL_EALARMS_ATTACHURIS>(
                        r => r.AttachmentId,
                        r => r.AlarmId,
                        a => a.Id == dry.Id);
                    if (!attachuris.NullOrEmpty()) full.AttachmentUris.AddRangeComplement(attachuris);
                    
                }
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }

            return full?? dry;
        }

        public IEnumerable<EMAIL_ALARM> HydrateAll(IEnumerable<EMAIL_ALARM> dry)
        {
            List<EMAIL_ALARM> full = null;
            try
            {
                var keys = dry.Select(q => q.Id).ToArray();
                full = db.Select<EMAIL_ALARM>(q => Sql.In(q.Id, keys));

                //1. retrieve relationships
                if (!full.NullOrEmpty())
                {
                    var rattends = db.Select<REL_EALARMS_ATTENDEES>(q => Sql.In(q.Id, keys));
                    var rattachbins = db.Select<REL_EALARMS_ATTACHBINS>(q => Sql.In(q.Id, keys));
                    var rattachuris = db.Select<REL_EALARMS_ATTACHURIS>(q => Sql.In(q.Id, keys));

                    //2. retrieve secondary entities
                    var attends = (!rattends.Empty()) ? db.Select<ATTENDEE>(q => Sql.In(q.Id, rattends.Select(r => r.AttendeeId).ToArray())) : null;
                    var attachbins = (!rattachbins.Empty()) ? db.Select<ATTACH_BINARY>(q => Sql.In(q.Id, rattachbins.Select(r => r.AttachmentId).ToArray())):null;
                    var attachuris = (!rattachuris.Empty()) ? db.Select<ATTACH_URI>(q => Sql.In(q.Id, rattachuris.Select(r => r.AttachmentId).ToArray())):null;

                    //3. Use Linq to stitch secondary entities to primary entities
                    full.ForEach(x =>
                    {
                        var xattachbins = from y in attachbins 
                                          join r in rattachbins on y.Id equals r.AttachmentId 
                                          join a in dry on r.AlarmId equals a.Id 
                                          where a.Id == x.Id select y;
                        if (!xattachbins.NullOrEmpty()) x.AttachmentBinaries.AddRangeComplement(xattachbins);


                        var xattachuris = from y in attachuris 
                                          join r in rattachuris on y.Id equals r.AttachmentId 
                                          join a in dry on r.AlarmId equals a.Id 
                                          where a.Id == x.Id select y;
                        if (!xattachuris.NullOrEmpty()) x.AttachmentUris.AddRangeComplement(xattachuris);


                        var xattendees = from y in attends 
                                       join r in rattends on y.Id equals r.AlarmId 
                                       join a in dry on r.Id equals a.Id 
                                       where a.Id == x.Id select y;
                        if (!xattendees.NullOrEmpty()) x.Attendees.AddRangeComplement(xattendees);

                    }); 
                }
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }

            return full ?? dry;
        }

        public EMAIL_ALARM Find(string key )
        {
            try
            {
                var dry = db.Select<EMAIL_ALARM>(q => q.Id == key).FirstOrDefault();
                return dry != null ? this.Hydrate(dry) : dry;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<EMAIL_ALARM> FindAll(IEnumerable<string> keys, int? skip = null, int? take = null)
        {
            try
            {
                var dry = db.Select<EMAIL_ALARM>(q => Sql.In(q.Id, keys.ToArray()), skip, take);
                return !dry.NullOrEmpty() ? this.HydrateAll(dry) : dry;
            }
            catch (ArgumentNullException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public IEnumerable<EMAIL_ALARM> Get(int? skip = null, int? take = null)
        {
            try
            {
                var dry = db.Select<EMAIL_ALARM>(skip, take);
                return !dry.NullOrEmpty() ? this.HydrateAll(dry) : dry;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void Save(EMAIL_ALARM entity)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    //Save dry event entity i.a. without related details
                    db.Save(entity, transaction);

                    //1. retrieve entity details
                    var attends = entity.Attendees;
                    var attachbins = entity.AttachmentBinaries;
                    var attachuris = entity.AttachmentUris;

                    //2. save details
                    if (!attends.NullOrEmpty())
                    {
                        db.SaveAll(attends, transaction);
                        var rattends = attends.Select(x => new REL_EALARMS_ATTENDEES { AlarmId = entity.Id, AttendeeId = x.Id });
                        var orattends = db.Select<REL_EALARMS_ATTENDEES>(q => q.Id == entity.Id && Sql.In(q.AttendeeId, attends.Select(x => x.Id).ToArray()));
                        db.SaveAll(!orattends.NullOrEmpty() ? rattends.Except(orattends) : rattends, transaction);
                    }

                    if (!attachbins.NullOrEmpty())
                    {
                        db.SaveAll(attachbins, transaction);
                        var rattachbins = attachbins.Select(x => new REL_EALARMS_ATTACHBINS { AlarmId = entity.Id, AttachmentId = x.Id });
                        var orattachbins = db.Select<REL_EALARMS_ATTACHBINS>(q => q.Id == entity.Id && Sql.In(q.AttachmentId, attachbins.Select(x => x.Id).ToArray()));
                        db.SaveAll(!orattachbins.NullOrEmpty() ? rattachbins.Except(orattachbins) : rattachbins, transaction);
                    }

                    if (!attachuris.NullOrEmpty())
                    {
                        db.SaveAll(attachuris, transaction);
                        var rattachuris = attachuris.Select(x => new REL_EALARMS_ATTACHURIS { AlarmId = entity.Id, AttachmentId = x.Id });
                        var orattachuris = db.Select<REL_EALARMS_ATTACHURIS>(q => q.Id == entity.Id && Sql.In(q.AttachmentId, attachuris.Select(x => x.Id).ToArray()));
                        var rdiffs = rattachuris.Except(orattachuris);
                        db.SaveAll(!orattachuris.NullOrEmpty() ? rattachuris.Except(orattachuris) : rattachuris, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    try { transaction.Rollback(); }
                    catch (Exception) { throw; }
                }
            }
        }

        public void Erase(string key)
        {
            try
            {
                db.Delete<EMAIL_ALARM>(q => q.Id.ToUpper() == key.ToUpper());
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void SaveAll(IEnumerable<EMAIL_ALARM> entities)
        {
            using (var transaction = db.OpenTransaction())
            {
                try
                {
                    db.SaveAll(entities, transaction);

                    //1. retrieve details of events
                    var attends = entities.Where(x => x.Attendees.OfType<ATTENDEE>().Count() > 0).SelectMany(x => x.Attendees.OfType<ATTENDEE>());
                    var attachbins = entities.Where(x => x.AttachmentBinaries.Count() > 0).SelectMany(x => x.AttachmentBinaries);
                    var attachuris = entities.Where(x => x.AttachmentUris.Count() > 0).SelectMany(x => x.AttachmentUris);

                    //2. save details of events
                    if (!attends.NullOrEmpty())
                    {
                        db.SaveAll(attends, transaction);
                        var rattends = entities.Where(x => !x.Attendees.OfType<ATTENDEE>().NullOrEmpty())
                            .SelectMany(a => a.Attendees.OfType<ATTENDEE>().Select(x => new REL_EALARMS_ATTENDEES { Id = a.Id, AttendeeId = x.Id }));
                        var orattends = db.Select<REL_EALARMS_ATTENDEES>(q => Sql.In(q.Id, entities.Select(x => x.Id)) && Sql.In(q.AttendeeId, attends.Select(x => x.Id).ToArray()));
                        db.SaveAll(!orattends.NullOrEmpty() ? rattends.Except(orattends) : rattends, transaction);
                    }

                    if (!attachbins.NullOrEmpty())
                    {
                        db.SaveAll(attachbins, transaction);
                        var rattachbins = entities.Where(x => !x.AttachmentBinaries.NullOrEmpty())
                            .SelectMany(a => a.AttachmentBinaries.Select(x => new REL_EALARMS_ATTACHBINS { Id = a.Id, AttachmentId = x.Id }));
                        var orattachbins = db.Select<REL_EALARMS_ATTACHBINS>(q => Sql.In(q.Id, entities.Select(x => x.Id)) && Sql.In(q.AttachmentId, attachbins.Select(x => x.Id).ToArray()));
                        db.SaveAll(!orattachbins.NullOrEmpty() ? rattachbins.Except(orattachbins) : rattachbins, transaction);
                    }

                    if (!attachuris.NullOrEmpty())
                    {
                        db.SaveAll(attachuris, transaction);
                        var rattachuris = entities.Where(x => !x.AttachmentUris.NullOrEmpty())
                            .SelectMany(a => a.AttachmentUris.Select(x => new REL_EALARMS_ATTACHURIS { Id = a.Id, AttachmentId = x.Id }));
                        var orattachuris = db.Select<REL_EALARMS_ATTACHURIS>(q => Sql.In(q.Id, entities.Select(x => x.Id)) && Sql.In(q.AttachmentId, attachuris.Select(x => x.Id)));
                        db.SaveAll(!orattachuris.NullOrEmpty() ? rattachuris.Except(orattachuris) : rattachuris, transaction);

                    }

                    transaction.Commit();
                }
                catch (Exception) 
                {
                    try { transaction.Rollback(); }
                    catch (Exception) { throw; }
                } 
            }

        }

        public void EraseAll(IEnumerable<string> keys)
        {
            try
            {
                db.Delete<EMAIL_ALARM>(q => Sql.In(q.Id, keys.ToArray()));
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void EraseAll()
        {
            try
            {
                db.DeleteAll<EMAIL_ALARM>();
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public void Patch(EMAIL_ALARM source, Expression<Func<EMAIL_ALARM, object>> fields, IEnumerable<string> keys = null)
        {
            //1. Get fields slected for patching
            var selection = fields.GetMemberNames();

            //2.Get list of all non-related event details (primitives)
            Expression<Func<EMAIL_ALARM, object>> primitives = x => new
            {
                x.Id,
                x.Action,
                x.Trigger,
                x.Duration,
                x.Description,
                x.Summary
            };


            //3.Get list of all related event details (relation)
            Expression<Func<EMAIL_ALARM, object>> relations = x => new
            {
                x.Attendees,
                x.AttachmentBinaries,
                x.AttachmentUris
            };

            //4. Get list of selected relations
            var srelation = relations.GetMemberNames().Intersect(selection, StringComparer.OrdinalIgnoreCase).Distinct(StringComparer.OrdinalIgnoreCase);

            //5. Patch relations
            if (!srelation.NullOrEmpty())
            {
                Expression<Func<EMAIL_ALARM, object>> attendsexpr = y => y.Attendees;
                Expression<Func<EMAIL_ALARM, object>> attachbinsexpr = y => y.AttachmentBinaries;
                Expression<Func<EMAIL_ALARM, object>> attachurisexpr = y => y.AttachmentUris;

                var okeys = (keys != null)
                    ? db.SelectParam<EMAIL_ALARM, string>(q => q.Id, p => Sql.In(p.Id, keys.ToArray())).ToArray()
                    : db.SelectParam<EMAIL_ALARM>(q => q.Id).ToArray();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        if (selection.Contains(attendsexpr.GetMemberName()))
                        {
                            var attends = source.Attendees.OfType<ATTENDEE>();
                            if (!attends.NullOrEmpty())
                            {
                                db.SaveAll(attends, transaction);
                                var rattends = okeys.SelectMany(x => attends.Select(y => new REL_EALARMS_ATTENDEES { Id = this.KeyGenerator.GetNextKey(), AlarmId = x, AttendeeId = y.Id }));
                                var orattends = db.Select<REL_EALARMS_ATTENDEES>(q => Sql.In(q.Id, okeys) && Sql.In(q.AttendeeId, attends.Select(x => x.Id).ToArray()));
                                db.SaveAll(!rattends.NullOrEmpty() ? rattends.Except(orattends) : rattends, transaction);

                            }
                        }

                        //if (selection.Contains(attachsexpr.GetMemberName()))
                        //{
                        //    var attachbins = source.Attachments.OfType<ATTACH_BINARY>();
                        //    if (!attachbins.NullOrEmpty() && !skip)
                        //    {
                        //        db.SaveAll(attachbins, transaction);
                        //        var rattachbins = eventids.SelectMany(x => attachbins.Select(y => new REL_EALARMS_ATTACHBINS { Id = this.KeyGenerator.GetNextKey(), AlarmId = x, AttachmentId = y.Id }));
                        //        var orattachbins = db.Select<REL_EALARMS_ATTACHBINS>(q => Sql.In(q.Id, eventids) && Sql.In(q.AttachmentId, attachbins.Select(x => x.Id).ToArray()));
                        //        db.SaveAll(!rattachbins.NullOrEmpty() ? rattachbins.Except(orattachbins) : rattachbins, transaction);
                        //    }

                        //    var attachuris = source.Attachments.OfType<ATTACH_URI>();
                        //    if (!attachuris.NullOrEmpty() && !skip)
                        //    {
                        //        db.SaveAll(attachuris, transaction);
                        //        var rattachuris = eventids.SelectMany(x => attachuris.Select(y => new REL_EALARMS_ATTACHURIS { Id = this.KeyGenerator.GetNextKey(), AlarmId = x, AttachmentId = y.Id }));
                        //        var orattachuris = db.Select<REL_EALARMS_ATTACHURIS>(q => Sql.In(q.Id, eventids) && Sql.In(q.AttachmentId, attachuris.Select(x => x.Id).ToArray()));
                        //        db.SaveAll(!rattachuris.NullOrEmpty() ? rattachuris.Except(orattachuris) : rattachuris, transaction);
                        //    }
                        //}

                        //transaction.Commit();
                    }
                    catch (Exception)
                    {
                        try { transaction.Rollback(); }
                        catch (Exception) { throw; }
                    }
 
                }
            }


            //6. Get list of selected primitives
            var sprimitives = primitives.GetMemberNames().Intersect(selection, StringComparer.OrdinalIgnoreCase).Distinct(StringComparer.OrdinalIgnoreCase);

            //7. Update matching event primitives
            if (!sprimitives.NullOrEmpty())
            {
                //try
                //{
                //    var patchstr = string.Format("f => new {{ {0} }}", string.Join(", ", sprimitives.Select(x => string.Format("f.{0}", x))));
                //    var patchexpr = patchstr.CompileToExpressionFunc<EMAIL_ALARM, object>(CodeDomLanguage.csharp, Utilities.GetReferencedAssemblyNamesFromEntryAssembly());
                //    db.UpdateOnly<EMAIL_ALARM, object>(source, patchexpr, where);
                //}
                //catch (NotImplementedException) { throw; }
                //catch (System.Security.SecurityException) { throw; }
                //catch (InvalidOperationException) { throw; }
                //catch (Exception) { throw; }
            }
        }

        public bool ContainsKey(string key)
        {
            try
            {
                return db.Count<EMAIL_ALARM>(q => q.Id == key) != 0;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }
        }

        public bool ContainsKeys(IEnumerable<string> keys, ExpectationMode mode = ExpectationMode.optimistic)
        {
            try
            {
                var dkeys = keys.Distinct().ToArray();
                if (mode == ExpectationMode.pessimistic || mode == ExpectationMode.unknown)
                    return db.Count<EMAIL_ALARM>(q => Sql.In(q.Id, dkeys)) == dkeys.Count();
                else return db.Count<EMAIL_ALARM>(q => Sql.In(q.Id, dkeys)) != 0;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception) { throw; }

        }

        public EMAIL_ALARM Dehydrate(EMAIL_ALARM full)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EMAIL_ALARM> Dehydrate(IEnumerable<EMAIL_ALARM> full)
        {
            throw new NotImplementedException();
        }
    }

}