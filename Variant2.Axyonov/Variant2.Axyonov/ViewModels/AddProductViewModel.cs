using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using Variant2.Axyonov.Models;

namespace Variant2.Axyonov.ViewModels
{
    internal class AddProductViewModel: ViewModelBase
    {
        private List<Product> _productsList=new();
        private List<Material> _materialsList = new();
        private string _countText = null!;
        private Material _materialItem = new();
        private Product _productItem = new();

        public AddProductViewModel()
        {
            using(LopushokDBAxyonovContext db = new LopushokDBAxyonovContext())
            {
                foreach (var item in db.Products)
                {
                    ProductsList.Add(item);
                }
                foreach (var item in db.Materials.ToList())
                {
                    MaterialsList.Add(item);
                }

            }
        }

        public List<Product> ProductsList 
        { 
            get => _productsList; 
            set
            {
                _productsList = value;
                OnPropertyChanged(nameof(ProductsList));
            }
        }
        public List<Material> MaterialsList
        { 
            get => _materialsList;
            set
            {
                _materialsList = value;
                OnPropertyChanged(nameof(MaterialsList));
            }
        }
        public string CountText { get => _countText; set => _countText = value; }
        public Material MaterialItem { get => _materialItem; set => _materialItem = value; }
        public Product ProductItem { get => _productItem; set => _productItem = value; }

        private void AddNewRecord()
        {

        }
    }
}