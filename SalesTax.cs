using System;
using System.Collections.Generic;

// ---------------------------------------------------
// Author: Komal Rangoonwala for Makkajai Dev Challenge
// Date: 17 Apr 2021, 21 Apr 2021, 22 Apr 2021, 23 Apr 2021
// ---------------------------------------------------

// A structure to hold and manage the data
public class Program
{
	public enum ProductCategory
	{
		Books = 1,
		Food = 2,
		MedicalProducts = 3,
		Other = 4,
	}
	
	public enum INPUT_TYPE
	{
		SYSTEM = 1,
		USER = 2,
	}
	
	public class Product
	{
		#region Properties
		string _name = string.Empty;
		double _price = 0;
		bool _isImported = false;
		ProductCategory _category = ProductCategory.Other;
		int _quantity = 0;
		double _totalPrice = 0;
		
		public string Name
		{
			get
			{
				return this._name;
			}
		}
		
		public double Price
		{
			get
			{
				return this._price;
			}
		}
		
		public bool IsImported
		{
			get
			{
				return this._isImported;
			}
		}
		
		public ProductCategory Category
		{
			get
			{
				return this._category;
			}
		}
		
		public int Quantity
		{
			get
			{
				return this._quantity;
			}
		}
		
		public double TotalPrice
		{
			get
			{
				return this._totalPrice;
			}
		}
		#endregion

		#region Constructors
		public Product(string Name, double Price, bool IsImported, ProductCategory Category, int Quantity)
		{
			this._name = Name;
			this._price = Price;
			this._isImported = IsImported;
			this._category = Category;
			this._quantity = Quantity;
			this._totalPrice = Price * Quantity;
		}
		#endregion
	}
	
	public interface ITax
	{
		Product GetProduct();
		double Calculate();
	}
	
	public class BaseTax : ITax
	{
		Product _product;
		public BaseTax(Product product)
		{
			this._product = product;
		}
		
		public Product GetProduct()
		{
			return this._product;
		}

		public double Calculate()
		{
			return 0;
		}
	}
	
	public class RegularTax : ITax
	{
		Product _product;
		public RegularTax(Product product)
		{
			this._product = product;
		}
		
		public Product GetProduct()
		{
			return this._product;
		}
		
		public double Calculate()
		{
			return RoundUpTo(this._product.TotalPrice * 0.1);
		}
	}
	
	public class ImportTax : ITax
	{
		Product _product;
		public ImportTax(Product product)
		{
			this._product = product;
		}
		
		public Product GetProduct()
		{
			return this._product;
		}

		public double Calculate()
		{
			return RoundUpTo(this._product.TotalPrice * 0.05);
		}
	}
	
	public class Invoice
	{
		#region Properties
		List<Product> _invoice = new List<Product>();
		double _totalTax = 0;
		double _totalAmount = 0;
		#endregion

		#region Constructors
		public Invoice(List<Product> invoice, double totalTax, double totalAmount)
		{
			this._invoice = invoice;
			this._totalTax = totalTax;
			this._totalAmount = totalAmount;
		}
		#endregion

		#region Methods
		public void Print()
		{
			Console.WriteLine("\nYour invoice:\n");
			foreach(Product objProduct in this._invoice)
				Console.WriteLine(objProduct.Quantity + " " + objProduct.Name + " at " + objProduct.Price);

			Console.WriteLine("Sales Taxes: " + this._totalTax.ToString("F2") + "\nTotal: " + this._totalAmount.ToString());
		}
		#endregion
	}
	
	#region Input methods
	// Method to test some fixed inputs as given in the question
	public static List<Product> GetFixedInput(int TestCase)
	{
		List<Product> Products = new List<Product>();
		switch(TestCase)
		{
			case 1:
				Product objProduct1 = new Product("book", 12.49, false, ProductCategory.Books, 1);
				Products.Add(objProduct1);
				
				Product objProduct2 = new Product("music CD", 14.99, false, ProductCategory.Other, 1);
				Products.Add(objProduct2);
				
				Product objProduct3 = new Product("chocolate bar", 0.85, false, ProductCategory.Food, 1);
				Products.Add(objProduct3);
				break;
				
			case 2:
				Product objProduct4 = new Product("imported box of chocolates", 10.00, true, ProductCategory.Food, 1);
				Products.Add(objProduct4);
				
				Product objProduct5 = new Product("imported bottle of perfume", 47.50, true, ProductCategory.Other, 1);
				Products.Add(objProduct5);				
				break;
				
			case 3:
				Product objProduct6 = new Product("imported bottle of perfume", 27.99, true, ProductCategory.Other, 1);
				Products.Add(objProduct6);
				
				Product objProduct7 = new Product("bottle of perfume", 18.99, false, ProductCategory.Other, 1);
				Products.Add(objProduct7);
				
				Product objProduct8 = new Product("packet of headache pills", 9.75, false, ProductCategory.MedicalProducts, 1);
				Products.Add(objProduct8);
				
				Product objProduct9 = new Product("box of imported chocolates", 11.25, true, ProductCategory.Food, 1);
				Products.Add(objProduct9);
				break;
		}
		
		return Products;
	}
	
	// Method to test user input
	public static List<Product> GetUserInput()
	{
		Console.WriteLine("Enter the number of products: ");
		int TotalProducts = Convert.ToInt32(Console.ReadLine());

		List<Product> Products = new List<Product>();
		for ( int i = 1; i <= TotalProducts; i++ )
		{
			Console.WriteLine("#" + i.ToString() + " Enter product name: ");
			string strName = Console.ReadLine();
			
			Console.WriteLine("#" + i.ToString() + " Enter product price: ");
			double dblPrice = Convert.ToDouble(Console.ReadLine());
			
			Console.WriteLine("#" + i.ToString() + " Is product imported? [true or false] ");
			bool blnIsImported = Convert.ToBoolean(Console.ReadLine());
			
			Console.WriteLine("#" + i.ToString() + " Enter product category [Books = 1, Food = 2, Medical products = 3, Other = 4]: ");
			ProductCategory eCategory = (ProductCategory) Convert.ToInt32(Console.ReadLine());
			
			Console.WriteLine("#" + i.ToString() + " Enter product quantity: ");
			int intQuantity = Convert.ToInt32(Console.ReadLine());
			
			Product objProduct = new Product(strName, dblPrice, blnIsImported, eCategory, intQuantity);
			Products.Add(objProduct);
		}
		
		return Products;
	}
	#endregion
		
	#region Main method
	// Main method where the execution begins
	public static void Main()
	{
		try
		{
			Console.WriteLine("Do you want to test fixed input or user input? Enter 1 for fixed system input, 2 for user input:");
			INPUT_TYPE InputType = (INPUT_TYPE) Convert.ToInt32(Console.ReadLine());
			if(InputType != INPUT_TYPE.SYSTEM && InputType != INPUT_TYPE.USER)
				throw new Exception("Invalid input! Please enter either 1 or 2.");

			List<Product> lstProducts = new List<Product>();
			if(InputType == INPUT_TYPE.SYSTEM) // System input
			{
				Console.WriteLine("Which test case do you want to test? 1, 2, or 3?");
				
				int intTestCase = Convert.ToInt32(Console.ReadLine());
				if(intTestCase < 1 || intTestCase > 3)
					throw new Exception("Invalid test case! Please enter either 1, 2 or 3.");
				
				lstProducts = GetFixedInput(intTestCase);
			}
			else // User input
				lstProducts = GetUserInput();
			
			if(lstProducts.Count <= 0)
				throw new Exception("Product list is empty. System is unable to process the orders and generate the invoice.");
			
			Invoice objInvoice = ProcessOrders(lstProducts);
			objInvoice.Print();
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}
	#endregion
		
	#region Core methods
	// Method to process the actual data
	public static Invoice ProcessOrders(List<Product> lstProducts)
	{
		List<Product> lstInvoice = new List<Product>();
		double TotalTax = 0;
		double TotalAmount = 0;
		
		foreach ( Product objProduct in lstProducts )
		{
			double taxToApply = 0;
			BaseTax baseTax = new BaseTax(objProduct);
			double calculatedBaseTax = baseTax.Calculate();
			taxToApply += calculatedBaseTax;

			if ( objProduct.Category == ProductCategory.Other )
			{
				RegularTax regularTax = new RegularTax(objProduct);
				double calculatedRegularTax = regularTax.Calculate();
				taxToApply += calculatedRegularTax;
			}
			
			if ( objProduct.IsImported )
			{
				ImportTax importTax = new ImportTax(objProduct);
				double calculatedImportTax = importTax.Calculate();
				taxToApply += calculatedImportTax;
			}
			
			TotalTax += taxToApply;
			double CalculatedPrice = objProduct.TotalPrice + taxToApply;
			TotalAmount += CalculatedPrice;
				
			Product objInvoiceProduct = new Product(objProduct.Name, CalculatedPrice, objProduct.IsImported, objProduct.Category, objProduct.Quantity);
			lstInvoice.Add(objInvoiceProduct);
		}
		
		return new Invoice(lstInvoice, TotalTax, TotalAmount);
	}
	
	#endregion
	
	#region Utility methods
	public static double RoundUpTo(double ValueToRound)
	{
		// Rounds upto nearest 0.05;
		return Math.Ceiling(ValueToRound * 20) / 20;
	}
	#endregion
}
