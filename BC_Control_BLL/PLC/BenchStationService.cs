using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Models.BenchConfig;
using System.Diagnostics;

namespace BC_Control_Helper
{
    public class BenchStationService
    {
        private readonly IBenchStationEntity _stations;
        private readonly IPLCHelper _plcHelper;
        private readonly IExcelOperation _excelOperation;
        private readonly ILogOpration _logOpration;
        private readonly string _path = @"D:\BC3100\212\File";
        public BenchStationService(IBenchStationEntity stations, IExcelOperation excelOperation, IPLCHelper plcHelper, ILogOpration logOpration)
        {
            _stations = stations;
            _excelOperation = excelOperation;
            _plcHelper = plcHelper;
            AnylizeStationEntity();
            _logOpration = logOpration;
        }
        public IBenchStationEntity GetBenchEntity()
        {
            return _stations;
        }
        public List<StationCollection> GetBenchEntity(StationEnum stationEnum)
        {
            return _stations.Stations.Where(filter => filter.StationType == stationEnum).ToList();
        }
        public void UpdateStationEntity()
        {
            try
            {
                
                Parallel.ForEach(_stations.Stations, station =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    try
                    {

                        station.ModuleStatus?.UpdateStatus();
                        station.ModuleDataCollection?.UpdateDataClasses();
                        station.IOViewDataCollection?.UpdateDataClasses();
                        station.ControlDataCollection?.UpdateDataClasses();
                        station.BatchDataCollection?.UpdateStatus();
                        if (stopwatch.ElapsedMilliseconds > 500)
                        {
                            _logOpration.WriteError($"{station.StationName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logOpration.WriteError(ex);
                    }
                    finally
                    {
                        stopwatch.Stop();
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region 私有方法        
        private void GetStationValue(StationCollection tKClass)
        {

        }
        private void AnylizeStationEntity()
        {
            try
            {
                string filepath = Path.Combine(_path, $"StationConfig.json");
                if (!File.Exists(filepath))
                {
                    SaveEntity(filepath);
                }
                string entity = File.ReadAllText(filepath);
                _stations.Stations = JSONHelper.JSONToEntity<List<StationCollection>>(entity).ToList();
                Parallel.ForEach(_stations.Stations, (item) => GetStationEntity(item));
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void SaveEntity(string filepath)
        {
            try
            {
                _stations.Stations = new List<StationCollection>();
                for (int i = 1; i < 9; i++)
                {
                    _stations.Stations.Add(new StationCollection()
                    {
                        StationName = i.ToString(),
                        StationNo = i,
                        StationType = 0,
                    });
                }
                string jsonString = JSONHelper.EntityToJSON(_stations.Stations);
                File.WriteAllText(filepath, jsonString);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void GetStationEntity(StationCollection tKClass)
        {
            try
            {
                string fileFullName = Path.Combine(_path, "Station", $"{tKClass.StationName}", $"{tKClass.StationName}Status.xlsx");
                string paramfileFullName = Path.Combine(_path, "Station", $"{tKClass.StationName}", $"{tKClass.StationName}Parameter.xlsx");


                tKClass.ModuleDataCollection = GetDataClassCollectionEntity(fileFullName, "ModuleData");
                ChangeData(tKClass.ModuleDataCollection.Select(src => (IPLCValue)src).ToList());


                tKClass.IOViewDataCollection = GetDataClassCollectionEntity(fileFullName, "IOStatus");
                ChangeData(tKClass.IOViewDataCollection.Select(src => (IPLCValue)src).ToList());


                tKClass.ParameterCollections = GetParameterCollectionEntity(paramfileFullName);
                ChangeData(tKClass.ParameterCollections.Select(src => (IPLCValue)src).ToList());


                tKClass.BatchDataCollection = GetStatusClassCollectionEntity(fileFullName, "ModuleStatus");
                ChangeData(tKClass.BatchDataCollection.Select(src => (IPLCValue)src).ToList());
                tKClass.BatchDataCollection.AnalysisStatusAttribute();


                tKClass.ModuleStatus = GetStatusClassCollectionEntity(fileFullName, "TemperatureControlOffList");
                ChangeData(tKClass.ModuleStatus.Select(src => (IPLCValue)src).ToList());
                tKClass.ModuleStatus.AnalysisStatusAttribute();

                tKClass.ControlDataCollection = GetDataClassCollectionEntity(fileFullName, "ControlList");
                ChangeData(tKClass.ControlDataCollection.Select(src => (IPLCValue)src).ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<StatusClass> GetStatusClassCollectionEntity(string path, string sheetName)
        {
            try
            {
                return _excelOperation.ReadExcelToObjects<StatusClass>(path, sheetName).ToList();
            }
            catch (Exception ex)
            {
                return new List<StatusClass>();
            }
        }
        private List<ParameterModel> GetParameterCollectionEntity(string path)
        {
            try
            {
                return _excelOperation.ReadExcelToObjects<ParameterModel>(path).ToList();
            }
            catch (Exception ex)
            {
                return new List<ParameterModel>();
            }
        }
        private List<DataClass> GetDataClassCollectionEntity(string path, string sheetName)
        {
            try
            {
                return _excelOperation.ReadExcelToObjects<DataClass>(path, sheetName).ToList();
            }
            catch (Exception ex)
            {
                return new List<DataClass>();
            }
        }
        private void ChangeData(List<IPLCValue> TemperatureControlOffList)
        {
            try
            {
                foreach (IPLCValue item in TemperatureControlOffList)
                {

                    _plcHelper.SetVariableType(item.ValueAddress, item);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

    }
}
