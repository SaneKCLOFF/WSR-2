using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Variant2.Axyonov.Models;

namespace Variant2.Axyonov.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<string> _filtherBox = new();
        private List<string> _sortBox = new();
        private List<TableItem> _tableItems = new();
        private List<TableItem> _viewTableItems = new();
        private List<ProductMaterial> _productMaterials = new();
        private string _search = null!;
        private string _selectedFiltherItem = null!;
        private string _selectedSortItem = null!;

        public List<TableItem> TableItems { get => _tableItems; set => _tableItems = value; }
        public List<TableItem> ViewTableItems { get => _viewTableItems; set => _viewTableItems = value; }
        public List<ProductMaterial> ProductMaterials { get => _productMaterials; set => _productMaterials = value; }
        public List<string> FiltherBox { get => _filtherBox; set => _filtherBox = value; }
        public List<string> SortBox { get => _sortBox; set => _sortBox = value; }
        public string SelectedFiltherItem 
        { 
            get => _selectedFiltherItem;
            set
            {
                _selectedFiltherItem = value;
                Filthering(value);
                
            }
        }
        public string SelectedSortItem 
        { 
            get => _selectedSortItem; 
            set
            {
                _selectedSortItem = value;
                Sorting(value);
            }
        }
        private void Sorting(string str)
        {
            var list = TableItems;
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
            //
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
