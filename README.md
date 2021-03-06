<p align="center">
  <img src="AspNetCore.AutoHealthCheck.png" alt="AspNetCore.AutoHealthCheck" width="100"/>
</p>

AspNetCore.AutoHealthCheck
====================
- Check automatically \r asp.net core applications with a lot of extensibility !
- Check how defensive your asp.net core application is.

![NuGet](https://img.shields.io/nuget/dt/AspNetCore.AutoHealthCheck.svg)
![NuGet](https://img.shields.io/nuget/v/AspNetCore.AutoHealthCheck.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Open Source Love](https://badges.frapsoft.com/os/v1/open-source.svg?v=102)](https://github.com/ellerbrock/open-source-badge/)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

|Build|Status|CodeFactor|
|------|-------------|-------------|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/3s1txd8lrbvn82v2/branch/master?svg=true)](https://ci.appveyor.com/project/davidrevoledo/aspnetcore-autohealthcheck/branch/master)|[![CodeFactor](https://www.codefactor.io/repository/github/davidrevoledo/aspnetcore.autohealthcheck/badge)](https://www.codefactor.io/repository/github/davidrevoledo/aspnetcore.autohealthcheck)
|dev|[![Build status](https://ci.appveyor.com/api/projects/status/ak0heuv6ckkaoft3?svg=true)](https://ci.appveyor.com/project/davidrevoledo/aspnetcore-autohealthcheck-4hy94)|[![CodeFactor](https://www.codefactor.io/repository/github/davidrevoledo/aspnetcore.autohealthcheck/badge/dev)](https://www.codefactor.io/repository/github/davidrevoledo/aspnetcore.autohealthcheck/overview/dev)

|Package|Status|
|------|-------------|
|AspNetCore.AutoHealthCheck|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck/)|
|AspNetCore.AutoHealthCheck.Abstractions|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck.Abstractions?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.Abstractions/)|
|AspNetCore.AutoHealthCheck.Diagnostics|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck.Diagnostics?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.Diagnostics/)|
|AspNetCore.AutoHealthCheck.ApplicationInsights|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck.ApplicationInsights?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.ApplicationInsights/)|
|AspNetCore.AutoHealthCheck.AzureStorage|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck.AzureStorage?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.AzureStorage/)|
|AspNetCore.AutoHealthCheck.Raygun|[![NuGet Version and Downloads count](https://buildstats.info/nuget/AspNetCore.AutoHealthCheck.Raygun?includePreReleases=true)](https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.Raygun/)|

# Contents

1. [Features](#features)
2. [Installation](#installation)
4. [Usage](#usage)
5. [Customising](#customising)
5. [Extensibility](#extensibility)
6. [Security](#security)
7. [Integrations](#integrations)
8. [What's next?](#next)
8. [License](#license)

## <a name="features"> Features </a>

-  Auto Call all the endpoints that an asp.net core applications expose.
-  Allow configure rules to determine when an endpoint is unhealthy.
-  Plugins to extend the behavior of the http calls. (ie. Headers, QueryParams, and so on).
-  Allow to call custom actions (ie weebhooks) for health resutls.
-  Automatic run on background using HostedServices
-  Full Async support.
-  Custom Probes.
-  Full Security control for health check endpoints.
-  Checks indetification by id.
-  Integrations
     - Microsoft.AspNetCore.Diagnostics.HealthChecks.
     - Application Insights.
     - Azure Storage Account.
     - Raygun.

====================

## <a name="installation"> Installation </a>

Grab the latest AspNetCore.AutoHealthCheck NuGet package and install in the desired package. https://www.nuget.org/packages/AspNetCore.AutoHealthCheck/
```sh
PM > Install-Package AspNetCore.AutoHealthCheck 
NET CLI - dotnet add package AspNetCore.AutoHealthCheck 
paket add AspNetCore.AutoHealthCheck 
```
====================

## <a name="usage"> Usage </a>

In the asp.net core application Startup just need to Add HealthCheck Service and UseAutoHealthCheck in Configure. That's it ! 

``` c#
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoHealthCheck();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.UseAutoHealthCheck();
        }
    }
```

The check can be made calling the endpoint with the deafault route+ `/api/autoHealthCheck`
ie:  http://localhost:50387/api/autoHealthCheck (The route can be configured also)

A json will be returned with the check information
with the following information:

- if health check was successfully.
- elapsed test time.
- endpoints who have failed.

``` JSON
{  
   "Success":false,
   "HttpStatus":500,
   "TimeStamp" : "2019-01-03T03:44:00.1240682Z",
   "HealthCheckId" : "7265ce70-09ed-46db-96db-0be7d0611fdc",
   "ElapsedSecondsTest":3,
   "UnhealthyEndpoints":[  
      {  
         "Route":"http://localhost:51555/api/Values/array",
         "HttpStatusCode":500,
         "HttpVerb":"POST"
      }
   ],
   "UnhealthyProbes":[  
      {  
         "Name":"CustomProbe",
         "ErrorMessage":"Custom message"
      }
   ]
}
```

To activate background monitor just configure the options in startup.

``` C#
    services.AddAutoHealthCheck(c =>
    {
        c.AutomaticRunConfigurations.AutomaticRunEnabled = true;
        c.BaseUrl = new Uri("http://localhost:50387");
        c.AutomaticRunConfigurations.SecondsInterval = 1;
    });
```
The url is required as asp.net core doesn't know exactly the URI if it is running behing a proxy reverse server like IIS in the moment the asp.net core application starts.

====================

## <a name="customising"> Customising </a>

1. [Intro](#customising_intro)
2. [Options](#customising_options)
3. [Hide Endpoints](#customising_hideendpoints)
4. [Custom Url](#customising_url)

### <a name="customising_intro"> Intro </a>
In order to customise the Check the following code is needed.

``` c#
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // add the rest of the configurations.
            services.AddAutoHealthCheck(c =>
            {
                c.DefaultUnHealthyResponseCode = HttpStatusCode.Accepted;
                c.PassCheckRule = response => response.Content == null;
            });
        }
    }
```

### <a name="customising_options"> Options </a>

- `DefaultUnHealthyResponseCode` : Allow to define the http code to return when a health check test has failed, defualt 500.

- `DefaultHealthyResponseCode` : Allow to define the http code to return when the check was successfully.

- `PassCheckRule` : Allow to define how to check if an endpoint is failing (for now is general) with this rule will check all the endpoints

default : Will fail if the endpoint return an status code between 500 - 599.
ie : ```c# c.PassCheckRule = response => !response.Headers.Contains("x-header"); ```

- `ExcludeRouteRegexs` : Allow to define a collection of refex to exclude endpoints to be called for the check.

- `DisableEndpointDiscovery` : If set to false, then automatic endpoint disovery and call will be disabled.

### <a name="customising_hideendpoints"> Hide endpoints </a>

If want to avoid a controller / method to be called just need a filter `AvoidAutoHealtCheckAttribute`

 ``` C#
    [AvoidAutoHealtCheck]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [AvoidAutoHealtCheck]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
     }
```

### <a name="customising_url"> Custom Url </a>

To run the endpoint to run the check in a custom url just need to configure it in `Configure` method like this:

``` C#
     app.UseAutoHealthCheck(c =>
    {
        c.RoutePrefix = "insights/healtcheck";
    });
```
====================

## <a name="extensibility"> Extensibility </a>

1. [Plugins](#plugins)
2. [Probes](#probes)

### <a name="plugins"> Plugins </a>

- `ResultPlugins` : Allow to define plugins to do something custom with a health check results.  This is a really great feature as could open the door to plugins like a plugin that call a webhook each time the health check fail to do something like reset the service or send an email.

In order to implement those plugins have to implement `IHealtCheckResultPlugin` interface and resolve one of or all the methods.

`ActionAfterResult` Do something after a any result.
`ActionAfterSuccess` Do something after a successfuly result.
`ActionAfterFail` Do something after a fail result.

Then just add the plugin to the configurations in any time of the application lifetime.

ie:  
``` C#
    public class ResultPlugin : IHealthCheckResultPlugin
    {
        private readonly ILogger _logger;

        public ResultPlugin(ILogger logger)
        {
            _logger = logger;
        }

        public string Name => "name";

        public Task ActionAfterResult(HealthyResponse result)
        {
            _logger.Log(LogLevel.Information, result.Success.ToString());
            return Task.CompletedTask;
        }

        public Task ActionAfterSuccess(HealthyResponse result)
        {
            return Task.CompletedTask;
        }

        public Task ActionAfterFail(HealthyResponse result)
        {
            return Task.CompletedTask;
        }
    }
```

- `HttpEndpointPlugins` : Allow to change the request content that are sent to check the endpoints of the asp.net application, here custom headers can be added, or query strings, things that are neccesary to hit endpoints.

To implement them just have to implement this interface `IHttpEndpointPlugin` and call:
  BeforeSend
  AfterReceive
  
  with the full request /response.
 
 ``` C#
    public class ResultPlugin : IHttpEndpointPlugin
    {
        public string Name => throw new NotImplementedException();

        public Task<HttpResponseMessage> AfterReceive(HttpResponseMessage response)
        {
            return Task.FromResult(response);
        }

        public Task<HttpRequestMessage> BeforeSend(HttpRequestMessage request)
        {
            request.Headers.Add("custom-header", "value");

            return Task.FromResult(request);
        }
    }
```

### <a name="probes"> Probes </a>

If custom probes are neded in the check engine that can be done easily creating your owns.
Just need to implement `IProbe` Interface like this.

 ``` C#
    public class CustomProbe : IProbe
    {
        public string Name => typeof(CustomProbe).Name;

        public Task<ProbeResult> Check()
        {
            return Task.FromResult(ProbeResult.Error("Custom message"));
        }
    }
```
If the probe was successfully just call `ProbeResult.Ok())` or `ProbeResult.Error` indicating the error message. that's it !

Not to allow probes to be called just register in Startup method **After** Calling `AddAutoHealthCheck`

 ``` C#
   public void ConfigureServices(IServiceCollection services)
   {
      services.AddAutoHealthCheck()
              .AddCustomProbe<CustomProbe>();
   }
```
 
====================

## <a name="security"> Security </a>

Security is an important concern in health checks, imagaine that you are checking external resources like databases or azure storages accoutns.
Malicious request can invalid your resources or even wors increse your billing if we are talking about pay-per-use resources. So it's important ensure in those cases that the request to hit resources is from a trusted source.

You can easily implemeny your own security with whole request like this.

 ``` C#
   public void Configure(IApplicationBuilder app, IHostingEnvironment env)
   {
      app.UseAutoHealthCheck(c =>
      {
          c.SecurityHandler = request => request.Query.ContainsKey("key") && request.Query["key"] == "1234";
      });
    }
```

Indicating when a request is valid, if is not then the healthcheck will return 401 without execute any endpoint or probe.


====================

## <a name="integrations"> Integrations </a>

1. [Microsoft.AspNetCore.Diagnostics.HealthChecks](#diagnostics)
2. [Application Insights](#applicationinsights)
3. [Azure Storage Account](#azurestorage)
4. [Raygun](#raygun)

### <a name="diagnostics"> Microsoft.AspNetCore.Diagnostics.HealthChecks </a>

This library is compatible with  [**Microsoft.AspNetCore.Diagnostics.HealthChecks.**](https://www.nuget.org/packages/Microsoft.AspNetCore.Diagnostics.HealthChecks) [** See usage here.**](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2)

As this health check exponse an endpoint as well this library does, then there is 2 approaches to integrate them.

1 - Use this endpoint as the primary and internally this will call other health check endpoint with a custom Probe Job.

In ConfigureServices do the following : 

https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.Diagnostics/
```sh
PM > Install-Package AspNetCore.AutoHealthCheck.Diagnostics
NET CLI - dotnet add AspNetCore.AutoHealthCheck.Diagnostics
paket add AspNetCore.AutoHealthCheck.Diagnostics
```

 ``` C#
  public void ConfigureServices(IServiceCollection services)
  {
     services.AddAutoHealthCheck(c =>
      {
          c.BaseUrl = new Uri("http://localhost:50382");
      })
      .AddAspNetCoreDiagnosticHealthCheck("health"); 
  }
```
The path is required as Microsoft.AspNetCore.Diagnostics.HealthChecks is not saving it so it cannot be retrieved. [**reference here.**](https://github.com/aspnet/Diagnostics/blob/082d505977d72c75b68935c93c21feded6928e5f/src/Microsoft.AspNetCore.Diagnostics.HealthChecks/Builder/HealthCheckApplicationBuilderExtensions.cs#L232)

With this approch you will receive the result as a normal probe Will faild for Degraded and Unhealthy.
ie : 

 ``` JSON
{  
   "Success":false,
   "HttpStatus":500,
   "UnhealthyProbes":[  
      {  
         "Name":"Microsoft.AspNetCore.Diagnostics.HealthChecks.Probe",
         "ErrorMessage":"Microsoft.AspNetCore.Diagnostics.HealthChecks.Probe failed.",
         "CustomData":{ }
      }
   ]
}

```

2 - Use Diagnostic endpoint as primary and internally this endpoint will be called as a custom health check.

In ConfigureServices do the following :

 ``` C#
 public void ConfigureServices(IServiceCollection services)
 {
    services.AddHealthChecks()
                .AddCheck<ExampleHealthCheck>("example_health_check")
                .AddAspNetCoreAutoHealthCheck();
 }
```
After Register Health Checks enable Auto Health Check integration, that way you can call the normal endpoint (Microsoft.AspNetCore.Diagnostics.HealthChecks) one, a custom check will call AutoHealthCheck endpoint and return 
        - Healthy  : If the result is success.
        - UnHealthy : If the result is not success.

** In both cases you can use custom probes, health check. :blush:

All BeatPulse Plugins are available to be used. https://github.com/Xabaril/BeatPulse


### <a name="applicationinsights"> Application Insights </a>

Application insights can be easily integrated with health check to track health check results.

https://www.nuget.org/packages/AspNetCore.AutoHealthCheck.ApplicationInsights/
```sh
PM > Install-Package AspNetCore.AutoHealthCheck.ApplicationInsights
NET CLI - dotnet add AspNetCore.AutoHealthCheck.ApplicationInsights
paket add AspNetCore.AutoHealthCheck.ApplicationInsights
```

 ``` C#
 public void ConfigureServices(IServiceCollection services)
 {
    services.AddHealthChecks()
            .AddAIResultPlugin();
 }
 
  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      app.UseAIResultPlugin(s =>
      {
          s.InstrumentationKey = "YourKeyHere";
          s.Mode = TrackMode.Event;
      });
  }
```
You can trace 3 different modes
  - Application insights events
  - Application insights availability
  - Application insights exceptions (only for failed check tests)

### <a name="azurestorage"> Azure Storage Account </a>

You can easily save your results in an azure storage account (blob).
Just install the package 

```sh
PM > Install-Package AspNetCore.AutoHealthCheck.AzureStorage
NET CLI - dotnet add AspNetCore.AutoHealthCheck.AzureStorage
paket add AspNetCore.AutoHealthCheck.AzureStorage
```
and do the following:

 ``` C#
 public void ConfigureServices(IServiceCollection services)
 {
    services.AddHealthChecks()
            .AddAzureStorageIntegration();
 }
 
  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      app.UseStorageHealthCheckIntegration("YourAzureStorageConnectionStringHere", c =>
      {
          c.OnlyTrackFailedResults = false;
          c.ContainerName = "foo";
      });
  }
```

You can track all the results or only those who have failed (deafault) and configure the container name (default aspnetcorehealthcheck).
A json file will be saved in a daily container with the health check result, you can track by the health check id.

### <a name="raygun"> Raygun </a>

You can easily track your errors or failed probes to Raygun and have 100% control of what is going on with your app.

```sh
PM > Install-Package AspNetCore.AutoHealthCheck.Raygun
NET CLI - dotnet add AspNetCore.AutoHealthCheck.Raygun
paket add AspNetCore.AutoHealthCheck.Raygun
```
and do the following:

 ``` C#
 public void ConfigureServices(IServiceCollection services)
 {
    services.AddHealthChecks()
            .AddRaygunIntegration();
 }
 
  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
      app.UseRaygunIntegration("YourRaygunKey", c =>
      {
          c.AvoidSendInDebug = true;
          c.Tags = new List<string>
          {
             "AnyTag"
          }
      });
  }
  
```
Just the api key to connect to raygun, additionally you can turn off sending logs when your are in DEBUG mode, and multiple custom tags can be added to provide more insights.

After that any failed check result will be posted to raygun with the result information within.

====================

## <a name="next"> What's next ? </a>

We want to add much more functionallity and integrations to this framework. If you have any suggestion feel free to open an issue or a PR.
Contributions are welcome.

What's next v 1.4 ? 
- Custom Request body for endpoints
- UI for Health Check visualization. 
- Result Stores.

====================


[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)

## <a name="license"> License </a>

MIT License
Copyright (c) 2018 David Revoledo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Made with ❤ in [DGENIX](https://www.dgenix.com/)

