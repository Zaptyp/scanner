﻿using System;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using static Utilities;


namespace Scanner.Views
{
    public sealed partial class ScanOptionsView : Page
    {
        public ScanOptionsView()
        {
            this.InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.PreviewRunning += ViewModel_PreviewRunning;
            ViewModel.ScannerSearchTipRequested += ViewModel_ScannerSearchTipRequested;
        }

        private async void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.NextDefaultScanAction))
            {
                await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
                {
                    switch (ViewModel.NextDefaultScanAction)
                    {
                        case ViewModels.ScanAction.AddPages:
                        case ViewModels.ScanAction.AddPagesToDocument:
                        default:
                            MenuFlyoutItemButtonScan.FontWeight = Windows.UI.Text.FontWeights.SemiBold;
                            MenuFlyoutItemButtonScanFresh.FontWeight = Windows.UI.Text.FontWeights.Normal;
                            break;
                        case ViewModels.ScanAction.StartFresh:
                            MenuFlyoutItemButtonScan.FontWeight = Windows.UI.Text.FontWeights.Normal;
                            MenuFlyoutItemButtonScanFresh.FontWeight = Windows.UI.Text.FontWeights.SemiBold;
                            break;
                    }
                });
            }
        }

        private async void ViewModel_PreviewRunning(object sender, EventArgs e)
        {
            await RunOnUIThreadAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReliablyOpenTeachingTip(TeachingTipPreview);
            });
        }

        private async void ComboBoxScanners_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
#if DEBUG
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                FlyoutBase.ShowAttachedFlyout(ComboBoxScanners);
            });
#endif
        }

        private async void ButtonScan_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
#if DEBUG
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            });
#endif
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RunOnUIThreadAsync(CoreDispatcherPriority.Normal, () =>
            {
                // fix RadioButtons losing index value when navigating to multiple different pages
                try
                {
                    int index;
                    index = RadioButtonsSource.SelectedIndex;
                    RadioButtonsSource.SelectedIndex = -1;
                    RadioButtonsSource.SelectedIndex = index;

                    index = RadioButtonsColorMode.SelectedIndex;
                    RadioButtonsColorMode.SelectedIndex = -1;
                    RadioButtonsColorMode.SelectedIndex = index;

                    index = RadioButtonsAutoCropMode.SelectedIndex;
                    RadioButtonsAutoCropMode.SelectedIndex = -1;
                    RadioButtonsAutoCropMode.SelectedIndex = index;
                }
                catch { }

                // fix ProgressRing getting stuck when navigating back to cached page
                ProgressRingScanners.IsActive = false;
                ProgressRingScan.IsActive = false;
                ProgressRingScanners.IsActive = true;
                ProgressRingScan.IsActive = true;
            });
        }

        private async void ProgressRingPreview_Loaded(object sender, RoutedEventArgs e)
        {
            // fix ProgressRing getting stuck when previewing multiple times
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                ProgressRingPreview.IsActive = false;
                ProgressRingPreview.IsActive = true;
            });
        }

        private async void ButtonPreview_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
#if DEBUG
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                FlyoutBase.ShowAttachedFlyout(ButtonPreview);
            });
#endif
        }

        private async void ButtonDebugPreviewFile_Clicked(object sender, RoutedEventArgs e)
        {
#if DEBUG
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                FlyoutBase.GetAttachedFlyout(ButtonPreview).Hide();
            });
#endif
            ViewModel.PreviewScanCommand.Execute("File");
        }

        private async void ButtonDebugPreviewFail_Clicked(object sender, RoutedEventArgs e)
        {
#if DEBUG
            await RunOnUIThreadAsync(CoreDispatcherPriority.Low, () =>
            {
                FlyoutBase.GetAttachedFlyout(ButtonPreview).Hide();
            });
#endif
            ViewModel.PreviewScanCommand.Execute("Fail");
        }

        private async void ViewModel_ScannerSearchTipRequested(object sender, EventArgs e)
        {
            await RunOnUIThreadAndWaitAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReliablyOpenTeachingTip(TeachingTipScannerSearchTip);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.ViewNavigatedToCommand.Execute(null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.ViewNavigatedFromCommand.Execute(null);
        }

        private void TeachingTipPreview_Closed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ViewModel.DismissPreviewScanCommand.Execute(null);
        }
    }
}
