using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using JonDJones.com.Core.Attributes;
using JonDJones.com.Core.Enums;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Interfaces;
using JonDJones.Com.Core.Routing;
using System;
using JonDJones.com.Core.Extensions;

namespace JonDJones.Com.Core.ViewModel.Base
{
    public class BlockViewModel<T> : IBlockViewModel<T>
        where T : BlockData
    {
        internal IEpiServerDependencies _epiServerDependencies;

        public BlockViewModel(T currentBlock, IEpiServerDependencies epiServerDependencies, DisplayOptionEnum _displayOptionTag)
        {
            _epiServerDependencies = epiServerDependencies;
            DisplayOptionTag = _displayOptionTag;

            if (currentBlock == null)
                throw new ArgumentNullException("currentBlock");

            CurrentBlock = currentBlock;
        }

        public T CurrentBlock { get; private set; }

        protected IContentRepository ContentRepository
        {
            get
            {
                return _epiServerDependencies.ContentRepository;
            }
        }


        protected ILinkResolver LinkResolver
        {
            get
            {
                return _epiServerDependencies.LinkResolver;
            }
        }

        public DisplayOptionEnum DisplayOptionTag { get; set; }

        public string GetBootstrapClass()
        {
            return DisplayOptionTag.GetAttributeOfEnumValue<BootstrapClassAttribute>().IfNotDefault(x => x.Name);
        }
    }
}