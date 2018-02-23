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
            { "I.C.U", "Hospital" }, { "Operation Theatre", "Hospital" },
       { "Reception Counter", "Hotel" }, { "Food Court", "Hotel" }
        };



        public HomePage()
        {
            InitializeComponent();

            //category.SelectedIndexChanged += Category_SelectedIndexChanged;
        }

        //private void Category_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
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

            //subCategory.ItemsSource = SubCategory.Keys;
            //PickerLabel.Text = PickerList.Items[PickerList.SelectedIndex];
        }

        private void subCategory_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Helpers.Settings.SubCategorySettings = subCategory.SelectedItem.ToString();

        }
    }
    }