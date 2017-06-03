using Model.Models;
using System.Collections.Generic;
using Common;
using Data.Repositories;
using Data.Infrastructure;
using System;
using System.Linq;

namespace Service
{
    public interface IProductService
    {
        Product Create(Product product);

        void Update(Product product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        Product GetById(int id);

        IEnumerable<Product> GetListProductByType(int type, int top);

        IEnumerable<Product> GetNewProducts(int top);

        IEnumerable<Product> GetRelatedProducts(int id, int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetListProductByKeyword(string keyword, int top);

        IEnumerable<Product> GetListProductByKeywordPaging(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Tag> GetListTagByProductId(int id);

        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByTagId(string tagId, int page, int pageSize, out int totalRow);

        Tag GetTag(string tagId);

        void Save();
    }

    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IProductTagRepository _productTagRepository;
        ITagRepository _tagRepository;
        IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public Product Create(Product product)
        {
            var resultProduct = _productRepository.Add(product);
            _unitOfWork.Commit();

            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] strTag = product.Tags.Split(',');

                for (int i = 0; i < strTag.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strTag[i]))
                    {
                        var tagID = StringHelper.ToUnsignString(strTag[i]);

                        if (_tagRepository.Count(x => x.ID == tagID) == 0)
                        {
                            var tag = new Tag();

                            tag.ID = tagID;
                            tag.Name = strTag[i];
                            tag.Type = CommonConstants.ProductTag;

                            _tagRepository.Add(tag);
                        }

                        ProductTag productTag = new ProductTag();

                        productTag.ProductID = product.ID;
                        productTag.TagID = tagID;

                        _productTagRepository.Add(productTag);
                    }
                }
            }

            return resultProduct;
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productRepository.GetMulti(x => x.Name.ToUpper().Contains(keyword.ToUpper()));
            }
            else
            {
                return _productRepository.GetAll();
            }
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);

            totalRow = query.Count();

            switch (sort)
            {
                case "new":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "price-asc":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price-desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetListProductByKeyword(string keyword, int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.Name.ToUpper().Contains(keyword.ToUpper())).Take(top);
        }

        public IEnumerable<Product> GetListProductByKeywordPaging(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
               query = query.Where(x => x.Status && x.Name.ToUpper().Contains(keyword.ToUpper()));
            }

            totalRow = query.Count();

            switch (sort)
            {
                case "new":
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
                case "price-asc":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "price-desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                default:
                    break;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetListProductByTagId(string tagId, int page, int pageSize, out int totalRow)
        {
            return _productRepository.GetListProductByTagId(tagId, page, pageSize, out totalRow);
        }

        public IEnumerable<Product> GetListProductByType(int type, int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.CategoryID == type).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public IEnumerable<Product> GetNewProducts(int top)
        {
            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetRelatedProducts(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);

            return _productRepository.GetMulti(x => x.Status && x.CategoryID == product.CategoryID && x.ID != id).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public Tag GetTag(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public void IncreaseView(int id)
        {
            var model = _productRepository.GetSingleById(id);

            if (model.ViewCount.HasValue)
            {
                model.ViewCount += 1;
            }
            else
            {
                model.ViewCount = 1;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);

            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] strTag = product.Tags.Split(',');

                for (int i = 0; i < strTag.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strTag[i]))
                    {
                        var tagID = StringHelper.ToUnsignString(strTag[i]);

                        if (_tagRepository.Count(x => x.ID == tagID) == 0)
                        {
                            var tag = new Tag();

                            tag.ID = tagID;
                            tag.Name = strTag[i];
                            tag.Type = CommonConstants.ProductTag;

                            _tagRepository.Add(tag);
                        }

                        _productTagRepository.DeleteMulti(x => x.ProductID == product.ID);

                        ProductTag productTag = new ProductTag();

                        productTag.ProductID = product.ID;
                        productTag.TagID = tagID;

                        _productTagRepository.Add(productTag);
                    }
                }
            }
        }
    }
}
