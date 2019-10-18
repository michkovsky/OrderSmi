using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OrderSmi.Model
{
	[Serializable]
	[XmlRoot(ElementName = "orders")]
	public class OrdersRoot : List<Order>
	{

	}
	[XmlType("order")]
	public class Order
	{
		[JsonIgnore]
		public int Id { get; set; }
		[XmlElement("oxid")]
		public int OxId { get; set; }
		[XmlElement("orderdate")]
		public DateTime OrderDate { get; set; }
		[XmlElement("billingaddress")]
		public Address BillingAddress { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.Unprocessed;
		[XmlArray(ElementName = "payments")]
		[XmlArrayItem(ElementName = "payment")]
		public List<Payment> Payments { get; set; } = new List<Payment>();
		[XmlArray(ElementName = "articles")]
		[XmlArrayItem(ElementName = "orderarticle")]
		public List<Article> Articles { get; set; } = new List<Article>();

		public int? InvoiceNumber { get; set; }
	}

	public enum OrderStatus
	{
		None = 0,
		Unprocessed = None,
		Canceled,
		Processed,
	}
	public class Address
	{
		public int Id { get; set; }
		[XmlElement("billemail")]
		public string Email { get; set; }
		[XmlElement("billfname")]
		public string FullName { get; set; }
		[XmlElement("billstreet")]
		public string Street { get; set; }
		[XmlElement("billstreetnr")]
		public string StreetNumber { get; set; }
		[XmlElement("billcity")]
		public string City { get; set; }
		[XmlElement("billzip")]
		public string ZipCode { get; set; }
		[XmlIgnore]
		public string Country { get; set; }
		[XmlElement("country")]
		[JsonIgnore]
		public Country CountryInternal
		{
			get
			{
				return new Model.Country() { Geo = Country };
			}
			set
			{
				this.Country = value?.Geo;
			}
		}
	}
	[XmlType("country")]
	public class Country
	{
		[XmlElement("geo")]
		public string Geo { get; set; }
	}

	public class Payment
	{
		public int Id { get; set; }
		[XmlElement("method-name")]
		public string Method { get; set; }
		[XmlElement("amount")]
		public decimal Amount { get; set; }
	}

	public class Article
	{
		public int Id { get; set; }
		[XmlElement("artnum")]
		public string ArticleNumber { get; set; }
		[XmlElement("title")]
		public string Title { get; set; }
		[XmlElement("amount")]
		public decimal Amount { get; set; }
		[XmlElement("brutprice")]
		public decimal BrutPrice { get; set; }
	}


}
