﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows.Threading;
using System.Windows.Media;
using NDatabase.Studio.Infrastructure;

namespace NDatabase.Studio.Modules.Ribbon
{
    /// <summary>
    /// Represents the ContributionAnalyzerTabViewModel class which handles the interaction logic for ContributionAnalyzerTabView..
    /// </summary>
    public class ContributionAnalyzerTabViewModel :Control ,INotifyPropertyChanged
    {
        #region Class Members
        IEventAggregator eventAggregator;
        private readonly IModuleManager moduleManager;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;
        string animationlabel;
        string groupButtonLabel;
        string activate3D;
        BackgroundWorker worker = new BackgroundWorker();
        Window win;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionAnalyzerTabViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="moduleManager">The module manager.</param>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="unityContainer">The unity container.</param>
        public ContributionAnalyzerTabViewModel(IEventAggregator eventAggregator, IModuleManager moduleManager, IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.eventAggregator = eventAggregator;
            this.moduleManager = moduleManager;
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
            this.AnimationLabel = "Activate Animation";
            this.Activate3DModeLabel = "Activate 3D Mode";
            this.GroupButtonLabel = "ShowGroup DropArea";
            RegisterDelegates();
            InitializeWorker();
        }

        #region EventHandler

        /// <summary>
        /// Handles the DoWork event of the worker control. Does the long work
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                ShowWindow();
                this.moduleManager.LoadModule("ConnectionsModule");
//                this.moduleManager.LoadModule("PortfolioGridModule");
//                this.moduleManager.LoadModule("CountrySectorChartModule");
                activateView();
            }));

        }


        //This indicates that the background process is complete.
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            win.Close();
        }

        /// <summary>
        /// Handles the view selection
        /// </summary>
        /// <param name="ViewName">Name of the view.</param>
        void SelectedViewEventHandler(string ViewName)
        {
            if (ViewName == "ContributionAnalyzer")
            {
                if(!worker.IsBusy)
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the animation activation/deactivation
        /// </summary>
        /// <param name="o">The o.</param>
        void ActivateAnimationHandler(object o)
        {
            if (this.AnimationLabel.Equals("Activate Animation"))
            {
                this.AnimationLabel = "Deactivate Animation";
                this.eventAggregator.GetEvent<AnimationEvents>().Publish(true);
            }
            else
            {
                this.AnimationLabel = "Activate Animation";
                this.eventAggregator.GetEvent<AnimationEvents>().Publish(false);
            }
        }

        /// <summary>
        /// Publishes the Blue Skin selected event.
        /// </summary>
        /// <param name="o"></param>
        void SelectBlueSkinHandler(object o)
        {
            this.eventAggregator.GetEvent<SkinChangedEvent>().Publish("Blue");
        }

        /// <summary>
        /// Publishes the Black Skin selected event.
        /// </summary>
        /// <param name="o"></param>
        void SelectBlackSkinHandler(object o)
        {
            this.eventAggregator.GetEvent<SkinChangedEvent>().Publish("Black");
        }

        /// <summary>
        /// Publishes the Silver Skin selected event.
        /// </summary>
        /// <param name="o"></param>
        void SelectSilverSkinHandler(object o)
        {
            this.eventAggregator.GetEvent<SkinChangedEvent>().Publish("Silver");
        }

        /// <summary>
        /// Publishes the Blend Skin selected event.
        /// </summary>
        /// <param name="o"></param>
        void SelectBlendSkinHandler(object o)
        {
            this.eventAggregator.GetEvent<SkinChangedEvent>().Publish("Blend");
        }

        /// <summary>
        /// Publishes the Pie Chart selected event.
        /// </summary>
        /// <param name="o"></param>
        void PieChartEventHandler(object o)
        {
            this.eventAggregator.GetEvent<ChartTypesEvent>().Publish("Pie");
        }

        /// <summary>
        /// Publishes the Funnel Chart selected event.
        /// </summary>
        /// <param name="o"></param>
        void FunnelChartEventHandler(object o)
        {
            this.eventAggregator.GetEvent<ChartTypesEvent>().Publish("Funnel");
        }

        /// <summary>
        /// Publishes the Doughnut Chart selected event.
        /// </summary>
        /// <param name="o"></param>
        void DoughnutChartEventHandler(object o)
        {
            this.eventAggregator.GetEvent<ChartTypesEvent>().Publish("Doughnut");
        }

        /// <summary>
        /// Publishes the Pyramid Chart selected event.
        /// </summary>
        /// <param name="o"></param>
        void PyramidChartEventHandler(object o)
        {
            this.eventAggregator.GetEvent<ChartTypesEvent>().Publish("Pyramid");
        }

        /// <summary>
        /// Handles the threeD mode activation/deactivation
        /// </summary>
        /// <param name="o">The o.</param>
        void Activate3DModeHandler(object o)
        {
            if (this.Activate3DModeLabel.Equals("Activate 3D Mode"))
            {
                this.Activate3DModeLabel = "Deactivate 3D Mode";
                this.eventAggregator.GetEvent<ThreeDEvent>().Publish(true);
            }
            else
            {
                this.Activate3DModeLabel = "Activate 3D Mode";
                this.eventAggregator.GetEvent<ThreeDEvent>().Publish(false);
            }
        }


        /// <summary>
        /// Shows and hides the group drop area.
        /// </summary>
        /// <param name="o">The object.</param>
        void ShowHideGroupDropArea(object o)
        {
            if (this.GroupButtonLabel.Equals("ShowGroup DropArea"))
            {
                this.GroupButtonLabel = "HideGroup DropArea";
                this.eventAggregator.GetEvent<ShowHideGroupingEvent>().Publish(true);
            }
            else
            {
                this.GroupButtonLabel = "ShowGroup DropArea";
                this.eventAggregator.GetEvent<ShowHideGroupingEvent>().Publish(false);
            }
        }
        #endregion

        #region Delegate Commands and  Properties

        /// <summary>
        /// Gets or sets the View .
        /// </summary>
        /// <value>The selected view.</value>
        public DelegateCommand<object> SelectView { get; set; }

        /// <summary>
        /// Gets or sets the activate animation.
        /// </summary>
        /// <value>The activated animation.</value>
        public DelegateCommand<object> ActivateAnimation { get; set; }

        /// <summary>
        /// Gets or sets the enable three D mode.
        /// </summary>
        /// <value>The enable three D mode.</value>
        public DelegateCommand<object> EnableThreeDMode { get; set; }

        /// <summary>
        /// Gets or sets the type of the pie.
        /// </summary>
        /// <value>The type of the pie.</value>
        public DelegateCommand<object> PieType { get; set; }

        /// <summary>
        /// Gets or sets the type of the funnel.
        /// </summary>
        /// <value>The type of the funnel.</value>
        public DelegateCommand<object> FunnelType { get; set; }

        /// <summary>
        /// Gets or sets the type of the doughnut.
        /// </summary>
        /// <value>The type of the doughnut.</value>
        public DelegateCommand<object> DoughnutType { get; set; }

        /// <summary>
        /// Gets or sets the type of the pyramid.
        /// </summary>
        /// <value>The type of the pyramid.</value>
        public DelegateCommand<object> PyramidType { get; set; }

        /// <summary>
        /// Gets or sets the select blue skin.
        /// </summary>
        /// <value>The select blue skin.</value>
        public DelegateCommand<object> SelectBlueSkin { get; set; }

        /// <summary>
        /// Gets or sets the select black skin.
        /// </summary>
        /// <value>The select black skin.</value>
        public DelegateCommand<object> SelectBlackSkin { get; set; }

        /// <summary>
        /// Gets or sets the select silver skin.
        /// </summary>
        /// <value>The select silver skin.</value>
        public DelegateCommand<object> SelectSilverSkin { get; set; }

        /// <summary>
        /// Gets or sets the select blend skin.
        /// </summary>
        /// <value>The select blend skin.</value>
        public DelegateCommand<object> SelectBlendSkin { get; set; }

        /// <summary>
        /// Gets or sets the option to show/hide Grid's group droparea
        /// </summary>
        public DelegateCommand<object> ShowGroupDropArea { get; set; }
       
        /// <summary>
        /// Gets or sets the animation label.
        /// </summary>
        /// <value>The animation label.</value>
        public string AnimationLabel
        {
            get
            {
                return animationlabel;
            }
            set
            {
                animationlabel = value;

                OnPropertyChanged("AnimationLabel");
            }
        }

        /// <summary>
        /// Gets or sets the group button label.
        /// </summary>
        /// <value>The group button label.</value>
        public string GroupButtonLabel
        {
            get
            {
                return this.groupButtonLabel;
            }
            set
            {
                this.groupButtonLabel = value;
                this.OnPropertyChanged("GroupButtonLabel");
            }
        }

        /// <summary>
        /// Gets or sets the activate3 D mode label.
        /// </summary>
        /// <value>The activate3 D mode label.</value>
        public string Activate3DModeLabel
        {
            get
            {
                return activate3D;
            }
            set
            {
                activate3D = value;

                OnPropertyChanged("Activate3DModeLabel");
            }
        }

        bool ischecked;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked from model.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is checked from model; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckedFromModel
        {
            get
            {
                return ischecked;
            }
            set
            {
                ischecked = value;

                OnPropertyChanged("IsCheckedFromModel");
            }
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Registers the delegates.
        /// </summary>
        private void RegisterDelegates()
        {
            //Skin Events
            this.SelectBlueSkin = new DelegateCommand<object>(o => eventAggregator.GetEvent<SkinChangedEvent>().Publish("Blue"));
            this.SelectView = new DelegateCommand<object>(o => eventAggregator.GetEvent<SelectedViewEvents>().Publish("ContributionAnalyzer"));
            this.SelectBlackSkin = new DelegateCommand<object>(o => eventAggregator.GetEvent<SkinChangedEvent>().Publish("Black"));
            this.SelectSilverSkin = new DelegateCommand<object>(o => eventAggregator.GetEvent<SkinChangedEvent>().Publish("Silver"));
            this.SelectBlendSkin = new DelegateCommand<object>(o => eventAggregator.GetEvent<SkinChangedEvent>().Publish("Blend"));

            this.ActivateAnimation = new DelegateCommand<object>(ActivateAnimationHandler);
            this.EnableThreeDMode = new DelegateCommand<object>(Activate3DModeHandler);
            this.PieType = new DelegateCommand<object>(PieChartEventHandler);
            this.FunnelType = new DelegateCommand<object>(FunnelChartEventHandler);
            this.DoughnutType = new DelegateCommand<object>(DoughnutChartEventHandler);
            this.PyramidType = new DelegateCommand<object>(PyramidChartEventHandler);
            this.eventAggregator.GetEvent<SelectedViewEvents>().Subscribe(SelectedViewEventHandler);

            this.ShowGroupDropArea = new DelegateCommand<object>(ShowHideGroupDropArea);
        }

        /// <summary>
        /// Initializes the background worker.
        /// </summary>
        void InitializeWorker()
        {
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        void ShowWindow()
        {
            ResourceDictionary rs = new ResourceDictionary();
            rs.Source = new Uri(@"Infrastructure\Brushes.xaml", UriKind.RelativeOrAbsolute);
            DrawingBrush brush = rs["background"] as DrawingBrush;
            win = new Window();
            //win.AllowsTransparency = true;
            Label label = new Label();
            label.Content = "Please Wait...Loading Modules...";
            label.FontWeight = FontWeights.Bold;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.FontSize = 14;
            label.FontFamily = new FontFamily("Verdana");
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.WindowStyle = WindowStyle.None;
            win.Background = brush;
            win.Height = 75;
            win.Width = 309;
            win.ShowInTaskbar = false;
            win.Topmost = true;
            win.Content = label;
            win.Show();
        }

        /// <summary>
        /// Activates and deactivates the view.
        /// </summary>
        private void activateView()
        {
            IRegion ribbonRegion = regionManager.Regions[RegionNames.RibbonRegion];
            object dview = ribbonRegion.GetView("dashboardView");
            ribbonRegion.Deactivate(dview);
            object cview = ribbonRegion.GetView("contributionAnalyzerView");
            ribbonRegion.Activate(cview);
            IRegion dockingRegion = regionManager.Regions[RegionNames.DockingRegion];
            object accountsview1 = dockingRegion.GetView("connectionsView");
            dockingRegion.Deactivate(accountsview1);
//            object stockview = dockingRegion.GetView("stocklistView");
//            dockingRegion.Deactivate(stockview);
//            object chartview1 = dockingRegion.GetView("historychartView");
//            dockingRegion.Deactivate(chartview1);
            object accountsview = dockingRegion.GetView("connectionsView");
            dockingRegion.Activate(accountsview);
//            object gridview = dockingRegion.GetView("portfoliogridView");
//            dockingRegion.Activate(gridview);
//            object chartview = dockingRegion.GetView("countrysectorchartView");
//            dockingRegion.Activate(chartview);

        }
        #endregion

        #region INotifyPropertyChanged Members
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
   
}
