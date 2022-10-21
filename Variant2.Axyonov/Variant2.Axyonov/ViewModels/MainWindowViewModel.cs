using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using Variant2.Axyonov.Models;
using Variant2.Axyonov.Views;

namespace Variant2.Axyonov.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Data
        private List<string> _filtherBox = new();
        private List<string> _sortBox = new();
        private List<TableItem> _tableItems = new();
        private List<TableItem> _viewTableItems = new();
        private List<ProductMaterial> _productMaterials = new();
        private string _search = null!;
        private string _selectedFiltherItem = null!;
        private string _selectedSortItem = null!;

        public List<TableItem> TableItems 
        { 
            get => _tableItems; 
            set
            {
                _tableItems = value;
                OnPropertyChanged(nameof(TableItems));
            }
        }
        public List<TableItem> ViewTableItems
        { 
            get => _viewTableItems; 
            set
            {
                _viewTableItems = value;
                OnPropertyChanged(nameof(ViewTableItems));
            }
        }
        public List<ProductMaterial> ProductMaterials 
        { 
            get => _productMaterials; 
            set
            {
                _productMaterials = value;
                OnPropertyChanged(nameof(ProductMaterials));
            }
        }
        public List<string> FiltherBox
        { 
            get => _filtherBox;
            set
            {
                _filtherBox = value;
                OnPropertyChanged(nameof(FiltherBox));
            }
        }
        public List<string> SortBox 
        { 
            get => _sortBox;
            set
            {
                _sortBox = value;
                OnPropertyChanged(nameof(SortBox));
            }
        }


        public string SelectedFiltherItem 
        { 
            get => _selectedFiltherItem;
            set
            {
                _selectedFiltherItem = value;
                Filthering(value);
                OnPropertyChanged(nameof(SelectedFiltherItem));

            }
        }
        public string SelectedSortItem 
        { 
            get => _selectedSortItem; 
            set
            {
                _selectedSortItem = value;
                Sorting(value);
                OnPropertyChanged(nameof(SelectedSortItem));
            }
        }
        #endregion
        private void Sorting(string str)
        {
            var list = ViewTableItems;
            if (str == "По типу продукта" || str=="")
            {
                TableItems = list.OrderBy(p => p.ProductTypeTitle).ToList();
            }
            else if (str == "По названию продукта" || str == "")
            {
                TableItems = list.OrderBy(p => p.ProductTitle).ToList();
            }
            else if (str == "По стоимости" || str == "")
            {
                TableItems = list.OrderBy(p => p.MinCostForAgent).ToList();
            }
            else if (str == "По артикулу" || str == "")
            {
                TableItems = list.OrderBy(p => p.ArticleNumber).ToList();
            }
            ViewTableItems = TableItems;
        }
        private void Filthering(string str)
        {
            ViewTableItems = TableItems.Where(p => p.ProductTypeTitle == str).ToList();
            if (str=="Все продукты")
            {
                ViewTableItems = TableItems;
            }
        }
        public string Search 
        { 
            get => _search;
            set
            {
                _search = value;
                ViewTableItems = TableItems.Where(p => p.ProductTitle.ToLower().Contains(value.ToLower())).ToList();
            }
        }

        public MainWindowViewModel()
        {
            CreatedFullTableItems();
            CreateSortingAndFilteringList();
            AddNewProductCommand = ReactiveCommand.Create(OpenAddProductWindow);
        }
        private void CreateSortingAndFilteringList()
        {
            using (LopushokDBAxyonovContext db = new LopushokDBAxyonovContext())
            {
                FiltherBox.Add("Все продукты");
                foreach (var item in db.ProductTypes)
                {
                    FiltherBox.Add(item.Title);
                }
            }
            SortBox.Add("По названию продукта");
            SortBox.Add("По типу продукта");
            SortBox.Add("По стоимость");
            SortBox.Add("По артикулу");
        }
        private void CreatedFullTableItems()
        {
            using(LopushokDBAxyonovContext db= new LopushokDBAxyonovContext())
            {
                ProductMaterials = db.ProductMaterials
                    .Include(p => p.Product)
                    .ThenInclude(pt=>pt.ProductType)
                    .Include(m=>m.Material)
                    .ToList();

                for (int i = 0; i < ProductMaterials.Count(); i++)
                {
                    var image = @"\products\picture.png";
                    if (ProductMaterials[i].Product.Image!="")
                    {
                        image = ProductMaterials[i].Product.Image;
                    }
                    TableItem tableItem = new TableItem
                    {
                        ProductTitle = ProductMaterials[i].Product.Title,
                        ProductTypeTitle = ProductMaterials[i].Product.ProductType.Title,
                        Image = new Bitmap("." + image),
                        MaterialTitle = ProductMaterials[i].Material.Title,
                        ArticleNumber = ProductMaterials[i].Product.ArticleNumber,
                        MinCostForAgent = ProductMaterials[i].Product.MinCostForAgent
                    }; 
                    TableItems.Add(tableItem);
                    ProductMaterials.Remove(ProductMaterials[i]);
                    for (int j = 0; j < ProductMaterials.Count(); j++)
                    {
                        if (TableItems.Last().ProductTitle == ProductMaterials[j].Product.Title)
                        {
                            TableItems.Last().MaterialTitle = TableItems.Last().MaterialTitle + ", " + ProductMaterials[j].Material.Title;
                            ProductMaterials.Remove(ProductMaterials[j]);
                            j--;
                        }
                    }
                }
                ViewTableItems = TableItems;
            }
        }
        public ReactiveCommand<Unit,Unit> AddNewProductCommand { get; }
        private void OpenAddProductWindow()
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.Show();
        }
        public class TableItem
        {
            public string ProductTitle { get; set; } = null!;
            public string ProductTypeTitle { get; set; } = null!;
            public Bitmap Image { get; set; } = null!;
            public string MaterialTitle { get; set; } = null!;
            public string ArticleNumber { get; set; } = null!;
            public decimal MinCostForAgent { get; set; }
        }
    }
}
