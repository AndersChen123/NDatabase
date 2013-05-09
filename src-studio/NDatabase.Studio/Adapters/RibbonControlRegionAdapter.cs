﻿using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Regions;
using Syncfusion.Windows.Tools.Controls;

namespace NDatabase.Studio.Adapters
{
    /// <summary>
    /// Ribbon Control Region Adapter that helps to inject RibbonTab.
    /// </summary>
    [Export]
    public class RibbonControlRegionAdapter : RegionAdapterBase<Ribbon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonControlRegionAdapter"/> class.
        /// </summary>
        /// <param name="regionBehaviorFactory">The region behavior factory.</param>
        [ImportingConstructor]
        public RibbonControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {

        }

        /// <summary>
        /// Adapts the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="regionTarget">The region target.</param>
        protected override void Adapt(IRegion region, Ribbon regionTarget)
        {
            region.Views.CollectionChanged += delegate
                                                  {
                                                      foreach (var tab in region.Views.Cast<RibbonTab>())
                                                      {
                                                          if (!regionTarget.Items.Contains(tab))
                                                          {
                                                              regionTarget.Items.Add(tab);
                                                          }
                                                      }
                                                  };
        }

        /// <summary>
        /// Attaches the behaviors.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="regionTarget">The region target.</param>
        protected override void AttachBehaviors(IRegion region, Ribbon regionTarget)
        {
            // Add the behavior that syncs the items source items with the rest of the items
            region.Behaviors.Add(RibbonSyncBehavior.BehaviorKey, new RibbonSyncBehavior()
                                                                     {
                                                                         HostControl = regionTarget
                                                                     });

            base.AttachBehaviors(region, regionTarget);
        }

        /// <summary>
        /// Creates the region.
        /// </summary>
        /// <returns></returns>
        protected override IRegion CreateRegion()
        {
            return new Region();
        }
    }
}