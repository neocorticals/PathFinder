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
             { "OPD", "Hospital" }
            ,{ "Casualty", "Hospital" }
            ,{ "Emergency", "Hospital" }
            ,{ "Cardiologist", "Hospital" }
            ,{ "Ortho", "Hospital" }
            ,{ "Neuro", "Hospital" }
            ,{ "Reception", "Hospital" }
            ,{ "Enquiry", "Hospital" }
            ,{ "I.C.U", "Hospital" }
            ,{ "Operation Theatre", "Hospital"}

            ,{ "ANTARES (Conference room)", "DMI Pune" }
            ,{ "CANOPUS (Conference room)", "DMI Pune" }
            ,{ "POLLUX", "DMI Pune" }
            ,{ "CAPELLA", "DMI Pune" }
            ,{ "RIGEL", "DMI Pune" }
            ,{ "FRESH BREW", "DMI Pune" }

        }; 

        public HomePage()
        {
            InitializeComponent(); 
        } 

        private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            subCategory.Items.Clear();

            var filtered_SubCategories = SubCategory.Where(x => x.Value == category.SelectedItem.ToString()).Select(g => g.Key).ToList();

            if (filtered_SubCategories.Count == 0)
            {
                subCategory.Items.Clear();
            }
            else
            {
                foreach (string item in filtered_SubCategories)
                {

                    subCategory.Items.Add(item);
                }
            }

            Helpers.Settings.CategorySettings = category.SelectedItem.ToString(); 
        }

        private void subCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (subCategory.Items.Count > 0)
            {
                Helpers.Settings.SubCategorySettings = subCategory.SelectedItem.ToString();
                BeaconService.Start();
                this.Navigation.PushModalAsync(new Navigator());
            }
        }
    }
}