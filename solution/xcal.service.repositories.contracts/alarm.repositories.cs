﻿using System.Collections.Generic;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using reexmonkey.technical.data.contracts;
using reexmonkey.xcal.domain.models;

namespace reexmonkey.xcal.service.repositories.contracts
{

    #region audio alarm repository

    /// <summary>
    /// Specifies a general interface for a repository of audio alerts
    /// </summary>
    public interface IAudioAlarmRepository :
        IReadRepository<AUDIO_ALARM, string>,
        IWriteRepository<AUDIO_ALARM, string>
    {

        /// <summary>
        /// Populates a sparse audio alarm entity with details from its consitutent entities
        /// </summary>
        /// <param name="dry">The audio alarm entity to be populated</param>
        /// <returns>The populated audio alarm entity</returns>
        AUDIO_ALARM Hydrate(AUDIO_ALARM dry);

        /// <summary>
        /// Populates audio alarm entities with details from respective constituent entities
        /// </summary>
        /// <param name="dry">The sparse audio alarm entities to be populated</param>
        /// <returns>Populated audio alarm entities</returns>
        IEnumerable<AUDIO_ALARM> Hydrate(IEnumerable<AUDIO_ALARM> dry);

        /// <summary>
        /// Depopulates aggregate entities from event
        /// </summary>
        /// <param name="full">The audio alarm entity to depopulate</param>
        /// <returns>Depopulated event</returns>
        AUDIO_ALARM Dehydrate(AUDIO_ALARM full);

        /// <summary>
        /// Depopulates aggregate entities from respective events
        /// </summary>
        /// <param name="full">The audio alarm entities to depopulate</param>
        /// <returns>Depopulated events</returns>
        IEnumerable<AUDIO_ALARM> Dehydrate(IEnumerable<AUDIO_ALARM> full);
    }

    /// <summary>
    /// Specifies an interface for a repository of audio alerts connected to an ORMlite source
    /// </summary>
    public interface IAudioAlarmOrmLiteRepository : IAudioAlarmRepository
    {
        /// <summary>
        /// Gets or sets the connection factory of ORMLite datasources
        /// </summary>
        IDbConnectionFactory DbConnectionFactory { get; set; }
    }


    /// <summary>
    /// Specifies an interface for a repository of audio alerts connected to a NoSQL Redis source
    /// </summary>
    public interface IAudioAlarmRedisRepository : IAudioAlarmRepository
    {
        /// <summary>
        /// Gets or sets the connection factory of ORMLite datasources
        /// </summary>
        IRedisClientsManager RedisClientsManager { get; set; }
    } 

    #endregion

    #region display alarm repository

    /// <summary>
    /// Specifies a general interface for a repository of display alerts
    /// </summary>
    public interface IDisplayAlarmRepository :
        IReadRepository<DISPLAY_ALARM, string>,
        IWriteRepository<DISPLAY_ALARM, string> {}

    /// <summary>
    /// Specifies an interface for a repository of display alerts connected to an ORMlite source
    /// </summary>
    public interface IDisplayAlarmOrmLiteRepository : IDisplayAlarmRepository
    {
        /// <summary>
        /// Gets the connection factory of ORMLite datasources
        /// </summary>
        IDbConnectionFactory DbConnectionFactory { get; set; }
    }


    /// <summary>
    /// Specifies an interface for a repository of display alerts connected to a NoSQL Redis source
    /// </summary>
    public interface IDisplayAlarmRedisRepository : IDisplayAlarmRepository
    {
        /// <summary>
        /// Gets the connection factory of ORMLite datasources
        /// </summary>
        IRedisClientsManager RedisClientsManager { get; set; }
    }

    #endregion

    #region email alarm repository


    /// <summary>
    /// Specifies a general interface for a repository of email alerts
    /// </summary>
    public interface IEmailAlarmRepository :
        IReadRepository<EMAIL_ALARM, string>,
        IWriteRepository<EMAIL_ALARM, string>
    {

        /// <summary>
        /// Populates a sparse email alarm entity with details from its consitutent entities
        /// </summary>
        /// <param name="dry">The sparse email alarm entity to be populated</param>
        /// <returns>The populated event entity</returns>
        EMAIL_ALARM Hydrate(EMAIL_ALARM dry);

        /// <summary>
        /// Populates email alarm entities with details from respective constituent entities
        /// </summary>
        /// <param name="dry">The sparse email alarm entities to be populated</param>
        /// <returns>Populated event entities</returns>
        IEnumerable<EMAIL_ALARM> HydrateAll(IEnumerable<EMAIL_ALARM> dry);

        /// <summary>
        /// Depopulates aggregate entities from email alarm
        /// </summary>
        /// <param name="full">The email alarm entity to depopulate</param>
        /// <returns>Depopulated event</returns>
        EMAIL_ALARM Dehydrate(EMAIL_ALARM full);

        /// <summary>
        /// Depopulates aggregate entities from respective events
        /// </summary>
        /// <param name="full">The audio alarm entities to depopulate</param>
        /// <returns>Depopulated events</returns>
        IEnumerable<EMAIL_ALARM> DehydrateAll(IEnumerable<EMAIL_ALARM> full);

    }

    /// <summary>
    /// Specifies an interface for a repository of email alerts connected to an ORMlite source
    /// </summary>
    public interface IEmailAlarmOrmLiteRepository : IEmailAlarmRepository
    {
        /// <summary>
        /// Gets the connection factory of ORMLite datasources
        /// </summary>
        IDbConnectionFactory DbConnectionFactory { get; set; }
    }


    /// <summary>
    /// Specifies an interface for a repository of email alerts connected to a NoSQL Redis source
    /// </summary>
    public interface IEmailAlarmRedisRepository : IEmailAlarmRepository
    {
        /// <summary>
        /// Gets the connection factory of ORMLite datasources
        /// </summary>
        IRedisClientsManager RedisClientsManager { get; set; }
    }


    #endregion

}
