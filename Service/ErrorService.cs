using Data.Infrastructure;
using Data.Repositories;
using Model.Models;

namespace Service
{
    public interface IErrorService
    {
        Error Create(Error error);

        void Save();
    }

    public class ErrorService : IErrorService
    {
        IErrrorRepository _errorRepository;
        IUnitOfWork _unitOfWork;

        public ErrorService(IErrrorRepository errorRepository, IUnitOfWork unitOfWork)
        {
            _errorRepository = errorRepository;
            _unitOfWork = unitOfWork;
        }

        public Error Create(Error error)
        {
           return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
