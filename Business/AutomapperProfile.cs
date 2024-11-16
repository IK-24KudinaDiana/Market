using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
			CreateMap<Receipt, ReceiptModel>()
			   .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
			   .ForMember(rm => rm.OperationDate, r => r.MapFrom(x => x.OperationDate))
			   .ForMember(rm => rm.IsCheckedOut, r => r.MapFrom(x => x.IsCheckedOut))
			   .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id).ToList())) 
			   .ReverseMap();

			CreateMap<ReceiptDetail, ReceiptDetailModel>()
				.ForMember(cm => cm.ProductId, c => c.MapFrom(x => x.ProductId))
				.ForMember(cm => cm.Quantity, c => c.MapFrom(x => x.Quantity))
				.ForMember(cm => cm.ReceiptId, c => c.MapFrom(x => x.ReceiptId))
				.ReverseMap();

			CreateMap<Product, ProductModel>()
				.ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
				.ForMember(cm => cm.ProductCategoryId, c => c.MapFrom(c => c.ProductCategoryId))
				.ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
				.ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
				.ReverseMap();

			CreateMap<Customer, CustomerModel>()
			   .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
			   .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
			   .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
			   .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
			   .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
			   .ReverseMap();

			CreateMap<Person, CustomerModel>().ForMember(x => x.Name, r => r.MapFrom(x => x.Name))
				.ForMember(x => x.Surname, r => r.MapFrom(x => x.Surname))
				.ForMember(x => x.BirthDate, r => r.MapFrom(x => x.BirthDate))
				.ReverseMap();

			CreateMap<ProductCategory, ProductCategoryModel>()
				.ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
				.ReverseMap();
		}
    }
}