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
	public partial class HomePage : ContentPage
	{
        Dictionary<string, string> SubCategory = new Dictionary<string, string>
        {
            { "OPD", "Hospital" }, { "Casualty", "Hospital" },
            { "Emergency", "Hospital" }, { "Cardiologist", "Hospital" },
            { "Ortho", "Hospital" }, { "Neuro", "Hospital" },
            { "Reception", "Hospital" }, { "Enquiry", "Hospital" },
            { "I.C.U", "Hospital" }, { "Operation Theatre", "Hospital" }
       
        };
        public HomePage ()
		{
			InitializeComponent();
		}

        private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //PickerLabel.Text = PickerList.Items[PickerList.SelectedIndex];
        }
    }
}