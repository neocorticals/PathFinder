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

            ,{ "Server Room", "DMI Pune" }
            ,{ "Conference Room TV", "DMI Pune" }
            ,{ "Conference Room Projector", "DMI Pune" }

        }; 

        public HomePage()
        {
            InitializeComponent(); 
        } 

        private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var filtered_SubCategories = SubCategory.Where(x => x.Value == category.SelectedItem.ToString()).Select(g => g.Key).ToList();

            if (filtered_SubCategories.Count == 0)
            {
                subCategory.Items.Clear();
            }
            else
            {
                //subCategory.Items.Clear();
                foreach (string item in filtered_SubCategories)
                {
                    subCategory.Items.Add(item);
                }
            }

            Helpers.Settings.CategorySettings = category.SelectedItem.ToString(); 
        }

        private void subCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Helpers.Settings.SubCategorySettings = subCategory.SelectedItem.ToString();
            BeaconService.Start();
            this.Navigation.PushModalAsync(new Navigator());
        }
    }
}