using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using ProformaInvoiceApp.Services;
using ProformaInvoiceApp.Models;

namespace ProformaInvoiceApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private DatabaseService dbService;
        private TextBox txtInvoiceNumber, txtBuyer, txtProduct, txtPackSize, txtQuantity, txtRate;
        private DateTimePicker dtpInvoiceDate;
        private Button btnAddInvoice;

        public MainForm()
        {
            InitializeComponents();
            dbService = new DatabaseService();
        }

        private void InitializeComponents()
        {
            this.Text = "Proforma Invoice Database";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Invoice Number
            var lblInvoice = new Label { Text = "Invoice Number:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtInvoiceNumber = new TextBox { Location = new Point(120, 20), Size = new Size(200, 20) };
            
            // Invoice Date
            var lblDate = new Label { Text = "Invoice Date:", Location = new Point(20, 50), Size = new Size(100, 20) };
            dtpInvoiceDate = new DateTimePicker { Location = new Point(120, 50), Size = new Size(200, 20) };
            
            // Buyer
            var lblBuyer = new Label { Text = "Buyer:", Location = new Point(20, 80), Size = new Size(100, 20) };
            txtBuyer = new TextBox { Location = new Point(120, 80), Size = new Size(200, 20) };
            
            // Product
            var lblProduct = new Label { Text = "Product:", Location = new Point(20, 110), Size = new Size(100, 20) };
            txtProduct = new TextBox { Location = new Point(120, 110), Size = new Size(200, 20) };
            
            // Pack Size
            var lblPackSize = new Label { Text = "Pack Size:", Location = new Point(20, 140), Size = new Size(100, 20) };
            txtPackSize = new TextBox { Location = new Point(120, 140), Size = new Size(200, 20) };
            
            // Quantity
            var lblQuantity = new Label { Text = "Quantity:", Location = new Point(20, 170), Size = new Size(100, 20) };
            txtQuantity = new TextBox { Location = new Point(120, 170), Size = new Size(200, 20) };
            
            // Rate
            var lblRate = new Label { Text = "Rate:", Location = new Point(20, 200), Size = new Size(100, 20) };
            txtRate = new TextBox { Location = new Point(120, 200), Size = new Size(200, 20) };
            
            // Add Invoice Button
            btnAddInvoice = new Button { Text = "Add Invoice", Location = new Point(120, 230), Size = new Size(100, 30) };
            btnAddInvoice.Click += BtnAddInvoice_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblInvoice, txtInvoiceNumber,
                lblDate, dtpInvoiceDate,
                lblBuyer, txtBuyer,
                lblProduct, txtProduct,
                lblPackSize, txtPackSize,
                lblQuantity, txtQuantity,
                lblRate, txtRate,
                btnAddInvoice
            });
        }

        private void BtnAddInvoice_Click(object sender, EventArgs e)
        {
            if (ValidateInvoice())
            {
                var invoice = new Invoice
                {
                    InvoiceNumber = txtInvoiceNumber.Text,
                    InvoiceDate = dtpInvoiceDate.Value,
                    Buyer = txtBuyer.Text,
                    ProductName = txtProduct.Text,
                    PackSize = txtPackSize.Text,
                    Quantity = int.Parse(txtQuantity.Text),
                    Rate = decimal.Parse(txtRate.Text),
                    BaseProductCode = "TEMP001",
                    OriginalPrice = decimal.Parse(txtRate.Text)
                };

                dbService.AddInvoice(invoice);
                MessageBox.Show("Invoice added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInvoiceForm();
            }
        }

        private bool ValidateInvoice()
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
            {
                MessageBox.Show("Please enter invoice number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter valid quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(txtRate.Text, out decimal rate) || rate <= 0)
            {
                MessageBox.Show("Please enter valid rate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ClearInvoiceForm()
        {
            txtInvoiceNumber.Clear();
            dtpInvoiceDate.Value = DateTime.Now;
            txtBuyer.Clear();
            txtProduct.Clear();
            txtPackSize.Clear();
            txtQuantity.Clear();
            txtRate.Clear();
        }
    }
}