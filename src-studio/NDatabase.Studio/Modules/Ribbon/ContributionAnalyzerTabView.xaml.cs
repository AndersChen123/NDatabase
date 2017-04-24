﻿using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;

namespace NDatabase.Studio.Modules.Ribbon
{
    /// <summary>
    /// Interaction logic for ContributionAnalyzerTabView.xaml
    /// </summary>
    public partial class ContributionAnalyzerTabView 
    {
        #region Memebers
        IEventAggregator eventAggregator;
        private readonly IModuleManager moduleManager;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;
      
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionAnalyzerTabView"/> class.
        /// </summary>
        /// <param name="eventAgg">The event agg.</param>
        /// <param name="moduleManager">The module manager.</param>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="unityContainer">The unity container.</param>
        /// <param name="viewModel">The view model.</param>
        public ContributionAnalyzerTabView(IEventAggregator eventAgg, IModuleManager moduleManager, IRegionManager regionManager, IUnityContainer unityContainer, ContributionAnalyzerTabViewModel viewModel)
        {
            this.eventAggregator = eventAgg;
            this.moduleManager = moduleManager;
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
            InitializeComponent();
            this.DataContext = viewModel;
          
        }

        /// <summary>
        /// Gets the parent ribbon Window
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        private FrameworkElement GetParent(FrameworkElement parent)
        {
            while (parent != null)
            {
                parent = (FrameworkElement)parent.Parent;

                if (parent is RibbonWindow)
                {
                    break;
                }
            }
            return parent;
        }

        /// <summary>
        /// Handling the custom color settings
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        //private void colorBox_ColorChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        //{
        //    FrameworkElement parent = d as FrameworkElement;
        //    parent = GetParent(parent);
        //    SolidColorBrush customcolor = new SolidColorBrush(colorBox.Color);
        //    SkinManager.SetActiveColorScheme(parent, customcolor);
        //}
    }
}
