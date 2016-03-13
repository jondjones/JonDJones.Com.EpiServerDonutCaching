using DevTrends.MvcDonutCaching;
using EPiServer.Logging.Compatibility;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace JonDJones.com.Core.Donut
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class EpiServerDonutCacheAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Protected
        protected readonly ICacheHeadersHelper CacheHeadersHelper;
        protected readonly ICacheSettingsManager CacheSettingsManager;
        protected readonly EpiServerDonutHoleFiller DonutHoleFiller;
        protected readonly IKeyGenerator KeyGeneratord;
        protected readonly IReadWriteOutputCacheManager OutputCacheManager;
        protected CacheSettings CacheSettings;

        // Private
        private bool? _noStore;
        private OutputCacheOptions? _options;

        public EpiServerDonutCacheAttribute() : this(new KeyBuilder()) { }

        public EpiServerDonutCacheAttribute(IKeyBuilder keyBuilder) :
            this(
               new OutputCacheManager(OutputCache.Instance, keyBuilder),
               new EpiServerDonutHoleFiller(new EncryptingActionSettingsSerialiser(new ActionSettingsSerialiser(), new Encryptor())),
               new CacheSettingsManager(),
               new CacheHeadersHelper()
        )
        { }

        protected EpiServerDonutCacheAttribute(IReadWriteOutputCacheManager outputCacheManager,
                                               EpiServerDonutHoleFiller donutHoleFiller, 
                                               ICacheSettingsManager cacheSettingsManager,
                                               ICacheHeadersHelper cacheHeadersHelper)
        {
            OutputCacheManager = outputCacheManager;
            DonutHoleFiller = donutHoleFiller;
            CacheSettingsManager = cacheSettingsManager;
            CacheHeadersHelper = cacheHeadersHelper;

            Duration = -1;
            Location = (OutputCacheLocation)(-1);
            Options = OutputCache.DefaultOptions;
        }

        /// <summary>
        /// Gets or sets the cache duration, in seconds.
        /// </summary>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vary-by-param value.
        /// </summary>
        public string VaryByParam
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vary-by-custom value.
        /// </summary>
        public string VaryByCustom
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cache profile name.
        /// </summary>
        public string CacheProfile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public OutputCacheLocation Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to store the cache.
        /// </summary>
        public bool NoStore
        {
            get
            {
                return _noStore ?? false;
            }
            set
            {
                _noStore = value;
            }
        }

        /// <summary>
        /// Get or sets the <see cref="OutputCacheOptions"/> for this attributes. Specifying a value here will
        /// make the <see cref="OutputCache.DefaultOptions"/> value ignored.
        /// </summary>
        public DevTrends.MvcDonutCaching.OutputCacheOptions Options
        {
            get
            {
                return _options ?? OutputCacheOptions.None;
            }
            set
            {
                _options = value;
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (CacheSettings != null)
            {
                ExecuteCallback(filterContext, true);
            }
        }

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CacheSettings = BuildCacheSettings();

            // Custom Code
            var cacheKey = EpiServerDonutHelper.GenerateUniqueCacheKey(filterContext);

            // If we are unable to generate a cache key it means we can't do anything
            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }

            // Are we actually storing data on the server side ?
            if (CacheSettings.IsServerCachingEnabled)
            {
                CacheItem cachedItem = null;

                // If the request is a POST, we lookup for NoCacheLookupForPosts option
                // We are fetching the stored value only if the option has not been set and the request is not a POST
                if (
                    (CacheSettings.Options & OutputCacheOptions.NoCacheLookupForPosts) != OutputCacheOptions.NoCacheLookupForPosts ||
                    filterContext.HttpContext.Request.HttpMethod != "POST"
                )
                {
                    cachedItem = OutputCacheManager.GetItem(cacheKey);
                }

                // We have a cached version on the server side
                if (cachedItem != null)
                {
                    // We inject the previous result into the MVC pipeline
                    // The MVC action won't execute as we injected the previous cached result.

                    var donutReplacedHtml = DonutHoleFiller.ReplaceDonutHoleContent(cachedItem.Content, filterContext, CacheSettings.Options);

                    Logger.ErrorFormat("PRE DONUT HTML FROM CACHE {0} {1}", cacheKey, cachedItem.Content);
                    Logger.ErrorFormat("DONUT REPLACED HTML FROM CACHE {0} {1}", cacheKey, donutReplacedHtml);

                    filterContext.Result = new ContentResult
                    {
                        Content = donutReplacedHtml,
                        ContentType = cachedItem.ContentType
                    };
                }
            }

            // Did we already injected something ?
            if (filterContext.Result != null)
            {
                return; // No need to continue 
            }

            // We are hooking into the pipeline to replace the response Output writer
            // by something we own and later eventually gonna cache
            var cachingWriter = new StringWriter(CultureInfo.InvariantCulture);

            var originalWriter = filterContext.HttpContext.Response.Output;

            filterContext.HttpContext.Response.Output = cachingWriter;

            // Will be called back by OnResultExecuted -> ExecuteCallback
            filterContext.HttpContext.Items[cacheKey] = new Action<bool>(hasErrors =>
            {
                // Removing this executing action from the context
                filterContext.HttpContext.Items.Remove(cacheKey);

                // We restore the original writer for response
                filterContext.HttpContext.Response.Output = originalWriter;

                if (hasErrors)
                {
                    return; // Something went wrong, we are not going to cache something bad
                }

                var itemToRenderWithDonuts = cachingWriter.ToString();
                var originalWriterString = originalWriter.ToString();
                // Now we use owned caching writer to actually store data
                var cacheItem = new CacheItem
                {
                    Content = itemToRenderWithDonuts,
                    ContentType = filterContext.HttpContext.Response.ContentType
                };

                var donutRemovedHtml = DonutHoleFiller.RemoveDonutHoleWrappers(cacheItem.Content, filterContext, CacheSettings.Options);

                filterContext.HttpContext.Response.Write(donutRemovedHtml);

                Logger.ErrorFormat("PRE DONUT HTML LIVE {0} {1}", cacheKey, cacheItem.Content);
                Logger.ErrorFormat("DONUT REPLACED HTML LIVE {0} {1}", cacheKey, donutRemovedHtml);

                if (CacheSettings.IsServerCachingEnabled && filterContext.HttpContext.Response.StatusCode == 200)
                {
                    OutputCacheManager.AddItem(cacheKey, cacheItem, DateTime.UtcNow.AddSeconds(CacheSettings.Duration));
                }
            });
        }

        /// <summary>
        /// Called after an action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (CacheSettings == null)
            {
                return;
            }

            var cacheKey = EpiServerDonutHelper.GenerateUniqueCacheKey(filterContext);

            // See OnActionExecuting
            ExecuteCallback(filterContext, filterContext.Exception != null);

            // If we are in the context of a child action, the main action is responsible for setting
            // the right HTTP Cache headers for the final response.
            if (!filterContext.IsChildAction)
            {
                CacheHeadersHelper.SetCacheHeaders(filterContext.HttpContext.Response, CacheSettings);
            }
        }

        /// <summary>
        /// Builds the cache settings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Web.HttpException">
        /// The 'duration' attribute must have a value that is greater than or equal to zero.
        /// </exception>
        protected CacheSettings BuildCacheSettings()
        {
            CacheSettings cacheSettings;

            if (string.IsNullOrEmpty(CacheProfile))
            {
                cacheSettings = new CacheSettings
                {
                    IsCachingEnabled = CacheSettingsManager.IsCachingEnabledGlobally,
                    Duration = Duration,
                    VaryByCustom = VaryByCustom,
                    VaryByParam = VaryByParam,
                    Location = (int)Location == -1 ? OutputCacheLocation.Server : Location,
                    NoStore = NoStore,
                    Options = Options,
                };
            }
            else
            {
                var cacheProfile = CacheSettingsManager.RetrieveOutputCacheProfile(CacheProfile);

                cacheSettings = new CacheSettings
                {
                    IsCachingEnabled = CacheSettingsManager.IsCachingEnabledGlobally && cacheProfile.Enabled,
                    Duration = Duration == -1 ? cacheProfile.Duration : Duration,
                    VaryByCustom = VaryByCustom ?? cacheProfile.VaryByCustom,
                    VaryByParam = VaryByParam ?? cacheProfile.VaryByParam,
                    Location = (int)Location == -1 ? ((int)cacheProfile.Location == -1 ? OutputCacheLocation.Server : cacheProfile.Location) : Location,
                    NoStore = _noStore.HasValue ? _noStore.Value : cacheProfile.NoStore,
                    Options = Options,
                };
            }

            if (cacheSettings.Duration == -1)
            {
                throw new HttpException("The directive or the configuration settings profile must specify the 'duration' attribute.");
            }

            if (cacheSettings.Duration < 0)
            {
                throw new HttpException("The 'duration' attribute must have a value that is greater than or equal to zero.");
            }

            return cacheSettings;
        }

        /// <summary>
        /// Executes the callback.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="hasErrors">if set to <c>true</c> [has errors].</param>
        private void ExecuteCallback(ControllerContext context, bool hasErrors)
        {
            // Custom code
            var cacheKey = EpiServerDonutHelper.GenerateUniqueCacheKey(context);

            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }

            var callback = context.HttpContext.Items[cacheKey] as Action<bool>;

            if (callback != null)
            {
                callback.Invoke(hasErrors);
            }
        }
    }
}