using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PathFinder.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Navigator : ContentPage
	{
		public Navigator ()
		{
			InitializeComponent ();

            lblCategory.Text = Helpers.Settings.CategorySettings;
            lblSubCategory.Text = Helpers.Settings.SubCategorySettings;

        }
	}
}