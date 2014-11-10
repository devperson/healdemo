using HealthDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthDemo.Pages
{
    public class TipDetailPage : MasterPage
    {
        private TipViewModel VM { get; set; }
        public TipDetailPage()
            : base()
        {
            VM = ViewModelLocator.TipVM;
            BindingContext = VM.SelectedTip;
            lblTitle.Text = "Health Tip";
        }


        protected override void RenderContentView(StackLayout parent)
        {
            var rootScrollView = new ScrollView() { Orientation = ScrollOrientation.Vertical, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            var stlayout = new StackLayout() { Padding = new Thickness(0, 10, 0, 0), Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand };

            var lblTitle = new Label()
            {
                TextColor = Color.Black,
                Font = Font.SystemFontOfSize(16)
            };
            lblTitle.SetBinding(Label.TextProperty, new Binding("Title"));
            var stkl = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 0, 0, 0), Children = { lblTitle} };

            var lblDescription = new Label()
            {
                TextColor = Color.Black,
                Font = Font.SystemFontOfSize(14),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 150
            };
            lblDescription.SetBinding(Label.TextProperty, new Binding("Description"));

            var stackLayoutDetails = new StackLayout() { BackgroundColor = Color.White, Orientation = StackOrientation.Vertical, Padding = new Thickness(15, 15, 7, 15), Spacing = 10 };
            stackLayoutDetails.Children.Add(lblDescription);
            var frame1 = new Frame() { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 10, 20, 20) };
            var frmae2 = new Frame() { BackgroundColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 1 };
            frmae2.Content = stackLayoutDetails;
            frame1.Content = frmae2;

            stlayout.Children.Add(stkl);
            stlayout.Children.Add(frame1);
            rootScrollView.Content = stlayout;
            parent.Children.Add(rootScrollView);
        }
    }
}
