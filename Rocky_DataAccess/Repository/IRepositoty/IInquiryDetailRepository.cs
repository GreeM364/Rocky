using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail inquiryDetail);
    }
}
