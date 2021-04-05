using Backend.Models;
using Backend.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductShopWebForms
{
    public partial class Products : System.Web.UI.Page
    {
        private readonly ProductService _productService = new ProductService();
        private readonly ShopService _shopService = new ShopService();
        private readonly CategoryService _categoryService = new CategoryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillGrid();
                FillGridShop();
                FillGridCategories();
                FillShoplb();
            }
        }

        private void FillShoplb()
        {
            List<ShopModel> shops = _shopService.GetQueryShop().ToList();
            listboxShop.DataValueField = "Id";
            listboxShop.DataTextField = "Name";
            listboxShop.DataSource = shops;
            listboxShop.DataBind();
        }

        private void FillGridCategories()
        {
            var categories = _categoryService.GetQueryCategory().ToList();
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataSource = categories;
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem()
            {
                Value = "",
                Text = "-- Select --"
            });
        }

        private void FillGridShop()
        {
            var shops = _shopService.GetQueryShop().ToList();

            gvShops.DataSource = shops;
            gvShops.DataBind();
            SetColumnVisibilities(gvShops);
            gvShops.SelectedIndex = -1;
        }

        private void FillGrid()
        {
            try
            {
                var products = _productService.GetQuery().ToList();

                gvProducts.DataSource = products;
                gvProducts.DataBind();
                SetColumnVisibilities(gvProducts);
                gvProducts.SelectedIndex = -1;
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        private void SetColumnVisibilities(GridView gridView)
        {
            if (gridView.Rows != null && gridView.Rows.Count > 0)
            {
                gridView.HeaderRow.Cells[1].Visible = false;
                foreach (GridViewRow row in gridView.Rows)
                {
                    row.Cells[1].Visible = false;
                }
            }
        }

        protected void bCleanProduct_Click(object sender, EventArgs e)
        {
            tbProductName.Text = "";
            tbUnitPriceProduct.Text = "";
            tbStockProduct.Text = "";
            ddlCategory.SelectedValue = "";
            listboxShop.Items.Clear();
        }

        protected void lbAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                pShopDetails.Visible = false;
                pProductDetails.Visible = false;
                if (string.IsNullOrWhiteSpace(tbProductName.Text))
                {
                    lMessage.Text = "Name must be entered";
                    return;
                }
                double? unitprice = null;
                if (!string.IsNullOrWhiteSpace(tbUnitPriceProduct.Text))
                {
                    unitprice = Convert.ToDouble(tbUnitPriceProduct.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                }
                int? stock = null;
                if (!string.IsNullOrWhiteSpace(tbStockProduct.Text))
                {
                    stock = Convert.ToInt32(tbStockProduct.Text);
                }

                if (ddlCategory.SelectedValue == "")
                {
                    lMessage.Text = "Category must be selected.";
                    return;
                }
                List<int> shopIdleri = new List<int>();
                foreach (ListItem item in listboxShop.Items)
                {
                    if (item.Selected)
                        shopIdleri.Add(Convert.ToInt32(item.Value));
                }

                ProductModel model = new ProductModel()
                {
                    Name = tbProductName.Text,
                    UnitPrice = unitprice,
                    Stock = stock,
                    CategoryId = Convert.ToInt32(ddlCategory.SelectedValue),
                    ShopIdleri = shopIdleri
                };
                _productService.Add(model);
                FillGrid();
                lMessage.Text = "The product has been registered!";

            }
            catch (FormatException exc)
            {
                lMessage.Text = "Unit price and stock must be numerical!";
            }
            catch (Exception exc)
            {
                lMessage.Text = "An error occurred during the process!";
            }
        }

        protected void lbUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                pShopDetails.Visible = false;
                pProductDetails.Visible = false;
                if (gvProducts.SelectedIndex == -1)
                {
                    lMessage.Text = "Select registration!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbProductName.Text))
                {
                    lMessage.Text = "Name must be entered";
                    return;
                }
                double? unitprice = null;
                if (!string.IsNullOrWhiteSpace(tbUnitPriceProduct.Text))
                {
                    unitprice = Convert.ToDouble(tbUnitPriceProduct.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                }
                int? stock = null;
                if (!string.IsNullOrWhiteSpace(tbStockProduct.Text))
                {
                    stock = Convert.ToInt32(tbStockProduct.Text);
                }

                if (ddlCategory.SelectedValue == "")
                {
                    lMessage.Text = "Category must be selected.";
                    return;
                }
                ProductModel model = new ProductModel()
                {
                    Id = Convert.ToInt32(gvProducts.SelectedRow.Cells[1].Text),
                    Name = tbProductName.Text,
                    UnitPrice = unitprice,
                    Stock = stock,
                    CategoryId = Convert.ToInt32(ddlCategory.SelectedValue)
                };
                model.ShopIdleri = listboxShop.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Convert.ToInt32(item.Value)).ToList();
                _productService.Update(model);
                FillGrid();
                lMessage.Text = "The product has been registered!";
                bCleanProduct_Click(null, null);
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillShoplb();
            FillGridCategories();
            FillDetails();
        }

        private void FillDetails()
        {
            try
            {
                int id = Convert.ToInt32(gvProducts.SelectedRow.Cells[1].Text);
                ProductModel model = _productService.GetQuery().SingleOrDefault(product => product.Id == id);
                tbProductName.Text = model.Name;
                tbUnitPriceProduct.Text = "";
                if (model.UnitPrice.HasValue)
                {
                    tbUnitPriceProduct.Text = model.UnitPrice.Value.ToString(new CultureInfo("tr"));
                }
                tbStockProduct.Text = "";
                if (model.Stock.HasValue)
                {
                    tbStockProduct.Text = model.Stock.Value.ToString();
                }
                ddlCategory.SelectedValue = "";
                if (model.CategoryId.HasValue)
                {
                    ddlCategory.SelectedValue = model.CategoryId.Value.ToString();
                }
                listboxShop.ClearSelection();
                foreach (ListItem item in listboxShop.Items)
                {
                    foreach (int shopId in model.ShopIdleri)
                    {
                        if (item.Value == shopId.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        protected void lbDeleteProduct_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = false;
            pProductDetails.Visible = false;
            if (gvProducts.SelectedIndex == -1)
            {
                lMessage.Text = "Select registration!";
                return;
            }
            int id = Convert.ToInt32(gvProducts.SelectedRow.Cells[1].Text);
            _productService.Delete(id);
            lMessage.Text = "Registration deleted!";
            FillGrid();
            bCleanProduct_Click(null, null);
        }

        protected void lbDetailProduct_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = false;
            if (gvProducts.SelectedIndex == -1)
            {
                lMessage.Text = "Select registration!";
                return;
            }
            int id = Convert.ToInt32(gvProducts.SelectedRow.Cells[1].Text);
            ProductModel model = _productService.GetQuery().SingleOrDefault(product => product.Id == id);
            lDetailName.Text = model.Name;
            lDetailUnitPrice.Text = model.UnitPrice.HasValue ? model.UnitPrice.Value.ToString("C2", new CultureInfo("tr")) : "";
            lDetailStock.Text = model.Stock.HasValue ? model.Stock.Value.ToString("C2", new CultureInfo("tr")) : "";
            lDetailCategory.Text = model.Category;
            lDetailShop.Text = model.ShopsText;
            pProductDetails.Visible = true;
            bCleanProduct_Click(null, null);
        }

        protected void lbAddShop_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = false;
            pProductDetails.Visible = false;
            if (string.IsNullOrWhiteSpace(tbShopName.Text))
            {
                lMessage.Text = "Name must be entered";
                return;
            }
            if (string.IsNullOrWhiteSpace(tbShopAdress.Text))
            {
                lMessage.Text = "Address must be entered";
                return;
            }
            ShopModel model = new ShopModel()
            {
                Name = tbShopName.Text,
                Address = tbShopAdress.Text
            };                
            _shopService.Add(model);
            FillGridShop();
            lMessage.Text = "The shop has been registered!";
            bCleanShop_Click(null, null);
        }

        protected void gvShops_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDetailsShops();
        }

        private void FillDetailsShops()
        {
            try
            {
                int id = Convert.ToInt32(gvShops.SelectedRow.Cells[1].Text);
                ShopModel model = _shopService.GetQueryShop().SingleOrDefault(shop => shop.Id == id);
                tbShopName.Text = model.Name;
                tbShopAdress.Text = model.Address;
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        protected void bCleanShop_Click(object sender, EventArgs e)
        {
            tbShopName.Text = "";
            tbShopAdress.Text = "";
        }

        protected void lbUpdateShop_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = false;
            pProductDetails.Visible = false;
            if (string.IsNullOrWhiteSpace(tbShopName.Text))
            {
                lMessage.Text = "Name must be entered";
                return;
            }
            if (string.IsNullOrWhiteSpace(tbShopAdress.Text))
            {
                lMessage.Text = "Address must be entered";
                return;
            }
            ShopModel model = new ShopModel()
            {
                Id = Convert.ToInt32(gvShops.SelectedRow.Cells[1].Text),
                Name = tbShopName.Text,
                Address = tbShopAdress.Text
            };
            _shopService.Update(model);
            FillGridShop();            
            lMessage.Text = "The shop has been registered!";
            bCleanShop_Click(null, null);
        }

        protected void lbDeleteShop_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = false;
            pProductDetails.Visible = false;
            if (gvShops.SelectedIndex == -1)
            {
                lMessage.Text = "Select registration!";
                return;
            }
            int id = Convert.ToInt32(gvShops.SelectedRow.Cells[1].Text);
            _shopService.Delete(id);
            lMessage.Text = "Registration deleted!";
            FillGridShop();
            bCleanShop_Click(null, null);
        }

        protected void lbDetailShop_Click(object sender, EventArgs e)
        {
            pShopDetails.Visible = true;
            pProductDetails.Visible = false;
            if (gvShops.SelectedIndex == -1)
            {
                lMessage.Text = "Select registration!";
                return;
            }
            int id = Convert.ToInt32(gvShops.SelectedRow.Cells[1].Text);
            ShopModel model = _shopService.GetQueryShop().SingleOrDefault(shop => shop.Id == id);
            lDetailShopName.Text = model.Name;
            lDetailAddress.Text = model.Address;
            bCleanShop_Click(null, null);
        }
    }
}