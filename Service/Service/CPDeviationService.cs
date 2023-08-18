﻿using AutoMapper;
using Core.Data.DTO;
using Core.Data.Entities;
using Core.Utilities;
using Microsoft.Extensions.Logging;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitofWork;

namespace Service.Service
{
    public class CPDeviationService : ICPDeviationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckpointDeviations> _logger;
        private ResultModel _resultModel;

        public CPDeviationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CheckpointDeviations> logger, ResultModel resultModel)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _resultModel = resultModel;
        }
        public async Task<ResultModel> CreateOrUpdate(CPDeviationDTO model)
        {
            try
            {
                var data = _mapper.Map<CheckpointDeviations>(model);
                if(data.CheckpointDeviationId == 0) 
                {
                    data.CPDevCode = GetNextCode();
                    var result = _unitOfWork.CPDeviationRepository.Insert(data);
                }
                else 
                {
                    _unitOfWork.CPDeviationRepository.UpdateVoid(data);
                }

                var list = _unitOfWork.CPDeviationRepository.GetAll();
                _unitOfWork.Commit();
                _resultModel.Success = true;
                _resultModel.Data = list;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }

        public async Task<ResultModel> Delete(int id)
        {
            try
            {
                var result = _unitOfWork.CPDeviationRepository.Get(x => x.CheckpointDeviationId == id).FirstOrDefault();
                if (result != null)
                {
                    _unitOfWork.CPDeviationRepository.Delete(id);
                    _unitOfWork.Commit();
                    _resultModel.Success = true;
                    _resultModel.Message = "Record deleted sucessfully.";
                }
                else
                {
                    _resultModel.Success = false;
                    _resultModel.Message = "Record not found.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }

        public ResultModel Get()
        {
            try
            {
                _resultModel.Data = _mapper.Map<List<CPDeviationDTO>>(_unitOfWork.CPDeviationRepository.GetAll().ToList());
                _resultModel.Success = true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }

        public ResultModel Get(int pageIndex = 0, int pageSize = int.MaxValue, string? Search = null)
        {
            try
            {
                var query = String.IsNullOrEmpty(Search) ? "" : DBUtil.GenerateSearchQuery<CPDeviationDTO>(Search);
                _resultModel.Data = _unitOfWork.CPDeviationRepository.PagedList(query, pageIndex, pageSize);
                _resultModel.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }

        public ResultModel Get(int id)
        {
            try
            {
                _resultModel.Data = _mapper.Map<CPDeviationDTO>(_unitOfWork.CPDeviationRepository.Get(s => s.CheckpointDeviationId == id).FirstOrDefault());

                return _resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }
        public ResultModel Export(string? Search = null)
        {
            try
            {
                List<CPDeviationDTO> data = new();
                data = _mapper.Map<List<CPDeviationDTO>>(_unitOfWork.CPDeviationRepository.Get(x => x.DeletedOn == null).ToList());
                if (!String.IsNullOrEmpty(Search))
                    data = data.Where(s => !String.IsNullOrEmpty(s.Deviation) && s.Deviation.Contains(Search) || !String.IsNullOrEmpty(s.CPDevCode) && s.CPDevCode.Contains(Search)).ToList();

                byte[] content = ExcelExportUtility.ExportToExcel<CPDeviationDTO>(data);
                _resultModel.Success = true;
                _resultModel.Data = content;
                _resultModel.Message = $"Total Items Exported {data.Count}";
                return _resultModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:", ex);
                _resultModel.Success = false;
                _resultModel.Message = "Error While Get Record";
            }
            return _resultModel;
        }
        private string GetNextCode()
        {
            string strCCCode = string.Empty;
            string strPref = "CD";
            try
            {
                string sqlQuery = "SELECT FORMAT(Code,'" + strPref + "-0000') FROM ";
                sqlQuery += "(SELECT IsNull(MAX(SUBSTRING(CPDevCode, PATINDEX('%[0-9]%', CPDevCode),Len(CPDevCode))),0) + 1 As Code FROM tblCPDeviation WHERE PATINDEX('%[-]%',CPDevCode) = 3 AND PATINDEX('%[0-9]%', CPDevCode) > 0  )D ";
                var dpt = _unitOfWork.CPDeviationRepository.FreeDynamicQuery(sqlQuery);

                strCCCode = (dpt != null) ? ((object[])((System.Collections.Generic.IDictionary<string, object>)dpt).Values)[0].ToString() : "CD-0001";
            }
            catch (Exception e)
            {
                strCCCode = "CD-0001";
            }
            return strCCCode;
        }
    }
}
