﻿using Amazon;
using Amazon.DynamoDBv2;
using EasyDynamo.Abstractions;
using EasyDynamo.Exceptions;
using EasyDynamo.Tools.Validators;
using System;

namespace EasyDynamo.Builders
{
    public class DynamoContextOptionsBuilder
    {
        private readonly IDynamoContextOptions options;

        protected internal DynamoContextOptionsBuilder(IDynamoContextOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Adds a specific name for the table corresponding to the given entity.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamoContextOptionsBuilder UseTableName<TEntity>(string tableName) 
            where TEntity : class, new()
        {
            InputValidator.ThrowIfNullOrWhitespace(tableName);

            this.options.UseTableName<TEntity>(tableName);

            return this;
        }

        /// <summary>
        /// Adds a specific access key to the dynamo client's credentials
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamoContextOptionsBuilder UseAccessKeyId(string accessKey)
        {
            InputValidator.ThrowIfNullOrWhitespace(accessKey);

            this.options.AccessKeyId = accessKey;

            return this;
        }

        /// <summary>
        /// Adds a specific access secret to the dynamo client's credentials
        /// </summary>
        /// <exception cref="DynamoContextConfigurationException"></exception>
        public DynamoContextOptionsBuilder UseSecretAccessKey(string accessSecret)
        {
            InputValidator.ThrowIfNullOrWhitespace(accessSecret);

            this.options.SecretAccessKey = accessSecret;

            return this;
        }

        /// <summary>
        /// Use a local instance of a dynamoDb on a given service url. For example: "http://localhost:8013".
        /// </summary>
        /// <param name="serviceUrl">Required parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamoContextOptionsBuilder UseLocalMode(string serviceUrl)
        {
            InputValidator.ThrowIfNullOrWhitespace(
                serviceUrl, $"{nameof(serviceUrl)} must be provided.");

            options.ServiceUrl = serviceUrl ?? options.ServiceUrl;

            return this;
        }

        /// <summary>
        /// Adds a specific service url to the configuration. For example: "http://localhost:8013".
        /// </summary>
        /// <param name="serviceUrl">Required parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamoContextOptionsBuilder UseServiceUrl(string serviceUrl)
        {
            InputValidator.ThrowIfNullOrWhitespace(
                serviceUrl, $"{nameof(serviceUrl)} must be provided.");

            this.options.ServiceUrl = serviceUrl;

            return this;
        }

        /// <summary>
        /// Adds a specific region to the configuration.
        /// </summary>
        /// <param name="region">Required parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamoContextOptionsBuilder UseRegionEndpoint(RegionEndpoint region)
        {
            InputValidator.ThrowIfNull(
                region, $"{nameof(region)} must be provided.");

            this.options.RegionEndpoint = region;

            return this;
        }

        /// <summary>
        /// Adds a schema fully supporting 2014 L, M, BOOL, NULL additions.
        /// </summary>
        public DynamoContextOptionsBuilder UseEntryConversionV2()
        {
            this.options.Conversion = DynamoDBEntryConversion.V2;

            return this;
        }
    }
}
