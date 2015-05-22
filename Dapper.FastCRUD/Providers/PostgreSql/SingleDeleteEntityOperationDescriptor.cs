﻿namespace Dapper.FastCrud.Providers.PostgreSql
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Class for Dapper extensions
    /// </summary>
    internal class SingleDeleteEntityOperationDescriptor<TEntity> : EntityOperationDescriptor<EntityDescriptor<TEntity>, TEntity>, ISingleDeleteEntityOperationDescriptor<TEntity>
    {
        private readonly string _sqlQuery;

        public SingleDeleteEntityOperationDescriptor(EntityDescriptor<TEntity> entityDescriptor)
            : base(entityDescriptor)
        {
             _sqlQuery = string.Format(
            CultureInfo.InvariantCulture,
            "DELETE FROM {0} WHERE {1}",
            this.EntityDescriptor.TableName,
            string.Join(
                " and ",
                this.EntityDescriptor.KeyPropertyDescriptors.Select(
                    (propInfo, index) =>
                        string.Format(CultureInfo.InvariantCulture, "{0}=@{1}", propInfo.Name, propInfo.Name))));
        }

        public bool Execute(
            IDbConnection connection,
            TEntity keyEntity,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null)
        {

            return connection.Execute(
                _sqlQuery,
                keyEntity,
                transaction: transaction,
                commandTimeout: (int?)commandTimeout?.TotalSeconds) > 0;
        }
    }
}