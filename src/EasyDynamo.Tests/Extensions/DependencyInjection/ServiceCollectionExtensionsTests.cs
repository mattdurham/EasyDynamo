﻿using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using EasyDynamo.Abstractions;
using EasyDynamo.Config;
using EasyDynamo.Exceptions;
using EasyDynamo.Extensions.DependencyInjection;
using EasyDynamo.Tests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyDynamo.Tests.Extensions.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly Mock<IDynamoContextOptions> contextOptionsMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly IServiceCollection services;

        public ServiceCollectionExtensionsTests()
        {
            this.contextOptionsMock = new Mock<IDynamoContextOptions>();
            this.configurationMock = new Mock<IConfiguration>();
            this.services = new ServiceCollection();
        }

        #region AddDynamoContext with only IConfiguration

        [Fact]
        public async Task AddDynamoContext_AddsAwsOptionsToServiceProvider()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                var serviceProvider = this.services.BuildServiceProvider();
                var awsOptions = serviceProvider.GetService<AWSOptions>();

                Assert.NotNull(awsOptions);
            });
        }

        [Fact]
        public async Task AddDynamoContext_InvokesOnConfiguring()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                FakeDynamoContext.OnConfiguringInvoked = false;

                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.True(FakeDynamoContext.OnConfiguringInvoked);
            });
        }

        [Fact]
        public async Task AddDynamoContext_InvokesOnModelCreating()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                FakeDynamoContext.OnModelCreatingInvoked = false;

                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.True(FakeDynamoContext.OnModelCreatingInvoked);
            });
        }

        [Fact]
        public async Task AddDynamoContext_AddsContextToServicesAsSingleton()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.Contains(this.services, s =>
                    s.ImplementationType == typeof(FakeDynamoContext) &&
                    s.Lifetime == ServiceLifetime.Singleton);
            });
        }

        [Fact]
        public async Task AddDynamoContext_AddsDynamoContextToServicesAsSingleton()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IDynamoDBContext) &&
                    s.ImplementationType == typeof(DynamoDBContext) &&
                    s.Lifetime == ServiceLifetime.Singleton);
            });
        }

        [Fact]
        public async Task AddDynamoContext_CloudMode_AddsDynamoClientToServices()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IAmazonDynamoDB) &&
                    s.ImplementationType == typeof(AmazonDynamoDBClient));
            });
        }

        [Fact]
        public async Task AddDynamoContext_LocalMode_AddsDynamoClientToServices()
        {
            this.contextOptionsMock
                .SetupGet(o => o.Profile)
                .Returns("ApplicationDevelopment");
            this.contextOptionsMock
                .SetupGet(o => o.LocalMode)
                .Returns(false);
            this.contextOptionsMock
                .SetupGet(o => o.RegionEndpoint)
                .Returns(RegionEndpoint.APNortheast1);

            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IAmazonDynamoDB) &&
                    s.ImplementationType == typeof(AmazonDynamoDBClient));
            });
        }

        #endregion

        #region AddDynamoContext with ContextOptions and IConfiguration

        [Fact]
        public async Task AddDynamoContext_ValidOptions_AddsAwsOptionsToServiceProvider()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options => 
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                var serviceProvider = this.services.BuildServiceProvider();
                var awsOptions = serviceProvider.GetService<AWSOptions>();

                Assert.NotNull(awsOptions);
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptions_ContextAddedTwice_ThrowsException()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.Throws<DynamoContextConfigurationException>(
                        () => services.AddDynamoContext<FakeDynamoContext>(
                            this.configurationMock.Object,
                            options =>
                            {
                                options.Profile = "ApplicationDevelopment";
                                options.LocalMode = false;
                                options.RegionEndpoint = RegionEndpoint.APNortheast1;
                            }));
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptions_InvokesOnConfiguring()
        {
            await TestRetrier.RetryAsync(() =>
            {
                FakeDynamoContext.OnConfiguringInvoked = false;

                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                services.AddDynamoContext<FakeDynamoContext>(this.configurationMock.Object);

                Assert.True(FakeDynamoContext.OnConfiguringInvoked);
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptions_InvokesOnModelCreating()
        {
            await TestRetrier.RetryAsync(() =>
            {
                FakeDynamoContext.OnModelCreatingInvoked = false;

                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.True(FakeDynamoContext.OnModelCreatingInvoked);
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptions_AddsContextToServicesAsSingleton()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.Contains(this.services, s =>
                    s.ImplementationType == typeof(FakeDynamoContext) &&
                    s.Lifetime == ServiceLifetime.Singleton);
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptions_AddsDynamoContextToServicesAsSingleton()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IDynamoDBContext) &&
                    s.ImplementationType == typeof(DynamoDBContext) &&
                    s.Lifetime == ServiceLifetime.Singleton);
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptionsAndCloudMode_AddsDynamoClientToServices()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = false;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IAmazonDynamoDB) &&
                    s.ImplementationType == typeof(AmazonDynamoDBClient));
            });
        }

        [Fact]
        public async Task AddDynamoContext_ValidOptionsAndLocalMode_AddsDynamoClientToServices()
        {
            await TestRetrier.RetryAsync(() =>
            {
                services.AddDynamoContext<FakeDynamoContext>(
                    this.configurationMock.Object,
                    options =>
                    {
                        options.Profile = "ApplicationDevelopment";
                        options.LocalMode = true;
                        options.RegionEndpoint = RegionEndpoint.APNortheast1;
                    });

                Assert.Contains(this.services, s =>
                    s.ServiceType == typeof(IAmazonDynamoDB) &&
                    s.ImplementationType == typeof(AmazonDynamoDBClient));
            });
        }

        [Fact]
        public async Task AddDynamoContext_OptionsIsNull_ThrowsException()
        {
            await TestRetrier.RetryAsync(() =>
            {
                Assert.Throws<ArgumentNullException>(
                    () => services.AddDynamoContext<FakeDynamoContext>(
                        this.configurationMock.Object,
                        default(Action<DynamoContextOptions>)));
            });
        }

        #endregion
    }
}
