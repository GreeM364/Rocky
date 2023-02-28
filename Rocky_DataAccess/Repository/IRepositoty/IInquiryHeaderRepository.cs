using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader inquiryHeader);
    }
}
