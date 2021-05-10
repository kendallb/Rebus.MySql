﻿using System;
using System.Threading.Tasks;

#pragma warning disable 1998

namespace Rebus.MySql
{
    /// <summary>
    /// Implementation of <see cref="IDbConnectionProvider"/> that uses an async function to retrieve the <see cref="IDbConnection"/>
    /// </summary>
    public class DbConnectionFactoryProvider : IDbConnectionProvider
    {
        readonly Func<Task<IDbConnection>> _connectionFactory;

        /// <summary>
        /// Creates the connection provider to use the specified <paramref name="connectionFactory"/> to create <see cref="IDbConnection"/>s
        /// </summary>
        public DbConnectionFactoryProvider(Func<Task<IDbConnection>> connectionFactory) => _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

        /// <summary>
        /// Gets a nice ready-to-use database connection with an open transaction
        /// </summary>
        public IDbConnection GetConnection()
        {
            IDbConnection connection = null;
            AsyncHelpers.RunSync(async () =>
            {
                connection = await _connectionFactory().ConfigureAwait(false);
            });
            return connection;
        }

        /// <summary>
        /// Gets a nice ready-to-use database connection with an open transaction
        /// </summary>
        public async Task<IDbConnection> GetConnectionAsync() => await _connectionFactory().ConfigureAwait(false);
    }
}
